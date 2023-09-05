using Epidi.Models.Model;
using Epidi.Models.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.Helpers
{
    public class AuthenticateResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public int StepCompleted { get; set; }
        public int ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public long CountryId { get; set; }

        public bool RememberMe { get; set; }

        public AuthenticateResponse(HttpStatusCode _statusCode, string _ErrorMessage)
        {
            statusCode = _statusCode;
            ErrorMessage = _ErrorMessage;
        }

        public AuthenticateResponse(ApplicationUserDetail user, string token)
        {
            Id = user.Id;
            Email = user.Email;
            Token = token;
            CountryId = user.CountryId;
            RememberMe = user.IsTermsConditionAgreed;
            StepCompleted = user.StepCompleted;
            statusCode = HttpStatusCode.OK;
        }
    }
}
