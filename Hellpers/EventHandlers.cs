using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellpers
{
    public class ListeningChangedEventHandler
    {
        public event EventHandler<ListeningChangedEventArgs> ListneningStatusChangedEventHandler;

        public void OnListneningStatusChanged(ListeningChangedEventArgs e)
        {
            if (ListneningStatusChangedEventHandler != null)
            {
                ListneningStatusChangedEventHandler(this, e);
            }
        }
    }

    public class FrequencyDetectedEventHandler
    {
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
