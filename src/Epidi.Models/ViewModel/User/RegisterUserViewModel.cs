using Epidi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.User
{
    public class RegisterUserViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ReEnterPassword { get; set; }

        public long CountryId { get; set; }

        public UserRole Role { get; set; }
        public string PromoCode { get; set; }
        public bool IsDesclaimer { get; set; }
        public string SocialId { get; set; }
        public string SocialType { get; set; }
    }

    public class ApplicationUserDetail
    {
        public long Id { get; set; }
        public string Email { get; set; }

        public long CountryId { get; set; }
        public int ErrorType { get; set; }

        public UserRole Role { get; set; }

        public int StepCompleted { get; set; }
        
        public string VerificationCode { get; set; }
        public bool IsTermsConditionAgreed { get; set; }
        public string PromoCode { get; set; }
        public bool IsDesclaimer { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode statusCode { get; set; }
    }


    public class RegisterUserDataViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int MobileCountryCode { get; set; }

        public string Mobile { get; set; }
    }
}
