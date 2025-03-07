using Piloto.Api.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Models
{
    public class ProductSupplier :Entity
    {
        public ProductSupplier() { }
        public ProductSupplier(int? id, int? productId, Product product, int? supplierId, Supplier supplier)
        {
            Id = id; 
            ProductId = productId;
            Product = product;
            SupplierId = supplierId;
            Supplier = supplier;
        }

        public int? ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        public int? SupplierId { get; set; } // In lack of better name.
        public virtual Supplier Supplier { get; set; } = null!;

        public override bool IsValid()
        {
            ValidationResult = new ProductSupplierSelfValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
