using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellpers
{
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

    public class ListeningChangedEventArgs : EventArgs
    {
        public bool IsListnening { get; }

        public ListeningChangedEventArgs(bool listnening)
        {
            IsListnening = listnening;
        }
    }
}
