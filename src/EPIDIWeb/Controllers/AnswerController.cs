using Epidi.Models.ViewModel.Question;
using Epidi.Service.AnswerService;
using Microsoft.AspNetCore.Mvc;

namespace EPIDIWeb.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerService _answerService;
        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;

        }
        public async Task<IActionResult> AnsDetails(int questionId,string questionName)
        {

            ViewBag.Title = "Answer List";
            ViewBag.Method = "Answer";
            ViewBag.DetailName = questionName;
            var resp = await _answerService.GetAnswerByQuestionId(questionId);

            ViewBag.QuestionId = questionId;

            return View(resp);
        }

        public IActionResult AddAnswer(int questionId)
        {
            AnswerViewModel answerViewModel = new AnswerViewModel();
            answerViewModel.QuestionId = questionId;
            return View(answerViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SaveAnswer(AnswerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var res = _answerService.SaveAnswer(model);
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

        [HttpGet]
        public async Task<IActionResult> Edit(long answerId)
        {
            var questions = await _answerService.GetAnswerByAnswerId(answerId);
            ;
            if (questions != null)
            {
                return View("AddAnswer", questions);
            }
            else
            {
                return View(new AnswerViewModel());
            }
        }

        [HttpGet]
        public IActionResult DeleteAnswer(int answerId)
        {
            var res = _answerService.DeleteAnswer(answerId);
            return Json(res);
        }

        [HttpPost]
        public IActionResult DeleteAnswerPOST(int AnswerId)
        {
            var res = _answerService.DeleteAnswer(AnswerId);
            return Json(res);
        }

    }
}
