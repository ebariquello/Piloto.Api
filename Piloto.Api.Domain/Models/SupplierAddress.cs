using Piloto.Api.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Models
{
    public class SupplierAddress : Entity
    {
        public SupplierAddress() : base() { }
        public SupplierAddress(int? id, string name, string country, string city, string postalCode, string street, string houseNumber, int? supplierId, Supplier supplier)
        {
            Id = id;
            Name = name;
            Country = country;
            City = city;
            PostalCode = postalCode;
            Street = street;
            HouseNumber = houseNumber;
            SupplierId = supplierId;
            Supplier = supplier;
        }

        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string City { get; set; } = null!;

        public string PostalCode { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string HouseNumber { get; set; } = null!;

        public int? SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public override bool IsValid()
        {
            ValidationResult = new SupplierAddressSelfValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
