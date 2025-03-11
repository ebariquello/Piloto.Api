using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests.Domain.Supplier
{

    [Collection(nameof(SupplierCollection))]
    public class SupplierServiceTests
    {
        public SupplierTestsFixture Fixture { get; set; }

        public SupplierServiceTests(SupplierTestsFixture fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "Add New Success")]
        [Trait("Category", "Supplier Service Tests")]
        public async  void SupplierService_AddNew_ShouldAddWithSuccess()
        {
            // Arrange
            var serviceSupplier = Fixture.GetServiceSupplier();
            var supplier = Fixture.GetValidSupplier();

            // Act
            await serviceSupplier.AddAsync(supplier);

            // Assert
            Fixture.RepositorySupplierMock.Verify(r => r.AddAsync(supplier), Times.Once);
          
        }

        [Fact(DisplayName = "Add New Fail")]
        [Trait("Category", "Supplier Service Tests")]
        public async void SupplierService_AddNew_ShouldNotAddWithSuccess()
        {
            // Arrange
            var serviceSupplier = Fixture.GetServiceSupplier();
            var supplier = Fixture.GetInvalidSupplier();

            // Act
            await serviceSupplier.AddAsync(supplier);

            // Assert
            Fixture.RepositorySupplierMock.Verify(r => r.AddAsync(supplier), Times.Never);
           
        }

        [Fact(DisplayName = "Get All Suppliers")]
        [Trait("Category", "Supplier Service Tests")]
        public async  void CustomerService_GetAll_ShouldReturnsMoreThan1()
        {
            // Arrange
            var serviceSupplier = Fixture.GetServiceSupplier();
            Fixture.RepositorySupplierMock.Setup(c => c.GetAsync(null,null,null,true,true)).ReturnsAsync(Fixture.GetMixedSuppliers());

            // Act
            var suppliers = await serviceSupplier.GetAsync();

            // Assert Fluent Assertions
            suppliers.Should().HaveCount(c => c > 1).And.OnlyHaveUniqueItems();
           
        }
    }
}
