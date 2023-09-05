using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.InstrumentRepository
{
    public class InstrumentSpecificationRepository : RepositoryBase,IInstrumentSpecificationRepository
    {
        public InstrumentSpecificationRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }

        public async Task<Tuple<List<InstrumentSpecificationViewModel>, long>> GetAllnstrumentSpecification(PageParam pageParam, string search)
        {
            List<InstrumentSpecificationViewModel> instrumentSpecificationViewModels = new();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[dbo].[InstrumentMapping_Specification]", param, commandType: CommandType.StoredProcedure, commandTimeout : 10000).ConfigureAwait(false);

            instrumentSpecificationViewModels.AddRange(res.Read<InstrumentSpecificationViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<InstrumentSpecificationViewModel>, long>(instrumentSpecificationViewModels, no);
            return tuple;
        }

        public async Task<InstrumentSpecificationViewModel> GetInstrumentSpecification(EditInstrumentSpecificationViewModel model)
        {
            List<InstrumentSpecificationViewModel> instrumentSpecificationViewModels = new();
            var param = new DynamicParameters();
            param.Add("@LPId", model.LPId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@MasterInstrumentId", model.MasterInstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@LPInstrumentId", model.LPInstrumentId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@TradeId", model.TradeId, dbType: DbType.Int64, direction: ParameterDirection.Input);


            var res = await _db.QueryMultipleAsync("[dbo].[GET_InstrumentMapping_Specification]", param, commandType: CommandType.StoredProcedure).ConfigureAwait(false);

            instrumentSpecificationViewModels.AddRange(res.Read<InstrumentSpecificationViewModel>());
            return instrumentSpecificationViewModels.FirstOrDefault();
        }

        public List<ResponseViewModel> UpdateInstrumentSpecification(InstrumentSpecificationViewModel model)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@LPId", model.LPId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@MasterInstrumentId", model.MasterInstrumentId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@LPInstrumentId", model.LPInstrumentId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@TradeId", model.TradeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@TTFrom", model.TTFrom, dbType: DbType.Time, direction: ParameterDirection.Input);
            param.Add("@TTTo", model.TTTo, dbType: DbType.Time, direction: ParameterDirection.Input);
            param.Add("@QTFrom", model.QTFrom, dbType: DbType.Time, direction: ParameterDirection.Input);
            param.Add("@QTTo", model.QTTo, dbType: DbType.Time, direction: ParameterDirection.Input);
            param.Add("@TradeDays", model.TradeDays, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@MinValue", model.MinValue, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Multiplier", model.Multiplier, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@OnOff", model.OnOff, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@ContractSize", model.ContractSize, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            param.Add("@ISIN", model.ISIN, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[UPDATE_InstrumentMapping_Specification]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;

        }

        public List<ResponseViewModel> DeleteInstrumentSpecification(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Delete_InstrumentMappingSpecification]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }



    }
}
