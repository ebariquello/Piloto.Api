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

namespace Piloto.Api.UnitTests.Domain.ProductSupplier
{
    [CollectionDefinition(nameof(ProductSupplierCollection))]
    public class ProductSupplierCollection : ICollectionFixture<ProductSupplierTestsFixture>
    {
    }

    public class ProductSupplierTestsFixture : IDisposable
    {
        public Mock<IRepositoryProductSupplier> RepositoryProductSupplierMock { get; set; }
        public Mock<IServiceProductSupplier> ServiceProductSupplierMock { get; set; }


        public ServiceProductSupplier GetServiceProductSupplier()
        {
            var mocker = new AutoMoqCore.AutoMoqer();
            mocker.Create<ServiceProduct>();

            var ServiceProductSupplier = mocker.Resolve<ServiceProductSupplier>();

            RepositoryProductSupplierMock = mocker.GetMock<IRepositoryProductSupplier>();
            ServiceProductSupplierMock = mocker.GetMock<IServiceProductSupplier>();


            return ServiceProductSupplier;
        }

        public Models.ProductSupplier GetValidProductSupplier()
        {
            return GenerateProductSuppliers(1).First();
        }

        public Models.ProductSupplier GetInvalidProductSupplier()
        {
            var ProductTests = new Faker<Models.ProductSupplier>("pt_BR")
                .CustomInstantiator(
                    f => new Models.ProductSupplier(
                    null,
                    f.Random.Int(1, int.MaxValue),
                    null,
                    0,
                    null
                    ));

            return ProductTests;
        }
       
        public ICollection<Models.ProductSupplier> GetMixedProducts()
        {
            var productsSuppliers = new List<Models.ProductSupplier>();

            productsSuppliers.AddRange(GenerateProductSuppliers(50).ToList());


            return productsSuppliers;
        }
        public void Dispose()
        {
            // Dispose what you have!
        }




        private ICollection<Models.ProductSupplier> GenerateProductSuppliers(int number)
        {
            var productsTests = new Faker<Models.ProductSupplier>("pt_BR")
               .CustomInstantiator(
                   f => new Models.ProductSupplier(
                   f.Random.Int(1, int.MaxValue),
                   f.Random.Int(1, int.MaxValue),
                   null,
                   f.Random.Int(1, int.MaxValue),
                   null
                   ));

            return productsTests.Generate(number);
        }


    }
}
