using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MoneyManager.Client;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Reducers;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MoneyManager
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.AddConfiguration();
            builder.Services.ConfigureServices();
            builder.RootComponents.Add<App>("app");
            
            var host = builder.Build();
            await host.RunAsync();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddOptions();

            var initialState = new AppState();
            var store = new Store<AppState>(RootReducer.Reducer, initialState);
            services.AddSingleton(store);

            services.AddScoped<Bootstrapper>();

            services.AddScoped<ModalService>();
            services.AddScoped<MessageService>();


            services.AddAuthorizationCore();
            services.AddScoped<JwtAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<JwtAuthStateProvider>()
            );
            services.AddScoped<HttpClient>();
            services.AddScoped<AuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<LocalStorage>();
            services.AddScoped<SessionStorage>();

            services.AddScoped<AccountService>();
            services.AddScoped<CurrencyService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<TransactionService>();
        }

        private static void AddConfiguration(this WebAssemblyHostBuilder builder)
        {
            builder.Configuration
                .AddJsonFile(
                    provider: new EmbeddedFileProvider(Assembly.GetExecutingAssembly()),
                    path: "appsettings.json",
                    optional: true,
                    reloadOnChange: false
                );
        }
    }
}
