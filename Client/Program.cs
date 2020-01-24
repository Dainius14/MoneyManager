using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Client;
using System;
using System.Threading.Tasks;

namespace MoneyManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            webHost.Run();

            Task.Run(() => webHost.Services.GetRequiredService<Bootstrapper>().InitAsync());
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
}
