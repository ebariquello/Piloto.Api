using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests.Domain.Product
{

    [Collection(nameof(ProductCollection))]
    public class ProductServiceTests
    {
        public ProductFixtureTests Fixture { get; set; }

        public ProductServiceTests(ProductFixtureTests fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "Add New Success")]
        [Trait("Category", "Product Service Tests")]
        public async void ProductService_AddNew_ShouldAddWithSuccess()
        {
            // Arrange
            var serviceProduct = Fixture.GetServiceProduct();
            var product = Fixture.GetValidProduct();

            // Act
            await serviceProduct.AddAsync(product);

            // Assert
            Fixture.RepositoryProductMock.Verify(r => r.AddAsync(product), Times.Once);

        }

        [Fact(DisplayName = "Add New Fail")]
        [Trait("Category", "Product Service Tests")]
        public async  void ProductService_AddNew_ShouldNotAddWithSuccess()
        {
            // Arrange
            var serviceProduct = Fixture.GetServiceProduct();
            var product = Fixture.GetInvalidProduct();

            // Act
            await serviceProduct.AddAsync(product);

            // Assert
            Fixture.RepositoryProductMock.Verify(r => r.AddAsync(product), Times.Never);

        }

        [Fact(DisplayName = "Get All Products")]
        [Trait("Category", "Product Service Tests")]
        public async  void CustomerService_GetAll_ShouldReturnsMoreThan1()
        {
            // Arrange
            var serviceProduct = Fixture.GetServiceProduct();
            Fixture.RepositoryProductMock.Setup(c => c.GetAsync(null,null,null,true,true)).ReturnsAsync(Fixture.GetMixedProducts());

            // Act
            var products = await serviceProduct.GetAsync();

            // Assert Fluent Assertions
            products.Should().HaveCount(c => c > 1).And.OnlyHaveUniqueItems();

        }
    }
}
