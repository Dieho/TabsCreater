using System;
using SoundAnalysis;
using SoundCapture;

namespace FftGuitarTuner
{
    public class Listener
    {
        public SoundCaptureDevice device;
        private static bool _isListenning;
        private static Listener instance;
        private static object syncRoot = new Object();
        private  FrequencyInfoSource _frequencyInfoSource;
        public FrequencyInfoSource FrequencyInfoSource => _frequencyInfoSource;

        protected Listener()
        {

        }

        public static Listener Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Listener();
                    }
                }
                return instance;
            }
        }

        public void StopListenning(EventHandler<FrequencyDetectedEventArgs> freqSource)
        {
            _isListenning = false;
            _frequencyInfoSource.Stop();
            _frequencyInfoSource.FrequencyDetected -= freqSource;
            _frequencyInfoSource = null;
        }

        public void StartListenning(EventHandler<FrequencyDetectedEventArgs> freqSource)
        {
            _isListenning = true;
            _frequencyInfoSource = new SoundFrequencyInfoSource(device);
            _frequencyInfoSource.FrequencyDetected += freqSource;
            _frequencyInfoSource.Listen();
        }
    }
}
