using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces
{
    public interface IMapperSupplier
    {
        ICollection<SupplierDTO> MapperListSuppliers(ICollection<Supplier> suppliers);

        SupplierDTO MapperToDTO(Supplier supplier);

        Supplier MapperToEntity(SupplierDTO supplierDTO);
    }
}
