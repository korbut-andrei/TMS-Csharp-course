using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrainingProjectFile
{
    public class BackgroundTickerService
    {
        private readonly TickerService _tickerService;

        public BackgroundTickerService(TickerService tickerService)
        {
            _tickerService = tickerService;
            Console.WriteLine("BackgroundTickerService instantiated with TickerService.");

        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("BackgroundTickerService ExecuteAsync started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"BackgroundTickerService ExecuteAsync started {DateTime.Now.TimeOfDay}.");

                var currentTime = DateTime.Now.TimeOfDay;
                _tickerService.OnTick(currentTime);
                await Task.Delay(1000, stoppingToken);

                Console.WriteLine($"BackgroundTickerService ExecuteAsync started {DateTime.Now.TimeOfDay}.");

            }
        }
    }
}
