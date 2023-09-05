using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.MasterLP;
using Epidi.Service.MasterLPService;
using Microsoft.AspNetCore.Mvc;

namespace EPIDIWeb.Controllers
{
    public class MasterLPController : Controller
    {
        private readonly IMasterLPService _masterLPService;
        public MasterLPController(IMasterLPService masterLPService)
        {
            _masterLPService = masterLPService;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Master LP  List";
            return View();
        }
        [HttpPost]
        public JsonResult Get_All()
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new PageParam();

            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _masterLPService.GetAll(pageParam, search);

            totalRecord = response.Item2;
            filteredRecord = response.Item2;

            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
        }
        public IActionResult AddMasterLP()
        {
            return View(new MasterLPViewModel());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveMasterLP(MasterLPViewModel model)
        {
            try
            {
                ResponseViewModel responseViewModel = new ResponseViewModel();
                if (model != null)
                {
                    var res = await _masterLPService.Upsert(model);
                    if (res != null)
                    {
                        responseViewModel.Code = 200;
                        responseViewModel.Message = "MasterLP Saved Successfully";
                    }
                    else
                    {
                        responseViewModel.Code = 400;
                        responseViewModel.Message = "MasterLP Not Saved";
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
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var response = await _masterLPService.GetById(Id);

            if (response != null)
            {
                return View("AddMasterLP", response);
            }
            else
            {
                return View(new MasterLPViewModel());
            }
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var res = _masterLPService.Delete(Id);
            return Json(res);
        }
    }
}
