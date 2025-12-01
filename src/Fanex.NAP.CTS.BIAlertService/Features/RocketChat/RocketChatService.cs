namespace Fanex.NAP.CTS.BIAlertService.Features.RocketChat
{    
    using Fanex.NAP.CTS.Core.Extentions;
    using Fanex.NAP.CTS.Core.Helpers.Api;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRocketChatService
    {
        Task LoginAsync();

        Task LogoutAsync();

        Task<bool> PostMessageAsync(RCPostMessageModel content);
    }

    public class RocketChatService : IRocketChatService
    {
        private readonly RCConfigModel _config;
        private RCResponseLoginModel? _loginSession;
        private readonly SemaphoreSlim _loginLock = new(1, 1);

        public RocketChatService(IOptions<RCConfigModel> options)
        {
            _config = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task LoginAsync()
        {
            var loginUrl = $"{_config.ApiBaseURL}{_config.Endpoint.Login}";

            var response = await ApiHelper.Post<RCResponseLoginModel>(
                loginUrl,
                new RCLoginModel
                {
                    user = _config.Username,
                    password = _config.Password
                });

            if (response == null)
            {
                LogExtension.LogError($"RocketChat login failed for user { _config.Username }");
                throw new InvalidOperationException("RocketChat login failed");
            }

            _loginSession = response;
        }

        public Task LogoutAsync()
        {
            _loginSession = null;

            return Task.CompletedTask;
        }

        public async Task<bool> PostMessageAsync(RCPostMessageModel content)
        {
            await EnsureLoggedInAsync();

            var postMessageUrl =
                $"{_config.ApiBaseURL}{_config.Endpoint.PostMessage}";

            return await ApiHelper.Post<bool>(
                postMessageUrl,
                content,
                new Dictionary<string, string>{
                    { "X-Auth-Token", _loginSession!.data.authToken },
                    { "X-User-Id", _loginSession!.data.userId }
                });
        }

        private async Task EnsureLoggedInAsync()
        {
            if (_loginSession != null)
            { 
                return; 
            }

            await _loginLock.WaitAsync();

            try
            {
                if (_loginSession == null)
                {
                    await LoginAsync();
                }
            }
            finally
            {
                _loginLock.Release();
            }
        }
    }
}
