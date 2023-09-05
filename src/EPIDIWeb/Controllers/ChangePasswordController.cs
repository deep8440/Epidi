using Epidi.Models.Model;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.User;
using Epidi.Service.ApplicationUserService;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace EPIDIWeb.Controllers
{
    public class ChangePasswordController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        public ChangePasswordController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var email = HttpContext.Session.GetString("email");
            var response = await _applicationUserService.ChangePassword(email, model.OldPassword, CommonFunction.EncodePasswordToBase64(model.NewPassword), CommonFunction.EncodePasswordToBase64(model.ConfirmPassword));
            if(String.Equals(response, "Sucess"))
            {
                return Json(true);
            }
            else if(String.Equals(response, "InvalidOldPassword"))
            {
                return Json("InvalidOldPassword");
            }
            else if (String.Equals(response, "NewAndConfirmPasswordIncorrect"))
            {
                return Json("NewAndConfirmPasswordIncorrect");
            }
            else
            {
                return Json(false);
            }
            
            //return Json(false);
        }
    }
}
