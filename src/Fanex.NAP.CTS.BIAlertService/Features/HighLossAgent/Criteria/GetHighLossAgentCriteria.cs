namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetHighLossAgentCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_Days")]
        public int Days { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_HighLossAgent_SP2";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
