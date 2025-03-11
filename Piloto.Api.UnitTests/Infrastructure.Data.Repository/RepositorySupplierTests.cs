using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Infrastructure.Data;
using Piloto.Api.Infrastructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piloto.Api.Domain.Models;

namespace Piloto.Api.UnitTests.Infrastructure.Data.Repository
{
    [Collection(nameof(RepositoryCollection))]
    public class RepositorySupplierTests
    {
        public RepositoryTestsFixture Fixture { get; set; }

        public RepositorySupplierTests(RepositoryTestsFixture fixture)
        {
            Fixture = fixture;
        }
        [Fact]
        public async void Add_When_Has_No_Suppliers()
        {
            ///Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IUnitOfWork<DbContext> unitOfWork =
                serviceScope.ServiceProvider.GetService<IUnitOfWork<DbContext>>();
     
            IRepositorySupplier repoSupplier = serviceScope.ServiceProvider.GetService<IRepositorySupplier>();

            var supplier = Fixture.GetSupplierHasAddressHasNoProducts();

            /// Act
            var savedSupplierResultAdd = await repoSupplier.AddAsync(supplier);
            await unitOfWork.SaveChangeAsync();

            var findLastSavedSupplier = await repoSupplier
                .FindAsync(p => supplier.Name == p.Name &&
                supplier.CNPJ == p.CNPJ &&
                p.SupplierAddresses.Count == 1&& 
                p.ProductSuppliers.Count == 0);

            var suppliers = await repoSupplier.GetAsync();
            /// Assert
            suppliers.Should().HaveCount(c => c >= 1).And.OnlyHaveUniqueItems();
            Assert.Single(findLastSavedSupplier);
            var lastSavedSupplier = findLastSavedSupplier.FirstOrDefault();
            Assert.Equal(savedSupplierResultAdd.Id, lastSavedSupplier.Id);
            Assert.Equal(supplier.Name, lastSavedSupplier.Name);
            Assert.Equal(supplier.CNPJ, lastSavedSupplier.CNPJ);
          
            Assert.Null(lastSavedSupplier.ProductSuppliers);
        }
        [Fact]
        public async void Update_When_Has_No_Products_To_Has_Products()
        {
            ///Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IUnitOfWork<DbContext> unitOfWork =
                serviceScope.ServiceProvider.GetService<IUnitOfWork<DbContext>>();
        
            IRepositorySupplier repoSupplier = serviceScope.ServiceProvider.GetService<IRepositorySupplier>();

            var supplier = Fixture.GetSupplierHasAddressHasNoProducts();
            var supplier2 = Fixture.GetSupplierHasAddressHasNoProducts();

            /// Act
            var savedSupplierResultAdd = await repoSupplier.AddAsync(supplier);
            var savedSupplierResultAdd2 = await repoSupplier.AddAsync(supplier2);
            await unitOfWork.SaveChangeAsync(true);

            savedSupplierResultAdd.ProductSuppliers = SharedTestsFixture.GenerateProductsForSupplier(savedSupplierResultAdd, 2);
            var updatedSupplier = await repoSupplier.UpdateAsync(savedSupplierResultAdd);
            await unitOfWork.SaveChangeAsync(true);

            var findLastSavedSupplier = await repoSupplier
                .GetByIdAsync(updatedSupplier.Id.Value, repoSupplier.GetQuery(s=>s.ProductSuppliers));

            var findLastSavedSupplier2 = await repoSupplier
                    .GetByIdAsync(savedSupplierResultAdd2.Id.Value, repoSupplier.GetQuery(s => s.ProductSuppliers));

            /// Assert
            Assert.Equal(savedSupplierResultAdd.Id, findLastSavedSupplier.Id);
            Assert.Equal(supplier.Name, findLastSavedSupplier.Name);
            Assert.Equal(supplier.CNPJ, findLastSavedSupplier.CNPJ);
            Assert.Equal(2, findLastSavedSupplier.ProductSuppliers.Count);
            Assert.Empty(findLastSavedSupplier2.ProductSuppliers);
        }

        [Fact]
        public async void Delete_And_Clear_Supplier_And_Its_Relationships()
        {
            ///Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IUnitOfWork<DbContext> unitOfWork =
                serviceScope.ServiceProvider.GetService<IUnitOfWork<DbContext>>();

            IRepositoryProduct repoProduct = serviceScope.ServiceProvider.GetService<IRepositoryProduct>();
            IRepositorySupplier repoSupplier = serviceScope.ServiceProvider.GetService<IRepositorySupplier>();
            IRepositoryProductSupplier repoSupplierSupplier = serviceScope.ServiceProvider.GetService<IRepositoryProductSupplier>();


            var supplier = Fixture.GetSupplieHasAddressHasProducts();

            /// Act
            var savedSupplierResultAdd = await repoSupplier.AddAsync(supplier);

            await unitOfWork.SaveChangeAsync(true);

            // Find Last Supplier
            var findLastSavedSupplier = await repoSupplier
                .GetByIdAsync(savedSupplierResultAdd.Id.Value);
            //Find Relationship Supplier Suppliers
            var findLastSavedProductSupplierBySupplierId = await repoSupplierSupplier
                .FindAsync(ps => ps.SupplierId == savedSupplierResultAdd.Id.Value);

            //Kill RelationShip
            findLastSavedProductSupplierBySupplierId.ToList().ForEach(async (ps) => await repoSupplierSupplier.RemoveAsync(ps));

            //Kill Supplier
            findLastSavedSupplier.ProductSuppliers.ToList().ForEach(async (ps) => await repoProduct.RemoveAsync(ps.Product));

            //Kill Supplier, Should kill Everything even Addresses
            var result = await repoSupplier.RemoveAsync(findLastSavedSupplier);

            //Commit
            await unitOfWork.SaveChangeAsync();

            //try find the supplier
            findLastSavedSupplier = await repoSupplier
              .GetByIdAsync(savedSupplierResultAdd.Id.Value);

            /// Assert
            Assert.Null(findLastSavedSupplier);

        }


        
    }
}
