namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.Data.Repository;
    using Fanex.NAP.CTS.Core.Service;
    using System.Collections.Generic;
    using System.Linq;

    public interface IHighLossAgentDataAccess
    {
        bool IsReadyAlertHighLossAgent(int days);

        IEnumerable<HighLossAgentAlertEntity> GetAlertHighLossAgent(int days);
    }

    public class HighLossAgentDataAccess : IHighLossAgentDataAccess
    {        
        private readonly ICriteriaService _repository;

        public HighLossAgentDataAccess(ICriteriaService repository)
        {
            _repository = repository;
        }

        public bool IsReadyAlertHighLossAgent(int days)
        {
            var criteria = new IsReadyAlertHighLossAgentCriteria { ParamID = days };

            var result = _repository.Fetch<bool>(criteria);

            return result?.Any(x => x) == true;
        }

        public IEnumerable<HighLossAgentAlertEntity> GetAlertHighLossAgent(int days)
        {
            var criteria = new GetHighLossAgentCriteria { Days = days };
            
            return _repository.Fetch<HighLossAgentAlertEntity>(criteria);
        }
    }
}
