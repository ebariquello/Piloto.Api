using Bogus;
using Bogus.Extensions.Brazil;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.UnitTests
{
    public class SharedTestsFixture
    {

        public static ICollection<SupplierAddress> GenerateSupplierAddresses(int number, Supplier supplier)
        {
            var productsTests = new Faker<SupplierAddress>("pt_BR")
                .CustomInstantiator(
                    f => new SupplierAddress(
                        null,
                        f.Random.ArrayElement(new string[] { "Filial", "Matrix", "Stock" }),
                        f.Address.Country(),
                        f.Address.City(),
                        f.Address.ZipCode(),
                        f.Address.StreetAddress(true),
                        f.Address.BuildingNumber(),
                        supplier != null && supplier.Id.HasValue ? supplier.Id : null,
                        null
                        )
                  );

            return productsTests.Generate(number);
        }
        public static ICollection<Supplier> GenerateSuppliers(int number,
         bool hasAddress = false,
         int qtdeAddresses = 0)
        {
            var productSuppliersTests = new Faker<Supplier>("pt_BR")
                .CustomInstantiator(
                    f => new Supplier(
                        null,
                        f.Company.CompanyName(),
                        f.Company.Cnpj(),
                        null,
                        hasAddress ? GenerateSupplierAddresses(qtdeAddresses, null) : null
                        )
                  );

            return productSuppliersTests.Generate(number);
        }
        public static ICollection<ProductSupplier> GenerateSuppliersForProduct(Product product,
           int qtdeSuppliers,
           bool hasAddress = false,
           int qtyAddress= 0)
        {
            var productSuppliersTests = new Faker<ProductSupplier>("pt_BR")
                .CustomInstantiator(
                    f => new ProductSupplier(
                        null,
                        product.Id.HasValue ? product.Id : null,
                        product,
                        null,
                        GenerateSuppliers(1,
                        hasAddress,
                        qtyAddress)
                        .FirstOrDefault()
                        )
                  );

            return productSuppliersTests.Generate(qtdeSuppliers);
        }
        public static ICollection<ProductSupplier> GenerateProductsForSupplier(Supplier supplier,
            int qtyProducts)
        {
            var productSuppliersTests = new Faker<ProductSupplier>("pt_BR")
                .CustomInstantiator(
                    f => new ProductSupplier(
                        null,
                        null,
                        GenerateProducts(1).FirstOrDefault(),
                        supplier != null && supplier.Id.HasValue ? supplier.Id : null,
                        supplier
                        )
                  );

            return productSuppliersTests.Generate(qtyProducts);
        }
        public static ICollection<Product> GenerateProducts(int number,
         bool hasSuppliers = false,
         int qtySuppliers = 0,
         bool hasAddress = false,
         int qtyAddress= 0)
        {
            var productsTests = new Faker<Product>("pt_BR")
             .CustomInstantiator(
                 f => new Product(
                 null,
                 f.Commerce.ProductName(),
                 f.Random.Int(1, int.MaxValue),
                 (float)f.Finance.Amount(1, 100),
                 null
                 ));

            var results = productsTests.Generate(number);
            if (hasSuppliers)
            {
                results.ForEach(p => p.ProductSuppliers = GenerateSuppliersForProduct(p, qtySuppliers, hasAddress, qtyAddress));
               
            }
            return results;
        }

        public static  ICollection<Product> GetProductsHasNoSuppliers(int number)
        {
            var productsTests = new Faker<Product>("pt_BR")
                .CustomInstantiator(
                    f => new Product(
                    null,
                    f.Commerce.ProductName(),
                    f.Random.Int(),
                    (float)f.Finance.Amount(1, 100),
                    null
                    ));

            return productsTests.Generate(number);
        }
        public static Product GetProductHasSuppliersHasNoAddress()
        {
            var productsTests = new Faker<Product>("pt_BR")
             .CustomInstantiator(
                 f => new Product(
                 null,
                 f.Commerce.ProductName(),
                 f.Random.Int(1, int.MaxValue),
                 (float)f.Finance.Amount(1, 100),
                 null
                 ));

            var product = productsTests.Generate();
            product.ProductSuppliers = GenerateSuppliersForProduct(product, 2, false);
            return product;
        }
    }
}
