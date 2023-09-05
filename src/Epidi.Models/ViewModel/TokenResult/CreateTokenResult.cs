using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TokenResult
{
    public class CreateTokenResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpires { get; set; }
        public DateTimeOffset Issued { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
