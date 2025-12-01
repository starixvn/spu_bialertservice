namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class GetHighLossAgentCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "RuleID")]
        public int RuleID { get; set; }

        [SpParam(SqlParamName = "CaptureDate")]
        public DateTime CaptureDate { get; set; }

        [SpParam(SqlParamName = "BatchSize")]
        public int BatchSize { get; set; }

        [SpParam(SqlParamName = "FromDate")]
        public DateTime FromDate { get; set; } // output

        [SpParam(SqlParamName = "ToDate")]
        public DateTime ToDate { get; set; } // output

        [SpParam(SqlParamName = "TotalCust")]
        public int TotalCust { get; set; } // output

        public override string GetSettingKey()
        {
            return "CTS_BIAlert_GetCustByRuleID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
