using PayPal.Api;
using System.Collections.Generic;

namespace JoelScottFitness.PayPal.Configuration
{
    public static class PayPalConfiguration
    {
        private readonly static string ClientId;
        private readonly static string ClientSecret;

        static PayPalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            return new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
        }

        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}
