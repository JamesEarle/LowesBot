using Newtonsoft.Json;

namespace LowesBot.Dialogs
{
    public partial class ButtonData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        public static ButtonData Parse(string json)
        {
            return JsonConvert.DeserializeObject<ButtonData>(json);
        }
    }
}