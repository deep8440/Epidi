using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Margin;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.AnswerRepository;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.CountryWiseDocument
{
    public class CountryWiseDocumentRepository : RepositoryBase, ICountryWiseDocumentRepository
    {
        public CountryWiseDocumentRepository(IOptions<ConnectionSettings> connectionOptions,
        IConnectionProvider connectionProvider
       ) : base(connectionOptions: connectionOptions,
            connectionProvider: connectionProvider)
        {
        }

        public Tuple<List<CountryWiseDocumentViewModel>, long> GetCountryWiseDocumentList(PageParam pageParam, string search, string ColumnName, string SortType)
        {
            List<CountryWiseDocumentViewModel> response = new List<CountryWiseDocumentViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@ColumnName", ColumnName, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@SortType", SortType, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[CountryWiseDocument_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<CountryWiseDocumentViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<CountryWiseDocumentViewModel>, long>(response, no);
            return tuple;
        }

        public List<ResponseViewModel> DeleteCountryWiseDocument(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[CountryWiseDocument_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public async Task<CountryWiseDocumentViewModel> GetCountryWiseDocumentById(int Id)
        {
            CountryWiseDocumentViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", Id);
            var res = await _db.QueryMultipleAsync("[Get_CountryWiseDocument_ById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<CountryWiseDocumentViewModel>().FirstOrDefault();

            return response;
            //var result = _db.QueryFirstOrDefaultAsync<CountryWiseDocumentViewModel>(
            //@"SELECT CountryId,DocumentName, Id FROM CountryWiseDocument
            //   WHERE (@Id IS NULL OR Id = @Id)",
            //new
            //{
            //    Id
            //});

            //return result;
        }

        public List<ResponseViewModel> SaveCountryWiseDocument(CountryWiseDocumentViewModel model)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@CountryId", model.CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@DocumentName", model.DocumentName, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[CountryWise_Document_Upsert]", param, commandType: CommandType.StoredProcedure);

            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public async Task<List<ResponseViewModel>> CountryWiseDocument_Upsert(List<CountryWiseDocumentViewModel> model)
        {
            try
            {
                List<ResponseViewModel> response = new List<ResponseViewModel>();
                foreach (var item in model)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", item.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
                    param.Add("@CountryId", item.CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                    param.Add("@DocumentName", item.DocumentName, dbType: DbType.String, direction: ParameterDirection.Input);

                    var res = await _db.QueryMultipleAsync("[dbo].[CountryWise_Document_Upsert]", param, commandType: CommandType.StoredProcedure);

                    response = res.Read<ResponseViewModel>().ToList();
                }
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ResponseViewModel>> CountryWiseDocument_CopyData(CopyCountryDataViewModel model)
        {
            try
            {
                List<ResponseViewModel> response = new List<ResponseViewModel>();

                var param = new DynamicParameters();
                param.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@FromCountryId", model.FromCountry, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@ToCountryId", model.ToCountry, dbType: DbType.Int32, direction: ParameterDirection.Input);

                var res = await _db.QueryMultipleAsync("[dbo].[CountryWise_Document_CopyData]", param, commandType: CommandType.StoredProcedure);

                response = res.Read<ResponseViewModel>().ToList();

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CountryWiseDocumentViewModel> GetCountryWiseDocumentByCountryId(int CountryId)
        {
            CountryWiseDocumentViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@CountryId", CountryId);
            var res = await _db.QueryMultipleAsync("[CountryWiseDocument_GetByCountryId]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<CountryWiseDocumentViewModel>().FirstOrDefault();

            return response;
        }
    }
}
