using Microsoft.EntityFrameworkCore;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Infrastructure.Data.Repository;

//using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piloto.Api.Application.Services
{
    public class ApplicationServiceSupplier : IApplicationServiceSupplier
    {
        private readonly IServiceSupplier _serviceClient;
        private readonly IMapperSupplier _mapperSupplier;
        private readonly IUnitOfWork<DbContext> _unitOfWork;
        public ApplicationServiceSupplier(IServiceSupplier serviceClient, IMapperSupplier mapperSupplier, IUnitOfWork<DbContext> unitOfWork)
        {
            _serviceClient = serviceClient;
            _mapperSupplier = mapperSupplier;
            _unitOfWork = unitOfWork;
        }
        public async Task<SupplierDTO> Add(SupplierDTO clientDTO)
        {
            var objClient = _mapperSupplier.MapperToEntity(clientDTO);
            var resultEntity = await _serviceClient.AddAsync(objClient);
            await _unitOfWork.SaveChangeAsync();
            var resultDTO = _mapperSupplier.MapperToDTO(resultEntity);
            return resultDTO;

        }

        public void Dispose()
        {
            _serviceClient.Dispose();
        }

        public async Task<ICollection<SupplierDTO>> GetAll()
        {
            var suppliers = await _serviceClient.GetAsync();
            if (suppliers != null)
                return _mapperSupplier.MapperListSuppliers(suppliers);
            return null;
        }

        public async Task<SupplierDTO> GetById(int Id)
        {
            var suplier = await _serviceClient.GetByIdAsync(Id);
            if (suplier != null)
                return _mapperSupplier.MapperToDTO(suplier);
            return null;
        }

        public async Task<int> Remove(SupplierDTO clientDTO)
        {
            var objClient = _mapperSupplier.MapperToEntity(clientDTO);
            var result = await _serviceClient.RemoveAync(objClient);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<SupplierDTO> Update(SupplierDTO clientDTO)
        {
            var objClient = _mapperSupplier.MapperToEntity(clientDTO);
            var resultEntity = await _serviceClient.UpdateAsync(objClient);
            await _unitOfWork.SaveChangeAsync();
            var resultDTO = _mapperSupplier.MapperToDTO(resultEntity);
            return resultDTO;
        }
    }
}