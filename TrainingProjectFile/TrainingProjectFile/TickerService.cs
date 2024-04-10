using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingProjectFile
{
    public class TickerService
    {
        public event EventHandler<TickerArgs> Tick;

        public TickerService()
        {

            // Subscribe to tick events upon construction
            Tick += OnTickEverySecond;
            Tick += OnTickEvery5Seconds;
            Console.WriteLine("TickerService instantiated.");

        }

        public void OnTick(TimeSpan timeStamp)
        {
            Tick?.Invoke(this, new TickerArgs(timeStamp));
        }

        public void OnTickEverySecond(object sender, TickerArgs args)
        {
            Console.WriteLine($"Current Timestamp: {args.TimeStamp}");
        }

        public void OnTickEvery5Seconds(object sender, TickerArgs args)
        {
            if (args.TimeStamp.TotalSeconds % 5 == 0)
            {
                Console.WriteLine($"Every 5 Seconds Timestamp: {args.TimeStamp}");
            }
        }
    }
}
