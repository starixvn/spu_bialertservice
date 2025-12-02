namespace Fanex.NAP.CTS.BIAlertService.Features.NotificationChannel
{
    using Fanex.NAP.CTS.BIAlertService.Features.RocketChat;
    using System;
    using System.Threading.Tasks;

    public interface INotificationChannel
    {
        Task<bool> SendAsync(string content, string extent);

        bool CanHandle(string channelType);
    }

    public class RocketChatNotificationChannel : INotificationChannel
    {
        private readonly IRocketChatService _rocketChatService;

        public RocketChatNotificationChannel(IRocketChatService rocketChatService)
        {
            _rocketChatService = rocketChatService ?? throw new ArgumentNullException(nameof(rocketChatService));
        }

        public bool CanHandle(string channelType)
        {
            return string.Equals(channelType, "RocketChat", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> SendAsync(string content, string extent)
        {
            return await _rocketChatService.PostMessageAsync(new RCPostMessageModel
            {
                channel = extent,
                text = content
            });
        }
    }
}
