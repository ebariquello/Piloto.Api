using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Piloto.Api.Application.DTO.DTO
{
    public class ProductDTO
    {
        public ProductDTO() { }
        public ProductDTO(
            int? id, string name, int stock, float price, ICollection<ProductSupplierDTO> productSupplierDTOs)
        {
            Id = id;
            Name = name;
            Stock = stock;
            Price = price;
            ProductSupplierDTOs = productSupplierDTOs;
        }

     

        public int? Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The Name is required to have some value greater than 3 chars",MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [Range(1, 9999, ErrorMessage ="The Stock must be between 1 and 9999")]
        public int Stock { get; set; }
        [Required]
        [Range(0, 9999999.99, ErrorMessage = "Invalid Target Price; Max 10 digits")]
        public float Price { get; set; }

        public ICollection<ProductSupplierDTO> ProductSupplierDTOs { get; set; }

    }
}