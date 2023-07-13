using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductManagement.InfrastructureData.Dto
{
    public class ProductDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Price")]
        public double Price { get; set; }
        [JsonPropertyName("Rate")]
        public int Rate { get; set; }
        [JsonPropertyName("Continued")]
        public string Avalidable { get; set; }
    }
}
