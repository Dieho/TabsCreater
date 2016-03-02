using SoundCapture;

namespace FftGuitarTuner
{
    class SoundFrequencyInfoSource : FrequencyInfoSource
    {
        readonly SoundCaptureDevice _device;
        Adapter _adapter;

        internal SoundFrequencyInfoSource(SoundCaptureDevice device)
        {
            _device = device;
        }

        public override void Listen()
        {
            _adapter = new Adapter(this, _device);
            _adapter.Start();
        }

        public override void Stop()
        {
            _adapter.Stop();
        }

        class Adapter : SoundCaptureBase
        {
            readonly SoundFrequencyInfoSource _owner;

            const double MinFreq = 60;
            const double MaxFreq = 1300;

            internal Adapter(SoundFrequencyInfoSource owner, SoundCaptureDevice device)
                : base(device)
            {
                _owner = owner;
            }

            protected override void ProcessData(short[] data)
            {
                double[] x = new double[data.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = data[i] / 32768.0;
                }
                double freq = FrequencyUtils.FindFundamentalFrequency(x, SampleRate, MinFreq, MaxFreq);
                _owner.OnFrequencyDetected(new FrequencyDetectedEventArgs(freq));
            }
        }
    }
}
