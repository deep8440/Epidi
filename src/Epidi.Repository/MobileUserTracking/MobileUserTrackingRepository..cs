using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.DBConnection;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Dapper;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.MobileUserTracking;
using System.Data;

namespace Epidi.Repository.MobileUserTracking
{
    public class MobileUserTrackingRepository : RepositoryBase, IMobileUserTrackingRepository
    {
        public MobileUserTrackingRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        // <summary>
        /// Saves a record to the MobileUserTracking table.
        /// returns INSERTED values saved successfully from TABLE
        /// </summary>
        public async Task<MobileUserTrackingViewModel> Upsert(MobileUserTrackingViewModel mobileUserTrackingViewModel)
        {
            MobileUserTrackingViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", mobileUserTrackingViewModel.Id);
            vParams.Add("@UserId", mobileUserTrackingViewModel.UserId);
            vParams.Add("@ModuleName", mobileUserTrackingViewModel.ModuleName);
            vParams.Add("@ScreenName", mobileUserTrackingViewModel.ScreenName);
            vParams.Add("@TraceNumber", mobileUserTrackingViewModel.TraceNumber);
            vParams.Add("@CreatedDate", mobileUserTrackingViewModel.CreatedDate);

            var res = await _db.QueryMultipleAsync("[MobileUserTracking_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<MobileUserTrackingViewModel>().FirstOrDefault();
            return response;
        }
    }
}
