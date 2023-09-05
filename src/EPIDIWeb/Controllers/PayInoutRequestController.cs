using DataTables.AspNet.AspNetCore; 
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.PayInoutRequest;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Users;
using Epidi.Service.PayInoutRequestService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SendGrid;

namespace EPIDIWeb.Controllers
{
    public class PayInoutRequestController : Controller
    {
        private readonly IPayInoutRequestService _payInoutRequestService;
        public PayInoutRequestController(IPayInoutRequestService payInoutRequestService)
        {
            _payInoutRequestService = payInoutRequestService;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "PayInout Request List";
            GetUsersList();

			return View();
        }
        [HttpPost]
        public JsonResult PayInoutRequest_All(string UserId,string FromDate,string ToDate)
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();

                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _payInoutRequestService.GetAll(pageParam, search, UserId,FromDate,ToDate);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }

        [HttpPost]
		public IActionResult PayInOutNote(PayInoutRequestViewModel model)
		{
			return View(model);
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
        public IActionResult SavePayInOutRequest(PayInoutRequestViewModel model)
        {
            try
            {
                if(model.Status == "Approved")
                {
					if (model.RequestType == "IN")
					{
						model.RequestType = "Credit";
						model.CurrentBalance = model.CurrentBalance + model.Amount;

					}
					if (model.RequestType == "OUT")
					{
						model.RequestType = "Debit";
						model.CurrentBalance = model.CurrentBalance - model.Amount;
					}
				}
                else
                {
					if (model.RequestType == "IN")
					{
						model.RequestType = "Credit";
					}
					if (model.RequestType == "OUT")
					{
						model.RequestType = "Debit";
					}
				}
                
				var response = _payInoutRequestService.PayInOut_Transaction_Upsert(model);
				return Json(response);
            }
            catch (Exception)
            {

                throw;
            }
        }

		public void GetUsersList()
		{
			var users = _payInoutRequestService.PayInoutRequest_Users_GetAll().Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name +" "+ p.Surname });

			ViewBag.UsersList = users;
		}

        //for PayoutRequest start

        public IActionResult AddPayoutRequest()
        {
            GetUsersList();
            GetRequestTypeListALL();
            GetRequestStatusListALL();
            return View(new UsersViewModel());
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SavePayout(UsersViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {

                    var res = await _payInoutRequestService.Upsert(model);
                    //if (res != null)
                    //{
                    //    responseViewModel.Code = 200;
                    //    responseViewModel.Message = "PayOutRequest Saved Successfully";
                    //}
                    //else
                    //{
                    //    responseViewModel.Code = 400;
                    //    responseViewModel.Message = "PayOutRequest Not Saved";
                    //}
                    //return Json(responseViewModel);
                }
                else
                {
                    return Json("");
                }
                //return Json(responseViewModel);
                //return View("Index", responseViewModel);
                return Json(new { StatusCode = 200, Message = "PayOutRequest Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SavePayout_new(UsersViewModel model)
        {
            try
            {
                var res = await _payInoutRequestService.Upsert(model);
                return Json(new { StatusCode = 200, Message = "PayOutRequest Saved Successfully" });
            }
            catch
            {
                return Json(null);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _payInoutRequestService.GetById(Id);
            GetRequestTypeListALL();
            GetUsersList();
            GetRequestStatusListALL();
            if (response != null)
            {
                return View("EditPayoutRequest", response);
            }
            else
            {
                return View(new IBViewModel());
            }


        }

        public void GetRequestTypeListALL()
        {
            var DropdownlistRequestType = _payInoutRequestService.GetRequestTypeListALL().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.DropdownlistRequestType = DropdownlistRequestType;
        }

        public void GetRequestStatusListALL()
        {
            var DropdownlistRequestStatus = _payInoutRequestService.GetRequestStatusListALL().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Status });

            ViewBag.DropdownlistRequestStatus = DropdownlistRequestStatus;
        }

        [HttpPost]
        public JsonResult PayoutRequest_All(string UserId, string FromDate, string ToDate)
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();

                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _payInoutRequestService.GetAll_Payout(pageParam, search, UserId, FromDate, ToDate);

                totalRecord = response.Item2;
                filteredRecord = response.Item2;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
            }
        }

        public async Task<IActionResult> DeletePayout(int Id)
        {
            List<ResponseViewModel> responseViewModel = new List<ResponseViewModel>();
            ResponseViewModel responseViewModel1 = new ResponseViewModel();
            var res = await _payInoutRequestService.DeletePayoutById(Id);
            if (res != null)
            {
                responseViewModel1.Code = 200;
                responseViewModel1.Message = "Payout Deleted Successfully";
            }
            else
            {
                responseViewModel1.Code = 400;
                responseViewModel1.Message = "Payout Not Deleted";
            }
            responseViewModel.Add(responseViewModel1);
            return Json(responseViewModel);
        }
    }
}
