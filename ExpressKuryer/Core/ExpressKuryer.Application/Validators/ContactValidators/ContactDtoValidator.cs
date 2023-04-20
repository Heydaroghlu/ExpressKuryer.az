using ExpressKuryer.Application.DTOs.Contact;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Validators.ContactValidators
{
    public class ContactDtoValidator : AbstractValidator<ContactDto>
    {
        public ContactDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotNull().
                MaximumLength(35);

            RuleFor(x => x.Email)
                .NotEmpty().NotNull()
                .MaximumLength(100);

            RuleFor(x => x.Phone)
                .NotNull().NotEmpty()
                .MaximumLength(35);

            RuleFor(x => x.Message)
                .NotNull().NotEmpty()
                .MaximumLength(500);
        }
    }
}
