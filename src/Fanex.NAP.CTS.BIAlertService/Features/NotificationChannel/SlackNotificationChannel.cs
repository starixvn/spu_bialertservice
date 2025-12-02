namespace Fanex.NAP.CTS.BIAlertService.Features.NotificationChannel
{
    using Fanex.NAP.CTS.Core.Notification;
    using Fanex.NAP.CTS.Core.Notification.Model;
    using System;
    using System.Threading.Tasks;

    public class SlackNotificationChannel : INotificationChannel
    {
        private readonly INotificationService _notificationService;
        private readonly string _groupId;
        private readonly string _evironmentName;

        public SlackNotificationChannel(INotificationService notificationService, string groupId, string evironmentName)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

            if (string.IsNullOrWhiteSpace(groupId))
            {
                throw new ArgumentException("Group ID cannot be null or empty", nameof(groupId));
            }

            _groupId = groupId;
            _evironmentName = evironmentName;
        }

        public bool CanHandle(string channelType)
        {
            return string.Equals(channelType, "Slack", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> SendAsync(string content, string extent)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return false;
            }
          
            var result = _notificationService
                .Config(_groupId, _evironmentName)
                .Message(new MessageModel(extent, DateTimeOffset.Now, _evironmentName, content))
                .SendMessageInfo();

            return await Task.FromResult(result);
        }
    }
}