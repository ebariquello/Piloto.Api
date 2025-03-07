using FluentValidation;
using Piloto.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piloto.Api.Domain.Validations
{
  
    public class SupplierAddressSelfValidation : AbstractValidator<SupplierAddress>
    {
        public SupplierAddressSelfValidation()
        {
            RuleFor(c => c.Country)
             .NotEmpty().WithMessage("Please ensure you have entered the Country")
             .Length(2, 50).WithMessage("The Name must have between 2 and 50 characters");

            RuleFor(c => c.Street)
                .NotEmpty().WithMessage("Please ensure you have entered the Street")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
            
            RuleFor(c => c.City)
             .NotEmpty().WithMessage("Please ensure you have entered the City")
             .Length(2, 50).WithMessage("The Name must have between 2 and 50 characters");

            RuleFor(c => c.PostalCode)
               .NotEmpty().WithMessage("Please ensure you have entered the PostalCode")
               .Length(2, 50).WithMessage("The Name must have between 2 and 50 characters");
            
            //RuleFor(c => c.SupplierId)
            //   .NotEmpty()
            //   .GreaterThan(0).WithMessage("The Name must be greater than 0");


            RuleFor(c => c.HouseNumber)
               .NotEmpty().WithMessage("Please ensure you have entered the HouseNumber")
               .Length(1, 10).WithMessage("The Name must have between 1 and 10 characters");

            RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Please ensure you have entered the Name")
            .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
        }

        public static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-18);
        }
    }
}
