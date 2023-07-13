using Microsoft.Extensions.Caching.Memory;
using Moq;
using ProductManagement.Contracts.DomainEntities;
using ProductManagement.Contracts.RepositoryContracts;
using ProductManagement.Implementations;

namespace ProductManagement.ServiceTestSuit
{
    public class ServiceUnitTest
    {
        private readonly Mock<IProductRepository> _repositoryProductMock;
        private readonly Mock<IProductTranslationRepository> _repositoryTranslationMock;
        private readonly Mock<IProductCacheRepository> _repositoryCacheMock;

        private readonly ProductManagementService _productManagementService; 
        public ServiceUnitTest()
        {
            _repositoryProductMock = new Mock<IProductRepository>();
            _repositoryTranslationMock = new Mock<IProductTranslationRepository>();
            _repositoryCacheMock = new Mock<IProductCacheRepository>();

            _productManagementService = new ProductManagementService(_repositoryProductMock.Object, _repositoryTranslationMock.Object, _repositoryCacheMock.Object);

        }
        [Fact]
        public async void Assert_NotEmpty_When_ProductsFoundInCache()
        {

            //Arrange
            List<ProductByLenguage> productsInCache = GetProductByLenguages();
            
            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(productsInCache);

            //Act

            var response = await _productManagementService.GetProductList("ES");

            //Assert

            Assert.NotEmpty(response);   

        }            
        [Fact]
        public async void Assert_NameEqualAsExpectedOnes_When_ProductsFoundInCache()
        {

            //Arrange
            List<ProductByLenguage> productsInCache = GetProductByLenguages();
            
            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(productsInCache);

            //Act

            var response = await _productManagementService.GetProductList("ES");

            //Assert

            Assert.Equal(productsInCache.First().Name, response.First().Name);   

        }    
        [Fact]
        public async void Assert_NotEmpty_When_ProductsFound()
        {

            //Arrange
            var productList = GetProducts();

            var translationList = GetTranslactions();

            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(new List<ProductByLenguage>());

            _repositoryProductMock.Setup(e => e.GetProducts()).ReturnsAsync(productList);

            _repositoryTranslationMock.Setup(e => e.GetProductsTranslation()).ReturnsAsync(translationList);         

            //Act

            var response = await _productManagementService.GetProductList("ES");

            //Assert

            Assert.NotEmpty(response);   

        }        
        [Fact]
        public async void Assert_ProductNameEquals_When_ProductIsWhatWeExpected()
        {

            //Arrange
            var productList = GetProducts();

            var translationList = GetTranslactions();

            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(new List<ProductByLenguage>());

            _repositoryProductMock.Setup(e => e.GetProducts()).ReturnsAsync(productList);

            _repositoryTranslationMock.Setup(e => e.GetProductsTranslation()).ReturnsAsync(translationList);

            List<ProductByLenguage> expectedProductList = GetProductByLenguages();
            //Act

            var response = await _productManagementService.GetProductList("ES");

            //Assert

            Assert.Equal(response.First().Name , expectedProductList.First().Name);   

        }

        [Fact]
        public async void Assert_Empty_When_ProductByLanguageNotFound()
        {

            //Arrange
            var productList = GetProducts();

            var translationList = GetTranslactions();

            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(new List<ProductByLenguage>());

            _repositoryProductMock.Setup(e => e.GetProducts()).ReturnsAsync(productList);

            _repositoryTranslationMock.Setup(e => e.GetProductsTranslation()).ReturnsAsync(translationList);

            List<ProductByLenguage> expectedProductList = GetProductByLenguages();
            //Act

            var response = await _productManagementService.GetProductList("FR");

            //Assert

            Assert.Empty(response);

        }

        [Fact]
        public async void Assert_Empty_When_NotFoundAvalidableProduct()
        {

            //Arrange
            var productList = GetNoAvalidableProducts();

            var translationList = GetTranslactions();

            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(new List<ProductByLenguage>());

            _repositoryProductMock.Setup(e => e.GetProducts()).ReturnsAsync(productList);

            _repositoryTranslationMock.Setup(e => e.GetProductsTranslation()).ReturnsAsync(translationList);

            List<ProductByLenguage> expectedProductList = GetProductByLenguages();
            //Act

            var response = await _productManagementService.GetProductList("EN");

            //Assert

            Assert.Empty(response);

        }

        [Fact]
        public async void Assert_Exception_When_NotFoundProductRootPath()
        {

            //Arrange
            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(new List<ProductByLenguage>());

            _repositoryProductMock.Setup(e => e.GetProducts()).Throws(new Exception());

            List<ProductByLenguage> expectedProductList = GetProductByLenguages();
            
            //Act && Assert

            Assert.ThrowsAsync<Exception>(() => _productManagementService.GetProductList("EN"));

        } 
        [Fact]
        public async void Assert_Exception_When_NotFoundTranslationsRootPath()
        {

            //Arrange

            _repositoryCacheMock.Setup(e => e.GetCache<ProductByLenguage>(It.IsAny<string>())).Returns(new List<ProductByLenguage>());

            _repositoryTranslationMock.Setup(e => e.GetProductsTranslation()).Throws(new Exception());

            List<ProductByLenguage> expectedProductList = GetProductByLenguages();
            
            //Act && Assert

            Assert.ThrowsAsync<Exception>(() => _productManagementService.GetProductList("EN"));

        }

        private List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Avalidable = true,
                    Price = 2.89,
                    Rate = 10
                }
            };
        }

        private List<Product> GetNoAvalidableProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Avalidable = false,
                    Price = 2.89,
                    Rate = 10
                }
            };
        }

        private List<ProductTranslaction> GetTranslactions()
        {
            List<ProductTranslaction> productTranslactions = new List<ProductTranslaction>
            {
                new ProductTranslaction
                {
                    Id = 1,
                    Translations = new Dictionary<string, string>()

                }

            };

            var productTranslation = productTranslactions.FirstOrDefault();

            productTranslation.Translations.Add("ES", "Test product");

            return productTranslactions;
        }

        private List<ProductByLenguage> GetProductByLenguages()
        {
            return new List<ProductByLenguage>
            {
                new ProductByLenguage
                {
                    Name = "Test product",
                    Price = 2.89,
                    Rate = 10
                }
            };
        }
    }
}