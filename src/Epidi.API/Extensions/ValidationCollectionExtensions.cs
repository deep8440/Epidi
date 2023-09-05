using Epidi.Models.ViewModel.User;
using FluentValidation;

namespace Epidi.API.Extensions
{
    public static class ValidationCollectionExtensions
    {
        public static IServiceCollection ModelValications(this IServiceCollection service)
        {
          //  service.AddSingleton<IValidator<RegisterUserViewModel>, RegisterUserViewModelValidator>();
            return service;
        }
    }
}
