namespace Fanex.NAP.CTS.BIAlertService.JobService.Services
{    
    using Fanex.NAP.Client.AspNetCore.Session;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.CTS.BIAlertService.Features.Alert;
    using Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent;
    using Fanex.NAP.CTS.BIAlertService.Features.NotificationChannel;
    using Fanex.NAP.CTS.BIAlertService.Features.RocketChat;
    using Fanex.NAP.CTS.BIAlertService.JobService.Services.HighLossAgent;
    using Fanex.NAP.CTS.Core.Notification;
    using Fanex.NAP.CTS.Core.Service;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services, IAppSettings appSettings)
        {
            RegisterNAPService(services);

            RegisterRocketChatService(services, appSettings);

            RegisterDailyHighLossAgent(services, appSettings);
        }

        private static void RegisterNAPService(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<INapSession, NapSession>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddScoped<ICriteriaService, CriteriaService>();
        }

        public static void RegisterRocketChatService(IServiceCollection services, IAppSettings appSettings)
        {
            services.Configure<RCConfigModel>(options =>
            {
                var rocketChatConfig = appSettings.RocketChat;
                options.ApiBaseURL = rocketChatConfig.ApiBaseURL;
                options.Channel = rocketChatConfig.Channel;
                options.Username = rocketChatConfig.Username;
                options.Password = rocketChatConfig.Password;
                options.PowerBIHighLossAgentUrl = rocketChatConfig.PowerBIHighLossAgentUrl;
                options.Endpoint = rocketChatConfig.Endpoint;
            });

            services.AddSingleton<IRocketChatService, RocketChatService>();            
        }

        public static void RegisterDailyHighLossAgent(IServiceCollection services, IAppSettings appSettings)
        {
            services.AddScoped<IHighLossAgentDataAccess, HighLossAgentDataAccess>();
            services.AddScoped<IHighLossAgentService, HighLossAgentService>();
            services.AddScoped<IHighLossAgentJobService, HighLossAgentJobService>();

            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<INotificationChannel, RocketChatNotificationChannel>();
            services.AddSingleton<INotificationChannel>(sp =>
            {
                var notificationService = sp.GetRequiredService<INotificationService>();                        
                return new SlackNotificationChannel(notificationService, appSettings.LogGroupID, appSettings.EvironmentName);
            });

            services.AddSingleton<IAlertService, AlertService>();
        }
    }
}


