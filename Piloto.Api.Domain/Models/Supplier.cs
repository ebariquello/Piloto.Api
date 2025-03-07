using Piloto.Api.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Models
{
    public class Supplier : Entity
        
    {
        public Supplier()
        {
            ProductSuppliers = new HashSet<ProductSupplier>();
            SupplierAddresses = new HashSet<SupplierAddress>();
        }

        public Supplier(int? id, string name, string cnpj, ICollection<ProductSupplier> productSuppliers, ICollection<SupplierAddress> supplierAddress)
        {
            Id = id;
            Name = name;
            CNPJ = cnpj;
            ProductSuppliers = productSuppliers;
            SupplierAddresses = supplierAddress;
        }

        public string Name { get; set; } = null!;

        public string CNPJ { get; set; } = null!;

        public ICollection<ProductSupplier> ProductSuppliers {get; set;}
        public ICollection<SupplierAddress> SupplierAddresses { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SupplierSelfValidation().Validate(this);
            return ValidationResult.IsValid;
        }


    }
}
