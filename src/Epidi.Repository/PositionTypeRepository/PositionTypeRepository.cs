using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.PositionType;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.PositionTypeRepository
{
    public class PositionTypeRepository : RepositoryBase, IPositionTypeRepository
    {
        public PositionTypeRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }

        public async Task<List<PositionTypeViewModel>> Get_All()
        {
            List<PositionTypeViewModel> response = new List<PositionTypeViewModel>();
            var res = await _db.QueryMultipleAsync("[PositionType_All]", null, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<PositionTypeViewModel>());

            return response;
        }       
    }
}
