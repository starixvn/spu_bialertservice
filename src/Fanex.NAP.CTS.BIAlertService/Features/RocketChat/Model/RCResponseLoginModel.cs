namespace Fanex.NAP.CTS.BIAlertService.Features.RocketChat
{
    public class RCUser
    {
        public string userId { get; set; }

        public string authToken { get; set; }
    }

    public class RCResponseLoginModel
    {
        public string status { get; set; }

        public RCUser data { get; set; }
    }
}
