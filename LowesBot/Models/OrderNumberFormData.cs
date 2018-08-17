using Newtonsoft.Json;

namespace LowesBot.Models
{
    public partial class OrderNumberFormData
    {
        [JsonProperty("orderNumber", Required = Required.Always)]
        public string OrderNumber { get; set; }

        public static bool TryParse(string json, out OrderNumberFormData data)
        {
            try
            {
                data = JsonConvert.DeserializeObject<OrderNumberFormData>(json);
                return true;
            }
            catch
            {
                data = default(OrderNumberFormData);
                return false;
            }
        }
    }
}