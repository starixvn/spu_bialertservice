namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.NAP.CTS.BIAlertService.Shares;
    using System;

    public interface IHighLossAgentService
    {
        bool IsReadyAlertHighLossAgent(RuleHighLossAgent rule, DateTime checkDate);

        HighLossAgentAlertModel GetAlertHighLossAgent(RuleHighLossAgent rule, DateTime captureDate, int batchSize);     
    }

    public class HighLossAgentService : IHighLossAgentService
    {
        private readonly IHighLossAgentDataAccess _dataAccess;

        public HighLossAgentService(IHighLossAgentDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public bool IsReadyAlertHighLossAgent(RuleHighLossAgent rule, DateTime checkDate)
        {
            return _dataAccess.IsReadyAlertHighLossAgent(rule, checkDate);
        }

        public HighLossAgentAlertModel GetAlertHighLossAgent(RuleHighLossAgent rule, DateTime captureDate, int batchSize)
        {
            return _dataAccess.GetAlertHighLossAgent(rule, captureDate, batchSize);
        }       
    }
}
