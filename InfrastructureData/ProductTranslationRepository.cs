using Microsoft.Extensions.Configuration;
using ProductManagement.Contracts.DomainEntities;
using ProductManagement.Contracts.RepositoryContracts;
using ProductManagement.InfrastructureData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductManagement.InfrastructureData
{
    public class ProductTranslationRepository : IProductTranslationRepository
    {
        private readonly string _apiPath;

        private readonly HttpClient _httpClient;

        public ProductTranslationRepository(IConfiguration config)
        {
            _apiPath = config.GetSection("AppiCalls:ProductsTranslation").Value;

            _httpClient = new HttpClient();


        }

        public async Task<List<ProductTranslaction>> GetProductsTranslation()
        {
            var response = await _httpClient.GetAsync(_apiPath);
            
            string responseAsString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseAsString)) return new List<ProductTranslaction>(); // AQUI LOG DE NOT FOUND 

            List<ProductTranslationDto> productTranslationDtos = JsonSerializer.Deserialize<List<ProductTranslationDto>>(responseAsString);

            return TransformDtoToEntity(productTranslationDtos);
        }

        private List<ProductTranslaction> TransformDtoToEntity(List<ProductTranslationDto> productTranslationDtos)
        {
            List<ProductTranslaction> productTranslationResponse = new List<ProductTranslaction>();

            productTranslationDtos.ForEach(productTranslationDto =>
            {
                productTranslationResponse.Add(new ProductTranslaction
                {
                    Id = productTranslationDto.Id,
                    Translations = productTranslationDto.Translations
                });
            });

            return productTranslationResponse;
        }
    }
}
