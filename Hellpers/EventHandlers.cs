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
}
