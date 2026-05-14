using Auth.Api.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

            services.AddDbContextFactory<AuthDbContext>((serviceProvider, options) =>
            {
                //var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

                var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                if (!env.IsDevelopment())
                {
                    var cs = GetConnectionStringFromSecret();
                    //if (logger != null)
                    //{
                    //    logger.LogInformation("connection string: " + cs);
                    //}
                    options.UseNpgsql(cs);
                }
                else
                {
                    options.UseNpgsql(dbOptions.ConnectionString);
                }
            });

            return services;
        }

        private static string GetConnectionStringFromSecret()
        {
            var secretsPath = "/run/secrets";
            var ipFile = Path.Combine(secretsPath, "auth_connection_string");
            if (File.Exists(ipFile))
            {
                return File.ReadAllText(ipFile).Trim();
            }
            return "";
        }
    }
}
