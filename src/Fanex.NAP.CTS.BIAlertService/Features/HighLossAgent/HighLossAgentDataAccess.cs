namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.NAP.CTS.BIAlertService.Shares;
    using Fanex.NAP.CTS.Core.Service;
    using System;
    using System.Linq;

    public interface IHighLossAgentDataAccess
    {
        bool IsReadyAlertHighLossAgent(RuleHighLossAgent rule, DateTime checkDate);

        HighLossAgentAlertModel GetAlertHighLossAgent(RuleHighLossAgent rule, DateTime captureDate, int batchSize);
    }

    public class HighLossAgentDataAccess : IHighLossAgentDataAccess
    {        
        private readonly ICriteriaService _repository;

        public HighLossAgentDataAccess(ICriteriaService repository)
        {
            _repository = repository;
        }

        public bool IsReadyAlertHighLossAgent(RuleHighLossAgent rule, DateTime checkDate)
        {
            var criteria = new IsReadyAlertHighLossAgentCriteria 
            {  
                RuleID = (int)rule, 
                CheckDate = checkDate
            };

            var result = _repository.Fetch<bool>(criteria);

            return result?.Any(x => x) == true;
        }

        public HighLossAgentAlertModel GetAlertHighLossAgent(RuleHighLossAgent rule, DateTime captureDate, int batchSize)
        {
            var criteria = new GetHighLossAgentCriteria 
            {  
                RuleID = (int)rule, 
                CaptureDate = captureDate, 
                BatchSize = batchSize 
            };

            var topCust = _repository.Fetch<HighLossAgentTopCustEntity>(criteria);

            return new HighLossAgentAlertModel
            {
                 TopCust = topCust,
                 FromDate = criteria.FromDate,
                 ToDate = criteria.ToDate,
                 TotalCust = criteria.TotalCust
            };
        }
    }
}
