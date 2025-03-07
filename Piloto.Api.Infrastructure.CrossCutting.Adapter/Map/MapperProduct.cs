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
    public class MapperProduct : IMapperProduct
    {
        public readonly IMapper _mapper; 
        public MapperProduct(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ICollection<ProductDTO> MapperListProducts(ICollection<Product> products)
        {
            return _mapper.Map<ICollection<ProductDTO>>(products);
        }

        public ProductDTO MapperToDTO(Product product)
        {
            return _mapper.Map<ProductDTO>(product);
        }

        public Product MapperToEntity(ProductDTO productDTO)
        {
            var result = _mapper.Map<Product>(productDTO);
            return result;
        }
    }
}
