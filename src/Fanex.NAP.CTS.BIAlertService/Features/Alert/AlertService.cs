namespace Fanex.NAP.CTS.BIAlertService.Features.Alert
{
    using Fanex.NAP.CTS.BIAlertService.Features.RocketChat;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAlertService
    {
        Task NotifyAsync(StringBuilder sbContent, string channel);
    }

    public class AlertService : IAlertService
    {
        private readonly IRocketChatService _rocketChat;        

        public AlertService(IRocketChatService rocketChat)
        {
            _rocketChat = rocketChat ?? throw new ArgumentNullException(nameof(rocketChat));
        }

        public async Task NotifyAsync(StringBuilder sbContent, string channel)
        {           
            await _rocketChat.PostMessageAsync(new RCPostMessageModel
            {
                channel = channel,
                text = sbContent.ToString()
            });
        }
    }
}
