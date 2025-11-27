namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using System;
    using System.Collections.Generic;    

    public interface IHighLossAgentService
    {
        bool IsReadyAlertHighLossAgent(int days);

        IEnumerable<HighLossAgentAlertEntity> GetAlertHighLossAgent(int days);

        bool AlertViaRocketChat(HighLossAgentAlertModel highLossAgent);
    }

    public class HighLossAgentService : IHighLossAgentService
    {
        private readonly IHighLossAgentDataAccess _dataAccess;

        public HighLossAgentService(IHighLossAgentDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public bool IsReadyAlertHighLossAgent(int days)
        {
            return _dataAccess.IsReadyAlertHighLossAgent(days);
        }

        public IEnumerable<HighLossAgentAlertEntity> GetAlertHighLossAgent(int days)
        {
            return _dataAccess.GetAlertHighLossAgent(days);
        }

        public bool AlertViaRocketChat(HighLossAgentAlertModel highLossAgent)
        {
            return false;
        }
    }
}
