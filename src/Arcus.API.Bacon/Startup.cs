using Arcus.API.Bacon.Repositories;
using Arcus.API.Bacon.Repositories.Interfaces;
using Arcus.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arcus.API.Bacon
{
    public class Startup : ApiStartup
    {
        public const string ComponentName = "Bacon API";
        private string ApiName => $"Arcus - {ComponentName}";

        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            services.AddControllers(options => 
            {
                options.ReturnHttpNotAcceptable = true;
                options.RespectBrowserAcceptHeader = true;

                RestrictToJsonContentType(options);
                ConfigureJsonFormatters(options);

            });
            
            services.AddHealthChecks();
            services.AddCustomHttpCorrelation(options => options.UpstreamService.ExtractFromRequest = true);

            services.AddScoped<IBaconRepository, BaconRepository>();

            ConfigureOpenApiGeneration(ApiName, "Arcus.API.Bacon.Open-Api.xml", services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandling();
            app.UseCustomHttpCorrelation();
            app.UseRouting();
            app.UseCustomRequestTracking();

            ExposeOpenApiDocs(ApiName, app);
        }
    }
}
