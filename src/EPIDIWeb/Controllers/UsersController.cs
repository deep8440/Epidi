using Dapper;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.User;
using Epidi.Service.UsersService;
using EPIDIWeb.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace EPIDIWeb.Controllers
{
	public class UsersController : Controller
	{
		private readonly IUsersService _usersService;
		private StraalConfigModel StraalConfigModel { get; set; }
		public UsersController(IUsersService usersService, IOptions<StraalConfigModel> StraalConfigOption)
		{
			_usersService = usersService;
			this.StraalConfigModel = StraalConfigOption.Value;
		}
		public IActionResult Index()
		{

			ViewBag.Title = "Users List";
			ViewBag.Method = "Users";
			return View();
		}

		[HttpPost]
		public JsonResult Users_All()
		{
			
				long totalRecord = 0;
				long filteredRecord = 0;
				var Draw = Request.Form["draw"].FirstOrDefault();
				PageParam pageParam = new PageParam();

				pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


				pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
				string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
				var response = _usersService.Users_GetAll(pageParam, search);

				totalRecord = response.Item2;
				filteredRecord = response.Item2;

				return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
			
		}

		[HttpGet]
		public async Task<IActionResult> UserPaymentTransaction(string customerId,string userName)
		{
			try
			{
				List<PaymentTransactionRespViewModel> paymentTransactionRespListViewModel = new List<PaymentTransactionRespViewModel>();

				APIResponseModel responseModel = new APIResponseModel();
				string url = StraalConfigModel.BaseUrl + "customers/" + customerId + "/transactions";
				responseModel = await APIService.GetAPIWithToken(url, StraalConfigModel.Token);

				if(responseModel.ResponseCode == 200)
				{
					var response = JsonConvert.DeserializeObject<Root>(responseModel.ResponseMessage);


					for (int i = 0; i < response.data.Count; i++)
					{
						PaymentTransactionRespViewModel paymentTransactionRespViewModel = new PaymentTransactionRespViewModel();

						string cardId = response.data[i].card.id;
						paymentTransactionRespViewModel.CustomerName = userName;
						paymentTransactionRespViewModel.Amount = response.data[i].amount;
						paymentTransactionRespViewModel.Currency = response.data[i].currency;
						double createdAt = response.data[i].created_at;
						DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(createdAt).ToLocalTime();
						string formattedDate = dt.ToString("dd-MM-yyyy");

						paymentTransactionRespViewModel.CreatedAt = formattedDate;


						string cardurl = StraalConfigModel.BaseUrl + "cards/" + cardId;
						APIResponseModel cardData = await APIService.GetAPIWithToken(cardurl, StraalConfigModel.Token);

						var cardResponse = JsonConvert.DeserializeObject<dynamic>(cardData.ResponseMessage);

						paymentTransactionRespViewModel.CardName = cardResponse.brand;


						paymentTransactionRespListViewModel.Add(paymentTransactionRespViewModel);
					}
				}

				
				return View(paymentTransactionRespListViewModel);
			}
			catch (Exception ex) 
			{

				throw;
			}
			
		}

        public async Task<IActionResult> DeleteUser(int Id)
        {

            List<ResponseViewModel> responseViewModel = new List<ResponseViewModel>();
            ResponseViewModel responseViewModel1 = new ResponseViewModel();
            var response = await _usersService.DeleteUser(Id);
            if (response != null)
            {
                responseViewModel1.Code = 200;
                responseViewModel1.Message = "User Deleted Successfully";
            }
            else
            {
                responseViewModel1.Code = 400;
                responseViewModel1.Message = "User Not Deleted";
            }
            responseViewModel.Add(responseViewModel1);
            return Json(responseViewModel);
        }
    }
}
