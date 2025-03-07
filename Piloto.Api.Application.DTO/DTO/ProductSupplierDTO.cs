using System;

namespace Piloto.Api.Application.DTO.DTO
{
    [Serializable]
    public class ProductSupplierDTO
    {
        public ProductSupplierDTO() { }
        public ProductSupplierDTO(int? id, int? productId, ProductDTO productDTO, int? supplierId, SupplierDTO supplierDTO)
        {
            Id = id;
            ProductId = productId;
            ProductDTO = productDTO;
            SupplierId = supplierId;
            SupplierDTO = supplierDTO;
        }

        public int? Id { get; set; }

        public int? ProductId { get; set; }
        public virtual ProductDTO ProductDTO { get; set; } = null!;

        public int? SupplierId { get; set; } // In lack of better name.
        public virtual SupplierDTO SupplierDTO { get; set; } = null!;



    }
}