using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.TokenResult
{
    public class TokenResultModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = "bearer";

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(".issued")]
        public string Issued { get; set; }

        [JsonProperty(".expires")]
        public string Expires { get; set; }

        [JsonProperty(".refresh_token_expires")]
        public string RefreshTokenExpires { get; set; }

       // public string[] Roles { get; set; }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string PushNotificationRegistrationId { get; set; }

        public long ExpirationTimestamp { get; set; }
    }
}
