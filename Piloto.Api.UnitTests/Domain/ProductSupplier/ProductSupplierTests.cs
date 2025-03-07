using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests.Domain.ProductSupplier
{
    [Collection(nameof(ProductSupplierCollection))]
    public class ProductSupplierTests
    {
        public ProductSupplierTestsFixture Fixture { get; set; }

        public ProductSupplierTests(ProductSupplierTestsFixture fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "New ProductSupplier Valid")]
        [Trait("Category", "ProductSupplier Tests")]
        public void Product_NewProduct_ShouldBeValid()
        {
            // Arrange
            var ProductSupplier = Fixture.GetValidProductSupplier();

            // Act
            var result = ProductSupplier.IsValid();

            // Assert Fluent Assertions (more expressive)
            result.Should().BeTrue();
            ProductSupplier.ValidationResult.Errors.Should().HaveCount(0);

            // Assert XUnit
            Assert.True(result);
            Assert.Equal(0, ProductSupplier.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "New ProductSupplier Invalid")]
        [Trait("Category", "ProductSupplier Tests")]
        public void Product_NewProduct_ShouldBeInvalid()
        {
            // Arrange
            var ProductSupplier = Fixture.GetInvalidProductSupplier();

            // Act
            var result = ProductSupplier.IsValid();

            // Assert Fluent Assertions (more expressive)
            result.Should().BeFalse();
            ProductSupplier.ValidationResult.Errors.Should().HaveCount(c => c > 0);

            // Assert XUnit
            Assert.False(result);
            Assert.NotEqual(0, ProductSupplier.ValidationResult.Errors.Count);
        }
    }
}
