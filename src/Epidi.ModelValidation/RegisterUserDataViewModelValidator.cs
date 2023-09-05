using Epidi.Models.ViewModel.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.ModelValidation
{
    public class RegisterUserDataViewModelValidator : AbstractValidator<RegisterUserDataViewModel>
    {
        public RegisterUserDataViewModelValidator()
        {
            RuleFor(x => x.Surname)
         .NotNull()
         .NotEmpty();

            RuleFor(x => x.Name)
         .NotNull()
         .NotEmpty();

            RuleFor(x => x.DateOfBirth)
        .NotNull()
        .NotEmpty();

            RuleFor(x => x.MobileCountryCode)
.NotNull()
.NotEmpty();

            RuleFor(x => x.Mobile)
     .NotNull()
     .NotEmpty();

        }
    }
}
