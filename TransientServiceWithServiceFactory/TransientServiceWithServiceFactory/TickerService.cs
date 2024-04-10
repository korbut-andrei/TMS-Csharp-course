using EventsTestProject;
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

        private readonly TransientService _transientService;
        public TickerService(TransientService transientService)
        {

            // Subscribe to tick events upon construction
            _transientService = transientService;
            Tick += OnTickEverySecond;
            Tick += OnTickEvery5Seconds;
            Console.WriteLine("TickerService instantiated.");

        }

        public void OnTick(TimeSpan timeStamp)
        {
            Console.WriteLine(_transientService.Id);
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
