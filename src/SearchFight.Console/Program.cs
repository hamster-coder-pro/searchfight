using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SearchFight.Console.Application;
using SearchFight.Console.DI;

namespace SearchFight.Console
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            System.Console.WriteLine("Started ...");

            IApplication application;
            try
            {
                var provider = Bootstrapper.Start();

                application = provider.GetRequiredService<IApplication>();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("Critical error during application initialization:");
#if DEBUG
                System.Console.WriteLine(exception);
#endif
                ExitApplication(args, 1); // exit code 1 means - error configuring the app...
                return;
            }

            await application.Execute(args);

            ExitApplication(args);
        }

        private static void ExitApplication(string[] args, int code = 0)
        {
            if (code == 0)
            {
                System.Console.WriteLine("Completed ...");
            }
            else
            {
                System.Console.WriteLine("Failed...");
            }

            if ((args != null) && args.Any(a => a == "--wait"))
            {
                System.Console.WriteLine("Press any key to close the app...");
                System.Console.ReadKey();
            }

            Environment.Exit(code);
        }
    }
}