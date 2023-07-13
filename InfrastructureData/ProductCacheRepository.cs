using Microsoft.Extensions.Caching.Memory;
using ProductManagement.Contracts.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.InfrastructureData
{
    public class ProductCacheRepository : IProductCacheRepository
    {
        private readonly IMemoryCache _memoryCache;

        public ProductCacheRepository(IMemoryCache memoryCache) // CONSTRUCTOR
        {
            _memoryCache = memoryCache;

        }

        public List<T> GetCache<T>(string key)
        {
            var response = _memoryCache.Get(key);

            return (List<T>)response;
        }

        public void SetCache<T>(string key, List<T> generic) // LE PONEMOS LA KEY QUE QUERAMOS 
        {
            _memoryCache.Set(key, generic);
        }
    }
}
