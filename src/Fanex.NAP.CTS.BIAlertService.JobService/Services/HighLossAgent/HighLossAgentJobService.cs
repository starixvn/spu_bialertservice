namespace Fanex.NAP.CTS.BIAlertService.JobService.Services.HighLossAgent
{
    using Fanex.NAP.CTS.BIAlertService.Features.Alert;
    using Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent;  
    using Fanex.NAP.CTS.BIAlertService.Shares;
    using Fanex.NAP.CTS.Core.Extentions;    
    using System;   
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHighLossAgentJobService
    {
        Task<bool> Execute(RuleHighLossAgent rule, int daysAgo, int retryTimesCheckDataReady, string serviceName);
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

        public async Task<bool> Execute(RuleHighLossAgent rule, int daysAgo, int retryTimesCheckDataReady, string serviceName)
        {
            try
            {
                var gmtMinus4 = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(-4), DateTimeKind.Unspecified)
                    .AddDays(-daysAgo);

                var gmtPlus7 = DateTime.SpecifyKind(DateTime.UtcNow.AddHours(7), DateTimeKind.Unspecified);

                if (IsReadyAlertHighLossAgent(rule, gmtMinus4, retryTimesCheckDataReady))
                {
                    var highLossAlert = _highLossAgentService.GetAlertHighLossAgent(rule, gmtMinus4, _topAgent);

                    var intro = GetIntro(rule, gmtPlus7);
                    
                    var rcContent = BuildRCContent(highLossAlert, intro);
                    _ = _alertService.NotifyRocketChatAsync(rcContent, _appSettings.RocketChat.Channel);

                    var serviceIntro = highLossAlert.BuildSlackIntro(serviceName);
                    var slackContent = BuildSlackContent(highLossAlert);
                    _ = _alertService.NotifySlackAsync(slackContent, serviceIntro);
                }
            }
            catch (Exception ex)
            {
                LogExtension.LogError(ex, serviceName);

                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        private bool IsReadyAlertHighLossAgent(RuleHighLossAgent rule, DateTime checkDate, int retryTimesCheckDataReady)
        {
            var countRetry = 0;

            while (countRetry <= retryTimesCheckDataReady)
            {
                var isDataReady = _highLossAgentService.IsReadyAlertHighLossAgent(rule, checkDate);

                if (isDataReady)
                {
                    return true; 
                }

                countRetry++;

                Thread.Sleep(Consts.SLEEPTIME_HIGHLOSSAGENTCHECKDATAREADY);
            }

            return false;
        }

        private StringBuilder BuildRCContent(HighLossAgentAlertModel highLossAlert, string intro)
        {
            var contentBuilder = new StringBuilder();

            contentBuilder.AppendLine(intro);

            var sbContent = highLossAlert.BuildRCContent(contentBuilder);

            var moreDetailUrl = string.Format($"{Consts.LINK_EMOJI} {Consts.MORE_DETAILS_REFER_POWERBIUrl}", _appSettings.RocketChat.PowerBIHighLossAgentUrl);

            sbContent.AppendLine(moreDetailUrl);

            return sbContent;
        }

        private StringBuilder BuildSlackContent(HighLossAgentAlertModel highLossAlert)
        {
            var contentBuilder = new StringBuilder();

            var sbContent = highLossAlert.BuildSlackContent(contentBuilder);

            return sbContent;
        }

        private string GetIntro(RuleHighLossAgent rule, DateTime gmtPlus7)
        {
            switch (rule)
            {
                case RuleHighLossAgent.RuleID_HighLossAgent3Days:
                    return string.Format($"{Consts.FIRE_EMOJI} {Consts.HIGHLOSSAGENTS3DAYS_INTRO}", gmtPlus7.ToString(Consts.CONVERT_TO_DATETIME_MMddyyyyHHmmss));                   
                case RuleHighLossAgent.RuleID_HighLossAgent7Days:
                    return string.Format($"{Consts.FIRE_EMOJI} {Consts.HIGHLOSSAGENTS7DAYS_INTRO}", gmtPlus7.ToString(Consts.CONVERT_TO_DATETIME_MMddyyyyHHmmss));                 
                default:
                    return string.Empty;
            }            
        }       
    }
}
