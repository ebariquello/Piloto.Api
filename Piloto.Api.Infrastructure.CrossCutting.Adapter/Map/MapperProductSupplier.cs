using AutoMapper;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Domain.Models;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.CrossCutting.Adapter.Map
{
    public class MapperProductSupplier : IMapperProductSupplier
    {
        public readonly IMapper _mapper; 
        private IMapperSupplier _mapperSupplier;
        public MapperProductSupplier(IMapper mapper, IMapperSupplier mapperSupplier)
        {
            _mapper = mapper;
            _mapperSupplier = mapperSupplier;
        }

        public ICollection<ProductSupplierDTO> MapperListProductSuppliers(ICollection<ProductSupplier> products)
        {
            var resultDtos= _mapper.Map<ICollection<ProductSupplierDTO>>(products);
            foreach (var product in resultDtos)
            {
                product.SupplierDTO = _mapperSupplier.MapperToDTO(products.FirstOrDefault(p => p.Id == product.Id).Supplier);
            }
            return resultDtos;
        }

        public ProductSupplierDTO MapperToDTO(ProductSupplier product)
        {
            return _mapper.Map<ProductSupplierDTO>(product);
        }

        public ProductSupplier MapperToEntity(ProductSupplierDTO productDTO)
        {
            var result = _mapper.Map<ProductSupplier>(productDTO);
            return result;
        }
    }
}
