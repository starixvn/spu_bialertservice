namespace Fanex.NAP.CTS.BIAlertService.JobService
{
    using Fanex.Data;
    using Fanex.Data.SqlServer;
    using Fanex.Logging.Sentry.RavenClient;   
    using Fanex.NAP.CTS.BIAlertService.JobService.Services;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

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
            var appSettings = new AppSettings(Configuration);

            services.AddFanexLogSentry(Configuration);
            services.AddSingleton<IAppSettings>(appSettings);            

            services.AddControllers();
            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fanex.NAP.CTS.BIAlertService.JobService", Version = "v1" });
            });

            services.RegisterService(appSettings);

            FanexDataConfig(appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Fanex.NAP.CTS.BIAlertService.JobService v1"));
            app.UseRouting();

            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });           
        }

        private void FanexDataConfig(IAppSettings appSettings)
        {
            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(Configuration)              
              .UseDefaultDbSettingProvider(resourcePath: appSettings.SPName)
              .Run();
        }
    }
}


