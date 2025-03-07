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
        public MapperProductSupplier(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ICollection<ProductSupplierDTO> MapperListProducts(ICollection<ProductSupplier> products)
        {
            return _mapper.Map<ICollection<ProductSupplierDTO>>(products);
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
