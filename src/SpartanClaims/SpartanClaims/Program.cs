using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SpartanClaims.Configurations;
using SpartanClaims.Infrastructure.Services;
using System.Threading.Tasks;

namespace SpartanClaims
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureServices((appBuilder, services) =>
                {
                    var configuration = appBuilder.Configuration;

                    ClaimsConfigurations claimsConfig = configuration.GetSection("SpartanClaims").Get<ClaimsConfigurations>();

                    services.AddOptions<ClaimsConfigurations>()
                        .Configure<IConfiguration>((settings, configuration) =>
                        {
                            configuration.GetSection("SpartanClaims").Bind(settings);
                        });

                    services.AddDbContext<ClaimsDbContext>(options =>
                    {
                        options.UseSqlServer(claimsConfig.ClaimsDbConnection);
                    });

                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowAllOrigins",
                            builder =>
                            {
                                builder
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                            });
                    });
                })
                .ConfigureOpenApi()
                .Build();

            host.Run();
        }

        public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
        {
            public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
            {
                Version = GetOpenApiDocVersion(),
                Title = GetOpenApiDocTitle()
            };

            public override OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();
        }
    }
}