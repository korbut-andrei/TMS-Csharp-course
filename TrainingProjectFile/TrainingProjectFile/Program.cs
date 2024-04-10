using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrainingProjectFile;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting the application...");

        IHost host = CreateHostBuilder(args).Build();
        Console.WriteLine("Host created.");


        await host.RunAsync();
        Console.WriteLine("Host has finished running.");

    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                Console.WriteLine("Configuring services...");

                services.AddSingleton<TickerService>();
                Console.WriteLine("TickerService registered.");

                services.AddSingleton<BackgroundTickerService>();
                Console.WriteLine("BackgroundTickerService registered.");

            })
            .UseConsoleLifetime(); // This ensures graceful shutdown for console apps
}
