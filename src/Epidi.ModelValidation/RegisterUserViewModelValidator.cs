using Epidi.Models.ViewModel.User;
using FluentValidation;

namespace Epidi.ModelValidation
{
    public class RegisterUserViewModelValidator: AbstractValidator<RegisterUserViewModel>
    {
        public RegisterUserViewModelValidator()
        {
            RuleFor(x => x.Email)
               .NotNull()
               .NotEmpty();

            RuleFor(x => x.CountryId)
               .NotNull()
               .NotEmpty();

            RuleFor(x => x.Role)
              .NotNull()
              .NotEmpty()
              ;

            RuleFor(x => x.Password)
                .MinimumLength(3)
                .MaximumLength(50)
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches("^[^£# “”]*$").WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.")
                .Matches(@"[""!@$%^&*(){}:;<>,.?/+\-_=|'[\]~\\]").WithMessage("'{PropertyName}' must contain one or more special characters.");

            RuleFor(customer => customer.Password).Equal(customer => customer.ReEnterPassword)
                .WithMessage("Password not matched.");
        }
    }
}