using Epidi.Models;
using Epidi.Models.ExceptionHelper;
using Epidi.Models.Extensions;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.TokenResult;
using Epidi.Models.ViewModel.User;
using Epidi.Repository.ApplicationUserRepository;
using Epidi.Service.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
//using Epidi.Repository.Provider;
//using Epidi.Repository.UserStrategy;
//using Epidi.Repository.UserStrategyContext;
//using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TokenService
{
    public class TokenService : ITokenService
    {

        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly JwtSetting _jwtSetting;
        public TokenService(
            IApplicationUserRepository applicationUserRepository,
            IOptions<JwtSetting> jwtSetting
            )
        {
            _applicationUserRepository = applicationUserRepository;
            _jwtSetting = jwtSetting.Value;
        }


        public async Task<AuthenticateResponse> Authenticate(LoginViewModel model)
        {
            if (model.SocialId == null || model.SocialId == "" || model.SocialType == null || model.SocialType == "")
            {
                if (model.Email != null || model.Email != "" || model.Password != null || model.Password != "")
                {
                    var user = await _applicationUserRepository.AuthenticateUser(model.Email, model.Password, model.SocialId, model.SocialType);
                    if (user == null)
                    {
                        AuthenticateResponse resp = new AuthenticateResponse(HttpStatusCode.Forbidden, " This email is not registered with our community. Please sign-up with our community");
                        resp.ErrorType = 1;
                        return resp;
                    }
                    else if (user.ErrorType == 2)
                    {
                        AuthenticateResponse resp = new AuthenticateResponse(HttpStatusCode.Forbidden, "Ooops! Wrong Password. Please Try again");
                        resp.ErrorType = 2;
                        return resp;
                    }

                    // authentication successful so generate jwt token
                    var token = generateJwtToken(user);
                    return new AuthenticateResponse(user, token);
                }
                else
                {
                    AuthenticateResponse resp = new AuthenticateResponse(HttpStatusCode.Accepted, "Email or password is reqired");
                    return resp;
                }

            }
            else
            {
                var user = await _applicationUserRepository.AuthenticateUser(model.Email, model.Password, model.SocialId, model.SocialType);
                if (user == null)
                {
                    AuthenticateResponse resp = new AuthenticateResponse(HttpStatusCode.Accepted, "Ooops! Wrong Password. Please Try again");
                    return resp;
                }
                var token = generateJwtToken(user);
                return new AuthenticateResponse(user, token);
            }


        }

        private string generateJwtToken(ApplicationUserDetail user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.Issuer);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                { new Claim("id", user.Id.ToString()),
                  new Claim("sub", user.Email.ToString()),
                  //new Claim("sub", user.Email.ToString()),
                  new Claim("country", user.CountryId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
