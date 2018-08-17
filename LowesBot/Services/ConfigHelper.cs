using System.Web.Configuration;

namespace LowesBot.Services
{
    public static class ConfigHelper
    {

        public static string BotId => Read("PFUserName");
        public static string MicrosoftAppId => Read("MicrosoftAppId");
        public static string MicrosoftAppPassword => Read("MicrosoftAppPassword");
        public static string LuisKey => Read("LuisKey");
        public static string LuisUrl => Read("LuisUrl");
        public static double LuisScore => double.Parse(Read("LuisScore"));
        public static string MapKey => Read("MapKey");

        private static string Read(string key)
        {
            var x = WebConfigurationManager.AppSettings[key];
            return x;
        }
    }
}