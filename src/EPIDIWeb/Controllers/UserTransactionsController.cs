using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.UsersTransactions;
using Epidi.Service.UsersService;
using Epidi.Service.UsersTransactionsService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPIDIWeb.Controllers
{
    public class UserTransactionsController : Controller
    {
		private readonly IUsersTransactionsService _usersTransactionsService;
		private readonly IUsersService _usersService;
		public UserTransactionsController(IUsersTransactionsService usersTransactionsService, IUsersService usersService)
		{
			_usersTransactionsService = usersTransactionsService;
			_usersService = usersService;
		}
        public IActionResult Index()
        {
			ViewBag.Title = "User Transactions List";
			ViewBag.Method = "User Transactions";
			GetTransactionUsersList();
			GetTransactionType();
			return View();
        }

		public IActionResult AddUserTransaction()
		{
			GetUsers();
			GetTransactionType();
			return View(new UsersTransactionViewModel());
		}

		[HttpPost]
		public JsonResult User_Transactions_All(string UserId, string FromDate, string ToDate,string Type)
		{
			{
				long totalRecord = 0;
				long filteredRecord = 0;
				var Draw = Request.Form["draw"].FirstOrDefault();
				PageParam pageParam = new PageParam();

				pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


				pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
				string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
				var response = _usersTransactionsService.Get_All_Users_Transaction(pageParam, search, UserId, FromDate, ToDate, Type);

				totalRecord = response.Item2;
				filteredRecord = response.Item2;

				return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
			}
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public IActionResult SaveUserTransaction(UsersTransactionViewModel model)
		{
			try
			{
				ResponseViewModel responseViewModel = new ResponseViewModel();
				if (model != null)
				{
					var res = _usersTransactionsService.Upsert(model);
					if(res != null)
					{
						responseViewModel.Code = 200;
						responseViewModel.Message = "User Transaction Saved Successfully";
					}
					else
					{
						responseViewModel.Code = 400;
						responseViewModel.Message = "User Transaction Not Saved Successfully";
					}
					return Json(responseViewModel);
				}
				else
				{
					return Json("");
				}
			}
			catch (Exception ex)
			{
				return Json(ex);
			}
		}

		public void GetTransactionUsersList()
		{
			var users = _usersTransactionsService.UserTransaction_Users_GetAll().Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name + " " + p.Surname });

			ViewBag.TransactionUsersList = users;
		}

		public void GetUsers()
		{
			PageParam pageParam = new PageParam();	

			var users = _usersService.Users_GetAll(pageParam, "").Item1.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.UserName });

			ViewBag.UsersList = users;
		}

		public void GetTransactionType()
		{
			var transactionType = new List<SelectListItem>
					{
						new SelectListItem{ Value = "Credit", Text = "Credit"},
						new SelectListItem{ Value = "Debit", Text = "Debit"}
					};

			ViewBag.transactionType = transactionType;
		}
	}
}
