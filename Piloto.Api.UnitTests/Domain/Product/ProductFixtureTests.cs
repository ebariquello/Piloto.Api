using Moq;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Domain.Services.Services;
using Models = Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;

namespace Piloto.Api.UnitTests.Domain.Product
{
    [CollectionDefinition(nameof(ProductCollection))]
    public class ProductCollection : ICollectionFixture<ProductFixtureTests>
    {
    }

    public class ProductFixtureTests : IDisposable
    {
        public Mock<IRepositoryProduct> RepositoryProductMock { get; set; }
        public Mock<IServiceProduct> ServiceProductMock { get; set; }


        public ServiceProduct GetServiceProduct()
        {
            var mocker = new AutoMoqCore.AutoMoqer();
            mocker.Create<ServiceProduct>();

            var ServiceProduct = mocker.Resolve<ServiceProduct>();

            RepositoryProductMock = mocker.GetMock<IRepositoryProduct>();
            ServiceProductMock = mocker.GetMock<IServiceProduct>();


            return ServiceProduct;
        }

        public Models.Product GetValidProduct()
        {
            return GenerateOneProduct();
        }

        public Models.Product GetInvalidProduct()
        {
            var ProductTests = new Faker<Models.Product>("pt_BR")
                .CustomInstantiator(
                    f => new Models.Product(
                    null,
                    "",
                    f.Commerce.Random.Int(),
                    f.Random.Float(),
                    null
                    ));

            return ProductTests;
        }
        public ICollection<Models.Product> GetProducts(int number)
        {
            var productsTests = new Faker<Models.Product>("pt_BR")
                .CustomInstantiator(
                    f => new Models.Product(
                    f.IndexVariable++,
                    f.Commerce.ProductName(),
                    f.Random.Int(),
                    (float)f.Finance.Amount(1,100),
                    null
                    ));

            var result = productsTests.Generate(number);
            result.ForEach(p => p.ProductSuppliers = SharedTestsFixture.GenerateSuppliersForProduct(p, 2));
            return result;
        }
        public ICollection<Models.Product> GetMixedProducts()
        {
            var products = new List<Models.Product>();

            products.AddRange(GenerateProducts(50).ToList());
            products.AddRange(GenerateProducts(50).ToList());

            return products;
        }
        public void Dispose()
        {
            // Dispose what you have!
        }
        private ICollection<Models.Supplier> GenerateSuppliersForProduct(int number, int productId)
        {
            var productsTests = new Faker<Models.Supplier>("pt_BR")
                .CustomInstantiator(
                    f => new Models.Supplier(
                        f.IndexVariable++,
                        f.Company.CompanyName(),
                        f.Company.Cnpj(),
                        null,
                        GenerateSupplierAddresses(2, f.IndexVariable)
                        )
                  );

            return productsTests.Generate(number);
        }



        public ICollection<Models.SupplierAddress> GenerateSupplierAddresses(int number, int? supplierId)
        {
            var productsTests = new Faker<Models.SupplierAddress>("pt_BR")
                .CustomInstantiator(
                    f => new Models.SupplierAddress(
                        null,
                        f.Random.ArrayElement(new string[] { "Filial", "Matrix", "Stock" }),
                        f.Address.Country(),
                        f.Address.City(),
                        f.Address.ZipCode(),
                        f.Address.StreetAddress(true),
                        f.Address.BuildingNumber(),
                        supplierId,
                        null
                        )
                  );

            return productsTests.Generate(number);
        }

        private ICollection<Models.Product> GenerateProducts(int number)
        {
            var productsTests = new Faker<Models.Product>("pt_BR")
               .CustomInstantiator(
                   f => new Models.Product(
                   f.IndexGlobal,
                   f.Commerce.ProductName(),
                   f.Random.Int(),
                   (float)f.Finance.Amount(1, 100),
                   null
                   ));

            return productsTests.Generate(number);
        }
        private Models.Product GenerateOneProduct()
        {
            var productsTests = new Faker<Models.Product>("pt_BR")
               .CustomInstantiator(
                   f => new Models.Product(
                   f.Random.Int(1, int.MaxValue),
                   f.Commerce.ProductName(),
                   f.Random.Int(1,int.MaxValue),
                   (float)f.Finance.Amount(1, 100),
                   null
                   ));

            return productsTests.Generate();
        }


    }
}
