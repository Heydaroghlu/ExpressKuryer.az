using ExpressKuryer.Application.DTOs.Vacancy;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.Validators.VacancyValidators
{
    public class VacancyDtoValidator : AbstractValidator<VacancyDto>
    {

        public VacancyDtoValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty().NotNull()
             .MaximumLength(35);

            RuleFor(x => x.Surname)
             .NotEmpty().NotNull().
             MaximumLength(35);

            RuleFor(x => x.Message)
             .NotEmpty().NotNull().
             MaximumLength(500);
        }

    }
}
