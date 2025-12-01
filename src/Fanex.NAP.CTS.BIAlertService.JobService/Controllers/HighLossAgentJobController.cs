namespace Fanex.NAP.CTS.BIAlertService.JobService.Controllers
{
    using Fanex.NAP.CTS.BIAlertService.JobService.Services.HighLossAgent;
    using Fanex.NAP.CTS.BIAlertService.Shares;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/HighLossAgentJobs")]
    public class HighLossAgentJobController : ControllerBase
    {
        private readonly IHighLossAgentJobService _highLossAgentJobService;

        public HighLossAgentJobController(IHighLossAgentJobService highLossAgentJobService) 
        {
            _highLossAgentJobService = highLossAgentJobService;
        }

        [HttpPost("DailyAlertHighLossAgent3Days")]
        public async Task<bool> DailyAlertHighLossAgent3Days([FromForm] int daysAgo)
        {
            return await _highLossAgentJobService.Execute(RuleHighLossAgent.RuleID_HighLossAgent3Days, daysAgo, nameof(DailyAlertHighLossAgent3Days));
        }

        [HttpPost("DailyAlertHighLossAgent7Days")]
        public async Task<bool> DailyAlertHighLossAgent7Days([FromForm] int daysAgo)
        {
            return await _highLossAgentJobService.Execute(RuleHighLossAgent.RuleID_HighLossAgent7Days, daysAgo, nameof(DailyAlertHighLossAgent7Days));
        }

    }
}
