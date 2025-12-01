namespace Fanex.NAP.CTS.BIAlertService.Features.RocketChat
{
    public class RCConfigModel
    {     
        public string ApiBaseURL { get; set; }

        public string Channel { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PowerBIHighLossAgentUrl { get; set; }

        public RCEndpointModel Endpoint { get; set; }
    }
}
