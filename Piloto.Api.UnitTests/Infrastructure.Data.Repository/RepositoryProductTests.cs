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
    public class RepositoryProductTests
    {
        public RepositoryTestsFixture Fixture { get; set; }

        public RepositoryProductTests(RepositoryTestsFixture fixture)
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

            IRepositoryProduct repoProduct = serviceScope.ServiceProvider.GetService<IRepositoryProduct>();

            var product = SharedTestsFixture.GetProductsHasNoSuppliers(1).FirstOrDefault();

            /// Act
            var savedProductResultAdd = await repoProduct.Add(product);
            await unitOfWork.SaveChangeAsync();

            var findLastSavedProduct = await repoProduct
                .Find(p => product.Name == p.Name &&
                product.Price == p.Price &&
                product.Stock == p.Stock &&
                p.ProductSuppliers.Count == 0);

            var products = await repoProduct.GetAll();
            /// Assert
            products.Should().HaveCount(c => c >= 1).And.OnlyHaveUniqueItems();
            Assert.Single(findLastSavedProduct);
            var lastSavedProduct = findLastSavedProduct.FirstOrDefault();
            Assert.Equal(savedProductResultAdd.Id, lastSavedProduct.Id);
            Assert.Equal(product.Name, lastSavedProduct.Name);
            Assert.Equal(product.Price, lastSavedProduct.Price);
            Assert.Equal(product.Stock, lastSavedProduct.Stock);
            Assert.Null(lastSavedProduct.ProductSuppliers);
        }
        [Fact]
        public async void Update_When_Has_No_ProductSupplier_To_Has_ProcutSupplier()
        {
            ///Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IUnitOfWork<DbContext> unitOfWork =
                serviceScope.ServiceProvider.GetService<IUnitOfWork<DbContext>>();

            IRepositoryProduct repoProduct = serviceScope.ServiceProvider.GetService<IRepositoryProduct>();

            var product = SharedTestsFixture.GetProductsHasNoSuppliers(1).FirstOrDefault();

            /// Act
            var savedProductResultAdd = await repoProduct.Add(product);

            await unitOfWork.SaveChangeAsync();

            savedProductResultAdd.ProductSuppliers = SharedTestsFixture.GenerateSuppliersForProduct(savedProductResultAdd, 2, false);
            var updatedProduct = await repoProduct.Update(savedProductResultAdd);
            await unitOfWork.SaveChangeAsync();

            var findLastSavedProduct = await repoProduct
                .GetById(updatedProduct.Id.Value);

            /// Assert
            Assert.Equal(savedProductResultAdd.Id, findLastSavedProduct.Id);
            Assert.Equal(product.Name, findLastSavedProduct.Name);
            Assert.Equal(product.Price, findLastSavedProduct.Price);
            Assert.Equal(product.Stock, findLastSavedProduct.Stock);
            Assert.Equal(2, findLastSavedProduct.ProductSuppliers.Count);
        }

        [Fact]
        public async void Delete_And_Clear_Product_And_Its_Relationships()
        {
            ///Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IUnitOfWork<DbContext> unitOfWork =
                serviceScope.ServiceProvider.GetService<IUnitOfWork<DbContext>>();

            IRepositoryProduct repoProduct = serviceScope.ServiceProvider.GetService<IRepositoryProduct>();
            IRepositorySupplier repoSupplier = serviceScope.ServiceProvider.GetService<IRepositorySupplier>();
            IRepositoryProductSupplier repoProductSupplier = serviceScope.ServiceProvider.GetService<IRepositoryProductSupplier>();


            var product = Fixture.GetProductHasSuppliersHasAddress();

            /// Act
            var savedProductResultAdd = await repoProduct.Add(product);

            await unitOfWork.SaveChangeAsync();

            // Find Last Product
            var findLastSavedProduct = await repoProduct
                .GetById(savedProductResultAdd.Id.Value);
            // Find Relationship Product Suppliers
            var findLastSavedProductSupplierByProductId = await repoProductSupplier
                .Find(ps => ps.ProductId == savedProductResultAdd.Id.Value);

            // Kill RelationShip
            findLastSavedProductSupplierByProductId.ToList().ForEach(async (ps) => await repoProductSupplier.Remove(ps));

            //Kill Supplier
            findLastSavedProduct.ProductSuppliers.ToList().ForEach(async (s) => await repoSupplier.Remove(s.Supplier));

            //Kill Product
            var result = await repoProduct.Remove(findLastSavedProduct);

            //Commit
            await unitOfWork.SaveChangeAsync();

            //try find the product
            findLastSavedProduct = await repoProduct
              .GetById(savedProductResultAdd.Id.Value);

            /// Assert
            Assert.Null(findLastSavedProduct);

        }


        
    }
}
