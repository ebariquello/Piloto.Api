using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Piloto.Api.Application.Interfaces;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = Piloto.Api.Domain.Models;

namespace Piloto.Api.UnitTests.Application.Supplier
{

    [Collection(nameof(ApplicationServiceSupplierCollection))]
    public class ApplicationSupplierServiceTests
    {
        public ApplicationServiceSupplierTestsFixture Fixture { get; set; }

        public ApplicationSupplierServiceTests(ApplicationServiceSupplierTestsFixture fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "Add New Success")]
        [Trait("Category", "Application Supplier Service Tests")]
        public async void ApplicationSupplierService_AddNew_ShouldAddWithSuccess()
        {
            // Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperSupplier mapperSupplier = serviceScope.ServiceProvider.GetService<IMapperSupplier>();
            IApplicationServiceSupplier applicationServiceSupplier = serviceScope.ServiceProvider.GetService<IApplicationServiceSupplier>();
            Fixture.UnitOfWorkMock.Setup(uow => uow.SaveChangeAsync(false)).ReturnsAsync(1);
            var supplierDTO = Fixture.GetValidSupplierDTO();
            Models.Supplier supplier = mapperSupplier.MapperToEntity(supplierDTO);
            var supplierAdded = new Models.Supplier(1, supplier.Name, supplier.CNPJ, supplier.ProductSuppliers, supplier.SupplierAddresses);
            Fixture.ServiceSupplierMock.Setup(s => s.AddAsync(supplier)).ReturnsAsync(supplierAdded);
            //Fixture.RepositorySupplierMock.Setup(s => s.Add(supplier)).ReturnsAsync(supplierAdded);
            // Act
            var result = await applicationServiceSupplier.Add(supplierDTO);

            // Assert
            Fixture.ServiceSupplierMock.Verify(s => s.AddAsync(supplier), Times.Once);
            //Fixture.RepositorySupplierMock.Verify(s => s.Add(supplier), Times.Once);
            Assert.Equal(supplierAdded.Id, result.Id);
            Assert.Equal(supplierAdded.CNPJ, result.CNPJ);
        }

        [Fact(DisplayName = "Add New Fail")]
        [Trait("Category", "Supplier Service Tests")]
        public void SupplierService_AddNew_ShouldNotAddWithSuccess()
        {
            var Supplier = Fixture.GetInvalidSupplierDTO();
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperSupplier mapperSupplier = serviceScope.ServiceProvider.GetService<IMapperSupplier>();
            IApplicationServiceSupplier applicationServiceSupplier = serviceScope.ServiceProvider.GetService<IApplicationServiceSupplier>();
            Fixture.ServiceSupplierMock.Setup(c => c.AddAsync(mapperSupplier.MapperToEntity(Supplier)));
            // Act
            applicationServiceSupplier.Add(Supplier);

            // Assert
           Assert.False(mapperSupplier.MapperToEntity(Supplier).IsValid());

        }

        [Fact(DisplayName = "Get All Suppliers")]
        [Trait("Category", "Supplier Service Tests")]
        public async void CustomerService_GetAll_ShouldReturnsMoreThan1()
        {
            // Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperSupplier mapperSupplier = serviceScope.ServiceProvider.GetService<IMapperSupplier>();
            IApplicationServiceSupplier applicationServiceSupplier = serviceScope.ServiceProvider.GetService<IApplicationServiceSupplier>();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfiguration()));
            IMapper mapper = mapperConfiguration.CreateMapper();

            Fixture.ServiceSupplierMock.Setup((c) => c.GetAsync(null,null, null,true, true)).ReturnsAsync(mapper.Map<ICollection<Models.Supplier>>(Fixture.GetMixedSupplierDTOs()));

            // Act
            var Suppliers = await applicationServiceSupplier.GetAll();

            // Assert Fluent Assertions
            Suppliers.ToList().Should().HaveCount(c => c > 1).And.OnlyHaveUniqueItems();

        }
    }
}
