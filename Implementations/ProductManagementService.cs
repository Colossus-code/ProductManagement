using Microsoft.Extensions.Caching.Memory;
using ProductManagement.Contracts.DomainEntities;
using ProductManagement.Contracts.RepositoryContracts;
using ProductManagement.Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Implementations
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTranslationRepository _productTranslationRepository;
        private readonly IProductCacheRepository _productCacheRepository;

        public ProductManagementService(IProductRepository productRepo, IProductTranslationRepository productTranslationRepo, IProductCacheRepository productCacheRepo)
        {
            _productRepository = productRepo;
            _productTranslationRepository = productTranslationRepo;
            _productCacheRepository = productCacheRepo;
            
        }
        public async Task<List<ProductByLenguage>> GetProductList(string lenguage)
        {
            List<ProductByLenguage> productsInCache = _productCacheRepository.GetCache<ProductByLenguage>(lenguage); 

            if(productsInCache != null && productsInCache.Count > 0)
            {
                return productsInCache;
            }

            List<Product> products = await _productRepository.GetProducts();

            List<ProductTranslaction> productTranslactions = await _productTranslationRepository.GetProductsTranslation();

            return await GetProductsByLenguage(products, productTranslactions, lenguage); 
        }
        private async Task<List<ProductByLenguage>> GetProductsByLenguage(List<Product> products, List<ProductTranslaction> productTranslactions, string lenguage)
        {
            List<ProductByLenguage> productByLenguages = new List<ProductByLenguage>(); 

            foreach(Product product in products.Where(e => e.Avalidable == true))
            {
                string productName = productTranslactions.FirstOrDefault(e => e.Id == product.Id).Translations.FirstOrDefault(i => i.Key.Equals(lenguage)).Value;

                if (!string.IsNullOrEmpty(productName))
                {
                    productByLenguages.Add(new ProductByLenguage
                    {
                        Name = productName,
                        Price = product.Price,
                        Rate = product.Rate
                    });
                };
            }

            _productCacheRepository.SetCache<ProductByLenguage>(lenguage, productByLenguages);

            return productByLenguages;
        }

    }
}
