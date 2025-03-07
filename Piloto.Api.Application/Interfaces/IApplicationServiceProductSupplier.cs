using Piloto.Api.Application.DTO.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piloto.Api.Application.Interfaces
{
    public interface IApplicationServiceProductSupplier
    {
        Task<ProductSupplierDTO> Add(ProductSupplierDTO productDTO);
        Task<ProductSupplierDTO> GetById(int Id);

        Task<ICollection<ProductSupplierDTO>> GetAll();
        Task<ProductSupplierDTO> Update(ProductSupplierDTO productDTO);

        Task<int> Remove(ProductSupplierDTO productDTO);

        void Dispose();

    }
}
