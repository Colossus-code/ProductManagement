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
    public class ProductRepository : IProductRepository
    {
        private readonly string _apiPath;

        private readonly HttpClient _httpClient;

        public ProductRepository(IConfiguration config)
        {
            _apiPath = config.GetSection("AppiCalls:Products").Value;

            _httpClient = new HttpClient();


        }

        public async Task<List<Product>> GetProducts()
        {
            var response = await _httpClient.GetAsync(_apiPath);

            if (!response.IsSuccessStatusCode)
            {
                // AQUI LOGER DE QUE PETA EL SERVIDOR RUTA NO ENCONTRADA 
            }
            string responseAsString = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseAsString)) return new List<Product>(); // AQUI LOG DE NOT FOUND 

            List<ProductDto> productDtos = JsonSerializer.Deserialize<List<ProductDto>>(responseAsString);

            return TransformDtoToEntity(productDtos);
        }

        private List<Product> TransformDtoToEntity(List<ProductDto> productDtos)
        {
            List<Product> productTranslationResponse = new List<Product>();

            productDtos.ForEach(productDto =>
            {
                productTranslationResponse.Add(new Product
                {
                    Id = productDto.Id,
                    Price = productDto.Price,
                    Rate = productDto.Rate,  
                    Avalidable = productDto.Avalidable == "yes" ? true : false
                });
            });

            return productTranslationResponse;
        }
    }
}

