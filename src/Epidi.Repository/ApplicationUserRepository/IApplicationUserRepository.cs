using Epidi.Models.Model;
using Epidi.Models.ViewModel.User;
using Epidi.Models.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.ApplicationUserRepository
{
    public interface IApplicationUserRepository
    {
        Task<long> Create(RegisterUserViewModel viewModel,string CustomerId);

        Task<long> Update(RegisterUserDataViewModel viewModel);

        Task<RegisterUserDataViewModel> GetRegisterUserData(long Id);

        Task<bool> CheckEmailExist(string Email);

        Task<bool> CheckContactExist(string Email);

        Task<ApplicationUserDetail> GetById(long? id, string Email, bool IsActive = false, bool IsEmailConfirm = false);

        Task<ApplicationUserDetail> AuthenticateUser(string Email, string Password, string SocialId, string SocialType);

        Task<ApplicationUserDetail> ConfirmUser(string Email, string VerificationCode);
        Task<string> GetUsersCustomerId(long Id);
        Task<bool> Login(AppUser model);
        Task<string> ForgotPassword(string Email);
        Task<string> ResetPassword(int id,string PASSWORD, string OLDPASSWORD);
        Task<string> ChangePassword(string email, string oldPassword, string newPassword, string confirmPassword);

        Task<string> UpdateTermsCondition(int userId);
        Task<string> UpdateTermsStepCondition(int userId);
       
        Task<long> PersonalInformationCreate(PersonalInfomation viewModel);
        Task<long> PersonalInformationUpdated(PersonalInfomation viewModel);

        Task<ChangePasswordByEmailIdViewModel> ChangePasswordByEmail(string Email, string Password);

        // By Niti for 
        Task<UsersViewModel> GetUserProfile(int UserId);
        Task<Userpromocodemodel> GetUserPromocodeByUserId(int UserId);

    }
}
