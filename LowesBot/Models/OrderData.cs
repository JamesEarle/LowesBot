using Newtonsoft.Json;
using System;

namespace LowesBot.Models
{
    [Serializable]
    public partial class OrderData
    {
        [JsonProperty("orderNumber")]
        public string Number { get; set; }

        public string Date { get; set; } = "August 18, 2018";
        public string Status { get; set; } = "Shipped";
        public string Amount { get; set; } = "$1,234";
        public string Description { get; set; } = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
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