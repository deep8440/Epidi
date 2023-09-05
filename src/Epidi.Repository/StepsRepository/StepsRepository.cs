using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Steps;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.ConnectionProvider;
using Epidi.Repository.CountryRepository;
using Epidi.Repository.IBRepository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.StepsRepository
{
    public class StepsRepository : RepositoryBase, IStepsRepository
    {

        public StepsRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider
           ) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public async Task<Tuple<List<StepsViewModel>, long>> GetAllSteps(PageParam pageParam, string search, int CountryId = 0)
        {
            List<StepsViewModel> countryViewModels = new List<StepsViewModel>();
            var param = new DynamicParameters();
            param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@CountryId", CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
           
            var res = _db.QueryMultiple("[dbo].[Steps_All]", param, commandType: CommandType.StoredProcedure);

            countryViewModels.AddRange(res.Read<StepsViewModel>());
            long no =  res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<StepsViewModel>, long>(countryViewModels, no);
            return tuple;
        }

        public async Task<List<StepAndPageNumberStatusViewModel>> GetStepAndPageNumberStatusInfo(int CountryId = 0)
        {
            List<StepAndPageNumberStatusViewModel> stepAndPageNumberStatusViewModels = new List<StepAndPageNumberStatusViewModel>();
            var param = new DynamicParameters();
            param.Add("@CountryId", CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[GetStepAndPageNumberStatusInfo]", param, commandType: CommandType.StoredProcedure);

            stepAndPageNumberStatusViewModels.AddRange(res.Read<StepAndPageNumberStatusViewModel>());
            return stepAndPageNumberStatusViewModels;
        }
        public  List<ResponseViewModel> DeleteSteps(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@StepsId", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Steps_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;

        }
      

        public async Task<StepsViewModel> GetStepsById(int Id)
        {
            StepsViewModel StepsViewModel = new StepsViewModel();
            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Steps_GetById]", param, commandType: CommandType.StoredProcedure);


            StepsViewModel = res.Read<StepsViewModel>().FirstOrDefault();
            return StepsViewModel;
        }

        public async Task<StepsViewModel> SaveSteps(StepsViewModel model)
        {

            StepsViewModel response = new StepsViewModel();

            var param = new DynamicParameters();
            param.Add("@StepsId", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@Title", model.Title, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@CountryId", model.CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@Priority", model.Priority, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@PageSize", model.PageSize, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@EstimateTime", model.EstimateTime, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@FooterTitle", model.FooterTitle, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@FooterDescription", model.FooterDescription, dbType: DbType.String, direction: ParameterDirection.Input);
            param.Add("@IsTermsAndConditions", model.IsTermsAndConditions, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            var res = _db.QueryMultiple("[dbo].[Steps_Upsert]", param, commandType: CommandType.StoredProcedure);
            response = res.Read<StepsViewModel>().FirstOrDefault();

            return response;

        }
        public async Task<List<StepsViewModel>> Steps_GetByCountryId(int countryId, int userId)
        {

            List<StepsViewModel> response = new List<StepsViewModel>();
            var param = new DynamicParameters();
            param.Add("@CountryId", countryId, dbType: DbType.Int64, direction: ParameterDirection.Input);
            param.Add("@UserId", userId, dbType: DbType.Int64, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[Steps_GetByCountryId]", param, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<StepsViewModel>());
            return response.ToList();
        }
    }
}
