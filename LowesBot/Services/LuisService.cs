using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LowesBot.Services
{

    public static class LuisService
    {
        private static HttpClient _client;
        private static Uri _uri;

        static LuisService()
        {
            _client = new HttpClient();
            SetupLuisUrl();
        }

        private static void SetupLuisUrl()
        {
            var uri = ConfigHelper.LuisUrl;
            if (!uri.EndsWith("q="))
            {
                throw new UriFormatException("Luis endpoints should end with 'q='.");
            }
            _uri = new Uri($"{_uri}{HttpUtility.UrlEncode(uri)}");
        }

        public static async Task<LuisResult> RecognizeAsync(string text)
        {
            var uri = $"{_uri}{HttpUtility.UrlEncode(text)}";
            var json = await _client.GetStringAsync(_uri);
            return JsonConvert.DeserializeObject<LuisResult>(json);
        }
    }
}