namespace Fanex.NAP.CTS.Core.Notification
{
    using Fanex.NAP.CTS.Core.Notification.Model;
    using RestSharp;
    using System.Net;
    using System.Text;

    public interface INotificationService
    {
        INotificationService Message(MessageModel messageModel);

        INotificationService Config(string groupId, string environmentString);

        bool SendMessageError();

        bool SendMessageInfo();
    }

    public class NotificationService : INotificationService
    {
        private MessageModel _messageModel;
        private string _token;
        private string _groupId;
        private string _apiUrl;
        private string _proxy;

        public INotificationService Message(MessageModel messageModel)
        {
            if (messageModel != null)
            {
                _messageModel = messageModel;
            }
            else
            {
                throw new ArgumentException("Parameter cannot be null", nameof(messageModel));
            }

            return this;
        }

        /// <summary>
        /// Config telegram api setting.
        /// </summary>     
        public INotificationService Config(string groupId, string environmentString)
        {
            _token = "0A40aOc7CtpzjbBSJu8KNWQRoXyjRvf9";
            _groupId = groupId;
            _apiUrl = "https://bot.nexdev.net/skynex/api/messages/forwardwithtoken";
            _proxy = new HashSet<string> { "KLDR" }.Contains(environmentString) ? "iusqd.gamora.prod:8801" : null;

            if (string.IsNullOrWhiteSpace(_groupId))
            {
                throw new ArgumentException("Group Id cannot be null", nameof(groupId));
            }

            return this;
        }

        public bool SendMessageError()
        {
            var message = new StringBuilder();

            message.Append("\U0001F525 ERROR \U0001F525 in `[`**" + _messageModel.SystemName + "**`]`");

            return SendMessage(message);
        }

        public bool SendMessageInfo()
        {
            var message = new StringBuilder();

            message.Append("✅ INFO ✅ in `[`**" + _messageModel.SystemName + "**`]`");

            return SendMessage(message);
        }

        private bool SendMessage(StringBuilder message)
        {
            message.Append("\n\r");
            message.Append("**Environment**: ");
            message.Append(_messageModel.Environment);
            message.Append("\n\r");
            message.Append("**Timestamp**: ");
            message.Append(_messageModel.Timestamp);
            message.Append("\n\r");
            message.Append("**Message**: ");
            message.Append(_messageModel.Message);

            if (!string.IsNullOrWhiteSpace(_messageModel.MoreInfo))
            {
                message.Append("\n\r");
                message.Append("**More Info**: ");
                message.Append(_messageModel.MoreInfo);
            }

            var options = new RestClientOptions(_apiUrl);

            if (!string.IsNullOrEmpty(_proxy))
            {
                var proxy = new WebProxy($"http://{_proxy}");
                options.Proxy = proxy;
            }

            var client = new RestClient(options);

            var request = new RestRequest()
                .AddQueryParameter("token", _token)
                .AddQueryParameter("message", message.ToString())
                .AddQueryParameter("conversationId", _groupId);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var response = client.ExecuteAsync(request, Method.Post).GetAwaiter().GetResult();
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}