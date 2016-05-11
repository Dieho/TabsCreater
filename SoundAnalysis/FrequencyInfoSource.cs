using System;

namespace SoundAnalysis
{
    public abstract class FrequencyInfoSource
    {
        public abstract void Listen();
        public abstract void Stop();

        public event EventHandler<FrequencyDetectedEventArgs> FrequencyDetected;

        protected void OnFrequencyDetected(FrequencyDetectedEventArgs e)
        {
            if (FrequencyDetected != null)
            {
                FrequencyDetected(this, e);
            }
        }
    }

    public class FrequencyDetectedEventArgs : EventArgs
    {
        readonly double _frequency;

        public double Frequency
        {
            get { return _frequency; }
        }

        public FrequencyDetectedEventArgs(double frequency)
        {
            _frequency = frequency;
        }
    }
}
