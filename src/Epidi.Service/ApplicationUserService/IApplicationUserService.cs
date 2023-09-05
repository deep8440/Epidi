using Dapper;
using Epidi.Models.Model;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Models.ViewModel.User;
using Epidi.Models.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.ApplicationUserService
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserDetail> Create(RegisterUserViewModel viewModel, string CustomerId);

        Task<RegisterUserDataViewModel> Update(RegisterUserDataViewModel viewModel);

        Task<ApplicationUserDetail> GetById(long? id, string Email, 
            bool IsActive = false, bool IsEmailConfirm = false);

        Task<ApplicationUserDetail> ConfirmUser(string Email, string VerificationCode);
        Task<string> GetUsersCustomerId(long Id);
        Task<bool> Login(AppUser model);
        Task<string> ForgotPassword(string Email);
        Task<string> ResetPassword(int id,string PASSWORD, string OLDPASSWORD);
        Task<string> ChangePassword(string email, string oldPassword, string newPassword, string confirmPassword);

        Task<string> UpdateTermsCondition(int userId);
        Task<string> UpdateTermsStepCondition(int userId);
        Task<long> PersonalInformationCreate(PersonalInfomation viewModel);
        Task<long> PersonalInformationUpdate(PersonalInfomation viewModel);

        Task<ChangePasswordByEmailIdViewModel> ChangePasswordByEmail(string Email, string Password);

        // By Niti for Get UserProfile
        Task<UsersViewModel> GetUserProfile(int UserId);
        Task<Userpromocodemodel> GetUserPromocodeByUserId(int UserId);
        Task<bool> DuplicateUser(string Email);











    }
}
