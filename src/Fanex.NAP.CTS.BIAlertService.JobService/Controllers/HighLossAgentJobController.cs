namespace Fanex.NAP.CTS.BIAlertService.JobService.Controllers
{
    using Fanex.NAP.CTS.BIAlertService.JobService.Services.HighLossAgent;
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

        [HttpPost("DailyAlertHighLossAgent")]
        public async Task<bool> DailyAlertHighLossAgent([FromForm] int days)
        {
            return await _highLossAgentJobService.Execute(days);
        }

    }
}
