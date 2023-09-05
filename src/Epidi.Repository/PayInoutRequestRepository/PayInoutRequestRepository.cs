using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Data;
using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Repository.ConnectionProvider;
using Epidi.Models.ViewModel.PayInoutRequest;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Users;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.IB;

namespace Epidi.Repository.PayInoutRequestRepository
{
    public class PayInoutRequestRepository :RepositoryBase , IPayInoutRequestRepository
    {
        public PayInoutRequestRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public async Task<PayInoutRequestViewModel> Upsert(PayInoutRequestViewModel model)
        {
            PayInoutRequestViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@UserId", model.UserId);
            vParams.Add("@Status", model.Status);
            vParams.Add("@ActionDate", model.ActionDate);
            vParams.Add("@Amount", model.Amount);
            vParams.Add("@RequestType", model.RequestType);
            vParams.Add("@CustomerNote", model.CustomerNote);
            vParams.Add("@AdminNote", model.AdminNote);            
            vParams.Add("@CreatedBy", model.CreatedBy);
            var res =await _db.QueryMultipleAsync("[PayInoutRequest_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<PayInoutRequestViewModel>().FirstOrDefault();
            
            return response;
        }
        public List<ResponseViewModel> Delete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[PayInoutRequest_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }
        public Tuple<List<PayInoutRequestViewModel>, long> GetAll(PageParam pageParam, string search,string userId,string fromDate,string toDate)
        {
            List<PayInoutRequestViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@FromDate", fromDate, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@ToDate", toDate, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@UserId", userId, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[PayInoutRequest_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<PayInoutRequestViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<PayInoutRequestViewModel>, long>(response, no);
            return tuple;
        }

		public List<ResponseViewModel> PayInOut_Transaction_Upsert(PayInoutRequestViewModel model)
		{
			List<ResponseViewModel> response = new List<ResponseViewModel>();

			var vParams = new DynamicParameters();
			vParams.Add("@PayInOutId", model.Id);
			vParams.Add("@UserId", model.UserId);
			vParams.Add("@Type", model.RequestType);
			vParams.Add("@CreatedBy", model.UserId);
			vParams.Add("@Comment", model.Comment);
			vParams.Add("@Amount", model.Amount);
			vParams.Add("@Status", model.Status);
			vParams.Add("@CurrentBalance", model.CurrentBalance);
			var res = _db.QueryMultiple("[PayInOut_Transaction_Upsert]", vParams, commandType: CommandType.StoredProcedure);

			response = res.Read<ResponseViewModel>().ToList();

			return response;
		}

		public List<UsersViewModel> PayInoutRequest_Users_GetAll()
		{
			List<UsersViewModel> response = new List<UsersViewModel>();

			var vParams = new DynamicParameters();
			vParams.Add("@Search", "");

			var res =  _db.QueryMultiple("[PayInoutRequest_Users_GetAll]", vParams, commandType: CommandType.StoredProcedure);

			response =  res.Read<UsersViewModel>().ToList();

			return response;
		}

        //for PayoutRequest start
        public async Task<List<UsersViewModel>> GetRequestTypeListALL()
        {
            var result = await _db.QueryAsync<UsersViewModel>(
               @"SELECT Id, Name FROM PayoutRequestType"
               );

            return result.ToList();
        }

        public async Task<List<UsersViewModel>> GetRequestStatusListALL()
        {
            var result = await _db.QueryAsync<UsersViewModel>(
               @"SELECT Id, Status FROM RequestStatus"
               );

            return result.ToList();
        }

        public async Task<UsersViewModel> Upsert(UsersViewModel model)
        {
            UsersViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@RequestBy", model.RequestBy);
            vParams.Add("@RequestType", model.RequestType);
            vParams.Add("@RequestStatusId", model.RequestStatusId);
            vParams.Add("@Amount", model.Amount);
            vParams.Add("@CustomerNote", model.CustomerNote);
            vParams.Add("@CurrentBalance", model.CurrentBalance);
            var res = await _db.QueryMultipleAsync("[PayoutRequest_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersViewModel>().FirstOrDefault();
            return response;
        }

        public async Task<UsersViewModel> GetById(int Id)
        {
            UsersViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[PayoutRequest_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersViewModel>().FirstOrDefault();

            return response;

        }

        public Tuple<List<PayInoutRequestViewModel>, long> GetAll_Payout(PageParam pageParam, string search, string userId, string fromDate, string toDate)
        {
            List<PayInoutRequestViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@FromDate", fromDate, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@ToDate", toDate, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@Id", userId, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[PayoutRequest_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<PayInoutRequestViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<PayInoutRequestViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<int> DeletePayoutById(int Id)
        {
            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[Delete_Payout_ById]", vParams, commandType: CommandType.StoredProcedure);

            return 1;
        }
    }
}
