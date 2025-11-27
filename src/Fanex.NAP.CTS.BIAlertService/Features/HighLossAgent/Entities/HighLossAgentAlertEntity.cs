namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using System;       

    public class HighLossAgentAlertEntity
    {
        public long CustID { get; set; }

        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
