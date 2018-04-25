using JoelScottFitness.PayPal.Constants;
using JoelScottFitness.PayPal.Properties;
using PayPal.Api;
using System.Collections.Generic;

namespace JoelScottFitness.PayPal.Configuration
{
    public static class PayPalConfiguration
    {
        private readonly static string ClientId;
        private readonly static string ClientSecret;

        private static Dictionary<string, string> PayPalConfig;

        static PayPalConfiguration()
        {
            var config = GetConfig();
            ClientId = config[PayPalProperties.ClientId];
            ClientSecret = config[PayPalProperties.ClientSecret];
        }

        public static Dictionary<string, string> GetConfig()
        {
            if (PayPalConfig == null)
            {
                PayPalConfig = new Dictionary<string, string>()
                {
                    { PayPalProperties.Mode, Settings.Default.Mode },
                    { PayPalProperties.ConnectionTimeout, Settings.Default.ConnectionTimeout },
                    { PayPalProperties.RequestRetries, Settings.Default.RequestRetries },
                };

                if (Settings.Default.Mode == PayPalProperties.LiveMode)
                {
                    PayPalConfig.Add(PayPalProperties.ClientId, Settings.Default.LCID);
                    PayPalConfig.Add(PayPalProperties.ClientSecret, Settings.Default.LCS);
                }
                else
                {
                    PayPalConfig.Add(PayPalProperties.ClientId, Settings.Default.SCID);
                    PayPalConfig.Add(PayPalProperties.ClientSecret, Settings.Default.SCS);
                };
            }

            return PayPalConfig;
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
