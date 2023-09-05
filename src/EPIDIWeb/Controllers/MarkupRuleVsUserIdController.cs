using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.MarkupRuleVsUserId;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.MarkupRuleVsUserIdService;
using Epidi.Service.UsersService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPIDIWeb.Controllers
{
    public class MarkupRuleVsUserIdController : Controller
    {
        private readonly IMarkupRuleVsUserIdService _MarkupRuleVsUserIdService;
        private readonly IUsersService _usersService;
        public MarkupRuleVsUserIdController(IMarkupRuleVsUserIdService MarkupRuleVsUserIdService, IUsersService usersService)
        {
            _MarkupRuleVsUserIdService = MarkupRuleVsUserIdService;
            _usersService = usersService;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "QuoteMarkUpRule List";
            GetUsers();
            GetQuoteMarkUpRule();
            return View(new MarkupRuleVsUserId());
        }
        [HttpPost]
        public JsonResult MarkupRuleVsUserId_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _MarkupRuleVsUserIdService.GetMarkupRuleVsUserId();

                totalRecord = response.Result.Count;
                filteredRecord = response.Result.Count;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Result });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId )
        {
            MarkupRuleVsUserId.CreatedDate= DateTime.Now;
            var response=await _MarkupRuleVsUserIdService.AddMarkupRuleVsUserId(MarkupRuleVsUserId);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> EditMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId )
        {
            var response=await _MarkupRuleVsUserIdService.EditMarkupRuleVsUserId(MarkupRuleVsUserId);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> GetMarkupRuleVsUserIdById(int Id)
        {
            var response = await _MarkupRuleVsUserIdService.GetMarkupRuleVsUserIdById(Id);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteMarkupRuleVsUserId(int id)
        {
            var response =await _MarkupRuleVsUserIdService.RemoveMarkupRuleVsUserId(id);
            return Json(response);
        }
        public void GetUsers()
        {
            PageParam pageParam = new PageParam();

            var users = _usersService.Users_GetAll(pageParam, "").Item1.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.UserName });

            ViewBag.UsersList = users;
        }
         public void GetQuoteMarkUpRule()
        {
            PageParam pageParam = new PageParam();

            var quoteMarkUpRule = _MarkupRuleVsUserIdService.QuoteMarkUpRule_GetAll(pageParam, "").Item1.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.QuoteMarkUpRuleList = quoteMarkUpRule;
        }

    }
}
