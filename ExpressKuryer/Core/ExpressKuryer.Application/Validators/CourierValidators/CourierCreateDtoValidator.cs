using ExpressKuryer.Application.DTOs.CourierDTOs;
using ExpressKuryer.Application.HelperManager;
using FluentValidation;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Validators.CourierValidators
{
    public class CourierCreateDtoValidator : AbstractValidator<CourierCreateDto>
    {
        public CourierCreateDtoValidator()
        {
            RuleFor(x => x.CourierPerson.Address).NotNull().NotEmpty().MaximumLength(500);
            RuleFor(x=>x.CourierPerson.Name).NotEmpty().NotNull().MaximumLength(200);
            RuleFor(x=>x.CourierPerson.Surname).NotEmpty().NotNull().MaximumLength(200);
            RuleFor(x => x.CourierPerson.Password).NotNull().NotEmpty().MinimumLength(5);
            RuleFor(x => x.CourierPerson.DuplicatePassword).NotEmpty().NotNull().MinimumLength(5).Equal(x=>x.CourierPerson.Password);
        }
    }
}
