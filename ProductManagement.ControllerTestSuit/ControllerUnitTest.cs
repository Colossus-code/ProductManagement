using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using ProductManagement.Contracts.DomainEntities;
using ProductManagement.Contracts.ServiceContracts;
using ProductManagement.WebApi.Controllers;
using ProductManagement.WebApi.Models;
using System.Net;
using System.Runtime.CompilerServices;

namespace ProductManagement.ControllerTestSuit
{
    public class ControllerUnitTest
    {

        private readonly Mock<IProductManagementService> _serviceMock;
        private readonly ProductManagementController _productController;
        public ControllerUnitTest()
        {
            _serviceMock = new Mock<IProductManagementService>();
           
            _productController = new ProductManagementController( _serviceMock.Object);

        }
        [Fact]
        public async void Assert_CodeStatus404_When_NotProductFound()
        {

            //Arrange
            _serviceMock.Setup(response => response.GetProductList("EN")).ReturnsAsync(new List<ProductByLenguage>());
            var expectedResult = 404;
            var expectedMessage = "Not found product in EN";
            //Act

            var actionResult = await _productController.GetProductsByLenguage("EN");

            ObjectResult response = actionResult as ObjectResult;
            //Assert

            Assert.Equal(expectedResult, response.StatusCode);
            Assert.True(response.Value.Equals(expectedMessage));

        }
        [Fact]
        public async void Assert_CodeStatus500_When_UnexpectedExceptionHasThrown()
        {

            //Arrange
            _serviceMock.Setup(response => response.GetProductList("EN")).Throws(new Exception());
            var expectedResult = 500;
            var expectedMessage = "Unexpected Error";
            //Act

            var actionResult = await _productController.GetProductsByLenguage("EN");

            ObjectResult response = actionResult as ObjectResult;
            //Assert

            Assert.Equal(expectedResult, response.StatusCode);
            Assert.True(response.Value.Equals(expectedMessage));
        }
        [Fact]
        public async void Assert_CodeStatus200_When_EverythingGoneOkay()
        {

            //Arrange

            List<ProductByLenguage> productsReturned = new List<ProductByLenguage>
            {
                new ProductByLenguage
                {
                    Name = "Test product",
                    Price = 1,
                    Rate = 10
                }
            };

            ProductModel expectedMessage = new ProductModel
            {
                Name = productsReturned.First().Name,
                Price = "1.1$",
                Rate = "10/10"
            };

            _serviceMock.Setup(response => response.GetProductList(It.IsAny<string>())).ReturnsAsync(productsReturned);
            
            var expectedResult = 200;
            //Act

            var actionResult = await _productController.GetProductsByLenguage("EN");

            ObjectResult response = actionResult as ObjectResult;

            List<ProductModel> responseProduct = response.Value as List<ProductModel>;

            //Assert

            Assert.Equal(expectedResult, response.StatusCode);
            Assert.NotNull(response.Value);
            Assert.True(responseProduct.First().Name == expectedMessage.Name);
        }
    }
}