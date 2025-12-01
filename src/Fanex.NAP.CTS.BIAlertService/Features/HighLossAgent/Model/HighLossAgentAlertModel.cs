namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.NAP.CTS.BIAlertService.Shares;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class HighLossAgentAlertModel
    {
        public HighLossAgentAlertModel() 
        {
            TopCust = Enumerable.Empty<HighLossAgentTopCustEntity>();          
        }

        public IEnumerable<HighLossAgentTopCustEntity> TopCust { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int TotalCust { get; set; }

        public StringBuilder BuildContent(StringBuilder contentBuilder)
        {            
            contentBuilder.AppendLine($"{Constants.WARNING_EMOJI} {Constants.DATA_RANGE_LABEL}: " +
                $"{FromDate.ToString(Constants.CONVERT_TO_DATE_MMddyyyy)} " +
                $"- {ToDate.ToString(Constants.CONVERT_TO_DATE_MMddyyyy)}");

            contentBuilder.AppendLine($"{Constants.SOS_EMOJI} {Constants.AFFECTED_AGENTS_LABEL}: {TotalCust}");

            var topUsernames = string.Join(",", TopCust.Select(x => x.UserName));
            contentBuilder.AppendLine($"{Constants.TOP_5_LABEL}: {topUsernames}");

            return contentBuilder;
        }
    }
}
