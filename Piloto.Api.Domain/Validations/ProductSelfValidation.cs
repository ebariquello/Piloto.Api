using FluentValidation;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Validations
{
  
    public class ProductSelfValidation : AbstractValidator<Product>
    {
        public ProductSelfValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
            RuleFor(c => c.Stock)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithMessage("The stock must be greater than or equal 0");
            RuleFor(c => c.Price)
               .NotEmpty()
               .GreaterThan(0)
               .WithMessage("The price must be greater than 0");
        }

    }
}
