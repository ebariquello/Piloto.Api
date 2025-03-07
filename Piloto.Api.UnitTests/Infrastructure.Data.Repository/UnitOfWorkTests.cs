//using FluentAssertions;
//using Microsoft.Extensions.DependencyInjection;
//using Piloto.Api.Domain.Core.Interfaces.Repositories;
//using Piloto.Api.Infrastructure.Data;
//using Piloto.Api.Infrastructure.Data.Repository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq; 
//namespace Piloto.Api.UnitTests.Infrastructure.Data.Repository
//{
//    public class UnitOfWorkTests
//    {
//        public RepositoryTestsFixture Fixture { get; set; }

//        public UnitOfWorkTests(RepositoryTestsFixture fixture)
//        {
//            Fixture = fixture;
//        }
//        [Fact]
//        public async void Add_When_Has_No_Suppliers()
//        {
//            ///Arrange
//            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
//            IUnitOfWork<MySQLStockManagementDBContext> unitOfWork =
//                serviceScope.ServiceProvider.GetService<IUnitOfWork<MySQLStockManagementDBContext>>();
//            IRepositoryProduct repoProduct = serviceScope.ServiceProvider.GetService<IRepositoryProduct>();

//            var product = Fixture.Get_Valid_Product_Has_No_Suppliers();

//            /// Act
//            var savedProductResultAdd = await repoProduct.Add(product);
//            await unitOfWork.SaveChangeAsync();

//            var findLastSavedProduct = await repoProduct
//                .Find(p => product.Name == p.Name &&
//                product.Price == p.Price &&
//                product.Stock == p.Stock &&
//                p.ProductSuppliers.Count == 0);

//            var products = await repoProduct.GetAll();
//            /// Assert
//            unitOfWork.Verify();
          
//        }
//    }
//}
