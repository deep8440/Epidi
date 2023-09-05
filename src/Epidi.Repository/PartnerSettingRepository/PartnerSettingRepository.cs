using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.PartnerSetting;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.PartnerSettingRepository
{
    public class PartnerSettingRepository : RepositoryBase, IPartnerSettingRepository
    {
        public PartnerSettingRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        public async Task<ResponseViewModel> Upsert(PartnerSettingViewModel aPartnerSetting)
        {
            try
            {
                ResponseViewModel response = new();

                var vParams = new DynamicParameters();
                vParams.Add("@Id", aPartnerSetting.Id);
                vParams.Add("@InstrumentId", aPartnerSetting.InstrumentId);
                vParams.Add("@UserId", aPartnerSetting.UserId);
                vParams.Add("@Markup_Per", aPartnerSetting.Markup_Per);
                vParams.Add("@Markup_Amt", aPartnerSetting.Markup_Amt);
                vParams.Add("@Buy_Per", aPartnerSetting.Buy_Per);
                vParams.Add("@Buy_Amt", aPartnerSetting.Buy_Amt);
                vParams.Add("@Sell_Per", aPartnerSetting.Sell_Per);
                vParams.Add("@Sell_Amt", aPartnerSetting.Sell_Amt);
                vParams.Add("@CreatedBy", aPartnerSetting.CreatedBy);
                vParams.Add("@UpdatedBy", aPartnerSetting.UpdatedBy);
                vParams.Add("@IsActive", aPartnerSetting.IsActive);
                var res = await _db.QueryMultipleAsync("[PartnerSettings_Upsert]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<ResponseViewModel>().FirstOrDefault();

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
