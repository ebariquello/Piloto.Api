using Microsoft.EntityFrameworkCore;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Infrastructure.Data.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piloto.Api.Application.Services
{
    public class ApplicationServiceProductSupplier : IApplicationServiceProductSupplier
    {
        private readonly IServiceProductSupplier _serviceProductSupplier;
        private readonly IMapperProductSupplier _mapperProductSupplier;
        private readonly IUnitOfWork<DbContext> _unitOfWork;
        public ApplicationServiceProductSupplier(IServiceProductSupplier serviceProductSupplier, IMapperProductSupplier mapperProductSupplier, IUnitOfWork<DbContext> unitOfWork)
        {
            _serviceProductSupplier = serviceProductSupplier;
            _mapperProductSupplier = mapperProductSupplier;
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductSupplierDTO> Add(ProductSupplierDTO productSuplierDTO)
        {
            var objProductSupplier = _mapperProductSupplier.MapperToEntity(productSuplierDTO);
            var resultEntity = await _serviceProductSupplier.AddAsync(objProductSupplier);
            await _unitOfWork.SaveChangeAsync();
            var resultDTO = _mapperProductSupplier.MapperToDTO(resultEntity);
            return resultDTO;
;        }

        public void Dispose()
        {
            _serviceProductSupplier.Dispose();
        }

        public async Task<ICollection<ProductSupplierDTO>> GetAll()
        {
            var products = await _serviceProductSupplier.GetAsync();
            if (products != null)
                return _mapperProductSupplier.MapperListProductSuppliers(products);
            
            return null;

        }

        public async Task<ProductSupplierDTO> GetById(int Id)
        {
            var product = await _serviceProductSupplier.GetByIdAsync(Id);
            if (product != null)
                return _mapperProductSupplier.MapperToDTO(product);

            return null;
        }

        public async Task<int> Remove(ProductSupplierDTO productDTO)
        {
            var objProduct = _mapperProductSupplier.MapperToEntity(productDTO);
            var result = await _serviceProductSupplier.RemoveAync(objProduct);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<ProductSupplierDTO> Update(ProductSupplierDTO productDTO)
        {
            var objProduct = _mapperProductSupplier.MapperToEntity(productDTO);
            var resultEntity = await _serviceProductSupplier.UpdateAsync(objProduct);
            await _unitOfWork.SaveChangeAsync();
            var resultDTO = _mapperProductSupplier.MapperToDTO(resultEntity);
            return resultDTO;
        }
    }
}