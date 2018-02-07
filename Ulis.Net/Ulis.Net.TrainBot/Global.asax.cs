using System;
using System.Web.Configuration;
using System.Web.Http;
using Ulis.Net.Library;

namespace Ulis.Net.TrainBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Lazy<UlisClient> _ulisClient = new Lazy<UlisClient>(() =>
        {
            var translatorClient = bool.Parse(WebConfigurationManager.AppSettings["UseMicrosoftTranslator"])
                    ? new MicrosoftTranslatorClient(WebConfigurationManager.AppSettings["MicrosoftSubscriptionKey"]) as ITranslatorClient
                    : new GoogleTranslatorClient(WebConfigurationManager.AppSettings["GoogleSubscriptionKey"]);

            return new UlisClient(translatorClient, WebConfigurationManager.AppSettings["LuisAppId"],
                WebConfigurationManager.AppSettings["LuisAppKey"], WebConfigurationManager.AppSettings["LuisRegion"]);
        });

        public static UlisClient UlisClient => _ulisClient.Value;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}