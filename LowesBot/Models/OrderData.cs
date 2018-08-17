using Newtonsoft.Json;

namespace LowesBot.Models
{
    public partial class OrderData
    {
        [JsonProperty("orderNumber")]
        public string Number { get; set; }

        public static bool TryParse(string json, out OrderData  data)
        {
            try
            {
                data = JsonConvert.DeserializeObject<OrderData>(json);
                return true;
            }
            catch 
            {
                data = default(OrderData);
                return false;
            }
        }
    }
}