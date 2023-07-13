using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductManagement.Contracts.DomainEntities
{
    public class Product
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public int Rate { get; set; }

        public bool Avalidable { get; set; }
    }
}
