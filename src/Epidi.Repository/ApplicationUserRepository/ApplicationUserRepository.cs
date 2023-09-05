using Dapper;
using Epidi.Enums;
using Epidi.Models.DBConnection;
using Epidi.Models.ExceptionHelper;
using Epidi.Models.Model;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Models.ViewModel.User;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.ConnectionProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Epidi.Repository.ApplicationUserRepository
{
    public class ApplicationUserRepository : RepositoryBase, IApplicationUserRepository
    {
        public ApplicationUserRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }

        public async Task<long> Create(RegisterUserViewModel viewModel, string CustomerId)
        {
            long retval = -1;
            long IbId = await _db.QuerySingleOrDefaultAsync<long>(
                         @"SELECT
                              Id
                              FROM
                              IB
                              WHERE PromoCode=@PromoCode and IsActive=1
                             ", new
                         {
                             viewModel.PromoCode
                         }).ConfigureAwait(false);
            

            retval = await _db.QuerySingleAsync<long>(
                    @"INSERT INTO Users 
                (
                    CountryId,
                    Email,
                    Password,
                    Role,
                    VerificationCode,
                    CustomerId,
                    IsActive,
                    CreatedDate,
                    IsEmailConfirm,
                    PromoCode,
                    IsDesclaimer,
                    SocialId,
                    SocialType,
                    IbId
                 )
                  OUTPUT inserted.Id
                  VALUES 
                 (
                    @CountryId,   
                    @Email, 
                    @Password,
                    @UserRole,
                    @VerificationCode,
                    @CustomerId,
                    @IsActive,
                    @CreatedDate,
                    @IsEmailConfirm,
                    @PromoCode,
                    @IsDesclaimer,
                    @SocialId,
                    @SocialType,
                    @IbId
                 );", new
                    {
                        viewModel.CountryId,
                        viewModel.Email,
                        @Password = CommonFunction.EncodePasswordToBase64(viewModel.Password),
                        @UserRole = viewModel.Role,
                        @VerificationCode = new Random().Next(1000, 9999).ToString(),
                        @CustomerId = CustomerId,
                        @IsActive = 1,
                        @CreatedDate = DateTime.Now,
                        @IsEmailConfirm = 0,
                        viewModel.PromoCode,
                        viewModel.IsDesclaimer,
                        viewModel.SocialId,
                        viewModel.SocialType,
                        IbId
                    }).ConfigureAwait(false);

            return retval;

        }

        public async Task<long> Update(RegisterUserDataViewModel viewModel)
        {
            long retval = -1;

            retval = await _db.ExecuteAsync(
                        @"UPDATE Users
                              SET
                                Surname = @Surname,
                                Name = @Name,
                                DateOfBirth = @DateOfBirth,
                                Mobile = @Mobile,
                                MobileCountryCode = @MobileCountryCode
                                WHERE
                                Id = @Id", new
                        {
                            viewModel.Surname,
                            viewModel.Name,
                            viewModel.DateOfBirth,
                            viewModel.Mobile,
                            viewModel.MobileCountryCode,
                            viewModel.Id
                        }).ConfigureAwait(false);

            return retval;
        }

        public async Task<RegisterUserDataViewModel> GetRegisterUserData(long Id)
        {
            var result = await _db.QuerySingleOrDefaultAsync<RegisterUserDataViewModel>(
                          @"SELECT
                              Id,
                              Surname,
                              Name,
                              DateOfBirth,
                              Mobile,
                              MobileCountryCode
                              FROM
                              Users
                              WHERE (@Id IS NULL OR Id = @Id)
                             ", new
                          {
                              Id
                          }).ConfigureAwait(false);
            return result;

        }

        public async Task<bool> CheckEmailExist(string Email)
        {
            var exists = await _db.ExecuteScalarAsync<bool>(
                      @"
                            SELECT 1 from 
                            Users
                            WHERE LOWER(Email) = LOWER(@Email) and IsActive = 1
                            ",
                      new
                      {
                          Email,
                      }).ConfigureAwait(false);

            return exists;
        }

        public async Task<bool> CheckContactExist(string Contact)
        {

            var exists = await _db.ExecuteScalarAsync<bool>(
                      @"
                            SELECT 1 from 
                            Users
                            WHERE LOWER(Mobile) = LOWER(@Mobile)
                            ",
                      new
                      {
                          @Mobile = Contact,
                      }).ConfigureAwait(false);

            return exists;

        }

        public async Task<ApplicationUserDetail> GetById(long? id, string Email, bool IsActive = false, bool IsEmailConfirm = false)
        {
            try
            {
                var result = await _db.QuerySingleOrDefaultAsync<ApplicationUserDetail>(
                                 @"SELECT
                              Id,
                              Email,
                              CountryId,
                              CustomerId,
                              VerificationCode,
                              PromoCode,
                              IsDesclaimer
                              FROM
                              Users
                              WHERE (@Id IS NULL OR Id = @id)
                              AND  (@Email IS NULL OR Email = @Email)
                              AND IsActive = @IsActive
                              AND IsEmailConfirm = @IsEmailConfirm
                             ", new
                                 {
                                     Id = id,
                                     Email,
                                     IsActive,
                                     IsEmailConfirm,
                                 }).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<ApplicationUserDetail> AuthenticateUser(string Email, string Password, string SocialId, string SocialType)
        {
            
            if (SocialId == null || SocialId == "" || SocialType == null || SocialType == "")
            {
                var result = await _db.QuerySingleOrDefaultAsync<ApplicationUserDetail>(
                              //@"SELECT Id,Email,CountryId,StepCompleted, IsTermsConditionAgreed
                              //   FROM Users
                              //   WHERE LOWER(Email) = LOWER(@Email) AND Password = @Password
                              //  "
                              @"SELECT Id,Email,CountryId,StepCompleted, IsTermsConditionAgreed,CASE 
                                WHEN usr.Password <> @Password THEN 2
                                    ELSE 0
                                END AS ErrorType
                            FROM (
                                SELECT Id,Email,CountryId,StepCompleted, IsTermsConditionAgreed, Password
                                FROM Users
                                WHERE LOWER(Email) = LOWER(@Email)
                            ) AS usr;"
                               , new
                               {
                                   Email,
                                   @Password = CommonFunction.EncodePasswordToBase64(Password),
                               }).ConfigureAwait(false);
                return result;
            }
            else
            {
                var result = await _db.QuerySingleOrDefaultAsync<ApplicationUserDetail>(
                               @"SELECT Id,Email,CountryId,StepCompleted, IsTermsConditionAgreed
                                  FROM Users
                                  WHERE  SocialId = @SocialId AND  SocialType = @SocialType
                                 "
                                , new
                                {
                                    SocialId,
                                    SocialType,
                                }).ConfigureAwait(false);
                return result;
            }


        }

        public async Task<ApplicationUserDetail> ConfirmUser(string Email, string VerificationCode)
        {

            ApplicationUserDetail response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Email", Email);
            vParams.Add("@VerificationCode", VerificationCode);

            var res = await _db.QueryMultipleAsync("[VerifyOtp]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ApplicationUserDetail>().FirstOrDefault();

            return response;

            //long retval = -1;
            //try
            //{
            //    var result = await _db.QuerySingleOrDefaultAsync<ApplicationUserDetail>(
            //                   @"SELECT
            //                  Id,
            //                  Email,
            //                  CountryId
            //                  FROM
            //                  Users
            //                  WHERE LOWER(Email) = LOWER(@Email) AND VerificationCode = @VerificationCode
            //                 ", new
            //                   {
            //                       Email,
            //                       VerificationCode,
            //                   }).ConfigureAwait(false);


            //    if (result != null)
            //    {
            //        retval = await _db.QuerySingleOrDefaultAsync<int>(
            //                 @"Update Users 
            //                   SET  
            //                   IsActive = 1
            //                  ,IsEmailConfirm = 1
            //                   WHERE Id = @Id;", new
            //                 {
            //                     @Id = result.Id
            //                 }).ConfigureAwait(false);
            //    }
            //}
            //catch(Exception ex)
            //{
            //    throw;
            //}
            //return retval;
        }

        public async Task<string> GetUsersCustomerId(long Id)
        {
            var result = await _db.QuerySingleOrDefaultAsync<string>(
                          @"SELECT
                              CustomerId
                              FROM
                              Users
                              WHERE (@Id IS NULL OR Id = @Id)
                             ", new
                          {
                              Id
                          }).ConfigureAwait(false);
            return result;

        }
        public async Task<bool> Login(AppUser model)
        {

            string password = CommonFunction.EncodePasswordToBase64(model.Password);
            var result = await _db.QuerySingleOrDefaultAsync<long>(@"SELECT COUNT(*) FROM USERS WHERE Email=@Email AND Password=@Password",
                new { model.Email, password }).ConfigureAwait(false);
            return result > 0 ? true : false;
        }
        public async Task<string> ForgotPassword(string Email)
        {
            var VerificationCode = await _db.QuerySingleOrDefaultAsync<string>(@"SELECT VerificationCode ID FROM USERS WHERE EMAIL=@EMAIL",
                new { Email }).ConfigureAwait(false);
            return VerificationCode;
        }
        public async Task<string> ResetPassword(int id, string PASSWORD, string OLDPASSWORD)
        {
            var result = await _db.QuerySingleOrDefaultAsync<long>(@"SELECT COUNT(*) FROM USERS WHERE Password=@OLDPASSWORD and Id=@id",
           new { OLDPASSWORD, id }).ConfigureAwait(false);
            if (result != null && result > 0)
            {
                var password = await _db.QuerySingleOrDefaultAsync<string>(@"UPDATE USERS SET PASSWORD=@PASSWORD WHERE ID=@ID",
                new { PASSWORD, id }).ConfigureAwait(false);
                return "Password updated Sucessfully";
            }
            else
            {
                throw new EntityNotFoundException("Old password", "", HttpStatusCode.NotFound.ToString(), General.Invalid);
            }
        }
        public async Task<string> ChangePassword(string email, string oldPassword, string newPassword, string confirmPassword)
        {
            var result = await _db.QuerySingleOrDefaultAsync<string>(@"SELECT Password FROM USERS WHERE Email=@email",
           new { email }).ConfigureAwait(false);
            var x = CommonFunction.DecodeFrom64(result);
            if (x != oldPassword)
            {
                return "InvalidOldPassword";
            }
            else if(newPassword != confirmPassword)
            {
                return "NewAndConfirmPasswordIncorrect";
            }
            else
            {
                var password = await _db.QuerySingleOrDefaultAsync<string>(@"UPDATE USERS SET Password=@newPassword WHERE Email=@email",
                new { newPassword, email }).ConfigureAwait(false);
                return "Sucess";
            }
        }

        public async Task<string> UpdateTermsCondition(int userId)
        {
            var terms = await _db.QuerySingleOrDefaultAsync<string>(@"UPDATE USERS SET IsTermsConditionAgreed=1 WHERE ID=@userId",
                new { userId }).ConfigureAwait(false);

            return terms;
        }

        public async Task<string> UpdateTermsStepCondition(int userId)
        {
            var terms = await _db.QuerySingleOrDefaultAsync<string>(@"UPDATE USERS SET IsTermsConditionStepAgreed=1 WHERE ID=@userId",
                new { userId }).ConfigureAwait(false);

            return terms;
        }






        //public async Task<ApplicationUser> GetUserAsync(LoginViewModel login)
        //{
        //    var result = await _db.QuerySingleOrDefaultAsync<ApplicationUser>(
        //               @"SELECT
        //                      Id as Id,
        //                      Email,
        //                      FROM
        //                      ApplicationUser
        //                      WHERE Email = @Email
        //                      and Password = @Password
        //                      and IsActive = true and IsEmailConfirmed = true", new
        //               {
        //                   @Email = login.Username,
        //                   @Password = login.Password
        //               }).ConfigureAwait(false);
        //    return result;
        //}
        public async Task<long> PersonalInformationCreate(PersonalInfomation viewModel)
        {
            try
            {
                viewModel.IsPersonalInfoCompleted = true;
                long retval = -1;
                retval = await _db.QuerySingleAsync<long>(
                        @"INSERT INTO Users 
                (
                    Name,
                    SurName,
                    DateOfBirth,
                    Mobile,
                    Address,
                    Address2,
                    PostCode,
                    MobileCountryCode,
                    NationlityId,
                    TaxResidenceId,
                    ResidentArea,
                    IDNumber,
                    PEP,
                    IsPersonalInfoCompleted,
                    TINNumber
                 )
                  OUTPUT inserted.Id
                  VALUES 
                 (
                    @Name,   
                    @SurName, 
                    @DateOfBirth,
                    @Mobile,
                    @Address,
                    @Address2,
                    @PostCode,
                    @MobileCountryCode,
                    @NationlityId,
                    @TaxResidenceId,
                    @ResidentArea,
                    @IDNumber,
                    @PEP,
                    @IsPersonalInfoCompleted,
                    @TINNumber
                 );", new
                        {
                            viewModel.Name,
                            viewModel.Surname,
                            viewModel.DateOfBirth,
                            viewModel.Mobile,
                            viewModel.Address,
                            viewModel.Address2,
                            viewModel.PostCode,
                            viewModel.MobileCountryCode,
                            viewModel.NationlityId,
                            viewModel.TaxResidenceId,
                            viewModel.ResidentArea,
                            viewModel.IDNumber,
                            viewModel.PEP,
                            viewModel.IsPersonalInfoCompleted,
                            viewModel.TINNumber
                        }).ConfigureAwait(false);

                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> PersonalInformationUpdated(PersonalInfomation viewModel)
        {
            viewModel.IsPersonalInfoCompleted = true;
            long retval = -1;
            retval = await _db.ExecuteAsync(
                        @"UPDATE Users
                              SET
                                Name = @Name,
                                Surname = @Surname,
                                DateOfBirth = @DateOfBirth,
                                Mobile = @Mobile,
                                Address = @Address,
                                Address2 = @Address2,
                                PostCode = @PostCode,
                                MobileCountryCode = @MobileCountryCode,
                                NationlityId = @NationlityId,
                                TaxResidenceId = @TaxResidenceId,
                                ResidentArea = @ResidentArea,
                                IDNumber = @IDNumber,
                                PEP = @PEP,
                                IsPersonalInfoCompleted = @IsPersonalInfoCompleted,
                                TINNumber = @TINNumber
                                WHERE
                                Id = @Id", new
                        {
                            viewModel.Name,
                            viewModel.Surname,
                            viewModel.DateOfBirth,
                            viewModel.Mobile,
                            viewModel.Address,
                            viewModel.Address2,
                            viewModel.PostCode,
                            viewModel.MobileCountryCode,
                            viewModel.NationlityId,
                            viewModel.TaxResidenceId,
                            viewModel.ResidentArea,
                            viewModel.IDNumber,
                            viewModel.PEP,
                            viewModel.IsPersonalInfoCompleted,
                            viewModel.TINNumber,
                            viewModel.Id
                        }).ConfigureAwait(false);

            return retval;
        }




        public async Task<ChangePasswordByEmailIdViewModel> ChangePasswordByEmail(string Email, string Password)
        {
            ChangePasswordByEmailIdViewModel response = new ChangePasswordByEmailIdViewModel();
            try
            {
                //                var result = await _db.QuerySingleOrDefaultAsync<ChangePasswordByEmailIdViewModel>(
                //                              //@"SELECT Id,Email,CountryId,StepCompleted, IsTermsConditionAgreed
                //                              //   FROM Users
                //                              //   WHERE LOWER(Email) = LOWER(@Email) AND Password = @Password
                //                              //  "
                //                              @"IF exists(select * from Users where Email = @Email)
                //BEGIN
                //   Update Users SET Password = @Password where Email = @Email
                //END;"
                //                               , new
                //                               {
                //                                   @Email = Email,
                //                                   @Password = CommonFunction.EncodePasswordToBase64(Password),
                //                               }).ConfigureAwait(false);
                //                return result;
                var vParams = new DynamicParameters();
                vParams.Add("@Email",Email);
                vParams.Add("@Password", CommonFunction.EncodePasswordToBase64(Password));

                var VerificationCode = await _db.QueryMultipleAsync("[ChangePassword_ByEmailId]", vParams, commandType: CommandType.StoredProcedure);
                response = VerificationCode.Read<ChangePasswordByEmailIdViewModel>().FirstOrDefault();
                return response;
            }
            catch (Exception ex)
            {
                return response;
            }
            
        }


        // By Niti 
        public async Task<UsersViewModel> GetUserProfile(int UserId)
        {
            UsersViewModel response = new UsersViewModel();

            try
            {
                var vParams = new DynamicParameters();
                vParams.Add("@UserId", UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                var res = await _db.QueryMultipleAsync("[UserProfile_GetById]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<UsersViewModel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                return response;
            }
            return response;
        }

        public async Task<Userpromocodemodel> GetUserPromocodeByUserId(int userId)
        {
            Userpromocodemodel response = new Userpromocodemodel();

            try
            {
                var vParams = new DynamicParameters();
                vParams.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                var res = await _db.QueryMultipleAsync("[UserPromocode_GetByUserId]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<Userpromocodemodel>().FirstOrDefault();
            }
            catch (Exception ex)
            {
                return response;
            }
            return response;
        }
    }
}
