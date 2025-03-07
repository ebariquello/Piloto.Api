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
using Piloto.Api.Infrastructure.Data;

namespace Piloto.Api.UnitTests.Application.Supplier
{
    [CollectionDefinition(nameof(ApplicationServiceSupplierCollection))]
    public class ApplicationServiceSupplierCollection : ICollectionFixture<ApplicationServiceSupplierTestsFixture>
    {
    }

    public class ApplicationServiceSupplierTestsFixture : IDisposable
    {
        public Mock<IServiceSupplier> ServiceSupplierMock { get; set; }
        public Mock<IUnitOfWork<DbContext>> UnitOfWorkMock { get; set; }
        //public Mock<IRepositorySupplier> RepositorySupplierMock { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public ApplicationServiceSupplierTestsFixture()
        {
            var mocker = new AutoMoqCore.AutoMoqer();
            UnitOfWorkMock= mocker.GetMock<IUnitOfWork<DbContext>>();
           
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfiguration()));
            IMapper mapper = mapperConfiguration.CreateMapper();
            var services = new ServiceCollection();

            services.AddScoped(typeof(IUnitOfWork<DbContext>), p=> { return UnitOfWorkMock.Object;  });
            
            services.AddScoped(typeof(IMapperSupplier), x =>
            {
                return new MapperSupplier(mapper);
            });
            services.AddScoped(typeof(IApplicationServiceSupplier),typeof(ApplicationServiceSupplier));
            ServiceSupplierMock = mocker.GetMock<IServiceSupplier>();
            //RepositorySupplierMock = mocker.GetMock<IRepositorySupplier>();
            services.AddScoped(typeof(IServiceSupplier), x =>
            {
                return ServiceSupplierMock.Object;
            });
            //services.AddScoped(typeof(IRepositorySupplier), p =>
            //{
            //    return RepositorySupplierMock.Object;
            //});

            services.AddAutoMapper(typeof(MapperConfiguration));

            ServiceProvider = services.BuildServiceProvider();

            
        }
        private Mock<DbSet<T>> CreateDbSetMock<T>() where T : class
        {
         
            var dbSetMock = new Mock<DbSet<T>>();
            return dbSetMock;
        }

        public SupplierDTO GetValidSupplierDTO()
        {
            return GenerateOneSupplierDTO();
        }
        public SupplierDTO GetValidSupplier()
        {
            return GenerateOneSupplierDTO();
        }

        public SupplierDTO GetInvalidSupplierDTO()
        {
            var supplierDTO = new Faker<SupplierDTO>("pt_BR")
                .CustomInstantiator(
                    f => new SupplierDTO(
                    null,
                    "",
                    f.Company.Cnpj(),
                    null,
                    null
                    ));

            return supplierDTO;
        }
        
        public ICollection<SupplierDTO> GetMixedSupplierDTOs()
        {
            var supplierDTOs = new List<SupplierDTO>();

            supplierDTOs.AddRange(GenerateSupplierDTOs(50).ToList());
            supplierDTOs.AddRange(GenerateSupplierDTOs(50).ToList());

            return supplierDTOs;
        }
        public void Dispose()
        {
            // Dispose what you have!
        }
        

        private ICollection<SupplierDTO> GenerateSupplierDTOs(int number)
        {
            var supplierDTOsTests = new Faker<SupplierDTO>("pt_BR")
               .CustomInstantiator(
                   f => new SupplierDTO(
                   f.IndexGlobal,
                   f.Company.CompanyName(),
                   f.Company.Cnpj(),
                   null,
                   null
                   ));

            return supplierDTOsTests.Generate(number);
        }
       

        private SupplierDTO GenerateOneSupplierDTO()
        {
            var supplierDTOTest = new Faker<SupplierDTO>("pt_BR")
              .CustomInstantiator(
                   f => new SupplierDTO(
                   f.IndexVariable++,
                   f.Company.CompanyName(),
                   f.Company.Cnpj(),
                   null,
                   null
                   ));

            var supplierDTO = supplierDTOTest.Generate();
            supplierDTO.ProductSupplierDTOs = GenerateProductDTOsForSupplierDTO(2, supplierDTO);
            supplierDTO.SupplierAddressDTOs = GenerateSupplierAddressesDTO(2, supplierDTO.Id);
            return supplierDTO;
        }

        private ICollection<ProductSupplierDTO> GenerateProductDTOsForSupplierDTO(int number, SupplierDTO supplierDTO)
        {
            //var productDTOForSupplierDTOColl = new HashSet<SupplierDTO>();
            //productDTOForSupplierDTOColl.Add(supplierDTO);
            var productsTests = new Faker<ProductSupplierDTO>("pt_BR")
                .CustomInstantiator(
                    f =>
                        new ProductSupplierDTO(
                        null,
                        null,
                            new ProductDTO(
                            f.IndexVariable++,
                            f.Commerce.ProductName(),
                            f.Random.Int(1, int.MaxValue),
                            (float)f.Finance.Amount(1, 100),
                            null
                            )
                        ,
                        null,
                        supplierDTO
                        )
                  );

            return productsTests.Generate(number);
        }

        private ICollection<SupplierAddressDTO> GenerateSupplierAddressesDTO(int number, int? supplierId)
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
