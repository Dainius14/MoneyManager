using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyManager.Core.Data;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Services;

namespace MoneyManager.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<DapperDbContext>(opt =>
                opt.ConnectionString = Configuration.GetConnectionString("MoneyContext")
            );

            services.AddScoped<IUnitOfWork, DapperUnitOfWork>();
            //services.AddScoped<ICategoryRepository, DapperCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<IAccountRepository, DapperAccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<ITransactionRepository, DapperTransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            //services.AddScoped<ITransactionDetailsRepository, DapperTransactionDetailsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:5000", "https://localhost:5001")
                       .WithMethods("GET", "POST", "PUT", "DELETE")
                       .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
