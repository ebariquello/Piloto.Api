using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Piloto.Api.Application.DTO.DTO;
using Piloto.Api.Application.Interfaces;
using Piloto.Api.Domain.Core.Interfaces.Repositories;
using Piloto.Api.Domain.Core.Interfaces.Services;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Interfaces;
using Piloto.Api.Infrastructure.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piloto.Api.Application.Services
{
    public class ApplicationServiceProduct : IApplicationServiceProduct
    {
        private readonly IServiceProduct _serviceProduct;
        private readonly IMapperProduct _mapperProduct;
        private readonly IUnitOfWork<DbContext> _unitOfWork;
        public ApplicationServiceProduct(IServiceProduct serviceProduct, IMapperProduct mapperProduct, IUnitOfWork<DbContext> unitOfWork)
        //private readonly IUnitOfWork _unitOfWork;

        {
            _serviceProduct = serviceProduct;
            _mapperProduct = mapperProduct;
            _unitOfWork = unitOfWork;
        }
        public async Task<ProductDTO> Add(ProductDTO productDTO)
        {
            var objProduct = _mapperProduct.MapperToEntity(productDTO);
            var resultEntity = await _serviceProduct.AddAsync(objProduct);
            await _unitOfWork.SaveChangeAsync();
            var resultDTO = _mapperProduct.MapperToDTO(resultEntity);
            return resultDTO;
            ;
        }
        public async Task<ICollection<ProductDTO>> AddRange(ICollection<ProductDTO> productDTO)
        {
            var resultEntity = new List<Domain.Models.Product>();
            foreach (var product in productDTO)
            {
                var objProduct = _mapperProduct.MapperToEntity(product);
                resultEntity.Add(await _serviceProduct.AddAsync(objProduct));
            }
            await _unitOfWork.SaveChangeAsync();
            var resultDTOs = _mapperProduct.MapperListProducts(resultEntity);
            return resultDTOs;
            ;
        }

        public void Dispose()
        {
            _serviceProduct.Dispose();
        }

        public async Task<ICollection<ProductDTO>> GetAll()
        {
            var query = _serviceProduct.GetQuery();
            query.Include(p => p.ProductSuppliers).ThenInclude(ps => ps.Supplier);

            var products = await _serviceProduct.GetAsync(query, null, orderBy: q => q.OrderBy(p => p.Name), true, true);
            if (products != null)
            {
                return _mapperProduct.MapperListProducts(products);
            }
            return null;

        }

        public async Task<ProductDTO> GetById(int Id)
        {
            var query = _serviceProduct.GetQuery();
            query.Include(p => p.ProductSuppliers).ThenInclude(ps => ps.Supplier);
            var product = await _serviceProduct.GetByIdAsync(Id, query, true, true);
            if (product != null)
                return _mapperProduct.MapperToDTO(product);

            return null;
        }

        public async Task<int> Remove(int id)
        {
            var productToDelete = await _serviceProduct.GetByIdAsync(id, null, true, true);
            //var objProduct = _mapperProduct.MapperToEntity(productDTO);
            var result = await _serviceProduct.RemoveAync(productToDelete);
            await _unitOfWork.SaveChangeAsync();
            return result;
        }

        public async Task<ProductDTO> Update(ProductDTO productDTO)
        {
            var objProduct = _mapperProduct.MapperToEntity(productDTO);
            var resultEntity = await _serviceProduct.UpdateAsync(objProduct);
            await _unitOfWork.SaveChangeAsync();
            var resultDTO = _mapperProduct.MapperToDTO(resultEntity);
            return resultDTO;
        }
    }
}