namespace Fanex.NAP.CTS.Core.Utilities
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigSettings
    {
        private static IConfiguration Configuration;

        public static void Setup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string SentryDsn => Configuration?["Sentry:Dsn"] ?? string.Empty;

        public static string SentryCategory => Configuration?["Sentry:Category"] ?? string.Empty;

        public static string GetConfigStringByName(string name)
        {
            return Configuration?[$"AppSettings:{name}"] ?? string.Empty;
        }

        public static string RabbitMQ_Host()
        {
            return Configuration?["RabbitMQ-Host"] ?? string.Empty;
        }

        public static string RabbitMQ_VirtualHost()
        {
            return Configuration?["RabbitMQ-VirtualHost"] ?? string.Empty;
        }

        public static string RabbitMQ_PrefetchCount()
        {
            return Configuration?["RabbitMQ-PrefetchCount"] ?? string.Empty;
        }

        public static string RabbitMQ_Username()
        {
            return Configuration?["RabbitMQ-Username"] ?? string.Empty;
        }

        public static string RabbitMQ_Password()
        {
            return Configuration?["RabbitMQ-Password"] ?? string.Empty;
        }

        public static string RabbitMQ_Port()
        {
            return Configuration?["RabbitMQ-Port"] ?? string.Empty;
        }

        public static string RabbitMQ_Exchange()
        {
            return Configuration?["RabbitMQ-Exchange"] ?? string.Empty;
        }

        public static string StoredProcedurePath => GetConfigStringByName("StoredProcedurePath");

        public static string GetProblemCaseMonitor => GetConfigStringByName("CTS_GetProblemCaseMonitor");

        public static string UpdateCategories => GetConfigStringByName("CTS_UpdateCategories");

        public static string RemoveProblemAccounts => GetConfigStringByName("CTS_RemoveProblemAccounts");

        public static string InsertAFCAccountsURL => GetConfigStringByName("AFC_InsertAccountsURL");

        public static string DeleteAFCAccountsURL => GetConfigStringByName("AFC_DeleteAccountsURL");

        public static string GetHoldKeyMMDetailURL => GetConfigStringByName("MMDetail_HoldKeyGetURL");

        public static string UpdateHoldKeyMMDetailURL => GetConfigStringByName("MMDetail_HoldKeyUpdateURL");

        public static string TVSTicketVoidingURL => GetConfigStringByName("MMDetail_TVSTicketVoidingURL");

        public static string MarkPAURL => GetConfigStringByName("MMDetail_MarkPAURL");

        public static int ImportEvidenceChunkSize
        {
            get
            {
                var size = 500;
                var configSize = GetConfigStringByName("Import-Evidence-ChunkSize");

                int.TryParse(configSize, out size);

                return size == 0 ? 500 : size;
            }
        }

        public static int SPUMMailingListId
        {
            get
            {
                var mailingListId = 80;
                var id = GetConfigStringByName("SPU_MailingListId");

                int.TryParse(id, out mailingListId);

                return mailingListId == 0 ? 80 : mailingListId;
            }
        }

        public static string EnvironmentName
        {
            get
            {
                var environment = GetConfigStringByName("Environment");

                switch (environment)
                {
                    case "1":
                        return "LOCAL";
                    case "2":
                        return "UAT";
                    case "3":
                        return "PRO";
                    case "4":
                        return "SIT";
                    case "5":
                        return "AWS";
                    case "6":
                        return "KL Simple";
                    case "7":
                        return "KL Prod";
                    default:
                        return "LOCAL";
                }
            }
        }

        public static string VoidRequestToTVSURL => GetConfigStringByName("VoidRequestToTVS-URL");

        public static int VoidRequestToTVSSubIssueTypeId
        {
            get
            {
                var subIssueTypeId = 0;
                var id = GetConfigStringByName("VoidRequestToTVS-SubIssueTypeId");

                int.TryParse(id, out subIssueTypeId);

                return subIssueTypeId == 0 ? 27 : subIssueTypeId;
            }
        }

        public static int VoidRequestToTVSActionId
        {
            get
            {
                var actionId = 0;
                var id = GetConfigStringByName("VoidRequestToTVS-ActionId");

                int.TryParse(id, out actionId);

                return actionId == 0 ? 27 : actionId;
            }
        }

        public static string GetAssociatedGroup => GetConfigStringByName("Graph_GetAssociatedGroup");

        public static int GetAccummulatedChunkSize
        {
            get
            {
                var size = 500;
                var configSize = GetConfigStringByName("Get-Accummulated-ChunkSize");

                int.TryParse(configSize, out size);

                return size == 0 ? 500 : size;
            }
        }

        public static int PushCCToExternalChunkSize
        {
            get
            {
                var size = 500;
                var configSize = GetConfigStringByName("PushCCToExternal-ChunkSize");

                int.TryParse(configSize, out size);

                return size == 0 ? 500 : size;
            }
        }

        public static int InternalChunkSize
        {
            get
            {
                var size = 500;
                var configSize = GetConfigStringByName("Internal-ChunkSize");

                int.TryParse(configSize, out size);

                return size == 0 ? 500 : size;
            }
        }

        public static bool IsWriteLogInfo
        {
            get
            {
                var isWriteLog = false;

                var configWriteLog = Configuration["IsWriteLogInfo"];

                bool.TryParse(configWriteLog, out isWriteLog);

                return isWriteLog;
            }
        }

        public static bool GetFlaggedEviByAssFromMySQL
        {
            get
            {
                var isFromMySQL = Configuration["GetFlaggedEviByAssFromMySQL"];

                return isFromMySQL == "1";
            }
        }

        public static bool IsSyncAFCData
        {
            get
            {
                var rs = Configuration["IsSyncAntiFraudCenterData"];

                return rs == "1";
            }
        }

        public static string ApiKey
        {
            get
            {
                var apiKey = Configuration["ApiKey"] ?? string.Empty;
                return apiKey;
            }
        }
    }
}