using Epidi.Models.Paging;
using Epidi.Models.ViewModel.LP;
using Epidi.Models.ViewModel.LPWiseInstrumentPriority;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.LPWiseInstrumentPriorityService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPIDIWeb.Controllers
{
    public class LPWiseInstrumentPriorityController : Controller
    {
        private readonly ILPWiseInstrumentPriorityService _lPWiseInstrumentPriorityService;
        private readonly IBDMService _BDMService;
        public LPWiseInstrumentPriorityController(ILPWiseInstrumentPriorityService lPWiseInstrumentPriorityService, IBDMService bDMService)
        {
            _lPWiseInstrumentPriorityService = lPWiseInstrumentPriorityService;
            _BDMService = bDMService;
        }
        public IActionResult Index()
        {
            GetLP();
            return View(new LPViewModel());
        }
        [HttpPost]
        public JsonResult LPWiseInstrumentPriority_All()
        {
            {
                long totalRecord = 0;
                long filteredRecord = 0;
                var Draw = Request.Form["draw"].FirstOrDefault();
                PageParam pageParam = new PageParam();
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
                var response = _lPWiseInstrumentPriorityService.GetLPWiseInstrumentPriority();

                totalRecord = response.Result.Count;
                filteredRecord = response.Result.Count;

                return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Result });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority )
        {
            lPWiseInstrumentPriority.CreatedDate= DateTime.Now;
            lPWiseInstrumentPriority.CreatedBy= 1;

            var response=await _lPWiseInstrumentPriorityService.AddLPWiseInstrumentPriority(lPWiseInstrumentPriority);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> EditLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority )
        {
            lPWiseInstrumentPriority.UpdatedDate= DateTime.Now;
            lPWiseInstrumentPriority.UpdatedBy= 1;

            var response=await _lPWiseInstrumentPriorityService.EditLPWiseInstrumentPriority(lPWiseInstrumentPriority);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> GetLPWiseInstrumentPriorityById(int Id)
        {
            var response = await _lPWiseInstrumentPriorityService.GetLPWiseInstrumentPriorityById(Id);
            return Json(response);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteLPWiseInstrumentPriority(int id)
        {
            int DeletedBy = 1;
            var response =await _lPWiseInstrumentPriorityService.RemoveLPWiseInstrumentPriority(id, DeletedBy);
            return Json(response);
        } 
        [HttpPost]
        public async Task<JsonResult> ActiveInActiveLPWiseInstrumentPriority(int id,bool IsActive)
        {
            var response =await _lPWiseInstrumentPriorityService.ActiveInActiveLPWiseInstrumentPriority(id, IsActive);
            return Json(response);
        } 
        public void GetLP()
        {
            var FieldsList = _lPWiseInstrumentPriorityService.Get_All_LP().Result.Select(p => new SelectListItem()
            {
                Value = p.LPId.ToString(),
                Text = p.LPName
            });

            ViewBag.LPList = FieldsList;
        }

    }
}
