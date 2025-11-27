namespace Fanex.NAP.CTS.Core.Extentions
{
    using System;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.CTS.Core.Utilities;

    public static class LogExtension
    {
        public static void LogInfo(this string message)
        {
            LogService.Info(ConfigSettings.EnvironmentName + " " + message);
        }

        public static void LogError(this Exception ex, string message = null)
        {
            var logMessage = ex.CreateLogMessage(message);

            LogService.Error(logMessage);
        }

        public static void LogError(this string message, Exception ex)
        {
            LogService.Error(ConfigSettings.EnvironmentName + " " + message, ex);
        }

        public static void LogError(this string message)
        {
            LogService.Error(ConfigSettings.EnvironmentName + " " + message);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S6566:Use DateTimeOffset instead of DateTime", Justification = "Legacy code or specific reason")]
        public static string CreateLogMessage(this Exception ex, string message = null)
        {
            return new LogMessage(ConfigSettings.EnvironmentName + " " + ex.Message)
                .AddMessage(message)
                .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                .AddMessage($"{DateTime.Now}").ToString(); // NOSONAR
        }
    }
}
