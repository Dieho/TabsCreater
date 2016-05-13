using System;
using System.Collections.Generic;
using System.Linq;
using Hellpers;
using SoundAnalysis;
using SoundCapture;

namespace FftGuitarTuner
{
    public class Listener
    {
        public SoundCaptureDevice Device;
        private static Listener _instance;
        private static object syncRoot = new Object();
        private Dictionary<string, SoundFrequencyInfoSource> frequencyInfoSourceList = new Dictionary<string, SoundFrequencyInfoSource>();

        protected Listener()
        {

        }

        public static Listener Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
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
            var exist = frequencyInfoSourceList.ContainsKey(callerClass.FullName);
            if (exist)
            {
                var source = frequencyInfoSourceList[callerClass.FullName];
                if (source.IsListening)
                {
                    source.Stop();
                    source.IsListening = false;
                    source.FrequencyDetected -= freqSource;
                }
            }
        }

        public void StartListenning(EventHandler<FrequencyDetectedEventArgs> freqSource, Type callerClass)
        {
            var exist = frequencyInfoSourceList.ContainsKey(callerClass.FullName);
            if (!exist)
            {
                var frequencyInfoSource = new SoundFrequencyInfoSource(Device);
                frequencyInfoSource.FrequencyDetected += freqSource;
                frequencyInfoSource.Listen();
                frequencyInfoSource.IsListening = true;
                frequencyInfoSourceList.Add(callerClass.FullName, frequencyInfoSource);
            }
            else
            {
                var source = frequencyInfoSourceList[callerClass.FullName];
                if (!source.IsListening)
                {
                    source.FrequencyDetected += freqSource;
                    source.Listen();
                    source.IsListening = true;
                }
            }
        }
    }
}
