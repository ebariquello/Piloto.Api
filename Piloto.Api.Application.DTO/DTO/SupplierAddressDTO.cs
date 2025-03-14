﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Application.DTO.DTO
{
    [Serializable]
    public class SupplierAddressDTO
    {
        public SupplierAddressDTO() { }
        public SupplierAddressDTO(int? id, string name, string country, string city, string postalCode, string street, string houseNumber, int? supplierId, SupplierDTO supplierDTO)
        {
            Id = id;
            Name = name;
            Country = country;
            City = city;
            PostalCode = postalCode;
            Street = street;
            HouseNumber = houseNumber;
            SupplierId = supplierId;
            SupplierDTO = supplierDTO;
        }

        public int? Id { get; set; }
        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public SupplierDTO? SupplierDTO { get; set; }

        public int? SupplierId{ get; set; }


    }
}
