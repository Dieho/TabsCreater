using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FftGuitarTuner;
using Hellpers;
using Microsoft.Win32.SafeHandles;
using Microsoft.DirectX.DirectSound;
using SoundAnalysis;

namespace SoundCapture
{
    /// <summary>
    /// Base class to capture audio samples.
    /// </summary>
    internal static class SoundCapturing// : IDisposable
    {
        public static List<SoundFrequencyInfoSource> Owners = new List<SoundFrequencyInfoSource>();
        public static SoundCaptureDevice Device;

        const double MinFreq = 60;
        const double MaxFreq = 1300;

        const int BufferSeconds = 3;
        const int NotifyPointsInSecond = 2;

        // change in next two will require also code change
        const int BitsPerSample = 16;
        const int ChannelCount = 1;

        static int _sampleRate = 44100;
        static bool _isCapturing;

        public static bool IsCapturing
        {
            get { return _isCapturing; }
        }

        public static int SampleRate
        {
            get { return _sampleRate; }
            set
            {
                if (_sampleRate <= 0) throw new ArgumentOutOfRangeException();

                EnsureIdle();

                _sampleRate = value;
            }
        }

        static Capture _capture;
        static CaptureBuffer _buffer;
        static Notify _notify;
        static int _bufferLength;
        static readonly AutoResetEvent PositionEvent;
        static readonly SafeWaitHandle PositionEventHandle;
        static readonly ManualResetEvent Terminated;
        //private static Task Task;

        static CancellationTokenSource _tokenSource2 = new CancellationTokenSource();
        private static CancellationToken _ct;

        static SoundCapturing()
        {
            //_device = device;

            PositionEvent = new AutoResetEvent(false);
            PositionEventHandle = PositionEvent.SafeWaitHandle;
            Terminated = new ManualResetEvent(true);
        }

        private static void EnsureIdle()
        {
            if (IsCapturing)
                throw new SoundCaptureException("Capture is in process");
        }

        /// <summary>
        /// Starts capture process.
        /// </summary>
        public static void Start(SoundFrequencyInfoSource source)
        {
            // EnsureIdle();
            Owners.Add(source);
            _isCapturing = true;
            if (_tokenSource2.IsCancellationRequested)
            {
                _tokenSource2=new CancellationTokenSource();
            }

            WaveFormat format = new WaveFormat
            {
                Channels = ChannelCount,
                BitsPerSample = BitsPerSample,
                SamplesPerSecond = SampleRate,
                FormatTag = WaveFormatTag.Pcm
            };
            format.BlockAlign = (short)((format.Channels * format.BitsPerSample + 7) / 8);
            format.AverageBytesPerSecond = format.BlockAlign * format.SamplesPerSecond;

            _bufferLength = format.AverageBytesPerSecond * BufferSeconds;
            CaptureBufferDescription desciption = new CaptureBufferDescription
            {
                Format = format,
                BufferBytes = _bufferLength
            };

            _capture = new Capture(Device.Id);
            _buffer = new CaptureBuffer(desciption, _capture);

            int waitHandleCount = BufferSeconds * NotifyPointsInSecond;
            BufferPositionNotify[] positions = new BufferPositionNotify[waitHandleCount];
            for (int i = 0; i < waitHandleCount; i++)
            {
                BufferPositionNotify position = new BufferPositionNotify
                {
                    Offset = (i + 1) * _bufferLength / positions.Length - 1,
                    EventNotifyHandle = PositionEventHandle.DangerousGetHandle()
                };
                positions[i] = position;
            }

            _notify = new Notify(_buffer);
            _notify.SetNotificationPositions(positions);

            Terminated.Reset();

            Task.Factory.StartNew(() => ThreadLoop(_ct), _ct);// {Name = "Sound capture"};
            //Task.Start();
        }

        private static void ThreadLoop(CancellationToken token)
        {
            _buffer.Start(true);
            try
            {
                int nextCapturePosition = 0;
                WaitHandle[] handles = {Terminated, PositionEvent};
                while (WaitHandle.WaitAny(handles) > 0)
                {
                    int capturePosition, readPosition;
                    _buffer.GetCurrentPosition(out capturePosition, out readPosition);

                    int lockSize = readPosition - nextCapturePosition;
                    if (lockSize < 0) lockSize += _bufferLength;
                    if ((lockSize & 1) != 0) lockSize--;

                    int itemsCount = lockSize >> 1;

                    short[] data =
                        (short[]) _buffer.Read(nextCapturePosition, typeof(short), LockFlag.None, itemsCount);
                    ProcessData(data);
                    nextCapturePosition = (nextCapturePosition + lockSize) % _bufferLength;
                    if (token.IsCancellationRequested)
                    {
                        return; // token.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (ArgumentException)
            {
            }
            finally
            {
                if (!_buffer.Disposed)
                    _buffer.Stop();
            }

        }

        /// <summary>
        /// Processes the captured data.
        /// </summary>
        /// <param name="data">Captured data</param>
        private static void ProcessData(short[] data)
        {
            double[] x = new double[data.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = data[i] / 32768.0;
            }
            double freq = FrequencyUtils.FindFundamentalFrequency(x, SampleRate, MinFreq, MaxFreq);
            Owners.ForEach(a => a.FrequencyDetectedEventHandler.OnFrequencyDetected(new FrequencyDetectedEventArgs(freq)));
        }

        /// <summary>
        /// Stops capture process.
        /// </summary>
        public static void Stop(SoundFrequencyInfoSource source)
        {
            if (_isCapturing)
            {
                Owners.Remove(source);
                if (Owners.Count == 0)
                {
                    _isCapturing = false;

                    _tokenSource2.Cancel();

                    Terminated.Set();

                    _notify.Dispose();
                    _buffer.Dispose();
                    _capture.Dispose();
                }
            }
        }


    }
}
