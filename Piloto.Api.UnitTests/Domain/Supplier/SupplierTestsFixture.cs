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

namespace Piloto.Api.UnitTests.Domain.Supplier
{
    [CollectionDefinition(nameof(SupplierCollection))]
    public class SupplierCollection : ICollectionFixture<SupplierTestsFixture>
    {
    }

    public class SupplierTestsFixture : IDisposable
    {
        public Mock<IRepositorySupplier> RepositorySupplierMock { get; set; }
        public Mock<IServiceSupplier> ServiceSupplierMock { get; set; }


        public ServiceSupplier GetServiceSupplier()
        {
            var mocker = new AutoMoqCore.AutoMoqer();
            mocker.Create<ServiceSupplier>();

            var ServiceSupplier = mocker.Resolve<ServiceSupplier>();

            RepositorySupplierMock = mocker.GetMock<IRepositorySupplier>();
            ServiceSupplierMock = mocker.GetMock<IServiceSupplier>();


            return ServiceSupplier;
        }

        public Models.Supplier GetValidSupplier()
        {
            return GenerateSuppliers(1).First();
        }

        public Models.Supplier GetInvalidSupplier()
        {
            var supplierTests = new Faker<Models.Supplier>("pt_BR")
                .CustomInstantiator(
                    f => new Models.Supplier(
                    null,
                    "",
                    f.Company.Cnpj(),
                    null,
                    null
                    ));

            return supplierTests;
        }
        public ICollection<Models.ProductSupplier> GetProducts(int number, Models.Supplier supplier)
        {
            var productsTests = new Faker<Models.ProductSupplier>("pt_BR")
                .CustomInstantiator(
                    f =>
                    new Models.ProductSupplier(
                        null
                        ,null
                        ,new Models.Product(
                            null,
                            f.Commerce.ProductName(),
                            f.Random.Int(),
                            (float)f.Finance.Amount(1, 100),
                            null
                        )
                        ,null
                        , supplier)
                    );

            return productsTests.Generate(number);
        }
        public ICollection<Models.SupplierAddress> GetSupplierAddresses(int number, int? supplierId)
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
        public ICollection<Models.Supplier> GetMixedSuppliers()
        {
            var customers = new List<Models.Supplier>();

            customers.AddRange(GenerateSuppliers(50).ToList());
            customers.AddRange(GenerateSuppliers(50).ToList());

            return customers;
        }

        private ICollection<Models.Supplier> GenerateSuppliers(int number)
        {
            var supplierTests = new Faker<Models.Supplier>("pt_BR")
               .CustomInstantiator(

                   f => new Models.Supplier(
                   f.IndexGlobal,
                   f.Company.CompanyName(),
                   f.Company.Cnpj(),
                   null,
                   GetSupplierAddresses(2, f.IndexGlobal)
                   ));

            //supplierTests.RuleFor(s => s.Id, f => f.Random.Int());

            var result = supplierTests.Generate(number);
            result.ForEach(s => s.ProductSuppliers = GetProducts(4, s));
            return result; 
        }

        public void Dispose()
        {
            // Dispose what you have!
        }
    }
}
