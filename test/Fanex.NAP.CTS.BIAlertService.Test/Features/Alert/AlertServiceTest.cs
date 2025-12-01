namespace Fanex.NAP.CTS.BIAlertService.Tests.Features.Alert
{
    using Fanex.NAP.CTS.BIAlertService.Features.Alert;
    using Fanex.NAP.CTS.BIAlertService.Features.RocketChat;
    using NSubstitute;
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class AlertServiceTest
    {       
        private readonly IRocketChatService _rocketChatService;
        private readonly IAlertService _alertService;

        public AlertServiceTest()
        {
            _rocketChatService = Substitute.For<IRocketChatService>();
            _alertService = new AlertService(_rocketChatService);
        }

        [Fact]
        public void Constructor_WithValidRocketChatService_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var service = new AlertService(_rocketChatService);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullRocketChatService_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new AlertService(null));
        }

        #region NotifyAsync Tests
        [Fact]
        public async Task NotifyAsync_WithValidContentAndChannel_ShouldCallPostMessageAsync()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message content");
            var channel = "test-channel";
            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == channel &&
                    x.text == "Test message content"));
        }

        [Fact]
        public async Task NotifyAsync_WithValidContent_ShouldConvertStringBuilderToString()
        {
            // Arrange
            var sbContent = new StringBuilder();
            sbContent.AppendLine("Line 1");
            sbContent.AppendLine("Line 2");
            sbContent.Append("Line 3");

            var channel = "test-channel";
            var expectedText = sbContent.ToString();

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.text == expectedText));
        }

        [Fact]
        public async Task NotifyAsync_WithDifferentChannels_ShouldPassCorrectChannel()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel1 = "channel-1";
            var channel2 = "channel-2";

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel1);
            await _alertService.NotifyAsync(sbContent, channel2);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == channel1));
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == channel2));
        }
        #endregion NotifyAsync Tests

        #region Edge Cases Tests
        [Fact]
        public async Task NotifyAsync_WithEmptyStringBuilder_ShouldStillCallPostMessageAsync()
        {
            // Arrange
            var sbContent = new StringBuilder();
            var channel = "test-channel";

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x =>
                    x.channel == channel &&
                    x.text == string.Empty));
        }

        [Fact]
        public async Task NotifyAsync_WithEmptyChannel_ShouldPassEmptyChannel()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel = string.Empty;

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == string.Empty));
        }

        [Fact]
        public async Task NotifyAsync_WithNullChannel_ShouldPassNullChannel()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            string channel = null;

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.channel == null));
        }

        [Fact]
        public async Task NotifyAsync_WithNullStringBuilder_ShouldThrowNullReferenceException()
        {
            // Arrange
            StringBuilder sbContent = null;
            var channel = "test-channel";

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => _alertService.NotifyAsync(sbContent, channel));

            // Verify RocketChat was not called
            await _rocketChatService.DidNotReceive().PostMessageAsync(Arg.Any<RCPostMessageModel>());
        }

        [Fact]
        public async Task NotifyAsync_WithLongContent_ShouldHandleCorrectly()
        {
            // Arrange
            var sbContent = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                sbContent.AppendLine($"Line {i}: This is a very long message content.");
            }

            var channel = "test-channel";
            var expectedText = sbContent.ToString();

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.text == expectedText));
        }

        [Fact]
        public async Task NotifyAsync_WithSpecialCharacters_ShouldPreserveContent()
        {
            // Arrange
            var sbContent = new StringBuilder();
            sbContent.AppendLine("Message with special chars: !@#$%^&*()");
            sbContent.AppendLine("Unicode: 中文 العربية 🚀");
            sbContent.AppendLine("New lines:\n\nMultiple lines");

            var channel = "test-channel";
            var expectedText = sbContent.ToString();

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.text == expectedText));
        }
        #endregion Edge Cases Tests

        #region Error Handling Tests
        [Fact]
        public async Task NotifyAsync_WhenRocketChatThrowsException_ShouldPropagateException()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel = "test-channel";
            var expectedException = new Exception("RocketChat error");

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromException<bool>(expectedException));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _alertService.NotifyAsync(sbContent, channel));

            Assert.Equal("RocketChat error", exception.Message);
        }

        [Fact]
        public async Task NotifyAsync_WhenRocketChatReturnsFalse_ShouldCompleteWithoutException()
        {
            // Arrange
            var sbContent = new StringBuilder("Test message");
            var channel = "test-channel";

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(false));

            // Act
            var exception = await Record.ExceptionAsync(
                () => _alertService.NotifyAsync(sbContent, channel));

            // Assert
            Assert.Null(exception); // Should complete without throwing
        }
        #endregion Error Handling Tests

        #region Multiple Calls Tests
        [Fact]
        public async Task NotifyAsync_WithMultipleCalls_ShouldCallPostMessageAsyncMultipleTimes()
        {
            // Arrange
            var sbContent1 = new StringBuilder("Message 1");
            var sbContent2 = new StringBuilder("Message 2");
            var sbContent3 = new StringBuilder("Message 3");
            var channel = "test-channel";

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act
            await _alertService.NotifyAsync(sbContent1, channel);
            await _alertService.NotifyAsync(sbContent2, channel);
            await _alertService.NotifyAsync(sbContent3, channel);

            // Assert
            await _rocketChatService.Received(3).PostMessageAsync(Arg.Any<RCPostMessageModel>());
        }

        [Fact]
        public async Task NotifyAsync_WithSameStringBuilder_ShouldReuseContent()
        {
            // Arrange
            var sbContent = new StringBuilder("Initial content");
            var channel = "test-channel";

            _rocketChatService.PostMessageAsync(Arg.Any<RCPostMessageModel>())
                              .Returns(Task.FromResult(true));

            // Act - First call
            await _alertService.NotifyAsync(sbContent, channel);

            // Modify StringBuilder
            sbContent.AppendLine(" - Modified");

            // Act - Second call with modified content
            await _alertService.NotifyAsync(sbContent, channel);

            // Assert
            await _rocketChatService.Received(2).PostMessageAsync(Arg.Any<RCPostMessageModel>());
            await _rocketChatService.Received(1).PostMessageAsync(
                Arg.Is<RCPostMessageModel>(x => x.text == "Initial content"));            
        }
        #endregion Multiple Calls Tests
    }
}
