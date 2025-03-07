using AutoMapper;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.CrossCutting.Adapter.Map
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<Supplier, SupplierDTO>()
                  .ForMember(x => x.SupplierAddressDTOs, y => y.MapFrom(x => x.SupplierAddresses))
                  .ForMember(x => x.ProductSupplierDTOs, y => y.MapFrom(x => x.ProductSuppliers));

            CreateMap<Product, ProductDTO>()
                .ForMember(x => x.ProductSupplierDTOs, y => y.MapFrom(x => x.ProductSuppliers));
            CreateMap<ProductDTO, Product>()
                 .ForMember(x => x.ProductSuppliers, y => y.MapFrom(x => x.ProductSupplierDTOs)).
                 ForMember(x => x.ValidationResult, opt => opt.Ignore()); ;
            CreateMap<ProductSupplier, ProductSupplierDTO>()
                .ForMember(psTarget => psTarget.ProductDTO, psOrigin => psOrigin.MapFrom(ps=>ps.Product) )
                .ForMember(psTarget => psTarget.SupplierDTO, psOrigin => psOrigin.MapFrom(ps => ps.Supplier))
                .ReverseMap();
            CreateMap<SupplierAddress, SupplierAddressDTO>()
                .ForMember(x => x.SupplierDTO, y => y.MapFrom(x => x.Supplier));

            CreateMap<SupplierAddressDTO, SupplierAddress>()
                 .ForMember(x => x.Supplier, y => y.MapFrom(x => x.SupplierDTO))
                .ForMember(x => x.ValidationResult, opt => opt.Ignore());
            CreateMap<SupplierDTO, Supplier>()
            .ForMember(x => x.ProductSuppliers, y => y.MapFrom(x => x.ProductSupplierDTOs))
            .ForMember(x => x.SupplierAddresses, y => y.MapFrom(x => x.SupplierAddressDTOs))
            .ForMember(x => x.ValidationResult, opt => opt.Ignore());
            ;



        }
    }
}
