using FluentValidation;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Validations
{
  
    public class SupplierSelfValidation : AbstractValidator<Supplier>
    {
        public SupplierSelfValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
            RuleFor(c => c.CNPJ)
                .NotEmpty().WithMessage("Please ensure you have entered the CNPJ")
                .Length(10, 20).WithMessage("The Name must have between 10 and 150 characters"); 
        }

    }
}
