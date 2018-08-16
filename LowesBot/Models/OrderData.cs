using Newtonsoft.Json;

namespace LowesBot.Dialogs
{
    public partial class OrderData
    {
        [JsonProperty("orderNumber")]
        public string Number { get; set; }

        public static OrderData Parse(string json)
        {
            return JsonConvert.DeserializeObject<OrderData>(json);
        }
    }
}