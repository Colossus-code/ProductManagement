using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProductManagement.Contracts.DomainEntities;
using ProductManagement.Contracts.ServiceContracts;
using ProductManagement.WebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductManagementController : ControllerBase
    {

        private readonly IProductManagementService _productManagementService;
        public ProductManagementController(IProductManagementService productManage)
        {

            _productManagementService = productManage;

        }

        [HttpGet]
        [Route("GetProductByName")]
        public async Task<IActionResult> GetProductsByLenguage([Required] string lenguage)
        {
            List<ProductModel> response = new List<ProductModel>();

            try
            {
                List<ProductByLenguage> productDomainEntity = await _productManagementService.GetProductList(lenguage.ToUpper());

                if (productDomainEntity.Count == 0)
                {
                    // Aqui el LOG 
                    return StatusCode(404, $"Not found product in {lenguage.ToUpper()}");
                }
                productDomainEntity.ForEach(product =>
                {
                    if (lenguage == "EN")
                    {
                        product.Price = product.Price * 1.10;

                        var price = Math.Round(product.Price, 2);

                        response.Add(new ProductModel
                        {
                            Name = product.Name,
                            Price = $"{price}$",
                            Rate = $"{product.Rate}/10"

                        });
                    }
                    else
                    {
                        response.Add(new ProductModel
                        {
                            Name = product.Name,
                            Price = $"{product.Price}€",
                            Rate = $"{product.Rate}/10"
                        });
                    }

                });
           
                // Si se quiere se puede aplicar como JSon. 

                return Ok(response);

            }
            catch (Exception ex)
            {
                // Aqui el LOG con EX
                return StatusCode(500, "Unexpected Error");
            }

        }
    }
}
