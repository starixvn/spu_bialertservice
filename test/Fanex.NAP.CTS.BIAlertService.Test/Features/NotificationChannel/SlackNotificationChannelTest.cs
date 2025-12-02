namespace Fanex.NAP.CTS.BIAlertService.Tests.Features.NotificationChannel
{
    using Fanex.NAP.CTS.BIAlertService.Features.NotificationChannel;
    using Fanex.NAP.CTS.Core.Notification;
    using Fanex.NAP.CTS.Core.Notification.Model;
    using NSubstitute;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class SlackNotificationChannelTest
    {
        private readonly INotificationService _notificationService;
        private readonly SlackNotificationChannel _slackNotificationChannel;
        private readonly string _groupId = "test-group-id";
        private readonly string _environmentName = "LOCAL";

        public SlackNotificationChannelTest()
        {
            _notificationService = Substitute.For<INotificationService>();
            _slackNotificationChannel = new SlackNotificationChannel(_notificationService, _groupId, _environmentName);
        }

        #region Constructor Tests
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
        {
            // Arrange
            var notificationService = Substitute.For<INotificationService>();
            var groupId = "test-group-id";
            var environmentName = "LOCAL";

            // Act
            var channel = new SlackNotificationChannel(notificationService, groupId, environmentName);

            // Assert
            Assert.NotNull(channel);
        }

        [Fact]
        public void Constructor_WithNullNotificationService_ShouldThrowArgumentNullException()
        {
            // Arrange
            var groupId = "test-group-id";
            var environmentName = "LOCAL";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new SlackNotificationChannel(null, groupId, environmentName));
        }

        [Fact]
        public void Constructor_WithNullGroupId_ShouldThrowArgumentException()
        {
            // Arrange
            var notificationService = Substitute.For<INotificationService>();
            string groupId = null;
            var environmentName = "LOCAL";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => new SlackNotificationChannel(notificationService, groupId, environmentName));

            Assert.Equal("Group ID cannot be null or empty (Parameter 'groupId')", exception.Message);
            Assert.Equal("groupId", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithEmptyGroupId_ShouldThrowArgumentException()
        {
            // Arrange
            var notificationService = Substitute.For<INotificationService>();
            var groupId = string.Empty;
            var environmentName = "LOCAL";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => new SlackNotificationChannel(notificationService, groupId, environmentName));

            Assert.Equal("Group ID cannot be null or empty (Parameter 'groupId')", exception.Message);
            Assert.Equal("groupId", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithWhitespaceGroupId_ShouldThrowArgumentException()
        {
            // Arrange
            var notificationService = Substitute.For<INotificationService>();
            var groupId = "   ";
            var environmentName = "LOCAL";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => new SlackNotificationChannel(notificationService, groupId, environmentName));

            Assert.Equal("Group ID cannot be null or empty (Parameter 'groupId')", exception.Message);
            Assert.Equal("groupId", exception.ParamName);
        }

        [Fact]
        public void Constructor_WithNullEnvironmentName_ShouldInitializeCorrectly()
        {
            // Arrange
            var notificationService = Substitute.For<INotificationService>();
            var groupId = "test-group-id";
            string environmentName = null;

            // Act
            var channel = new SlackNotificationChannel(notificationService, groupId, environmentName);

            // Assert
            Assert.NotNull(channel);
        }
        #endregion

        #region CanHandle Tests
        [Fact]
        public void CanHandle_WithSlack_ShouldReturnTrue()
        {
            // Arrange
            var channelType = "Slack";

            // Act
            var result = _slackNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_WithSlackLowerCase_ShouldReturnTrue()
        {
            // Arrange
            var channelType = "slack";

            // Act
            var result = _slackNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_WithSlackMixedCase_ShouldReturnTrue()
        {
            // Arrange
            var channelType = "SlAcK";

            // Act
            var result = _slackNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_WithRocketChat_ShouldReturnFalse()
        {
            // Arrange
            var channelType = "RocketChat";

            // Act
            var result = _slackNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanHandle_WithEmptyString_ShouldReturnFalse()
        {
            // Arrange
            var channelType = string.Empty;

            // Act
            var result = _slackNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanHandle_WithNull_ShouldReturnFalse()
        {
            // Arrange
            string channelType = null;

            // Act
            var result = _slackNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region SendAsync Tests
        [Fact]
        public async Task SendAsync_WithValidContent_ShouldCallConfigMessageAndSendMessageInfo()
        {
            // Arrange
            var content = "Test message content";
            var extent = "test-system";
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            _notificationService.Received(1).Config(_groupId, _environmentName);
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x =>
                    x.SystemName == extent &&
                    x.Environment == _environmentName &&
                    x.Message == content));
            _notificationService.Received(1).SendMessageInfo();
        }

        [Fact]
        public async Task SendAsync_WithValidContent_ShouldReturnTrue()
        {
            // Arrange
            var content = "Test message content";
            var extent = "test-system";
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            var result = await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendAsync_WhenSendMessageInfoReturnsFalse_ShouldReturnFalse()
        {
            // Arrange
            var content = "Test message content";
            var extent = "test-system";
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(false);

            // Act
            var result = await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SendAsync_WithEmptyContent_ShouldReturnFalse()
        {
            // Arrange
            var content = string.Empty;
            var extent = "test-system";

            // Act
            var result = await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            Assert.False(result);
            _notificationService.DidNotReceive().Config(Arg.Any<string>(), Arg.Any<string>());
            _notificationService.DidNotReceive().Message(Arg.Any<MessageModel>());
            _notificationService.DidNotReceive().SendMessageInfo();
        }

        [Fact]
        public async Task SendAsync_WithWhitespaceContent_ShouldReturnFalse()
        {
            // Arrange
            var content = "   ";
            var extent = "test-system";

            // Act
            var result = await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            Assert.False(result);
            _notificationService.DidNotReceive().Config(Arg.Any<string>(), Arg.Any<string>());
            _notificationService.DidNotReceive().Message(Arg.Any<MessageModel>());
            _notificationService.DidNotReceive().SendMessageInfo();
        }

        [Fact]
        public async Task SendAsync_WithNullContent_ShouldReturnFalse()
        {
            // Arrange
            string content = null;
            var extent = "test-system";

            // Act
            var result = await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            Assert.False(result);
            _notificationService.DidNotReceive().Config(Arg.Any<string>(), Arg.Any<string>());
            _notificationService.DidNotReceive().Message(Arg.Any<MessageModel>());
            _notificationService.DidNotReceive().SendMessageInfo();
        }

        [Fact]
        public async Task SendAsync_WithDifferentExtents_ShouldPassCorrectExtent()
        {
            // Arrange
            var content = "Test message";
            var extent1 = "system-1";
            var extent2 = "system-2";
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content, extent1);
            await _slackNotificationChannel.SendAsync(content, extent2);

            // Assert
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x => x.SystemName == extent1));
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x => x.SystemName == extent2));
        }

        [Fact]
        public async Task SendAsync_WithEmptyExtent_ShouldPassEmptyExtent()
        {
            // Arrange
            var content = "Test message";
            var extent = string.Empty;
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x => x.SystemName == string.Empty));
        }

        [Fact]
        public async Task SendAsync_WithNullExtent_ShouldPassNullExtent()
        {
            // Arrange
            var content = "Test message";
            string extent = null;
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x => x.SystemName == null));
        }

        [Fact]
        public async Task SendAsync_ShouldUseCorrectGroupIdAndEnvironmentName()
        {
            // Arrange
            var content = "Test message";
            var extent = "test-system";
            var groupId = "custom-group-id";
            var environmentName = "PROD";
            var notificationService = Substitute.For<INotificationService>();
            var channel = new SlackNotificationChannel(notificationService, groupId, environmentName);

            notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(notificationService);
            notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(notificationService);
            notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await channel.SendAsync(content, extent);

            // Assert
            notificationService.Received(1).Config(groupId, environmentName);
            notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x =>
                    x.Environment == environmentName));
        }

        [Fact]
        public async Task SendAsync_WithLongContent_ShouldHandleCorrectly()
        {
            // Arrange
            var content = string.Join("\n", Enumerable.Range(0, 1000).Select(i => $"Line {i}: This is a very long message content."));
            var extent = "test-system";
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x => x.Message == content));
        }

        [Fact]
        public async Task SendAsync_WithSpecialCharacters_ShouldPreserveContent()
        {
            // Arrange
            var content = "Message with special chars: !@#$%^&*()\nUnicode: 中文 العربية 🚀\nNew lines:\n\nMultiple lines";
            var extent = "test-system";
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content, extent);

            // Assert
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x => x.Message == content));
        }

        [Fact]
        public async Task SendAsync_WithMultipleCalls_ShouldCallSendMessageInfoMultipleTimes()
        {
            // Arrange
            var content1 = "Message 1";
            var content2 = "Message 2";
            var content3 = "Message 3";
            var extent = "test-system";
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content1, extent);
            await _slackNotificationChannel.SendAsync(content2, extent);
            await _slackNotificationChannel.SendAsync(content3, extent);

            // Assert
            _notificationService.Received(3).Config(_groupId, _environmentName);
            _notificationService.Received(3).Message(Arg.Any<MessageModel>());
            _notificationService.Received(3).SendMessageInfo();
        }

        [Fact]
        public async Task SendAsync_ShouldSetCorrectTimestamp()
        {
            // Arrange
            var content = "Test message";
            var extent = "test-system";
            var beforeCall = DateTimeOffset.Now;
            _notificationService.Config(Arg.Any<string>(), Arg.Any<string>())
                               .Returns(_notificationService);
            _notificationService.Message(Arg.Any<MessageModel>())
                               .Returns(_notificationService);
            _notificationService.SendMessageInfo()
                               .Returns(true);

            // Act
            await _slackNotificationChannel.SendAsync(content, extent);
            var afterCall = DateTimeOffset.Now;

            // Assert
            _notificationService.Received(1).Message(
                Arg.Is<MessageModel>(x =>
                    x.Timestamp >= beforeCall &&
                    x.Timestamp <= afterCall));
        }
        #endregion
    }
}