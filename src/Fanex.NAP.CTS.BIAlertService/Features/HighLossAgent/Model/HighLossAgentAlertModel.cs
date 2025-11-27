namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{   
    public class HighLossAgentAlertModel
    {
        public string Title { get; set; }

        public string DateRange { get; set; }

        public int AffectAgents { get; set; }

        public string Top5Agents { get; set; }
    }
}
