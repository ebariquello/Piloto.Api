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

namespace Piloto.Api.UnitTests.Application.ProductSupplier
{
    [CollectionDefinition(nameof(ApplicationServiceProductSupplierCollection))]
    public class ApplicationServiceProductSupplierCollection : ICollectionFixture<ApplicationServiceProductSupplierTestsFixture>
    {
        public ApplicationServiceProductSupplierCollection()
        {
            
        }
    }

    public class ApplicationServiceProductSupplierTestsFixture : IDisposable
    {
        public Mock<IServiceProductSupplier> ServiceProductSupplierMock { get; set; }
        public Mock<IUnitOfWork<DbContext>> UnitOfWorkMock { get; set; }
      

        //public Mock<IRepositoryProductSupplier> RepositoryProductSupplierMock { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public ApplicationServiceProductSupplierTestsFixture()
        {
            var mocker = new AutoMoqCore.AutoMoqer();
            UnitOfWorkMock = mocker.GetMock<IUnitOfWork<DbContext>>();

            var mockMapper = new Mock<IMapper>();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfiguration()));
            IMapper mapper = mapperConfiguration.CreateMapper();
            var services = new ServiceCollection();

            services.AddScoped(typeof(IMapperProductSupplier), x =>
            {
                return new MapperProductSupplier(mapper);
            });
            services.AddScoped(typeof(IUnitOfWork<DbContext>), p => { return UnitOfWorkMock.Object; });
            

            services.AddScoped(typeof(IApplicationServiceProductSupplier), typeof(ApplicationServiceProductSupplier));
            ServiceProductSupplierMock = mocker.GetMock<IServiceProductSupplier>();
            //RepositoryProductSupplierMock = mocker.GetMock<IRepositoryProductSupplier>();
            services.AddScoped(typeof(IServiceProductSupplier), x =>
            {
                return ServiceProductSupplierMock.Object;
            });
            //services.AddScoped(typeof(IRepositorySupplier), p =>
            //{
            //    return RepositoryProductSupplierMock.Object;
            //});
            services.AddAutoMapper(typeof(MapperConfiguration));

            ServiceProvider = services.BuildServiceProvider();

        }

        public ProductSupplierDTO GetValidProductSupplierDTO()
        {
            return GenerateOneProductSupplierDTO();
        }
        public ProductSupplierDTO GetValidProduct()
        {
            return GenerateOneProductSupplierDTO();
        }

        public ProductSupplierDTO GetInvalidProductSupplierDTO()
        {
            var productSupplierDTO = new Faker<ProductSupplierDTO>("pt_BR")
                .CustomInstantiator(
                    f => new ProductSupplierDTO(
                    null,
                    null, 
                    null,
                    null,
                    new SupplierDTO(null, f.Company.CompanyName(), f.Company.Cnpj(),null,null)
                    ));

            var psDTOResult = productSupplierDTO.Generate();

            psDTOResult.SupplierDTO.SupplierAddressDTOs = GenerateSupplierAddressesDTO(1, psDTOResult.SupplierDTO).ToList();
            return psDTOResult;
        }
        
        public ICollection<ProductSupplierDTO> GetMixedProductSupplierDTOs()
        {
            var productDTOs = new List<ProductSupplierDTO>();

            productDTOs.AddRange(GenerateProductSupplierDTOs(50).ToList());
            productDTOs.AddRange(GenerateProductSupplierDTOs(50).ToList());

            return productDTOs;
        }
        public void Dispose()
        {
            // Dispose what you have!
        }


        private ICollection<ProductSupplierDTO> GenerateProductSupplierDTOs(int number)
        {
            var productSupplierDTO = new Faker<ProductSupplierDTO>("pt_BR")
               .CustomInstantiator(
                   f => new ProductSupplierDTO(
                   null,
                   f.IndexGlobal,
                   new ProductDTO(f.UniqueIndex, f.Commerce.ProductName(), f.Commerce.Random.Number(1, 1000), f.Commerce.Random.Float(0, 1), null),
                   f.IndexGlobal,
                   new SupplierDTO(f.UniqueIndex, f.Company.CompanyName(), f.Company.Cnpj(), null, null)
                   ));

            var psDTOResults = productSupplierDTO.Generate(number);

            psDTOResults.ForEach(pDTO => { pDTO.SupplierDTO.SupplierAddressDTOs = GenerateSupplierAddressesDTO(1, pDTO.SupplierDTO).ToList(); });
            return psDTOResults;
        }
       

        private ProductSupplierDTO GenerateOneProductSupplierDTO()
        {
            var productSupplierDTO = new Faker<ProductSupplierDTO>("pt_BR")
               .CustomInstantiator(
                   f => new ProductSupplierDTO(
                   null,
                   null,
                   new ProductDTO(null, f.Commerce.ProductName(), f.Commerce.Random.Number(1, 1000), f.Commerce.Random.Float(0, 1), null),
                   f.IndexGlobal,
                   new SupplierDTO(null, f.Company.CompanyName(), f.Company.Cnpj(), null, null)
                   ));

            var psDTOResult = productSupplierDTO.Generate();

            psDTOResult.SupplierDTO.SupplierAddressDTOs = GenerateSupplierAddressesDTO(1, psDTOResult.SupplierDTO).ToList();
            return psDTOResult;
        }

        private ICollection<SupplierAddressDTO> GenerateSupplierAddressesDTO(int number, SupplierDTO supplier)
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
                        null,
                        supplier
                        )
                  );

            return supplierDTOs.Generate(number);
        }


    }
}
