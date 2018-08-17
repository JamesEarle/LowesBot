using Newtonsoft.Json;

namespace LowesBot.Models
{
    public partial class OrderData
    {
        [JsonProperty("orderNumber")]
        public string Number { get; set; }

        public string Date { get; set; } = "August 18, 2018";
        public string Status { get; set; } = "Shipped";
        public string Amount { get; set; } = "$1,234";
        public string Description { get; set; } = "The quick brown fox jumps over the lazy dog.";
        public string Image { get; set; } = "https://mobileimages.lowes.com/product/converted/039725/039725041142.jpg";

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