//using Epidi.Models.FrontEndViewModel;
using Epidi.Models.Model;
using Epidi.Models.ViewModel.IB;
using Epidi.Service.ApplicationUserService;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using SendGrid;
using SendGrid.Helpers.Mail;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace EPIDIWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IConfiguration _config;
        public AuthenticationController(IApplicationUserService applicationUserService,IConfiguration config)
        {
            _applicationUserService = applicationUserService;
            _config = config;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginView()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        public IActionResult ChangePassword(int id)
        {
            ViewBag.UserId = id;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(AppUser model)
        {
            var response = await _applicationUserService.ResetPassword(Convert.ToInt32(model.Id), model.Password,model.OldPassword);
            return Json(true);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(AppUser model)
        {
            if (ModelState.IsValid)
            {
                var response = await _applicationUserService.Login(model);
                if(response == true)
                {
                    if (string.IsNullOrEmpty(HttpContext.Session.GetString("email")))
                    {
                        HttpContext.Session.SetString("email", model.Email);
                    }
                }
                return Json(response);
            }
            return Json(false);
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            var password = await _applicationUserService.ForgotPassword(Email);
            try
            {
                var apikey = _config.GetValue<string>("SendGrid:APIKey");
                var client = new SendGridClient(apikey);

                var from_email = new EmailAddress("falgunmodi002@gmail.com", "Example User");
                var subject = "EPIDI Reset Password";
                var to_email = new EmailAddress("falgunmodi002@gmail.com", "Example User");
                var plainTextContent = "Below is link for your change password";
                var htmlContent = "Below is link for your change password<br/><br/><a href='https://localhost:44349/Authentication/ChangePassword/" + password+"'>Click here</a>";
                var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(password);
        }
    }
}
