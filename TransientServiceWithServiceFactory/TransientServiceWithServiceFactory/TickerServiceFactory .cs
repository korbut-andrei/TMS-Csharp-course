using EventsTestProject;
using TrainingProjectFile;

namespace TransientServiceWithServiceFactory
{
    public class TickerServiceFactory : ITickerServiceFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public int Id { get; set; }

        public TickerServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            Random rand = new Random();
            // Generate a random integer between 0 and 100 (inclusive)
            int randomNumber = rand.Next(0, 101);
            Id = randomNumber;
            Console.WriteLine($"TickerServiceFactory instance created with Id: {Id}");

        }

        public TickerService CreateTickerService()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var transientService = scope.ServiceProvider.GetRequiredService<TransientService>();
                return new TickerService(transientService);
            }
        }
    }
}
