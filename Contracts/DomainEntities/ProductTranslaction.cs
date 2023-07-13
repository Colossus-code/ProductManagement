using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductManagement.Contracts.DomainEntities
{
    public class ProductTranslaction
    {

        public int Id { get; set; }

        public Dictionary<string, string> Translations { get; set; }
    }
}
