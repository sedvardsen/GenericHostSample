using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var lifeTime = host.Services.GetService<IHostApplicationLifetime>();

            lifeTime.ApplicationStopped.Register(ApplicationStopped);
            lifeTime.ApplicationStopping.Register(ApplicationStopping);
            lifeTime.ApplicationStarted.Register(ApplicationStart);

            host.Run();
        }
        private static void ApplicationStopping()
        {
            Console.WriteLine("STOPPING");
            File.WriteAllText($"{DateTime.Now:yyyy-dd-M-HH-mm-ss}-STOPPING", "ApplicationStopping was called");
        }

        private static void ApplicationStopped()
        {
            Console.WriteLine("STOP");
            File.WriteAllText($"{DateTime.Now:yyyy-dd-M-HH-mm-ss}-STOP", "ApplicationStopped was called");
        }

        private static void ApplicationStart()
        {
            Console.WriteLine("START");
            File.WriteAllText($"{DateTime.Now:yyyy-dd-M-HH-mm-ss}-START", "ApplicationStart was called");
        }


        static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddHostedService<Service>();
              });
    }

    public class Service : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Ping");
                await Task.Delay(10000, stoppingToken);
            }
        }
    }

}

