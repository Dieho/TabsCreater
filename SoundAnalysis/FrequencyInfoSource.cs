using System;
using Hellpers;

namespace SoundAnalysis
{
    public abstract class FrequencyInfoSource
    {

        public abstract void Listen();
        public abstract void Stop();

        public event EventHandler<FrequencyDetectedEventArgs> FrequencyDetected;

        public void OnFrequencyDetected(FrequencyDetectedEventArgs e)
        {
            if (FrequencyDetected != null)
            {
                FrequencyDetected(this, e);
            }
        }
    }
}
