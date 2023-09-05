namespace Epidi.API.Common
{
	public class APIResponseModel
	{
		public int ResponseCode { get; set; }
		public dynamic ResponseData { get; set; }
		public string ResponseMessage { get; set; }
		public List<Errors> errors { get; set; }
	}
	public class Errors
	{
		public long code { get; set; }
		public string message { get; set; }
	}
}
