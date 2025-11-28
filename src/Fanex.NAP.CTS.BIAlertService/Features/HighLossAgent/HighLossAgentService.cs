namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.NAP.CTS.Core.Helpers.Api;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        public async Task<bool> AlertViaRocketChat(HighLossAgentAlertModel highLossAgent)
        {
            var url = "";

            await ApiHelper.Post<string>(url, highLossAgent, 300);

            return false;
        }
    }
}
