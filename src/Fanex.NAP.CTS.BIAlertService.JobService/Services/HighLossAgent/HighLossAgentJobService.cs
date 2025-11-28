namespace Fanex.NAP.CTS.BIAlertService.JobService.Services.HighLossAgent
{
    using Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent;
    using Fanex.NAP.CTS.Core.Extentions;
    using System;
    using System.Threading.Tasks;

    public interface IHighLossAgentJobService
    {
        Task<bool> Execute(int days);
    }

    public class HighLossAgentJobService : IHighLossAgentJobService
    {
        private readonly string serviceName;
        private readonly IHighLossAgentService _highLossAgentService;

        public HighLossAgentJobService(IHighLossAgentService highLossAgentService) 
        {
            serviceName = "High-Loss Agent Alert";
            _highLossAgentService = highLossAgentService;
        }

        public Task<bool> Execute(int days)
        {
            try
            {
                if (_highLossAgentService.IsReadyAlertHighLossAgent(days))
                {
                    _highLossAgentService.GetAlertHighLossAgent(days);

                    _highLossAgentService.AlertViaRocketChat(GetAlertViaRocketChat_DataTest());
                }
            }
            catch (Exception ex)
            {
                LogExtension.LogError(ex, serviceName);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private HighLossAgentAlertModel GetAlertViaRocketChat_DataTest() 
        {
            return new HighLossAgentAlertModel
            {
                 Title = "High-loss Agents in 3 days - <Alert Timestamp>",
                 DateRange = "Data Range: 11/15/2025 - 11/17/2025",
                 AffectAgents = "Affected Agents: Total 77",
                 Top5Agents = "Agent1, Agent2, Agent3, Agent4, Agent5"
            };
        }
    }
}
