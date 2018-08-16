using Newtonsoft.Json;

namespace LowesBot.Dialogs
{
    public partial class FormData
    {
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }

        public static FormData Parse(string json)
        {
            return JsonConvert.DeserializeObject<FormData>(json);
        }
    }
}