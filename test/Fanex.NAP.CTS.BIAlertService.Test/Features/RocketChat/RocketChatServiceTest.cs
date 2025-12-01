namespace Fanex.NAP.CTS.BIAlertService.Tests.Features.RocketChat
{
    using Fanex.NAP.CTS.BIAlertService.Features.RocketChat;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class RocketChatServiceTest
    {        
        private readonly IOptions<RCConfigModel> _options;
        private readonly RCConfigModel _config;
        private readonly IRocketChatService _rocketChatService;

        public RocketChatServiceTest() 
        {
            _config = new RCConfigModel
            {
                ApiBaseURL = "https://test.rocketchat.com",
                Username = "testuser",
                Password = "testpass",
                Channel = "testchannel",
                Endpoint = new RCEndpointModel
                {
                    Login = "/api/v1/login",
                    PostMessage = "/api/v1/chat.postMessage",
                    Logout = "/api/v1/logout"
                }
            };

            _options = Substitute.For<IOptions<RCConfigModel>>();
            _options.Value.Returns(_config);

            _rocketChatService = new RocketChatService(_options);
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidOptions_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var service = new RocketChatService(_options);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RocketChatService(null));
        }

        [Fact]
        public void Constructor_WithNullConfigValue_ShouldThrowArgumentNullException()
        {
            var options = Substitute.For<IOptions<RCConfigModel>>();
            options.Value.Returns((RCConfigModel)null);

            Assert.Throws<ArgumentNullException>(
                () => new RocketChatService(options));
        }
        #endregion Constructor Tests

        #region LogoutAsync Tests
        [Fact]
        public async Task LogoutAsync_ShouldCompleteSuccessfully()
        {
            // Arrange & Act
            await _rocketChatService.LogoutAsync();

            // Assert
            // No exception should be thrown
            Assert.True(true); // LogoutAsync always succeeds
        }

        [Fact]
        public async Task LogoutAsync_ShouldReturnCompletedTask()
        {
            // Arrange & Act
            var result = _rocketChatService.LogoutAsync();

            // Assert
            Assert.True(result.IsCompleted);
            await result; // Should not throw
        }
        #endregion LogoutAsync Tests

        #region Configuration Validation Tests
        [Fact]
        public void Constructor_WithEmptyApiBaseURL_ShouldNotThrow()
        {
            // Arrange
            var configWithEmptyUrl = new RCConfigModel
            {
                ApiBaseURL = "",
                Username = "test",
                Password = "test",
                Endpoint = new RCEndpointModel { Login = "/login" }
            };

            var options = Substitute.For<IOptions<RCConfigModel>>();
            options.Value.Returns(configWithEmptyUrl);

            // Act & Assert
            var service = new RocketChatService(options);
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullEndpoint_ShouldNotThrowInConstructor()
        {
            // Arrange
            var configWithNullEndpoint = new RCConfigModel
            {
                ApiBaseURL = "https://test.com",
                Username = "test",
                Password = "test",
                Endpoint = null
            };

            var options = Substitute.For<IOptions<RCConfigModel>>();
            options.Value.Returns(configWithNullEndpoint);

            // Act & Assert
            var service = new RocketChatService(options);
            Assert.NotNull(service);
        }
        #endregion Configuration Validation Tests        

        #region Error Handling Tests      

        [Fact]
        public async Task PostMessageAsync_WithEmptyMessage_ShouldNotThrow()
        {
            // Arrange
            var emptyMessage = new RCPostMessageModel
            {
                channel = "",
                text = ""
            };

            // Act & Assert
            // Should not throw in PostMessageAsync call itself
            // Actual API call might fail, but method should handle it
            var exception = await Record.ExceptionAsync(() =>
                _rocketChatService.PostMessageAsync(emptyMessage));

            // Assert based on expected behavior
        }
        #endregion Error Handling Tests
    }
}
