using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.BDM;
using Epidi.Models.ViewModel.BDMCommisionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.BDM
{
    public class BDMRepository : RepositoryBase, IBDMRepository
    {
        public BDMRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public Tuple<List<BDMViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<BDMViewModel> response = new List<BDMViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[BDM_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<BDMViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<BDMViewModel>, long>(response, no);
            return tuple;
        }
        public async Task<List<InstrumentMasterViewModel>> Get_All_Instrument()
        {
            List<InstrumentMasterViewModel> response = new List<InstrumentMasterViewModel>();

            var res = await _db.QueryMultipleAsync("[Instrument_GetAll]", commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<InstrumentMasterViewModel>());
            return response.ToList();
        }
        public async Task<BDMViewModel> Upsert(BDMViewModel model)
        {
            BDMViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@Name", model.Name);

            var res = await _db.QueryMultipleAsync("[BDM_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<BDMViewModel>().FirstOrDefault();
            if (response != null)
            {
                BDMCommisionMarkUpSettingInstrumentWiseViewModel model_commission = new BDMCommisionMarkUpSettingInstrumentWiseViewModel();
                model_commission = model.BDMCommision;
                model_commission.BDMID = response.Id;
                await SaveBDMCommision(model_commission);
            }

            return response;
        }
        public async Task<BDMCommisionMarkUpSettingInstrumentWiseViewModel> SaveBDMCommision(BDMCommisionMarkUpSettingInstrumentWiseViewModel model)
        {
            BDMCommisionMarkUpSettingInstrumentWiseViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@MasterInstrumentalId", model.MasterInstrumentalId);
            vParams.Add("@BDMID", model.BDMID);
            vParams.Add("@MarkUpPer1000", model.MarkUpPer1000);
            vParams.Add("@MarkUpPer", model.MarkUpPer);
            vParams.Add("@BuySwapPer1000", model.BuySwapPer1000);
            vParams.Add("@BuySwapPer", model.BuySwapPer);
            vParams.Add("@SellSwapPer1000", model.SellSwapPer1000);
            vParams.Add("@SellSwapPer", model.SellSwapPer);
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@ModifiedBy", model.ModifiedBy);
            vParams.Add("@IsActive", model.IsActive);
            var res = await _db.QueryMultipleAsync("[BDMCommision_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<BDMCommisionMarkUpSettingInstrumentWiseViewModel>().FirstOrDefault();
            return response;
        }
        public async Task<BDMViewModel> GetById(int Id)
        {
            BDMViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[BDM_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<BDMViewModel>().FirstOrDefault();

            return response;

        }
        public async Task<BDMCommisionMarkUpSettingInstrumentWiseViewModel> GetBDMCommision_BDMID(int Id)
        {
            BDMCommisionMarkUpSettingInstrumentWiseViewModel response1 = new();

            var vParams1 = new DynamicParameters();
            vParams1.Add("@BDMID", Id);
            var res1 = await _db.QueryMultipleAsync("[BDMCommision_GetByBDMID]", vParams1, commandType: CommandType.StoredProcedure);

            response1 = res1.Read<BDMCommisionMarkUpSettingInstrumentWiseViewModel>().FirstOrDefault();

            return response1;

        }



        public async Task<List<BDMViewModel>> GetBDMListALL()
        {
            var result = await _db.QueryAsync<BDMViewModel>(
               @"SELECT Id, Name FROM BDM"
               );

            return result.ToList();
        }

    }
}
