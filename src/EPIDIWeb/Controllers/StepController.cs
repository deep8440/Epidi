using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Steps;
using Epidi.Service.CountryService;
using Epidi.Service.QuestionService;
using Epidi.Service.StepsService;
using EPIDIWeb.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace EPIDIWeb.Controllers
{
    public class StepController : Controller
    {
        private readonly IStepsService _stepService;
        private readonly ICountryService _countryService;
        private readonly IQuestionService _questionService;
        public StepController(IStepsService StepsService, ICountryService countryService, IQuestionService questionService)
        {
            _stepService = StepsService;
            _countryService = countryService;
            _questionService = questionService;
        }

        #region List
        public IActionResult Index()
        {
            ViewBag.Title = "Steps  List";
            ViewBag.Method = "Step";
            return View();
        }

        [HttpPost]
        public JsonResult Step_All()
        {
            long totalRecord = 0;
            long filteredRecord = 0;
            var Draw = Request.Form["draw"].FirstOrDefault();
            PageParam pageParam = new();
            pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
            var response = _stepService.GetAllSteps(pageParam, search);

            totalRecord = response.Result.Item2;
            filteredRecord = response.Result.Item2;

            var responseData = OrderByExtension.OrderBy(response.Result.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();


            return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = responseData });
            //return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord });
        }
        #endregion

        #region Add Steps
        public IActionResult AddSteps()
        {
            GetCountry();
            return View(new StepsViewModel());
        }
        public IActionResult CopySteps(int CountryId)
        {
            ViewBag.CountryId = CountryId;
            List<SelectListItem> CountryListWithSelected = GetCountry(CountryId);
            GetCountry();
            GetStepsByCountryId(CountryId);
            CountryListWithSelected.Find(x => x.Value == CountryId.ToString()).Selected = true;

            return View(CountryListWithSelected);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveStep(StepsViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var res = _stepService.SaveSteps(model);
                    return Json(res);
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
        #endregion

        #region Edit
        [HttpGet]

        public async Task<IActionResult> Edit(int StepId)
        {
            var EditStep = await _stepService.GetStepsById(StepId);
            GetCountry();
            if (EditStep != null)
            {
                return View("AddSteps", EditStep);
            }
            else
            {
                return View(new StepsViewModel());
            }
        }
        #region Get Country
        public List<SelectListItem> GetCountry(int CountryId)
        {
            var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName }).ToList();
            return country;
        }

        public void GetCountry()
        {
            var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName }).ToList();

            ViewBag.CountryList = country;
        }

        public void GetStepsByCountryId(int CountryId)
        {
            var steps = _stepService.Steps_GetByCountryId(CountryId, 0).Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Title });
            ViewBag.StepsList = steps;
        }

        #endregion
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> DeleteStep(int StepId)
        {
            var res = _stepService.DeleteSteps(StepId);
            return Json(res);
            //return View("Index");
        }
        #endregion

        #region CopyStep
        [HttpGet]
        public async Task<IActionResult> Step_Copy(int FromCountry,int ToCountry)
        {
            PageParam pageParam = new();
            pageParam.Offset = 0;
            pageParam.Limit = 100;
            
            var from_Steps = _stepService.GetAllSteps(pageParam,"",FromCountry);

            foreach (var item in from_Steps.Result.Item1)
            {
                item.Id = 0;
                item.CountryId = ToCountry;
                await _stepService.SaveSteps(item);
            }
            return Json(new { StatusCode = 200, Message = "Step copy Successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> Step_CopyByCountry(int FromCountry, int SelectedStep, int ToCountry)
        {
            PageParam pageParam = new();
            pageParam.Offset = 0;
            pageParam.Limit = 100;

            var from_Steps = _stepService.GetAllSteps(pageParam, "", FromCountry);
            //List<StepsViewModel> steps = await _stepService.Steps_GetByCountryId(FromCountry);
            var step = from_Steps.Result.Item1.Where(x => x.Id == SelectedStep).FirstOrDefault();

            if (step != null)
            {
                step.Id = 0;
                step.CountryId = ToCountry;
                var response = await _stepService.SaveSteps(step);

                
                var questions = GetQuestionsByCountryIdAndStepId(FromCountry, SelectedStep);
                if (questions != null && questions.Count>0)
                {
                    var Country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName }).ToList();
                    string CountryName = Country.Where(x => x.Value == ToCountry.ToString()).FirstOrDefault().Text;
                    
                    foreach (var item in questions)
                    {
                        item.QuestionId = 0;
                        item.CountryId = ToCountry;
                        item.CountryName = CountryName;
                        _questionService.SaveQuestions(item);
                    }
                }
                return Json(new { StatusCode = response.Code, Message = response.Message.ToString() });

            }
            return Json(new { StatusCode = 400, Message = "Step not found" });
        }

        public List<QuestionViewModel> GetQuestionsByCountryIdAndStepId(int CountryId,int StepId)
        {
            PageParam pageParam = new PageParam();
            if (CountryId == 0)
                pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            else
                pageParam.Offset = 0;

            pageParam.Limit = 100;
            var response = _questionService.GetAllQuestionForAdmin(pageParam, "", CountryId,0,"","");

            return response.Item1.Where(x=>x.StepId == StepId).ToList();
        }
        #endregion
    }
}
