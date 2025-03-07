using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests.Domain.ProductSupplier
{

    [Collection(nameof(ProductSupplierCollection))]
    public class ProductSupplierServiceTests
    {
        public ProductSupplierTestsFixture Fixture { get; set; }

        public ProductSupplierServiceTests(ProductSupplierTestsFixture fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "Add New Success")]
        [Trait("Category", "Product Service Tests")]
        public async  void ProductService_AddNew_ShouldAddWithSuccess()
        {
            // Arrange
            var serviceProduct = Fixture.GetServiceProductSupplier();
            var Product = Fixture.GetValidProductSupplier();

            // Act
            await serviceProduct.Add(Product);

            // Assert
            Fixture.RepositoryProductSupplierMock.Verify(r => r.Add(Product), Times.Once);

        }

        [Fact(DisplayName = "Add New Fail")]
        [Trait("Category", "Product Service Tests")]
        public async  void ProductService_AddNew_ShouldNotAddWithSuccess()
        {
            // Arrange
            var serviceProduct = Fixture.GetServiceProductSupplier();
            var Product = Fixture.GetInvalidProductSupplier();

            // Act
            await serviceProduct.Add(Product);

            // Assert
            Fixture.RepositoryProductSupplierMock.Verify(r => r.Add(Product), Times.Never);

        }

        [Fact(DisplayName = "Get All Products")]
        [Trait("Category", "Product Service Tests")]
        public async void CustomerService_GetAll_ShouldReturnsMoreThan1()
        {
            // Arrange
            var serviceProduct = Fixture.GetServiceProductSupplier();
            Fixture.RepositoryProductSupplierMock.Setup(c => c.GetAll()).ReturnsAsync(Fixture.GetMixedProducts());

            // Act
            var Products = await serviceProduct.GetAll();

            // Assert Fluent Assertions
            Products.Should().HaveCount(c => c > 1).And.OnlyHaveUniqueItems();

        }
    }
}
