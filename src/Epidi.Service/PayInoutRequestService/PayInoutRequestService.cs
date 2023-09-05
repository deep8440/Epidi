using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IB;
using Epidi.Models.ViewModel.PayInoutRequest;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.PayInoutRequestRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PayInoutRequestService
{
    public class PayInoutRequestService : IPayInoutRequestService
    {
        private readonly IPayInoutRequestRepository _payInoutRequestRepository;

        public PayInoutRequestService(IPayInoutRequestRepository payInoutRequestRepository)
        {
            _payInoutRequestRepository = payInoutRequestRepository;
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            return _payInoutRequestRepository.Delete(Id);
        }

        public async Task<PayInoutRequestViewModel> Upsert(PayInoutRequestViewModel model)
        {
            return await _payInoutRequestRepository.Upsert(model);
        }
        public Tuple<List<PayInoutRequestViewModel>, long> GetAll(PageParam pageParam, string Search, string userId, string fromDate, string toDate)
        {
            return _payInoutRequestRepository.GetAll(pageParam, Search,userId,fromDate,toDate);
        }

        public List<ResponseViewModel> PayInOut_Transaction_Upsert(PayInoutRequestViewModel model)
        {
			return _payInoutRequestRepository.PayInOut_Transaction_Upsert(model);
		}

		public List<UsersViewModel> PayInoutRequest_Users_GetAll()
        {
			return _payInoutRequestRepository.PayInoutRequest_Users_GetAll();
		}

        //for PayoutRequest start
        public async Task<List<UsersViewModel>> GetRequestTypeListALL()
        {
            return await _payInoutRequestRepository.GetRequestTypeListALL();
        }

        public async Task<List<UsersViewModel>> GetRequestStatusListALL()
        {
            return await _payInoutRequestRepository.GetRequestStatusListALL();
        }

        public async Task<UsersViewModel> Upsert(UsersViewModel model)
        {
            return await _payInoutRequestRepository.Upsert(model);
        }

        public async Task<UsersViewModel> GetById(int Id)
        {
            return await _payInoutRequestRepository.GetById(Id);
        }

        public Tuple<List<PayInoutRequestViewModel>, long> GetAll_Payout(PageParam pageParam, string Search, string userId, string fromDate, string toDate)
        {
            return _payInoutRequestRepository.GetAll_Payout(pageParam, Search, userId, fromDate, toDate);
        }
        public async Task<int> DeletePayoutById(int Id)
        {
            return await _payInoutRequestRepository.DeletePayoutById(Id);
        }

    }
}
