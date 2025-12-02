namespace Fanex.NAP.CTS.BIAlertService.Features.Alert
{
    using Fanex.NAP.CTS.BIAlertService.Features.NotificationChannel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAlertService
    {
        Task NotifyRocketChatAsync(StringBuilder sbContent, string channel);

        Task NotifySlackAsync(StringBuilder sbContent, string serviceIntro);
    }

    public class AlertService : IAlertService
    {
        private readonly IEnumerable<INotificationChannel> _notificationChannels;

        public AlertService(IEnumerable<INotificationChannel> notificationChannels)
        {
            _notificationChannels = notificationChannels ?? throw new ArgumentNullException(nameof(notificationChannels));
        }

        public async Task NotifyRocketChatAsync(StringBuilder sbContent, string channel)
        {
            if (sbContent == null)
            {
                throw new ArgumentNullException(nameof(sbContent));
            }
          
            var rocketChatChannel = _notificationChannels.FirstOrDefault(x => x.CanHandle("RocketChat"));

            if (rocketChatChannel == null)
            {
                throw new InvalidOperationException("RocketChat notification_channel is not registered");
            }

            await rocketChatChannel.SendAsync(sbContent.ToString(), channel);
        }

        public async Task NotifySlackAsync(StringBuilder sbContent, string serviceIntro)
        {
            if (sbContent == null)
            {
                throw new ArgumentNullException(nameof(sbContent));
            }

            var slackChannel = _notificationChannels.FirstOrDefault(x => x.CanHandle("Slack"));

            if (slackChannel == null)
            {
                throw new InvalidOperationException("Slack notification_channel is not registered");
            }

            await slackChannel.SendAsync(sbContent.ToString(), serviceIntro);
        }
    }
}
