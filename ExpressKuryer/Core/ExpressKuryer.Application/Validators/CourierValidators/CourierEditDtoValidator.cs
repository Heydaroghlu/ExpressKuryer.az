using ExpressKuryer.Application.DTOs.CourierDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Validators.CourierValidators
{
    public class CourierEditDtoValidator : AbstractValidator<CourierEditDto>
    {
        public CourierEditDtoValidator()
        {
            RuleFor(x => x.CourierPerson.Address).NotNull().NotEmpty().MaximumLength(500);
            RuleFor(x => x.CourierPerson.Name).NotEmpty().NotNull().MaximumLength(200);
            RuleFor(x => x.CourierPerson.Surname).NotEmpty().NotNull().MaximumLength(200);
            RuleFor(x => x.CourierPerson.Password).NotNull().NotEmpty().MinimumLength(5);
            RuleFor(x => x.CourierPerson.DuplicatePassword).NotEmpty().NotNull().MinimumLength(5).Equal(x => x.CourierPerson.Password);
        }
    }
}
