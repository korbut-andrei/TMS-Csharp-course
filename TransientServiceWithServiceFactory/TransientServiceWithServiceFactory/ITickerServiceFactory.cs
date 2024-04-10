using TrainingProjectFile;

namespace TransientServiceWithServiceFactory
{
    public interface ITickerServiceFactory
    {
        TickerService CreateTickerService();

    }
}
