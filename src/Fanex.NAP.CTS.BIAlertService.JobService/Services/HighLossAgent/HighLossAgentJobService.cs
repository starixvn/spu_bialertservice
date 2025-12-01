namespace Fanex.NAP.CTS.BIAlertService.JobService.Services.HighLossAgent
{
    using Fanex.NAP.CTS.BIAlertService.Features.Alert;
    using Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent;
    using Fanex.NAP.CTS.BIAlertService.Features.RocketChat;
    using Fanex.NAP.CTS.BIAlertService.Shares;
    using Fanex.NAP.CTS.Core.Extentions;
    using Newtonsoft.Json;
    using System;
    using System.IO.Pipes;
    using System.Text;
    using System.Threading.Tasks;

    public interface IHighLossAgentJobService
    {
        Task<bool> Execute(RuleHighLossAgent rule, int daysAgo, string serviceName);
    }

    public class HighLossAgentJobService : IHighLossAgentJobService
    {
        private readonly int _topAgent = 5;
        private readonly IHighLossAgentService _highLossAgentService;
        private readonly IAlertService _alertService;
        private readonly IAppSettings _appSettings;      

        public HighLossAgentJobService(IHighLossAgentService highLossAgentService
            , IAlertService alertService
            , IAppSettings appSettings) 
        {           
            _highLossAgentService = highLossAgentService;
            _alertService = alertService;
            _appSettings = appSettings;
        }

        public async Task<bool> Execute(RuleHighLossAgent rule, int daysAgo, string serviceName)
        {
            try
            {
                var gmtMinus4 = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(-4), DateTimeKind.Unspecified)
                    .AddDays(-daysAgo);                

                if (_highLossAgentService.IsReadyAlertHighLossAgent(rule, gmtMinus4))
                {
                    var highLossAlert = _highLossAgentService.GetAlertHighLossAgent(rule, gmtMinus4, _topAgent);

                    var intro = GetIntro(rule, gmtMinus4.AddHours(11));
                    
                    var content = BuildContent(highLossAlert, intro);

                    _ = _alertService.NotifyAsync(content, _appSettings.RocketChat.Channel);
                }
            }
            catch (Exception ex)
            {
                LogExtension.LogError(ex, serviceName);

                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        private StringBuilder BuildContent(HighLossAgentAlertModel highLossAlert, string intro)
        {
            var contentBuilder = new StringBuilder();

            contentBuilder.AppendLine(intro);

            var sbContent = highLossAlert.BuildContent(contentBuilder);

            var moreDetailUrl = string.Format($"{Constants.LINK_EMOJI} {Constants.MORE_DETAILS_REFER_POWERBIUrl}", _appSettings.RocketChat.PowerBIHighLossAgentUrl);

            sbContent.AppendLine(moreDetailUrl);

            return sbContent;
        }

        private string GetIntro(RuleHighLossAgent rule, DateTime gmtPlus7)
        {
            switch (rule)
            {
                case RuleHighLossAgent.RuleID_HighLossAgent3Days:
                    return string.Format($"{Constants.FIRE_EMOJI} {Constants.HIGHLOSSAGENTS3DAYS_INTRO}", gmtPlus7.ToString(Constants.CONVERT_TO_DATETIME_MMddyyyyHHmmss));                   
                case RuleHighLossAgent.RuleID_HighLossAgent7Days:
                    return string.Format($"{Constants.FIRE_EMOJI} {Constants.HIGHLOSSAGENTS7DAYS_INTRO}", gmtPlus7.ToString(Constants.CONVERT_TO_DATETIME_MMddyyyyHHmmss));                 
                default:
                    return string.Empty;
            }            
        }        
    }
}
