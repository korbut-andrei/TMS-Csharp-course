using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingProjectFile
{
    public class TickerArgs : EventArgs
    {
        public TimeSpan TimeStamp { get; }

        public TickerArgs(TimeSpan timeStamp)
        {
            TimeStamp = timeStamp;
        }
    }
}
