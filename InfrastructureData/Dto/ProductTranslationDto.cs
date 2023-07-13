using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductManagement.InfrastructureData.Dto
{
    public class ProductTranslationDto
    {
        [JsonPropertyName("IdProduct")]
        public int Id{ get; set; }
        [JsonPropertyName("Translations")]
        public Dictionary <string,string> Translations { get; set; }
    }
}
