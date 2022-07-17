using Windows.UI.Xaml.Controls;

namespace IntranetUWP.Constanst
{
    public static class HttpConstants
    {
        public const string BaseUrl         = "https://intranetapi.azurewebsites.net/api";
        public const string LocalBaseUrl    = "https://localhost:44371/api";

        public const string IdentityBaseUrl = "https://totechsidentity.azurewebsites.net/api";
    }

    public static class SignalRConstants
    {
        public const string BaseUrl      = "https://intranetapi.azurewebsites.net/chathub";
        public const string LocalBaseUrl = "https://localhost:44371/chathub";
    }

    public static class AzureNotificationHubContants
    {
        public const string HubName          = "totechsnotificationshub";
        public const string ConnectionString = "Endpoint=sb://totechscorp.servicebus.windows.net/;SharedAccessKeyName=Test;SharedAccessKey=SdPV1Ybqlf/S4ZCfHOvOEfVQZKptyxmFWiwemy7B9ic=";
    }

    public static class SyncFusionConstants
    {
        public const string Key = "NjY3NjMzQDMyMzAyZTMyMmUzMG1Rc0FLNllrRVZBUjFCN2o0N3VPL3ZWMjJDUE8rUEg0ay9MNTY3OEcxQkk9";
    }
}
