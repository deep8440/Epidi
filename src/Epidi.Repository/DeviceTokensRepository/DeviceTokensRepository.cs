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
using Epidi.Models.ViewModel.UsersDocuments;
using Epidi.Models.ViewModel.DeviceTokens;
using System.Data;

namespace Epidi.Repository.DeviceTokensRepository
{
    public class DeviceTokensRepository : RepositoryBase, IDeviceTokensRepository
    {
        public DeviceTokensRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public async Task<DeviceTokenViewModel> Delete(int Id)
        {
            DeviceTokenViewModel response = new DeviceTokenViewModel();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);

            var res = await _db.QueryMultipleAsync("[DeviceToken_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<DeviceTokenViewModel>().FirstOrDefault();
            return response;
        }
        public async Task<DeviceTokenViewModel> Upsert(DeviceTokenViewModel model)
        {
            DeviceTokenViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@UserId", model.UserId);
            vParams.Add("@Token", model.Token);
            vParams.Add("@IMEI", model.IMEI);
            vParams.Add("@IsActive", model.IsActive);
            vParams.Add("@OS", model.OS);
            vParams.Add("@CreatedAt", model.CreatedAt);

            var res = await _db.QueryMultipleAsync("[DeviceToken_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<DeviceTokenViewModel>().FirstOrDefault();
            return response;
        }
    }
}
