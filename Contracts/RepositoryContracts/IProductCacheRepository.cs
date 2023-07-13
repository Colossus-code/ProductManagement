using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Contracts.RepositoryContracts
{
    public interface IProductCacheRepository
    {
        List<T> GetCache<T>(string key);

        void SetCache<T>(string key, List<T> generic);
    }
}
