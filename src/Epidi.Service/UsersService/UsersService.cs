using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.IBRepository;
using Epidi.Repository.UsersRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<UsersViewModel> Upsert(UsersViewModel model)
        {
            return await _usersRepository.Upsert(model);
        }
        public async Task<List<UserFieldsList>> GetUserFields(string tblName)
        {
            return await _usersRepository.GetUserFields(tblName);
        }
        public Tuple<List<UsersViewModel>,long> GetUsers_ByQuery(PageParam pageParam, string LeverageId)
        {
            return _usersRepository.GetUsers_ByQuery(pageParam, LeverageId);
        }
        public async Task<UsersVsRulesViewModel> UpsertUsersVsRoles(UsersVsRulesViewModel model)
        {
            return await _usersRepository.UpsertUsersVsRoles(model);
        }

        public Tuple<List<UsersViewModel>, long> GetUsersVsRole_ByRulsId(PageParam pageParam, string RulesId)
        {
            return _usersRepository.GetUsersVsRole_ByRulsId(pageParam, RulesId);
        }

        public Task<UsersViewModel> DeleteUserVsRules(int Id)
        {
            return _usersRepository.DeleteUserVsRules(Id);
        }

        public Tuple<List<UsersViewModel>, long> Users_GetAll(PageParam pageParam, string search)
        {
            return _usersRepository.Users_GetAll(pageParam, search);
        }

        public async Task<List<UsersFavourite>> UsersFavourite_GetByMasterInstrumentIdLPId(int MasterInstrumentId, int LPId)
        {
            return await _usersRepository.UsersFavourite_GetByMasterInstrumentIdLPId(MasterInstrumentId, LPId);
        }

        public async Task<UsersFavouriteInsert> UsersFavourite_Insert(UsersFavouriteInsert model)
        {
            return await _usersRepository.UsersFavourite_Insert(model);
        }
        public async Task<PayoutRequest> PayoutRequests_Insert(PayoutRequest model)
        {
            return await _usersRepository.PayoutRequests_Insert(model);
        }
        public async Task<UsersFavouriteInsert> UsersFavourite_Remove(int MasterInstrumentId, int UserId)
        {
            return await _usersRepository.UsersFavourite_Remove(MasterInstrumentId, UserId);
        }
        public async Task<List<UsersFavourite>> UsersFavourite_GetByUserId(int UserId)
        {
            return await _usersRepository.UsersFavourite_GetByUserId(UserId);
        }
        public async Task<List<PayoutRequestData>> PayoutRequest_GetByUserId(int UserId)
        {
            return await _usersRepository.PayoutRequest_GetByUserId(UserId);
        }
        public async Task<UsersViewModel> UserStepCompleted(int UserId)
		{
            return await _usersRepository.UserStepCompleted(UserId); 
        }

        //public Tuple<List<IBCommisionUsers>,long> IBCommisionUsers(string search)
        //{
        //    return _usersRepository.IBCommisionUsers(search);
        //}
        //public Tuple<List<IBCommisionUserChilds>,long> IBCommissionChilds(int id)
        //{
        //    return _usersRepository.IBCommissionChilds(id);
        //}

        public async Task<ResponseViewModel> DeleteUser(int Id)
        {
            return await _usersRepository.DeleteUser(Id);
        }

        public async Task<Tuple<List<PaymentRequestList>, long>> GetPaymentRequestListByUserId(PageParam pageParam, string search, int Id)
        {
            return await _usersRepository.GetPaymentRequestListByUserId(pageParam, search, Id);
        }
        public async Task<PayoutRequest> DepositMoneyInsert(PayoutRequest model)
		{
			return await _usersRepository.PayoutRequests_Insert(model);
		}

		public async Task<List<QuestionAnswerList>> GetQuestionAnswerListByUserId(int UserId)
		{
			return await _usersRepository.GetQuestionAnswerListByUserId(UserId);
		}

        public async Task<ResponseViewModel> UpdateAnswer(QuestionAnswerList model)
        {
            return await _usersRepository.UpdateAnswer(model);
        }

    }
}
