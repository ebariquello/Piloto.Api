using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Piloto.Api.Application.DTO.DTO
{
    [Serializable]
    public class SupplierDTO
    {
        public SupplierDTO() { }
        public SupplierDTO(int? id, string name, string cnpj, ICollection<ProductSupplierDTO> productSupplierDTOs, ICollection<SupplierAddressDTO> supplierAddressDTOs)
        {
            Id = id;
            Name = name;
            CNPJ = cnpj;
            ProductSupplierDTOs = productSupplierDTOs;
            SupplierAddressDTOs = supplierAddressDTOs;
        }

      

        public int? Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The Name is required to have some value greater than 3 chars", MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(18, ErrorMessage = "The CNPJ is required,with 18 chars", MinimumLength = 18)]
        public string CNPJ { get; set; }

        public ICollection<ProductSupplierDTO> ProductSupplierDTOs { get; set; }

        public ICollection<SupplierAddressDTO> SupplierAddressDTOs { get; set; }

    }
}