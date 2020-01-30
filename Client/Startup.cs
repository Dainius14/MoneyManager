using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Client;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Reducers;

namespace MoneyManager
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

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

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<Client.App>("app");
        }
    }
}
