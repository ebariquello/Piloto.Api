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
using Piloto.Api.Application.Services;
using Piloto.Api.Application.Interfaces;
using AutoMapper;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Map;
using Microsoft.Extensions.DependencyInjection;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Piloto.Api.Infrastructure.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Piloto.Api.Infrastructure.Data;
using Piloto.Api.Domain.Services;
using Piloto.Api.Application;
using Piloto.Api.Infrastructure.CrossCutting.Adapter;

namespace Piloto.Api.UnitTests.Application.Product
{
    [CollectionDefinition(nameof(ApplicationServiceProductCollection))]
    public class ApplicationServiceProductCollection : ICollectionFixture<ApplicationServiceProductTestsFixture>
    {
    }

    public class ApplicationServiceProductTestsFixture : IDisposable
    {
        //public Mock<IServiceProduct> ServiceProductMock { get; set; }
        ////public Mock<IRepositoryProduct> RepositoryProductMock { get; set; }
        //public Mock<IUnitOfWork<DbContext>> UnitOfWorkMock { get; set; }

        public IServiceProvider ServiceProvider { get; set; }
        public ApplicationServiceProductTestsFixture()
        {
            //var mocker = new AutoMoqCore.AutoMoqer();
            //UnitOfWorkMock = mocker.GetMock<IUnitOfWork<DbContext>>();
            //var mockMapper = new Mock<IMapper>();

            //MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfiguration()));
            //IMapper mapper = mapperConfiguration.CreateMapper();
            //var services = new ServiceCollection();

            //services.AddScoped(typeof(IMapperProduct), x =>
            //{
            //    return new MapperProduct(mapper);
            //});
            //services.AddScoped(typeof(IUnitOfWork<DbContext>), p => { return UnitOfWorkMock.Object; });


            //services.AddScoped(typeof(IApplicationServiceProduct), typeof(ApplicationServiceProduct));
            //ServiceProductMock = mocker.GetMock<IServiceProduct>();
            ////RepositoryProductMock = mocker.GetMock<IRepositoryProduct>();
            //services.AddScoped(typeof(IServiceProduct), x =>
            //{
            //    return ServiceProductMock.Object;
            //});
            ////services.AddScoped(typeof(IRepositoryProduct), p =>
            ////{
            ////    return RepositoryProductMock.Object;
            ////});
            //services.AddAutoMapper(typeof(MapperConfiguration));
            var services = new ServiceCollection();

            // Create a configuration for the in-memory database
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "DbRunAs", "InMemory" }
                })
                .Build();

            // Call AddDataServices with the in-memory configuration
            services.AddDataServices(configuration);
            services.AddRepositoryServicesDI();
            services.AddServicesDI();
            services.AddApplicationServicesDI();
            services.AddCrossCuttingAdapterServicesDI();

            ServiceProvider = services.BuildServiceProvider();

            // Ensure the database is created
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<StockManagementDBContext>();
                context.Database.EnsureCreated();
            }

        }

        public ProductDTO GetValidProductDTO()
        {
            return GenerateOneProductDTO();
        }
        public ProductDTO GetValidProduct()
        {
            return GenerateOneProductDTO();
        }

        public ProductDTO GetInvalidProductDTO()
        {
            var productDTO = new Faker<ProductDTO>("pt_BR")
                .CustomInstantiator(
                    f => new ProductDTO(
                    null,
                    "",
                    f.Commerce.Random.Int(),
                    f.Random.Float(),
                    null
                    ));

            return productDTO;
        }
        
        public ICollection<ProductDTO> GetMixedProductDTOs()
        {
            var productDTOs = new List<ProductDTO>();

            productDTOs.AddRange(GenerateProductDTOs(50).ToList());
            productDTOs.AddRange(GenerateProductDTOs(50).ToList());

            return productDTOs;
        }
        public void Dispose()
        {
            // Dispose what you have!
        }
        

        private ICollection<ProductDTO> GenerateProductDTOs(int number)
        {
            var productDTOsTests = new Faker<ProductDTO>("pt_BR")
               .CustomInstantiator(
                   f => new ProductDTO(
                   f.IndexGlobal,
                   f.Commerce.ProductName(),
                   f.Random.Int(),
                   (float)f.Finance.Amount(1, 100),
                   null
                   ));

            return productDTOsTests.Generate(number);
        }
       

        private ProductDTO GenerateOneProductDTO()
        {
            var productDTOTest = new Faker<ProductDTO>("pt_BR")
               .CustomInstantiator(
                   f => new ProductDTO(
                   f.Random.Int(1, int.MaxValue),
                   f.Commerce.ProductName(),
                   f.Random.Int(1, int.MaxValue),
                   (float)f.Finance.Amount(1, 100),
                   null
                   ));

            var productDTO = productDTOTest.Generate();
            productDTO.ProductSupplierDTOs = GenerateSupplierDTOsForProductDTO(2, productDTO);
            return productDTO;
        }

        private ICollection<ProductSupplierDTO> GenerateSupplierDTOsForProductDTO(int number, ProductDTO productDTO)
        {
            //var supplierDTOProductDTOColl = new HashSet<ProductSupplierDTO>();
            //supplierDTOProductDTOColl.Add(productDTO);
            var productsTests = new Faker<ProductSupplierDTO>("pt_BR")
                .CustomInstantiator(
                    f =>
                        new ProductSupplierDTO(null, productDTO.Id, productDTO, null, 
                            new SupplierDTO(
                            null,
                            f.Company.CompanyName(),
                            f.Company.Cnpj(),
                            null,
                            GenerateSupplierAddressesDTO(2, f.IndexVariable).ToList()
                            )
                        )
                  );

            return productsTests.Generate(number);
        }

        private ICollection<SupplierAddressDTO> GenerateSupplierAddressesDTO(int number, int supplierId)
        {
            var supplierDTOs = new Faker<SupplierAddressDTO>("pt_BR")
                .CustomInstantiator(
                    f => new SupplierAddressDTO(
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

            return supplierDTOs.Generate(number);
        }


    }
}
