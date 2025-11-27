namespace Fanex.NAP.CTS.BIAlertService.JobService.Services
{
    using Fanex.NAP.Client.AspNetCore.Session;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterNAPService(services);

            // Add your service registrations here
        }

        private static void RegisterNAPService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<INapSession, NapSession>();
            services.AddSingleton<ISecurityService, SecurityService>();
        }
    }
}

