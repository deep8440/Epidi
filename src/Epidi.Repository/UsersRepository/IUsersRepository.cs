using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.UsersRepository
{
    public interface IUsersRepository
    {
        Task<UsersViewModel> Upsert(UsersViewModel model);
        Task<List<UserFieldsList>> GetUserFields(string tblName);
        Tuple<List<UsersViewModel>, long> GetUsers_ByQuery(PageParam pageParam, string LeverageId);
        Task<UsersVsRulesViewModel> UpsertUsersVsRoles(UsersVsRulesViewModel model);
        Tuple<List<UsersViewModel>, long> GetUsersVsRole_ByRulsId(PageParam pageParam, string RulesId);
        Task<UsersViewModel> DeleteUserVsRules(int Id);
        Tuple<List<UsersViewModel>, long> Users_GetAll(PageParam pageParam, string search);
        Task<List<UsersFavourite>> UsersFavourite_GetByMasterInstrumentIdLPId(int MasterInstrumentId, int LPId);
        Task<UsersFavouriteInsert> UsersFavourite_Insert(UsersFavouriteInsert model);
        Task<PayoutRequest> PayoutRequests_Insert(PayoutRequest model);
        Task<List<UsersFavourite>> UsersFavourite_GetByUserId(int UserId);
        Task<List<PayoutRequestData>> PayoutRequest_GetByUserId(int UserId);
        Task<UsersViewModel> UserStepCompleted(int UserId);

		Task<UsersFavouriteInsert> UsersFavourite_Remove(int MasterInstrumentId, int UserId);

        Task<ResponseViewModel> DeleteUser(int Id);

        Task<Tuple<List<PaymentRequestList>, long>> GetPaymentRequestListByUserId(PageParam pageParam, string search, int Id);
        Task<PayoutRequest> DepositMoneyInsert(PayoutRequest model);
		Task<List<QuestionAnswerList>> GetQuestionAnswerListByUserId(int UserId);
        Task<ResponseViewModel> UpdateAnswer(QuestionAnswerList model);

    }
}
