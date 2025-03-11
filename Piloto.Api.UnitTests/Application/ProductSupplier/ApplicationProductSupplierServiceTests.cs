using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Piloto.Api.Application.DTO.DTO;
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

namespace Piloto.Api.UnitTests.Application.ProductSupplier
{

    [Collection(nameof(ApplicationServiceProductSupplierCollection))]
    public class ApplicationProductSupplierServiceTests
    {
        public ApplicationServiceProductSupplierTestsFixture Fixture { get; set; }

        public ApplicationProductSupplierServiceTests(ApplicationServiceProductSupplierTestsFixture fixture)
        {
            Fixture = fixture;
        }

        // AAA == Arrange, Act, Assert
        [Fact(DisplayName = "Add New Success")]
        [Trait("Category", "Application Product Service Tests")]
        public async void ApplicationProductService_AddNew_ShouldAddWithSuccess()
        {
            // Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperProductSupplier mapperProductSupplier = serviceScope.ServiceProvider.GetService<IMapperProductSupplier>();
            IApplicationServiceProductSupplier applicationServiceProductSupplier = serviceScope.ServiceProvider.GetService<IApplicationServiceProductSupplier>();
            Fixture.UnitOfWorkMock.Setup(uow => uow.SaveChangeAsync(false)).ReturnsAsync(1);
            var productSupplierDTO = Fixture.GetValidProductSupplierDTO();
            Models.ProductSupplier productSupplier = mapperProductSupplier.MapperToEntity(productSupplierDTO);
            var productSupplierAdded = new Models.ProductSupplier(1, 1, productSupplier.Product, 1, productSupplier.Supplier);
            Fixture.ServiceProductSupplierMock.Setup(s => s.AddAsync(productSupplier)).ReturnsAsync(productSupplierAdded);
            //Fixture.RepositoryProductSupplierMock.Setup(s => s.Add(productSupplier)).ReturnsAsync(productSupplierAdded);
            // Act
            var result = await applicationServiceProductSupplier.Add(productSupplierDTO);

            // Assert
            Fixture.ServiceProductSupplierMock.Verify(s => s.AddAsync(productSupplier), Times.AtLeastOnce);
            //Fixture.RepositoryProductSupplierMock.Verify(s => s.Add(productSupplier), Times.Once);
            Assert.Equal(productSupplierAdded.Id, result.Id);
        }

        [Fact(DisplayName = "Add New Fail")]
        [Trait("Category", "Product Service Tests")]
        public async void ProductService_AddNew_ShouldNotAddWithSuccess()
        {

            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperProductSupplier mapperProductSupplier = serviceScope.ServiceProvider.GetService<IMapperProductSupplier>();
            IApplicationServiceProductSupplier applicationServiceProductSupplier = serviceScope.ServiceProvider.GetService<IApplicationServiceProductSupplier>();
            Fixture.UnitOfWorkMock.Setup(uow => uow.SaveChangeAsync(false)).ReturnsAsync(1);
            var produtSupplierInvalidDTO = Fixture.GetInvalidProductSupplierDTO();
            Models.ProductSupplier productSupplier = mapperProductSupplier.MapperToEntity(produtSupplierInvalidDTO);
            var productSupplierAdded = new Models.ProductSupplier(1, 1, productSupplier.Product, 1, productSupplier.Supplier);
            Fixture.ServiceProductSupplierMock.Setup(s => s.AddAsync(productSupplier)).ReturnsAsync(productSupplierAdded);
           
            // Act
            var result = await applicationServiceProductSupplier.Add(produtSupplierInvalidDTO);

            // Assert
            Fixture.ServiceProductSupplierMock.Verify(s => s.AddAsync(productSupplier), Times.AtLeastOnce);
            //Fixture.RepositoryProductSupplierMock.Verify(s => s.Add(productSupplier), Times.Never);
        
            Assert.False(mapperProductSupplier.MapperToEntity(produtSupplierInvalidDTO).IsValid());

        }

        [Fact(DisplayName = "Get All Products")]
        [Trait("Category", "Product Service Tests")]
        public async void CustomerService_GetAll_ShouldReturnsMoreThan1()
        {
            // Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperProductSupplier mapperProductSupplier = serviceScope.ServiceProvider.GetService<IMapperProductSupplier>();
            IApplicationServiceProductSupplier applicationServiceProductSupplier = serviceScope.ServiceProvider.GetService<IApplicationServiceProductSupplier>();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfiguration()));
            IMapper mapper = mapperConfiguration.CreateMapper();
            Fixture.ServiceProductSupplierMock
              .Setup(c => c.GetAsync(null, null, null,true, true))
              .ReturnsAsync(mapper.Map<ICollection<Models.ProductSupplier>>(Fixture.GetMixedProductSupplierDTOs()));
            // Act
            var productSupplierDTOs = await applicationServiceProductSupplier.GetAll();

            // Assert Fluent Assertions
            productSupplierDTOs.ToList().Should().HaveCount(c => c > 1).And.OnlyHaveUniqueItems();

        }
    }
}
