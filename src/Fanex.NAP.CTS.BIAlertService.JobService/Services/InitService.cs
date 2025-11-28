namespace Fanex.NAP.CTS.BIAlertService.JobService.Services
{    
    using Fanex.NAP.Client.AspNetCore.Session;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent;
    using Fanex.NAP.CTS.BIAlertService.JobService.Services.HighLossAgent;
    using Fanex.NAP.CTS.Core.Service;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterNAPService(services);

            RegisterDailyHighLossAgent(services);
        }

        private static void RegisterNAPService(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<INapSession, NapSession>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddScoped<ICriteriaService, CriteriaService>();
        }

        public static void RegisterDailyHighLossAgent(IServiceCollection services)
        {            
            services.AddScoped<IHighLossAgentDataAccess, HighLossAgentDataAccess>();
            services.AddScoped<IHighLossAgentService, HighLossAgentService>();
            services.AddScoped<IHighLossAgentJobService, HighLossAgentJobService>();
        }
    }
}


