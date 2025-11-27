namespace Fanex.NAP.CTS.BIAlertService.Features.HighLossAgent
{
    using Fanex.Data;
    using Fanex.Data.Repository;   

    public class IsReadyAlertHighLossAgentCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_ParamID")]
        public int ParamID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_HighLossAgent_SP1";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
