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

        public StringBuilder BuildRCContent(StringBuilder contentBuilder)
        {            
            contentBuilder.AppendLine($"{Consts.WARNING_EMOJI} {Consts.DATA_RANGE_LABEL}: " +
                $"{FromDate.ToString(Consts.CONVERT_TO_DATE_MMddyyyy)} " +
                $"- {ToDate.ToString(Consts.CONVERT_TO_DATE_MMddyyyy)}");

            contentBuilder.AppendLine($"{Consts.SOS_EMOJI} {Consts.AFFECTED_AGENTS_LABEL}: {TotalCust}");

            var topUsernames = string.Join(", ", TopCust.Select(x => x.UserName));
            contentBuilder.AppendLine($"{Consts.TOP_5_LABEL}: {topUsernames}");

            return contentBuilder;
        }

        public string BuildSlackIntro(string serviceName)
        {
            return $"{serviceName} ({FromDate.ToString(Consts.CONVERT_TO_DATE_MMddyyyy)} " +
                $"- {ToDate.ToString(Consts.CONVERT_TO_DATE_MMddyyyy)})";
        }

        public StringBuilder BuildSlackContent(StringBuilder contentBuilder)
        {
            contentBuilder.AppendLine($" {Consts.AFFECTED_AGENTS_LABEL}: {TotalCust}");

            return contentBuilder;
        }
    }
}
