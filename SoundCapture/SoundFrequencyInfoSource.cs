using System;
using System.Threading;
using Hellpers;
using SoundAnalysis;
using SoundCapture;

namespace FftGuitarTuner
{
    public class SoundFrequencyInfoSource : FrequencyInfoSource
    {
        public bool IsListening { get; set; }
        readonly SoundCaptureDevice _device;


        public SoundFrequencyInfoSource(SoundCaptureDevice device)
        {
            _device = device;
        }

        public override void Listen()
        {
            SoundCapturing.Device = _device;
            SoundCapturing.Start(this);
        }

        public override void Stop()
        {
            SoundCapturing.Stop(this);
        }
    }
}
