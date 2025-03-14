using Piloto.Api.Domain.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Piloto.Api.Domain.Models
{

    public class Product:Entity
    {
        public Product()
        {
            ProductSuppliers = new HashSet<ProductSupplier>();
        }
        public Product(int? id, string name, int stock, float price, ICollection<ProductSupplier> suppliers)
        {
            Id = id;
            Name = name;
            Stock = stock;
            Price = price;
            ProductSuppliers = suppliers;
        }

        public string Name { get; set; } = null!;
        public int Stock { get; set; }

        public float Price { get; set; }

        //public ICollection<Supplier> Suppliers { get; set; }

        public ICollection<ProductSupplier> ProductSuppliers { get; set; }

        public bool HasStock()
        {
            return Stock > 0;
        }

        public override bool IsValid()
        {
            ValidationResult = new ProductSelfValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}