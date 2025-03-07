using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests.Domain.Product
{
    [Collection(nameof(ProductCollection))]
    public class ProductTests
    {
        public ProductFixtureTests Fixture { get; set; }

        public ProductTests(ProductFixtureTests fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "New Product Valid")]
        [Trait("Category", "Product Tests")]
        public void Product_NewProduct_ShouldBeValid()
        {
            // Arrange
            var Product = Fixture.GetValidProduct();

            // Act
            var result = Product.IsValid();

            // Assert Fluent Assertions (more expressive)
            result.Should().BeTrue();
            Product.ValidationResult.Errors.Should().HaveCount(0);

            // Assert XUnit
            Assert.True(result);
            Assert.Equal(0, Product.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "New Product Invalid")]
        [Trait("Category", "Product Tests")]
        public void Product_NewProduct_ShouldBeInvalid()
        {
            // Arrange
            var Product = Fixture.GetInvalidProduct();

            // Act
            var result = Product.IsValid();

            // Assert Fluent Assertions (more expressive)
            result.Should().BeFalse();
            Product.ValidationResult.Errors.Should().HaveCount(c => c > 0);

            // Assert XUnit
            Assert.False(result);
            Assert.NotEqual(0, Product.ValidationResult.Errors.Count);
        }
    }
}
