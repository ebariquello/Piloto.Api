using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces
{
    public interface IMapperProduct
    {
        ICollection<ProductDTO> MapperListProducts(ICollection<Product> products);

        ProductDTO MapperToDTO(Product product);

        Product MapperToEntity(ProductDTO productDTO);
    }
}
