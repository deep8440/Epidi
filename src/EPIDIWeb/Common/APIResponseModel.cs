namespace EPIDIWeb.Common
{
	public class APIResponseModel
	{
		public int ResponseCode { get; set; }
		public dynamic ResponseData { get; set; }
		public string ResponseMessage { get; set; }
	}
}
