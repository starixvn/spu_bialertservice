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


                }
            }
            catch (Exception ex)
            {
                LogExtension.LogError(ex, serviceName);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
