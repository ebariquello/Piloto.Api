using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests.Domain.Supplier
{
    [Collection(nameof(SupplierCollection))]
    public class SupplierTests
    {
        public SupplierTestsFixture Fixture { get; set; }

        public SupplierTests(SupplierTestsFixture fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "New Supplier Valid")]
        [Trait("Category", "Supplier Tests")]
        public void Supplier_NewSupplier_ShouldBeValid()
        {
            // Arrange
            var Supplier = Fixture.GetValidSupplier();

            // Act
            var result = Supplier.IsValid();

            // Assert Fluent Assertions (more expressive)
            result.Should().BeTrue();
            Supplier.ValidationResult.Errors.Should().HaveCount(0);

            // Assert XUnit
            Assert.True(result);
            Assert.Equal(0, Supplier.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "New Supplier Invalid")]
        [Trait("Category", "Supplier Tests")]
        public void Supplier_NewSupplier_ShouldBeInvalid()
        {
            // Arrange
            var Supplier = Fixture.GetInvalidSupplier();

            // Act
            var result = Supplier.IsValid();

            // Assert Fluent Assertions (more expressive)
            result.Should().BeFalse();
            Supplier.ValidationResult.Errors.Should().HaveCount(c => c > 0);

            // Assert XUnit
            Assert.False(result);
            Assert.NotEqual(0, Supplier.ValidationResult.Errors.Count);
        }
    }
}
