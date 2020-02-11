using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Client;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Reducers;
using System.Threading.Tasks;

namespace MoneyManager
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("app");
            ConfigureServices(builder.Services);

            
            var host = builder.Build();
            //host.Configuration.
            
            await host.RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
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
    }
}
