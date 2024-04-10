using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransientServiceWithServiceFactory;

namespace TrainingProjectFile
{
    public class BackgroundTickerService : BackgroundService
    {
        private readonly ITickerServiceFactory _tickerServiceFactory;

        public BackgroundTickerService(ITickerServiceFactory tickerServiceFactory)
        {
            _tickerServiceFactory = tickerServiceFactory;
            Console.WriteLine($"BackgroundTickerService created with tickerServiceFactory {(tickerServiceFactory as TickerServiceFactory)?.Id}.");

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("BackgroundTickerService ExecuteAsync started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"BackgroundTickerService ExecuteAsync started {DateTime.Now.TimeOfDay}.");

                var currentTime = DateTime.Now.TimeOfDay;
                var tickerService = _tickerServiceFactory.CreateTickerService();
                tickerService.OnTick(currentTime);
                await Task.Delay(1000, stoppingToken);

                Console.WriteLine($"BackgroundTickerService ExecuteAsync finished {DateTime.Now.TimeOfDay}.");

            }
        }
    }
}
