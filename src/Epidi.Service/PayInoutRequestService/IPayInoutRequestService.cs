using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.PayInoutRequest;
using Epidi.Models.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PayInoutRequestService
{
    public interface IPayInoutRequestService
    {
        Task<PayInoutRequestViewModel> Upsert(PayInoutRequestViewModel model);
        List<ResponseViewModel> Delete(int Id);
        Tuple<List<PayInoutRequestViewModel>, long> GetAll(PageParam pageParam, string Search, string userId, string fromDate, string toDate);

        List<ResponseViewModel> PayInOut_Transaction_Upsert(PayInoutRequestViewModel model);
        List<UsersViewModel> PayInoutRequest_Users_GetAll();

        //for PayoutRequest start
        Task<UsersViewModel> Upsert(UsersViewModel model);
        Task<List<UsersViewModel>> GetRequestTypeListALL();
        Task<List<UsersViewModel>> GetRequestStatusListALL();
        Task<UsersViewModel> GetById(int Id);
        Tuple<List<PayInoutRequestViewModel>, long> GetAll_Payout(PageParam pageParam, string Search, string userId, string fromDate, string toDate);
        Task<int> DeletePayoutById(int Id);
    }
}
