using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Epidi.Repository.CountryRepository
{
    public class CountryRepository : RepositoryBase, ICountryRepository
    {

        public CountryRepository(IOptions<ConnectionSettings> connectionOptions,
             IConnectionProvider connectionProvider
            ) : base(connectionOptions: connectionOptions,
                 connectionProvider: connectionProvider)
        {
        }

        public async Task<List<CountryDDl>> GetAllCountries()
        {
            List<CountryDDl> response = new List<CountryDDl>();

            var param = new DynamicParameters();
            var res = _db.QueryMultiple("[dbo].[GetAll_Countries]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<CountryDDl>().ToList();

            return response;
            //      var result = await _db.QueryAsync<CountryDDl>(
            //         @"SELECT CountryId as Id, CountryName, PhoneCode, OnboardingSteps,[Nationality],
            //[TaxResidence] FROM Country"
            //         );

            //      return result.ToList();
        }
      
        public async Task<ResponseViewModel> DeleteCountry(int Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);           

            var res = _db.QueryMultiple("[dbo].[Delete_Country]", param, commandType: CommandType.StoredProcedure);

            response=res.Read<ResponseViewModel>().SingleOrDefault();            
            return response;
        }

        public async Task<List<CountryViewModel>> GetCountryData()
        {
            List<CountryViewModel> response = new List<CountryViewModel>();

            var param = new DynamicParameters();
            var res = _db.QueryMultiple("[dbo].[Get_CountryData]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<CountryViewModel>().ToList();

            return response;
            //      var result = await _db.QueryAsync<CountryViewModel>(
            //         @"SELECT CountryId, CountryCode, CountryName, NiceName, Iso3, NumCode, PhoneCode, OnboardingSteps,Nationality,
            //TaxResidence FROM Country"
            //         );

            //      return result.ToList();
        }

        public Tuple<List<CountryViewModel>, long> GetAllCountriesForAdmin(PageParam pageParam, string search,string ColumnName,string SortType)
        {
            List <CountryViewModel> countryViewModels = new List<CountryViewModel>();
			var param = new DynamicParameters();
			param.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
			param.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
			param.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@ColumnName", ColumnName, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@SortType", SortType, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Country_All]", param, commandType: CommandType.StoredProcedure);

			countryViewModels.AddRange(res.Read<CountryViewModel>());
            long no = res.Read<long>().SingleOrDefault();
			var tuple = new Tuple<List<CountryViewModel>, long>(countryViewModels, no);
            return tuple;
		}

        public async Task<CountryViewModel> GetById(int Id)
        {
            CountryViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@CountryId", Id);
            var res = await _db.QueryMultipleAsync("[Country_GetById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<CountryViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<CountryViewModel> Upsert(CountryViewModel aCountry)
        {
            CountryViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@CountryId", aCountry.CountryId);
            vParams.Add("@CountryCode", aCountry.CountryCode);
            vParams.Add("@CountryName", aCountry.CountryName);
            vParams.Add("@NiceName", aCountry.NiceName);
            vParams.Add("@Iso3", aCountry.Iso3);
            vParams.Add("@NumCode", aCountry.NumCode);
            vParams.Add("@PhoneCode", aCountry.PhoneCode);
            vParams.Add("@OnboardingSteps", aCountry.OnboardingSteps);
            vParams.Add("@Nationality", aCountry.Nationality);
            vParams.Add("@TaxResidence", aCountry.TaxResidence);

            var res = await _db.QueryMultipleAsync("[Country_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<CountryViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<long> ImportCountryData_UpsertByDt(DataTable dtCountryData)
        {
            long roleId = 0;
            try
            {
                SqlConnection con = new SqlConnection(_connectionOptions.ConnectionString);
                await con.OpenAsync();

                SqlCommand sqlCommand = new SqlCommand("ImportCountryData_UpsertByDt", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParam = sqlCommand.Parameters.AddWithValue("@paramTable", dtCountryData);
                sqlParam.SqlDbType = SqlDbType.Structured;

                roleId = Convert.ToInt32(await sqlCommand.ExecuteScalarAsync());

                await con.CloseAsync();
                return roleId;
            }
            catch (Exception ex)
            {
                throw;
                return 0;
            }
        }

        public async Task<List<CountryViewModel>> GetCountryListAll()
        {
            List<CountryViewModel> response = new List<CountryViewModel>();

            var param = new DynamicParameters();
            var res = _db.QueryMultiple("[dbo].[Get_CountryListAll]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<CountryViewModel>().ToList();

            return response;
            //var result = await _db.QueryAsync<CountryViewModel>(
            //   @"SELECT CountryId, CountryName FROM Country"
            //   );

            //return result.ToList();
        }

    }
}
