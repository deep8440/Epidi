using Epidi.Models.ViewModel.TokenResult;
using Epidi.Models.ViewModel.User;
using Epidi.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TokenService
{
    public interface ITokenService
    {
        Task<AuthenticateResponse> Authenticate(LoginViewModel model);
    }
}
