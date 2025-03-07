using FluentValidation;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Validations
{
  
    public class ProductSupplierSelfValidation : AbstractValidator<ProductSupplier>
    {
        public ProductSupplierSelfValidation()
        {
            RuleFor(c => c.SupplierId)
                .NotEmpty().GreaterThan(0).WithMessage("Please ensure you have entered the SupplierId and value must be Greater than 0");
                
            RuleFor(c => c.ProductId)
                .NotEmpty().GreaterThan(0).WithMessage("Please ensure you have entered the ProductId and value must be Greater than 0"); ;
         
        }

    }
}
