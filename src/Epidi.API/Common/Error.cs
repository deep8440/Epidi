using Newtonsoft.Json;

namespace Epidi.API.Common
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
