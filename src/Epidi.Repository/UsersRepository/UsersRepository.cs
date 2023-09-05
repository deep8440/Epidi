using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.DBConnection;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Dapper;
using Epidi.Models.ViewModel.Users;
using System.Data;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.UsersTransactions;
using Epidi.Models.ViewModel.Common;

namespace Epidi.Repository.UsersRepository
{
    public class UsersRepository : RepositoryBase, IUsersRepository
    {
        public UsersRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        // <summary>
        /// Saves a record to the Users table.
        /// returns INSERTED values saved successfully from TABLE
        /// </summary>
        public async Task<UsersViewModel> Upsert(UsersViewModel model)
        {
            UsersViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@Name", model.Name);
            vParams.Add("@Surname", model.Surname);
            vParams.Add("@Address", model.Address);
            vParams.Add("@PostCode", model.PostCode);
            vParams.Add("@CountryId", model.CountryId);
            vParams.Add("@Email", model.Email);
            vParams.Add("@Mobile", model.Mobile);
            vParams.Add("@CurrentBalance", model.CurrentBalance);
            vParams.Add("@Leverage", model.Leverage);
            vParams.Add("@IsActive", model.IsActive);
            vParams.Add("@CreatedAt", model.CreatedAt);
            vParams.Add("@UpdatedAt", model.UpdatedAt);
            vParams.Add("@OnboardingEntity", model.OnboardingEntity);
            vParams.Add("@LegalEntityName", model.LegalEntityName);
            vParams.Add("@IBId", model.IBId);
            vParams.Add("@BDMId", model.BDMId);
            vParams.Add("@NationlityId", model.NationlityId);
            vParams.Add("@TaxResidenceId", model.TaxResidenceId);
            vParams.Add("@ResidentArea", model.ResidentArea);
            vParams.Add("@IDNumber", model.IDNumber);
            vParams.Add("@PEP", model.PEP);
            vParams.Add("@TINNumber", model.TINNumber);
            vParams.Add("@Address2", model.Address2);
            vParams.Add("@DateOfBirth", model.DateOfBirth);
            vParams.Add("@CountryCode", model.CountryCode);
            vParams.Add("@CustomerId", model.CustomerId);
            vParams.Add("@MobileCountryCode", model.MobileCountryCode);

            var res = await _db.QueryMultipleAsync("[Users_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersViewModel>().FirstOrDefault();

            return response;
        }
        public async Task<List<UserFieldsList>> GetUserFields(string tblName)
        {
            List<UserFieldsList> response = new List<UserFieldsList>();

            var vParams = new DynamicParameters();
            //vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            //vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            //vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@TableName", tblName);
            var res = await _db.QueryMultipleAsync("[GetFieldsNameByTable]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UserFieldsList>().ToList();

            return response;
        }

        public Tuple<List<UsersViewModel>, long> GetUsers_ByQuery(PageParam pageParam, string LeverageId)
        {
            List<UsersViewModel> response = new();

            var vParams = new DynamicParameters();            
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);            
            vParams.Add("@LeverageId", LeverageId);
            var res = _db.QueryMultiple("[GetUsers_ByLeverageRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<UsersViewModel>());
            long no = res.Read<long>().SingleOrDefault();

            var tuple = new Tuple<List<UsersViewModel>,long>(response, no);

            return tuple;
        }
        public async Task<UsersVsRulesViewModel> UpsertUsersVsRoles(UsersVsRulesViewModel model)
        {
            UsersVsRulesViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@RuleId", model.RuleId);
            vParams.Add("@UserId", model.UserId);

            var res = await _db.QueryMultipleAsync("[UsersVsRules_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersVsRulesViewModel>().FirstOrDefault();

            return response;
        }

        public Tuple<List<UsersViewModel>, long> GetUsersVsRole_ByRulsId(PageParam pageParam, string RulesId)
        {
            List<UsersViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@RulesId", RulesId);
            var res = _db.QueryMultiple("[GetUsersVsRole_ByLeverageRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<UsersViewModel>());
            long no = res.Read<long>().SingleOrDefault();

            var tuple = new Tuple<List<UsersViewModel>, long>(response, no);

            return tuple;
        }

        public async Task<UsersViewModel> DeleteUserVsRules(int Id)
        {
            UsersViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);

            var res = await _db.QueryMultipleAsync("[UsersVsRules_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersViewModel>().FirstOrDefault();

            return response;
        }

		public Tuple<List<UsersViewModel>, long> Users_GetAll(PageParam pageParam, string search)
		{
			List<UsersViewModel> response = new List<UsersViewModel>();

			var vParams = new DynamicParameters();
			vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
			vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
			vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

			var res = _db.QueryMultiple("[Users_GetAll]", vParams, commandType: CommandType.StoredProcedure);

			response.AddRange(res.Read<UsersViewModel>());
			long no = res.Read<long>().SingleOrDefault();
			var tuple = new Tuple<List<UsersViewModel>, long>(response, no);
			return tuple;
		}
        public async Task<List<UsersFavourite>> UsersFavourite_GetByMasterInstrumentIdLPId(int MasterInstrumentId, int LPId)
        {
            List<UsersFavourite> response = new List<UsersFavourite>();

            var vParams = new DynamicParameters();
            vParams.Add("@MasterInstrumentId", MasterInstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@LPId", LPId, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[UsersFavourite_GetByMasterInstrumentIdLPId]", vParams, commandType: CommandType.StoredProcedure);

            response= res.Read<UsersFavourite>().ToList();
            return response;
        }
        public async Task<List<UsersFavourite>> UsersFavourite_GetByUserId(int UserId)
        {
            List<UsersFavourite> response = new List<UsersFavourite>();

            var vParams = new DynamicParameters();
            vParams.Add("@UserId", UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[UsersFavourite_GetByUserId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersFavourite>().ToList();
            return response;
        }
        
        public async Task<List<PayoutRequestData>> PayoutRequest_GetByUserId(int UserId)
        {
            List<PayoutRequestData> response = new List<PayoutRequestData>();

            var vParams = new DynamicParameters();
            vParams.Add("@UserId", UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[PayoutRequest_GetByUserId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<PayoutRequestData>().ToList();
            return response;
        }

        public async Task<UsersFavouriteInsert> UsersFavourite_Insert(UsersFavouriteInsert model)
        {
            UsersFavouriteInsert response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@MasterInstrumentId", model.MasterInstrumentId);
            vParams.Add("@UserId", model.UserId);
            vParams.Add("@Day", model.Day);
            vParams.Add("@Hours", model.Hours);

            var res = await _db.QueryMultipleAsync("[UsersFavourite_Insert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersFavouriteInsert>().FirstOrDefault();

            return model;
        }
        
        public async Task<PayoutRequest> PayoutRequests_Insert(PayoutRequest model)
        {
            PayoutRequest response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Amount", model.Amount);
            vParams.Add("@RequestStatusId", model.RequestStatusId);
            vParams.Add("@RequestBy", model.RequestBy);
            vParams.Add("@RequestType", model.RequestType);
            vParams.Add("@CustomerNote", model.CustomerNote);
            vParams.Add("@AdminNote", model.AdminNote);
            vParams.Add("@CreatedDate", model.CreatedDate);
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@DeletedDate", model.DeletedDate);
            vParams.Add("@IsDelete", model.IsDelete);
            
            var res = await _db.QueryMultipleAsync("[Payout_Insert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<PayoutRequest>().FirstOrDefault();

            return response;
        }

        public async Task<ResponseViewModel> UpdateAnswer(QuestionAnswerList model)
        {
            ResponseViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@AnswerDesc", model.AnswerDesc); 
            vParams.Add("@AnswerId", model.AnswerId); 

            var res = await _db.QueryMultipleAsync("[Update_Answer]", vParams, commandType: CommandType.StoredProcedure);

            var result = res.Read<int>().FirstOrDefault();
            response.Id = result;
            return response;
        }
        public async Task<UsersViewModel> UserStepCompleted(int UserId)
        {
			UsersViewModel response = new();
			var vParams = new DynamicParameters();
			vParams.Add("@UserId", UserId);
			vParams.Add("@StepCompleted", true);

			var res = await _db.QueryMultipleAsync("[Users_StepCompleted]", vParams, commandType: CommandType.StoredProcedure);

			response = res.Read<UsersViewModel>().FirstOrDefault();

			return response;
		}

		public async Task<UsersFavouriteInsert> UsersFavourite_Remove(int MasterInstrumentId,int UserId)
        {
            UsersFavouriteInsert response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@MasterInstrumentId", MasterInstrumentId);
            vParams.Add("@UserId", UserId);

            var res = await _db.QueryMultipleAsync("[UsersFavourite_Remove]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersFavouriteInsert>().FirstOrDefault();

            return response;
        }

        public async Task<ResponseViewModel> DeleteUser(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var param = new DynamicParameters();
            param.Add("@UserId", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[RemoveUserRegistration]", param, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().SingleOrDefault();
            return response;
        }

        public async Task<Tuple<List<PaymentRequestList>, long>> GetPaymentRequestListByUserId(PageParam pageParam,string search, int Id)
        {
            List<PaymentRequestList> response = new List<PaymentRequestList>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = await _db.QueryMultipleAsync("[PayoutRequestList_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<PaymentRequestList>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<PaymentRequestList>, long>(response, no);
            return tuple;
        }

        public async Task<List<QuestionAnswerList>> GetQuestionAnswerListByUserId(int UserId)
        {
            List<QuestionAnswerList> response = new List<QuestionAnswerList>();

            var vParams = new DynamicParameters();
            vParams.Add("@UserId", UserId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[GetUserQuestionAnswer_ById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<QuestionAnswerList>().ToList();
            return response;
        }


        public async Task<PayoutRequest> DepositMoneyInsert(PayoutRequest model)
		{
			PayoutRequest response = new();

			var vParams = new DynamicParameters();
			vParams.Add("@Amount", model.Amount);
			vParams.Add("@RequestStatusId", model.RequestStatusId);
			vParams.Add("@RequestBy", model.RequestBy);
			vParams.Add("@RequestType", model.RequestType);
			vParams.Add("@CustomerNote", model.CustomerNote);
			vParams.Add("@AdminNote", model.AdminNote);
			vParams.Add("@CreatedDate", model.CreatedDate);
			vParams.Add("@CreatedBy", model.CreatedBy);
			vParams.Add("@DeletedDate", model.DeletedDate);
			vParams.Add("@IsDelete", model.IsDelete);

			var res = await _db.QueryMultipleAsync("[PayoutDeposite_Insert]", vParams, commandType: CommandType.StoredProcedure);

			response = res.Read<PayoutRequest>().FirstOrDefault();

			return response;
		}
	}
}
