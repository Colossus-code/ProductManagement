using ProductManagement.Contracts.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Contracts.RepositoryContracts
{
    public interface IProductTranslationRepository
    {
        Task<List<ProductTranslaction>> GetProductsTranslation();
    }
}
