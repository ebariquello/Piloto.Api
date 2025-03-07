using Piloto.Api.Application.DTO.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piloto.Api.Application.Interfaces
{
    public interface IApplicationServiceProduct
    {
        Task<ProductDTO> Add(ProductDTO productDTO);
        Task<ICollection<ProductDTO>> AddRange(ICollection<ProductDTO> productDTO);
        Task<ProductDTO> GetById(int Id);

        Task<ICollection<ProductDTO>> GetAll();
        Task<ProductDTO> Update(ProductDTO productDTO);

        Task<int> Remove(ProductDTO productDTO);

        void Dispose();

    }
}
