namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class IsReadyAlertHighLossAgentCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "RuleID")]
        public int RuleID { get; set; }

        [SpParam(SqlParamName = "CheckDate")]
        public DateTime CheckDate { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_BIAlert_GetRuleIDStatus";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
