using Piloto.Api.Application.DTO.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piloto.Api.Application.Interfaces
{
    public interface IApplicationServiceSupplier
    {
        Task<SupplierDTO> Add(SupplierDTO supplierDTO);
        Task<SupplierDTO> GetById(int Id);
        Task<ICollection<SupplierDTO>> GetAll();
        Task<SupplierDTO> Update(SupplierDTO supplierDTO);

        Task<int> Remove(SupplierDTO supplierDTO);

        void Dispose();

    }
}
