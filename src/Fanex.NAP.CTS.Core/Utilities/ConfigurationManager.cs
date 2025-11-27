namespace Fanex.NAP.CTS.Core.Utilities
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationManager
    {
        private static IConfiguration Configuration;

        public static void Setup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string AppSettings(string key) => Configuration?[$"AppSettings:{key}"] ?? string.Empty;
    }
}
