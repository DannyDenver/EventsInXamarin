using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsInXamarin.Model;

namespace EventsInXamarin
{
    public class JobChangedEventArgs : EventArgs
    {
        public Job Job { get; set; }
    }
}
