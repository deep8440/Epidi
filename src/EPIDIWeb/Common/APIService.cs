using Epidi.Models.ViewModel.User;
using Newtonsoft.Json;

namespace EPIDIWeb.Common
{
	public static class APIService
	{
		private static readonly HttpClient client = new HttpClient();
		private static readonly APIResponseModel responseModel = new APIResponseModel();
		#region Post API With Token
		public static async Task<dynamic> PostAPIWithToken(string url, StringContent stringContent = null, string token = "")
		{
			try
			{

				//client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", token);
				client.DefaultRequestHeaders.Remove("Authorization");
				client.DefaultRequestHeaders.Add("Authorization", token);
				var httpResponseMessage = await client.PostAsync(url, stringContent);
				var response = await httpResponseMessage.Content.ReadAsStringAsync();
				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					responseModel.ResponseData = JsonConvert.DeserializeObject<dynamic>(response);
					responseModel.ResponseCode = 400;
					return responseModel;
					//return JsonConvert.DeserializeObject<dynamic>(response); 
				}
				else
				{
					responseModel.ResponseData = JsonConvert.DeserializeObject<dynamic>(response);
					responseModel.ResponseCode = 200;
					return responseModel;
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		#endregion Post API With Token

		#region GET API With Token
		public static async Task<dynamic> GetAPIWithToken(string url, string token)
		{
			try
			{
				client.DefaultRequestHeaders.Remove("Authorization");
				client.DefaultRequestHeaders.Add("Authorization", token);
				var httpResponseMessage = await client.GetAsync(url);
				var response = await httpResponseMessage.Content.ReadAsStringAsync();

				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					//responseModel.ResponseData = JsonConvert.DeserializeObject<dynamic>(response);
					responseModel.ResponseMessage = response;
					responseModel.ResponseCode = 400;
					return responseModel;
				}
				else
				{
					//responseModel.ResponseData = JsonConvert.DeserializeObject<dynamic>(response);
					responseModel.ResponseMessage = response;
					responseModel.ResponseCode = 200;
					return responseModel;
				}
			}
			catch (Exception ex)
			{
				return ex;
			}
		}
		#endregion GET API With Token
	}
}
