namespace Fanex.NAP.CTS.BIAlertService.Tests.Features.NotificationChannel
{
    using Fanex.NAP.CTS.BIAlertService.Features.NotificationChannel;
    using Fanex.NAP.CTS.BIAlertService.Features.RocketChat;
    using NSubstitute;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class RocketChatNotificationChannelTest
    {
        private readonly IRocketChatService _rocketChatService;
        private readonly RocketChatNotificationChannel _rocketChatNotificationChannel;

        public RocketChatNotificationChannelTest()
        {
            _rocketChatService = Substitute.For<IRocketChatService>();
            _rocketChatNotificationChannel = new RocketChatNotificationChannel(_rocketChatService);
        }

        #region Constructor Tests
        [Fact]
        public void Constructor_WithValidRocketChatService_ShouldInitializeCorrectly()
        {
            // Arrange
            var rocketChatService = Substitute.For<IRocketChatService>();

            // Act
            var channel = new RocketChatNotificationChannel(rocketChatService);

            // Assert
            Assert.NotNull(channel);
        }

        [Fact]
        public void Constructor_WithNullRocketChatService_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new RocketChatNotificationChannel(null));
        }
        #endregion

        #region CanHandle Tests
        [Fact]
        public void CanHandle_WithRocketChat_ShouldReturnTrue()
        {
            // Arrange
            var channelType = "RocketChat";

            // Act
            var result = _rocketChatNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_WithRocketChatLowerCase_ShouldReturnTrue()
        {
            // Arrange
            var channelType = "rocketchat";

            // Act
            var result = _rocketChatNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_WithRocketChatMixedCase_ShouldReturnTrue()
        {
            // Arrange
            var channelType = "RoCkEtChAt";

            // Act
            var result = _rocketChatNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CanHandle_WithSlack_ShouldReturnFalse()
        {
            // Arrange
            var channelType = "Slack";

            // Act
            var result = _rocketChatNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanHandle_WithEmptyString_ShouldReturnFalse()
        {
            // Arrange
            var channelType = string.Empty;

            // Act
            var result = _rocketChatNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanHandle_WithNull_ShouldReturnFalse()
        {
            // Arrange
            string channelType = null;

            // Act
            var result = _rocketChatNotificationChannel.CanHandle(channelType);

            // Assert
            Assert.False(result);
        }
        #endregion

        #region SendAsync Tests
        [Fact]
        public async Task SendAsync_WithValidContentAndExtent_ShouldCallPostMessageAsync()
        {
            // Arrange
            var content = "Test message content";
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == extent &&
                    x.text == content));
        }

        [Fact]
        public async Task SendAsync_WithEmptyContent_ShouldStillCallPostMessageAsync()
        {
            // Arrange
            var content = string.Empty;
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == extent &&
                    x.text == string.Empty));
        }

        [Fact]
        public async Task SendAsync_WithNullContent_ShouldStillCallPostMessageAsync()
        {
            // Arrange
            string content = null;
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == extent &&
                    x.text == null));
        }

        [Fact]
        public async Task SendAsync_WithWhitespaceContent_ShouldStillCallPostMessageAsync()
        {
            // Arrange
            var content = "   ";
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == extent &&
                    x.text == content));
        }

        [Fact]
        public async Task SendAsync_WithDifferentExtents_ShouldPassCorrectExtent()
        {
            // Arrange
            var content = "Test message";
            var extent1 = "channel-1";
            var extent2 = "channel-2";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent1);
            await _rocketChatNotificationChannel.SendAsync(content, extent2);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == extent1));
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == extent2));
        }

        [Fact]
        public async Task SendAsync_WithEmptyExtent_ShouldPassEmptyExtent()
        {
            // Arrange
            var content = "Test message";
            var extent = string.Empty;
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == string.Empty));
        }

        [Fact]
        public async Task SendAsync_WithNullExtent_ShouldPassNullExtent()
        {
            // Arrange
            var content = "Test message";
            string extent = null;
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == null));
        }

        [Fact]
        public async Task SendAsync_WhenPostMessageAsyncReturnsTrue_ShouldReturnTrue()
        {
            // Arrange
            var content = "Test message";
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            var result = await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendAsync_WhenPostMessageAsyncReturnsFalse_ShouldReturnFalse()
        {
            // Arrange
            var content = "Test message";
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(false));

            // Act
            var result = await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SendAsync_WhenPostMessageAsyncThrowsException_ShouldPropagateException()
        {
            // Arrange
            var content = "Test message";
            var extent = "test-channel";
            var expectedException = new Exception("RocketChat service error");
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _rocketChatNotificationChannel.SendAsync(content, extent));

            Assert.Equal("RocketChat service error", exception.Message);
        }

        [Fact]
        public async Task SendAsync_WithLongContent_ShouldHandleCorrectly()
        {
            // Arrange
            var content = string.Join("\n", System.Linq.Enumerable.Range(0, 1000).Select(i => $"Line {i}: This is a very long message content."));
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == extent &&
                    x.text == content));
        }

        [Fact]
        public async Task SendAsync_WithSpecialCharacters_ShouldPreserveContent()
        {
            // Arrange
            var content = "Message with special chars: !@#$%^&*()\nUnicode: 中文 العربية 🚀\nNew lines:\n\nMultiple lines";
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content, extent);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == extent &&
                    x.text == content));
        }

        [Fact]
        public async Task SendAsync_WithMultipleCalls_ShouldCallPostMessageAsyncMultipleTimes()
        {
            // Arrange
            var content1 = "Message 1";
            var content2 = "Message 2";
            var content3 = "Message 3";
            var extent = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _rocketChatNotificationChannel.SendAsync(content1, extent);
            await _rocketChatNotificationChannel.SendAsync(content2, extent);
            await _rocketChatNotificationChannel.SendAsync(content3, extent);

            // Assert
            await _rocketChatService.Received(3).PostMessageAsync(Arg.Any<RCPostMessageModel>());
        }
        #endregion
    }
}