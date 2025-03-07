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
    public class MapperSupplier : IMapperSupplier
    {
        public readonly IMapper _mapper; 
        public MapperSupplier(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ICollection<SupplierDTO> MapperListSuppliers(ICollection<Supplier> products)
        {
            return _mapper.Map<ICollection<SupplierDTO>>(products);
        }

        public SupplierDTO MapperToDTO(Supplier product)
        {
            return _mapper.Map<SupplierDTO>(product);
        }

        public Supplier MapperToEntity(SupplierDTO productDTO)
        {
            return _mapper.Map<Supplier>(productDTO);
        }
    }
}
