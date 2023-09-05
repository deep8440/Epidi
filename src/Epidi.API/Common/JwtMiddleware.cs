using Epidi.Service.ApplicationUserService;
using Epidi.Service.Helpers;
using Epidi.Service.TokenService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Epidi.API.Common
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSetting _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSetting> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IApplicationUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, userService, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IApplicationUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Issuer);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                context.Items["User"] =  userService.GetById(userId,null,true,true);
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
