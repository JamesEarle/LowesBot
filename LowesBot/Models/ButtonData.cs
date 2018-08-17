using System;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace LowesBot.Models
{
    public partial class ButtonData
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        public static bool TryParse(string json, out ButtonData data)
        {
            try
            {
                data = JsonConvert.DeserializeObject<ButtonData>(json);
                return true;
            }
            catch
            {
                data = default(ButtonData);
                return false;
            }
        }
    }
}