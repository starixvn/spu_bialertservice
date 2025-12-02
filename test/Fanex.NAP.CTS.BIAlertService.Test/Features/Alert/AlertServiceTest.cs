namespace Fanex.NAP.CTS.BIAlertService.Tests.Features.Alert
{
    using Fanex.NAP.CTS.BIAlertService.Features.Alert;
    using Fanex.NAP.CTS.BIAlertService.Features.NotificationChannel;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class AlertServiceTest
    {
        private readonly INotificationChannel _rocketChatChannel;
        private readonly INotificationChannel _slackChannel;
        private readonly IAlertService _alertService;

        public AlertServiceTest()
        {
            _rocketChatChannel = Substitute.For<INotificationChannel>();
            _rocketChatChannel.CanHandle("RocketChat").Returns(true);
            _rocketChatChannel.CanHandle(Arg.Is<string>(x => x != "RocketChat")).Returns(false);

            _slackChannel = Substitute.For<INotificationChannel>();
            _slackChannel.CanHandle("Slack").Returns(true);
            _slackChannel.CanHandle(Arg.Is<string>(x => x != "Slack")).Returns(false);

            var channels = new List<INotificationChannel> { _rocketChatChannel, _slackChannel };
            _alertService = new AlertService(channels);
        }

        #region Constructor Tests
        [Fact]
        public void Constructor_WithValidNotificationChannels_ShouldInitializeCorrectly()
        {
            // Arrange
            var channels = new List<INotificationChannel> { _rocketChatChannel, _slackChannel };

            // Act
            var service = new AlertService(channels);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullNotificationChannels_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new AlertService(null));
        }

        [Fact]
        public void Constructor_WithEmptyNotificationChannels_ShouldInitializeCorrectly()
        {
            // Arrange
            var channels = new List<INotificationChannel>();

            // Act
            var service = new AlertService(channels);

            // Assert
            Assert.NotNull(service);
        }
        #endregion

        #region NotifyRocketChatAsync Tests
        [Fact]
        public async Task NotifyRocketChatAsync_WithValidContentAndChannel_ShouldCallSendAsync()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message content");
            var channel = "test-channel";
            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent, channel);

            // Assert
            await _rocketChatChannel.Received(1).SendAsync(
                "Test message content",
                channel);
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WithValidContent_ShouldConvertStringBuilderToString()
        {
            // Arrange
            var sbContent = new StringBuilder();
            sbContent.AppendLine("Line 1");
            sbContent.AppendLine("Line 2");
            sbContent.Append("Line 3");

            var channel = "test-channel";
            var expectedText = sbContent.ToString();

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent, channel);

            // Assert
            await _rocketChatChannel.Received(1).SendAsync(
                expectedText,
                channel);
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WithDifferentChannels_ShouldPassCorrectChannel()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel1 = "channel-1";
            var channel2 = "channel-2";

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent, channel1);
            await _alertService.NotifyRocketChatAsync(sbContent, channel2);

            // Assert
            await _rocketChatChannel.Received(1).SendAsync(Arg.Any<string>(), channel1);
            await _rocketChatChannel.Received(1).SendAsync(Arg.Any<string>(), channel2);
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WithEmptyStringBuilder_ShouldStillCallSendAsync()
        {
            // Arrange
            var sbContent = new StringBuilder();
            var channel = "test-channel";

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent, channel);

            // Assert
            await _rocketChatChannel.Received(1).SendAsync(
                string.Empty,
                channel);
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WithEmptyChannel_ShouldPassEmptyChannel()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel = string.Empty;

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent, channel);

            // Assert
            await _rocketChatChannel.Received(1).SendAsync(Arg.Any<string>(), string.Empty);
        }        

        [Fact]
        public async Task NotifyRocketChatAsync_WithNullStringBuilder_ShouldThrowArgumentNullException()
        {
            // Arrange
            StringBuilder sbContent = null;
            var channel = "test-channel";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _alertService.NotifyRocketChatAsync(sbContent, channel));

            // Verify channel was not called
            await _rocketChatChannel.DidNotReceive().SendAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WithLongContent_ShouldHandleCorrectly()
        {
            // Arrange
            var sbContent = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                sbContent.AppendLine($"Line {i}: This is a very long message content.");
            }

            var channel = "test-channel";
            var expectedText = sbContent.ToString();

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent, channel);

            // Assert
            await _rocketChatChannel.Received(1).SendAsync(expectedText, channel);
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WithSpecialCharacters_ShouldPreserveContent()
        {
            // Arrange
            var sbContent = new StringBuilder();
            sbContent.AppendLine("Message with special chars: !@#$%^&*()");
            sbContent.AppendLine("Unicode: 中文 العربية 🚀");
            sbContent.AppendLine("New lines:\n\nMultiple lines");

            var channel = "test-channel";
            var expectedText = sbContent.ToString();

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent, channel);

            // Assert
            await _rocketChatChannel.Received(1).SendAsync(expectedText, channel);
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WhenChannelThrowsException_ShouldPropagateException()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel = "test-channel";
            var expectedException = new Exception("Channel error");

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _alertService.NotifyRocketChatAsync(sbContent, channel));

            Assert.Equal("Channel error", exception.Message);
        }

        [Fact]
        public async Task NotifyRocketChatAsync_WhenChannelReturnsFalse_ShouldCompleteWithoutException()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel = "test-channel";

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(false));

            // Act
            var exception = await Record.ExceptionAsync(
                () => _alertService.NotifyRocketChatAsync(sbContent, channel));

            // Assert
            Assert.Null(exception); // Should complete without throwing
        }       

        [Fact]
        public async Task NotifyRocketChatAsync_WithMultipleCalls_ShouldCallSendAsyncMultipleTimes()
        {
            // Arrange
            var sbContent1 = new StringBuilder("Message 1");
            var sbContent2 = new StringBuilder("Message 2");
            var sbContent3 = new StringBuilder("Message 3");
            var channel = "test-channel";

            _rocketChatChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyRocketChatAsync(sbContent1, channel);
            await _alertService.NotifyRocketChatAsync(sbContent2, channel);
            await _alertService.NotifyRocketChatAsync(sbContent3, channel);

            // Assert
            await _rocketChatChannel.Received(3).SendAsync(Arg.Any<string>(), Arg.Any<string>());
        }
       
        #endregion

        #region NotifySlackAsync Tests
        [Fact]
        public async Task NotifySlackAsync_WithValidContentAndEnvironment_ShouldCallSendAsync()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message content");
            var environment = "PROD";
            _slackChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                         .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifySlackAsync(sbContent, environment);

            // Assert
            await _slackChannel.Received(1).SendAsync(sbContent.ToString(), environment);
        }
    
        [Fact]
        public async Task NotifySlackAsync_WithDifferentEnvironments_ShouldPassCorrectEnvironment()
        {
            // Arrange
            var content = "Test message";
            var environment1 = "PROD";
            var environment2 = "SIT";

            _slackChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                         .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifySlackAsync(new StringBuilder(content), environment1);
            await _alertService.NotifySlackAsync(new StringBuilder(content), environment2);

            // Assert
            await _slackChannel.Received(1).SendAsync(content, environment1);
            await _slackChannel.Received(1).SendAsync(content, environment2);
        }       

        [Fact]
        public async Task NotifySlackAsync_WhenChannelThrowsException_ShouldPropagateException()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var environment = "PROD";
            var expectedException = new Exception("Slack channel error");

            _slackChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                         .Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _alertService.NotifySlackAsync(sbContent, environment));

            Assert.Equal("Slack channel error", exception.Message);
        }
       
        [Fact]
        public async Task NotifySlackAsync_WithMultipleCalls_ShouldCallSendAsyncMultipleTimes()
        {
            // Arrange
            var content1 = new StringBuilder("Message 1");
            var content2 = new StringBuilder("Message 2");
            var content3 = new StringBuilder("Message 3");
            var environment = "PROD";

            _slackChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                         .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifySlackAsync(content1, environment);
            await _alertService.NotifySlackAsync(content2, environment);
            await _alertService.NotifySlackAsync(content3, environment);

            // Assert
            await _slackChannel.Received(3).SendAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task NotifySlackAsync_WithLongContent_ShouldHandleCorrectly()
        {
            // Arrange
            var content = new StringBuilder(string.Join("\n", Enumerable.Range(0, 1000).Select(i => $"Line {i}: This is a very long message content.")));
            var environment = "PROD";

            _slackChannel.SendAsync(Arg.Any<string>(), Arg.Any<string>())
                         .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifySlackAsync(content, environment);

            // Assert
            await _slackChannel.Received(1).SendAsync(content.ToString(), environment);
        }
        #endregion
    }
}