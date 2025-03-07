using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Piloto.Api.Application.Interfaces;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = Piloto.Api.Domain.Models;

namespace Piloto.Api.UnitTests.Application.Product
{

    [Collection(nameof(ApplicationServiceProductCollection))]
    public class ApplicationProductServiceTests
    {
        public ApplicationServiceProductTestsFixture Fixture { get; set; }

        public ApplicationProductServiceTests(ApplicationServiceProductTestsFixture fixture)
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
            IMapperProduct mapperProduct = serviceScope.ServiceProvider.GetService<IMapperProduct>();
            IApplicationServiceProduct applicationServiceProduct = serviceScope.ServiceProvider.GetService<IApplicationServiceProduct>();
            Fixture.UnitOfWorkMock.Setup(uow => uow.SaveChangeAsync()).ReturnsAsync(1);
            var productDTO = Fixture.GetValidProductDTO();
            Models.Product product = mapperProduct.MapperToEntity(productDTO);
            var productAdded = new Models.Product(1, productDTO.Name, product.Stock, product.Price, product.ProductSuppliers);
            Fixture.ServiceProductMock.Setup(s => s.Add(product)).ReturnsAsync(productAdded);
            //Fixture.RepositoryProductMock.Setup(s => s.Add(product)).ReturnsAsync(productAdded);
            // Act
            var result = await applicationServiceProduct.Add(productDTO);

            // Assert
            Fixture.ServiceProductMock.Verify(s => s.Add(product), Times.Once);
            //Fixture.RepositoryProductMock.Verify(s => s.Add(product), Times.Once);
            Assert.Equal(productAdded.Id, result.Id);
            Assert.Equal(productAdded.Name, result.Name);
        }

        [Fact(DisplayName = "Add New Fail")]
        [Trait("Category", "Product Service Tests")]
        public void ProductService_AddNew_ShouldNotAddWithSuccess()
        {
            var productDTO = Fixture.GetInvalidProductDTO();
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperProduct mapperProduct = serviceScope.ServiceProvider.GetService<IMapperProduct>();
            IApplicationServiceProduct applicationServiceProduct = serviceScope.ServiceProvider.GetService<IApplicationServiceProduct>();
            Models.Product product = mapperProduct.MapperToEntity(productDTO);
            Fixture.ServiceProductMock.Setup(c => c.Add(product));
            // Act
            applicationServiceProduct.Add(productDTO);

            // Assert
            Assert.False(product.IsValid());
            Fixture.ServiceProductMock.Verify(s => s.Add(product), Times.Once);
            //Fixture.RepositoryProductMock.Verify(s => s.Add(product), Times.Never);
        }

        [Fact(DisplayName = "Get All Products")]
        [Trait("Category", "Product Service Tests")]
        public async void CustomerService_GetAll_ShouldReturnsMoreThan1()
        {
            // Arrange
            IServiceScope serviceScope = Fixture.ServiceProvider.CreateScope();
            IMapperProduct mapperProduct = serviceScope.ServiceProvider.GetService<IMapperProduct>();
            IApplicationServiceProduct applicationServiceProduct = serviceScope.ServiceProvider.GetService<IApplicationServiceProduct>();
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfiguration()));
            IMapper mapper = mapperConfiguration.CreateMapper();

            Fixture.ServiceProductMock.Setup(c => c.GetAll()).ReturnsAsync(mapper.Map<ICollection<Models.Product>>(Fixture.GetMixedProductDTOs()));

            // Act
            var Products = await applicationServiceProduct.GetAll();

            // Assert Fluent Assertions
            Products.ToList().Should().HaveCount(c => c > 1).And.OnlyHaveUniqueItems();

        }
    }
}
