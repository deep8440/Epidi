using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }

    public class JwtSetting
    {
        public string Issuer { get; set; }
        public string BaseUrl { get; set; }

        public string Audience { get; set; }

        public int LifeTimeMinutes { get; set; }

        public int RefreshTokenLifeTimeDays { get; set; }

    }
    
    public class FileUploads
    {
        public string BaseUrl { get; set; }
    }
}
