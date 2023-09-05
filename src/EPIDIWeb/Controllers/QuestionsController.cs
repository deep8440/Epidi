//using AspNetCore;
using Epidi.Enums;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Service.AnswerService;
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
	public class QuestionsController : Controller
	{
		private readonly IQuestionService _questionService;
		private readonly ICountryService _countryService;
		private readonly IStepsService _stepService;
        private readonly IAnswerService _answerService;


        public QuestionsController(IQuestionService questionService, 
			ICountryService countryService,
			IStepsService StepsService,
            IAnswerService answerService)
		{
			_questionService = questionService;
			_countryService = countryService;
			_stepService = StepsService;
			_answerService = answerService;
        }

		//public IActionResult Index(long? CountryId, int? StepId, bool? IsActive)
		//{
		//    var questions = _questionService.GetQuestionsByCountryIdStepId(CountryId, StepId, IsActive == null ? true : false);
		//    return View(questions);
		//}

		public IActionResult Index()
		{
			ViewBag.Title = "Questions  List";
			ViewBag.Method = "Questions";
			GetCountry();
			return View();
		}

		public IActionResult AddQuestion()
		{
			GetCountry();
			GetAnsType();
			Steps_GetByCountryId(1);
			var model = new QuestionViewModel();
            if (model.Answers.Count <= 0)
                model.Answers.Add(new AnswerViewModel() { AnswerDesc = "" });

            return View(model);
		}
		public IActionResult AddMoreAnswer()
		{
            var model = new AnswerViewModel() { AnswerDesc = "" };
			return View("AddAnswer",model);
        }

		[HttpGet]
		public async Task<IActionResult> Edit(long QuestionId)
		{
			var questions = await _questionService.GetQuestionByQuestionId(QuestionId);
            var answers = await _answerService.GetAnswerByQuestionId(QuestionId);

			if(answers.Count > 0)
			    questions.Answers = answers;
            else
                questions.Answers.Add(new AnswerViewModel() { AnswerDesc = "" });

            GetCountry();
			GetAnsType();
			Steps_GetByCountryId(Convert.ToInt32(questions.CountryId));
			questions.StepId = Convert.ToInt32(questions.StepId);
            if (questions != null)
			{
				return View("AddQuestion", questions);
			}
			else
			{
				return View(new QuestionViewModel());
			}
		}


		[ValidateAntiForgeryToken]
		[HttpPost]
		public IActionResult Manage(QuestionViewModel model)
		{
			var questions = _questionService.ManageQuestions(model);
			return View(questions);
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public IActionResult SaveQuestions(QuestionViewModel model)
		{
			try
			{
				if (model != null)
				{
					var res = _questionService.SaveQuestions(model);
					for (int i = 0; i < model.Answers.Count; i++)
					{
                        model.Answers[i].QuestionId =  res[0].Id;
                        var ansRes = _answerService.SaveAnswer(model.Answers[i]);
                    }
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


		[HttpPost]
		public JsonResult Question_All(int countryId)
		{
			{
				
				long totalRecord = 0;
				long filteredRecord = 0;
				var Draw = Request.Form["draw"].FirstOrDefault();
				PageParam pageParam = new PageParam();
				if (countryId == 0)
					pageParam.Offset = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
				else
					pageParam.Offset = 0;

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                pageParam.Limit = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
				string search = Convert.ToString(Request.Form["search[value]"].FirstOrDefault() ?? "");
				var response = _questionService.GetAllQuestionForAdmin(pageParam, search, countryId,0,sortColumn, sortColumnDirection);

                //var responseData = OrderByExtension.OrderBy(response.Item1.AsQueryable(), sortColumn, sortColumnDirection).ToList();

                totalRecord = response.Item2;
				filteredRecord = response.Item2;

				return Json(new { draw = Draw, recordsTotal = totalRecord, recordsFiltered = filteredRecord, data = response.Item1 });
			}
		}

		[HttpGet]
		public IActionResult DeleteQuestion(int questionId)
		{
			var res = _questionService.DeleteQuestion(questionId);
			return Json(res);
		}

		public void GetCountry()
		{
			var country = _countryService.GetAllCountries().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.CountryName });

			ViewBag.CountryList = country;
		}

        [HttpGet]
        public IActionResult Steps_GetByCountryId(int CountryId)
        {
			//         List<SelectListItem> items = new List<SelectListItem>();
			var res = _questionService.Steps_GetByCountryId(CountryId);
			//         foreach (var master in res)
			//         {
			//             items.Add(new SelectListItem() { Text = master.CountryName.ToString(), Value = Convert.ToString(master.StepId) });
			//         }
			ViewBag.StepList = res;
			return Json(res);
        }
		
		[HttpGet]
        public IActionResult OnlySteps_GetByCountryId(int CountryId)
        {
			//         List<SelectListItem> items = new List<SelectListItem>();
			var res = _questionService.OnlySteps_GetByCountryId(CountryId);
			//         foreach (var master in res)
			//         {
			//             items.Add(new SelectListItem() { Text = master.CountryName.ToString(), Value = Convert.ToString(master.StepId) });
			//         }
			ViewBag.StepList = res;
			return Json(res);
        }


		public void GetAnsType()
		{
            //var enumData = from AnsType type in Enum.GetValues(typeof(AnsType))
            //               select new
            //               {
            //                   Value = type,
            //                   Text = Enum.GetName(typeof(AnsType), x),
            //               };


            var ansTypes = _questionService.GetAllAnsTypes().Result.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name });

            ViewBag.GetAnsType = ansTypes;


   //         var enumData = new SelectList(Enum.GetValues(typeof(AnsType))
			//			.OfType<Enum>()
			//			.Select(x => new SelectListItem
			//			{
			//				Text = Enum.GetName(typeof(AnsType), x),
			//				Value = (Convert.ToInt32(x)+1)
			//				.ToString()
			//			}), "Value", "Text");

			//ViewBag.GetAnsType = new SelectList(enumData, "Value", "Text");
			//new[]
			//{
			//	new SelectListItem { Value = "1", Text = "DropDown" },
			//	new SelectListItem { Value = "2", Text = "CheckBox" },
			//	new SelectListItem { Value = "3", Text = "Radio" },
			//	new SelectListItem { Value = "4", Text = "TextBox" },
			//	new SelectListItem { Value = "5", Text = "Multiline" },
			//};
		}
	}
}
