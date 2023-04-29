using ExpressKuryer.Application.DTOs.Partner;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Validators.PartnerValidators
{
    public class PartnerCreateDtoValidator : AbstractValidator<PartnerCreateDto>
    {

        public PartnerCreateDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(50);

            RuleFor(x => x.FormFile)
                .NotEmpty().NotNull();
        }

    }
}
