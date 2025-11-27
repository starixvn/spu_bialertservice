namespace Fanex.NAP.CTS.BIAlertService.Shares
{
    using Fanex.NAP.CTS.BIAlertService.Shares.Models;
    using System;
    using System.Net;
    using System.Net.Http;

    public static class ApiHelper
    {
        private static APIHelperConfig _config;

        public static void Setup(APIHelperConfig config)
        {
            _config = config;
        }

        public static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

            if (!string.IsNullOrEmpty(_config?.Proxy))
            {
                handler.Proxy = new WebProxy(_config.Proxy);
                handler.UseProxy = true;
            }

            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(5)
            };
        }
    }
}

