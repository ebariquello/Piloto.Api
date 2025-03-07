using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Models;
using Piloto.Api.Infrastructure.Data;
using Piloto.Api.Infrastructure.Data.Repository;
using Piloto.Api.Infrastructure.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Piloto.Api.UnitTests.Infrastructure.Data.Repository
{
    [CollectionDefinition(nameof(RepositoryCollection))]
    public class RepositoryCollection : ICollectionFixture<RepositoryTestsFixture>
    {
    }
    public class RepositoryTestsFixture
    {

        public IServiceProvider ServiceProvider { get; set; }
        public RepositoryTestsFixture()
        {
            var services = new ServiceCollection();
            var builder = new DbContextOptionsBuilder<StockManagementDBContext>();
            DbContextOptions<StockManagementDBContext> dbContextOptions = new DbContextOptionsBuilder<StockManagementDBContext>()
           .UseInMemoryDatabase(databaseName: "StockManagementDB")
           .Options;

            services.AddDbContext<StockManagementDBContext>(opt => opt.UseInMemoryDatabase(databaseName: "StockManagementDB"), ServiceLifetime.Scoped);
            services.AddScoped<Func<DbContext>>(
                (provider) =>
                    () => provider.GetService<StockManagementDBContext>()
                );
            services.AddScoped(typeof(IDbFactoryBase<>),
                typeof(DbFactoryBase<>));

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));


            services.AddScoped(typeof(IRepositoryProduct), typeof(RepositoryProduct));
            services.AddScoped(typeof(IRepositorySupplier), typeof(RepositorySupplier));
            services.AddScoped(typeof(IRepositoryProductSupplier), typeof(RepositoryProductSupplier));

            ServiceProvider = services.BuildServiceProvider();
          
        }
        public Supplier GetSupplierHasAddressHasNoProducts()
        {
            var suppliersTests = new Faker<Supplier>("pt_BR")
               .CustomInstantiator(
                   f => new Supplier(
                       null,
                       f.Company.CompanyName(),
                       f.Company.Cnpj(),
                       null,
                       SharedTestsFixture.GenerateSupplierAddresses(1, null)
                       )
                 );

            return suppliersTests.Generate();
        }
        public Supplier GetSupplieHasAddressHasProducts()
        {
            var suppliersTests = new Faker<Supplier>("pt_BR")
               .CustomInstantiator(
                   f => new Supplier(
                       null,
                       f.Company.CompanyName(),
                       f.Company.Cnpj(),
                       null,
                       SharedTestsFixture.GenerateSupplierAddresses(1, null)
                       )
                 );

            var result = suppliersTests.Generate();
            result.ProductSuppliers = SharedTestsFixture.GenerateProductsForSupplier(result, 2);
            return result;
        }
        public Product GetProductHasSuppliersHasAddress()
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
            product.ProductSuppliers = SharedTestsFixture.GenerateSuppliersForProduct(product, 2, true, 1);
            return product;
        }





    }
}
