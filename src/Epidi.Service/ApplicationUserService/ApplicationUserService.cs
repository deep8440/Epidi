using Epidi.Models.ExceptionHelper;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.User;
using Epidi.Repository.ApplicationUserRepository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Epidi.Notifications.Interface;
using Epidi.Models.Model;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Repository;
using Epidi.Repository.TermsConditionRepository;
using Epidi.Models.ViewModel.Users;

namespace Epidi.Service.ApplicationUserService
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ILogger _logger;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMessages _messages;
        private object _termsConditionRepository;

        public ApplicationUserService(ILoggerFactory loggerFactory,
            IApplicationUserRepository applicationUserRepository, IMessages messages)
        {
            _logger = loggerFactory.CreateLogger<ApplicationUserService>();
            _applicationUserRepository = applicationUserRepository;
            _messages = messages;
        }

        public async Task<ApplicationUserDetail> Create(RegisterUserViewModel viewModel, string CustomerId)
        {
            _logger.LogInformation("Inserting the Data");

            //if (await _applicationUserRepository.CheckEmailExist(viewModel.Email))
            //    throw new DuplicateFoundException("ApplicationUser", viewModel.Email,
            //       HttpStatusCode.Conflict.ToString(), General.Duplicate,General.DuplicateEmail);

            var retval = await _applicationUserRepository.Create(viewModel, CustomerId);

            var result = await _applicationUserRepository.GetById(retval, null, true, false);

            //send email of verification code
            string res = await _messages.SendEmailViaSendGrid(viewModel.Email, "OTP", result.VerificationCode);

            return result;
        }

        public async Task<ApplicationUserDetail> GetById(long? id,
            string Email,
            bool IsActive = false,
            bool IsEmailConfirm = false)
        {
            return await _applicationUserRepository.GetById(id, Email, IsActive, IsEmailConfirm);

        }

        public async Task<ApplicationUserDetail> ConfirmUser(string Email, string VerificationCode)
        {

            var response = _applicationUserRepository.ConfirmUser(Email, VerificationCode);

            //var response = _applicationUserRepository.GetById(null, Email, true, true);

            return await response;
        }
        public async Task<string> ResetPassword(int id,string PASSWORD,string OldPassword)
        {
            var response = _applicationUserRepository.ResetPassword(id, PASSWORD, OldPassword);
            return await response;
        }
        public async Task<string> ChangePassword(string email, string oldPassword, string newPassword, string confirmPassword)
        {
            var response = _applicationUserRepository.ChangePassword(email, oldPassword, newPassword, confirmPassword);
            return await response;
        }
        public async Task<string> ForgotPassword(string Email)
        {
            var password = _applicationUserRepository.ForgotPassword(Email);
            return await password;
        }
        public async Task<bool> Login(AppUser model)
        {
            var retval=await _applicationUserRepository.Login(model);
            return retval;
        }

        public async Task<RegisterUserDataViewModel> Update(RegisterUserDataViewModel viewModel)
        {
            if (await _applicationUserRepository.CheckContactExist(viewModel.Mobile))
                throw new DuplicateFoundException("Contact No", viewModel.Mobile,
                    HttpStatusCode.Conflict.ToString(), General.Duplicate);

            var retval = await _applicationUserRepository.Update(viewModel);

            return await _applicationUserRepository.GetRegisterUserData(viewModel.Id);
        }

        public async Task<string> GetUsersCustomerId(long Id)
        {
            return await _applicationUserRepository.GetUsersCustomerId(Id);
        }

        public async Task<string> UpdateTermsCondition(int userId)
        {
            var response = _applicationUserRepository.UpdateTermsCondition(userId);
            return await response;
        } 
        
        public async Task<string> UpdateTermsStepCondition(int userId)
        {
            var response = _applicationUserRepository.UpdateTermsStepCondition(userId);
            return await response;
        }

        public async Task<long> PersonalInformationCreate(PersonalInfomation viewModel)
        {
            var retval = await _applicationUserRepository.PersonalInformationCreate(viewModel);


            return retval;
        }
        public async Task<long> PersonalInformationUpdate(PersonalInfomation viewModel)
        {
            var retval = await _applicationUserRepository.PersonalInformationUpdated(viewModel);

            return retval;
        }


        public async Task<ChangePasswordByEmailIdViewModel> ChangePasswordByEmail(string Email, string Password)
        {
            var password = _applicationUserRepository.ChangePasswordByEmail(Email, Password);
            return await password;
        }

        // By Niti for GetUserProfile
        public async Task<UsersViewModel> GetUserProfile(int UserId)
        {
            return await _applicationUserRepository.GetUserProfile(UserId);
        }

        public async Task<Userpromocodemodel> GetUserPromocodeByUserId(int UserId)
        {
            return await _applicationUserRepository.GetUserPromocodeByUserId(UserId);
        }

        public async Task<bool> DuplicateUser(string Email)
        {

            bool response = await _applicationUserRepository.CheckEmailExist(Email);

            return response;
        }

    }
}
