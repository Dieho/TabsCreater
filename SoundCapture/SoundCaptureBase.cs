using System;
using System.Threading;
using Microsoft.DirectX.DirectSound;
using Microsoft.Win32.SafeHandles;

namespace SoundCapture
{
    /// <summary>
    /// Base class to capture audio samples.
    /// </summary>
    public abstract class SoundCaptureBase : IDisposable
    {
        const int BufferSeconds = 3;
        const int NotifyPointsInSecond = 2;

        // change in next two will require also code change
        const int BitsPerSample = 16; 
        const int ChannelCount = 1; 

        int _sampleRate = 44100;
        bool _isCapturing;
        bool _disposed;

        public bool IsCapturing
        {
            get { return _isCapturing; }
        }

        public int SampleRate
        {
            get { return _sampleRate; }
            set 
            {
                if (_sampleRate <= 0) throw new ArgumentOutOfRangeException();

                EnsureIdle();

                _sampleRate = value; 
            }
        }
        
        Capture _capture;
        CaptureBuffer _buffer;
        Notify _notify;
        int _bufferLength;
        readonly AutoResetEvent _positionEvent;
        readonly SafeWaitHandle _positionEventHandle;
        readonly ManualResetEvent _terminated;
        Thread _thread;
        readonly SoundCaptureDevice _device;

        protected SoundCaptureBase()
            : this(SoundCaptureDevice.GetDefaultDevice())
        {

        }

        protected SoundCaptureBase(SoundCaptureDevice device)
        {
            _device = device;

            _positionEvent = new AutoResetEvent(false);
            _positionEventHandle = _positionEvent.SafeWaitHandle;
            _terminated = new ManualResetEvent(true);
        }

        private void EnsureIdle()
        {
            if (IsCapturing)
                throw new SoundCaptureException("Capture is in process");
        }

        /// <summary>
        /// Starts capture process.
        /// </summary>
        public void Start()
        {
            EnsureIdle();

            _isCapturing = true;

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

            _capture = new Capture(_device.Id);
            _buffer = new CaptureBuffer(desciption, _capture);

            int waitHandleCount = BufferSeconds * NotifyPointsInSecond;
            BufferPositionNotify[] positions = new BufferPositionNotify[waitHandleCount];
            for (int i = 0; i < waitHandleCount; i++)
            {
                BufferPositionNotify position = new BufferPositionNotify
                {
                    Offset = (i + 1)*_bufferLength/positions.Length - 1,
                    EventNotifyHandle = _positionEventHandle.DangerousGetHandle()
                };
                positions[i] = position;
            }

            _notify = new Notify(_buffer);
            _notify.SetNotificationPositions(positions);

            _terminated.Reset();
            _thread = new Thread(ThreadLoop) {Name = "Sound capture"};
            _thread.Start();
            //ThreadLoop();
        }

        private void ThreadLoop()
        {
            _buffer.Start(true);
            try
            {
                int nextCapturePosition = 0;
                WaitHandle[] handles = { _terminated, _positionEvent };
                while (WaitHandle.WaitAny(handles) > 0)
                {
                    int capturePosition, readPosition;
                    _buffer.GetCurrentPosition(out capturePosition, out readPosition);

                    int lockSize = readPosition - nextCapturePosition;
                    if (lockSize < 0) lockSize += _bufferLength;
                    if((lockSize & 1) != 0) lockSize--;

                    int itemsCount = lockSize >> 1;

                    short[] data = (short[])_buffer.Read(nextCapturePosition, typeof(short), LockFlag.None, itemsCount);
                    ProcessData(data);
                    nextCapturePosition = (nextCapturePosition + lockSize) % _bufferLength;
                }
            }
            finally
            {
                _buffer.Stop();
            }
        }

        /// <summary>
        /// Processes the captured data.
        /// </summary>
        /// <param name="data">Captured data</param>
        protected abstract void ProcessData(short[] data);

        /// <summary>
        /// Stops capture process.
        /// </summary>
        public void Stop()
        {
            if (_isCapturing)
            {
                _isCapturing = false;

                _terminated.Set();
                _thread.Join();

                _notify.Dispose();
                _buffer.Dispose();
                _capture.Dispose();
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        ~SoundCaptureBase()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;
            GC.SuppressFinalize(this);
            if (IsCapturing) Stop();
            _positionEventHandle.Dispose();
            _positionEvent.Close();
            _terminated.Close();            
        }
    }
}
