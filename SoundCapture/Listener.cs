using System;
using System.Collections.Generic;
using FftGuitarTuner;
using Hellpers;

namespace SoundCapture
{
    public class Listener
    {
        public SoundCaptureDevice Device;
        private static Listener _instance;
        private static readonly object SyncRoot = new object();
        private readonly Dictionary<string, SoundFrequencyInfoSource> _frequencyInfoSourceList = new Dictionary<string, SoundFrequencyInfoSource>();

        protected Listener()
        {

        }

        public static Listener Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new Listener();
                    }
                }
                return _instance;
            }
        }

        public void StopListenning(EventHandler<FrequencyDetectedEventArgs> freqSource, Type callerClass)
        {
            var exist = _frequencyInfoSourceList.ContainsKey(callerClass.FullName ?? throw new InvalidOperationException());
            if (exist)
            {
                var source = _frequencyInfoSourceList[callerClass.FullName];
                if (source.IsListening)
                {
                    source.Stop();
                    source.IsListening = false;
                    source.FrequencyDetectedEventHandler.FrequencyDetected -= freqSource;
                }
            }
        }

        public void StartListenning(EventHandler<FrequencyDetectedEventArgs> freqSource, Type callerClass)
        {
            var exist = _frequencyInfoSourceList.ContainsKey(callerClass.FullName ?? throw new InvalidOperationException());
            if (!exist)
            {
                var frequencyInfoSource = new SoundFrequencyInfoSource(Device);
                frequencyInfoSource.FrequencyDetectedEventHandler.FrequencyDetected += freqSource;
                frequencyInfoSource.Listen();
                frequencyInfoSource.IsListening = true;
                _frequencyInfoSourceList.Add(callerClass.FullName, frequencyInfoSource);
            }
            else
            {
                var source = _frequencyInfoSourceList[callerClass.FullName];
                if (!source.IsListening)
                {
                    source.FrequencyDetectedEventHandler.FrequencyDetected += freqSource;
                    source.Listen();
                    source.IsListening = true;
                }
            }
        }
    }
}
