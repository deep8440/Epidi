using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.CommissionRule;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.InstrumentMaster;
using Epidi.Models.ViewModel.RuleInstrument;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.RuleInstrumentRepository
{
    public class RuleInstrumentRepository : RepositoryBase, IRuleInstrumentRepository
    {
        public RuleInstrumentRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }

        public List<ResponseViewModel> Delete(int Id)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = _db.QueryMultiple("[RuleInstrument_Delete]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public Tuple<List<RuleInstrumentViewModel>, long> GetAll(PageParam pageParam, string search)
        {
            List<RuleInstrumentViewModel> response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[RuleInstrument_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<RuleInstrumentViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<RuleInstrumentViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<RuleInstrumentViewModel> GetById(int Id)
        {
            RuleInstrumentViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[RuleInstrument_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleInstrumentViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<RuleInstrumentViewModel> Upsert(RuleInstrumentViewModel aRuleInstrument)
        {
            RuleInstrumentViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", aRuleInstrument.Id);
            vParams.Add("@RuleId", aRuleInstrument.RuleId);
            vParams.Add("@InstrumentId", aRuleInstrument.InstrumentId);
            vParams.Add("@CreatedBy", aRuleInstrument.CreatedBy);
            vParams.Add("@UpdatedBy", aRuleInstrument.UpdatedBy);
            var res = await _db.QueryMultipleAsync("[RuleInstrument_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<RuleInstrumentViewModel>().FirstOrDefault();

            return response;
        }
        public async Task<List<RuleInstrumentViewModel>> GetByRuleId(int RuleId)
        {
            try
            {
                List<RuleInstrumentViewModel> response = new();

                var vParams = new DynamicParameters();
                vParams.Add("@RuleId", RuleId);
                var res = await _db.QueryMultipleAsync("[RuleInstrument_GetByRuleId]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<RuleInstrumentViewModel>().ToList();

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ResponseViewModel> DeleteByRuleId(int RuleId)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@RuleId", RuleId);
            var res = _db.QueryMultiple("[RuleInstrument_DeleteByRuleId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }
        public async Task<Tuple<List<RuleInstrumentLPViewModel>, long>> GetByInstrumentLPRuleByRuleId(PageParam pageParam, string search, int RuleId, int lp_filter = 0,
            int instrument_filter = 0, int day_filter = 0)
        {
            try
            {
                List<RuleInstrumentLPViewModel> ruleInstrumentLPViewModel = new List<RuleInstrumentLPViewModel>();

                var vParams = new DynamicParameters();
                vParams.Add("@RuleId", RuleId);
                vParams.Add("@lp_filter", lp_filter);
                vParams.Add("@instrument_filter", instrument_filter);
                vParams.Add("@day_filter", day_filter);
                vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
                vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
                vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
                var res = await _db.QueryMultipleAsync("[RuleInstrumentLPRule_GetByRuleId]", vParams, commandType: CommandType.StoredProcedure);


                ruleInstrumentLPViewModel.AddRange(res.Read<RuleInstrumentLPViewModel>());
                long no = res.Read<long>().SingleOrDefault();
                var tuple = new Tuple<List<RuleInstrumentLPViewModel>, long>(ruleInstrumentLPViewModel, no);
                return tuple;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<InstrumentMasterViewModel> Get_InstrumentByRuleId(int RuleId)
        {
            try
            {
                InstrumentMasterViewModel response = new();

                var vParams = new DynamicParameters();
                vParams.Add("@RuleId", RuleId);

                var res = await _db.QueryMultipleAsync("[GetInstrumentByRuleId]", vParams, commandType: CommandType.StoredProcedure);

                response = res.Read<InstrumentMasterViewModel>().FirstOrDefault();

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<List<RuleInstrumentLPViewModelExport>> GetExPortQuoteMarkUpRule_Data(int RuleId)
        {
            try
            {
                List<RuleInstrumentLPViewModelExport> response = new();

                var vParams = new DynamicParameters();
                vParams.Add("@RuleId", RuleId);
                var res = await _db.QueryMultipleAsync("[QuoteMarkUpRule_Export]", vParams, commandType: CommandType.StoredProcedure, commandTimeout: 1800);

                response = res.Read<RuleInstrumentLPViewModelExport>().ToList();

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }




     
    }
}