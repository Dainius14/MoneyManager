
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Core.Data;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.Services;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<DapperDbContext>(opt =>
                opt.ConnectionString = _configuration.GetConnectionString("MoneyContext")
            );

            services.AddScoped<AccountService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<DashboardService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<CsvTransactionImportService>();
            services.AddScoped<UserService>();
            services.AddScoped<CurrentUserService>();
            services.AddScoped<IUnitOfWork, DapperUnitOfWork>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            var appSetingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSetingsSection);

            var secret = Encoding.ASCII.GetBytes(_configuration["App:Secret"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<UserService>();
                        Claim? userIdClaim = context.Principal.FindFirst(ClaimTypes.NameIdentifier);

                        if (userIdClaim != null)
                        {
                            if (int.TryParse(userIdClaim.Value, out int userId))
                            {
                                if ((await userService.GetOne(userId)) != null)
                                {
                                    return;
                                }
                            }
                        }

                        context.Fail("Unauthorized");
                    }, 
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
                
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };

            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin()
                           .WithMethods("GET", "POST", "PUT", "DELETE")
                           .WithExposedHeaders("Token-Expired")
                           .AllowAnyHeader();
                });
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
