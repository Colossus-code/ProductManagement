using ProductManagement.Contracts.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Contracts.ServiceContracts
{
    public interface IProductManagementService
    {
        Task<List<ProductByLenguage>> GetProductList(string lenguage);
    }
}
