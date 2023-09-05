using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Repository.ConnectionProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.TermsConditionRepository
{
    public class TermsConditionRepository : RepositoryBase, ITermsConditionRepository
    {
        public TermsConditionRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider
           ) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }

        public async Task<Tuple<List<TermsConditionViewModel>, long>> Get(PageParam pageParam, string search)
        {
            List<TermsConditionViewModel> response = new List<TermsConditionViewModel>();
            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[TermsCondition_Get]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<TermsConditionViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<TermsConditionViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<TermsConditionViewModel> Upsert(TermsConditionViewModel model)
        {
            TermsConditionViewModel response = new();

            var fileURL = model.FileList.Select(o => o.FileUrl).FirstOrDefault();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@TermsConditionText", model.TermsConditionText);
            vParams.Add("@CountryId", model.CountryId);
            vParams.Add("@FileUrl", fileURL);
            vParams.Add("@AutoFill", model.AutoFill);
            vParams.Add("@Title", model.Title);

            var res = await _db.QueryMultipleAsync("[TermsCondition_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<TermsConditionViewModel>().FirstOrDefault();

            return response;
        }

        public async Task<long> TermsConditionFile_Upsert(TCMultipleFilViewModel model)
        {
            TermsConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@TermsConditionId", model.TermsConditionId);
            //vParams.Add("@Id", model.Id);
            vParams.Add("@CountryId", model.CountryId);
            vParams.Add("@FileUrl", model.FileUrl);
            vParams.Add("@FileName", model.FileName);
            vParams.Add("@Title", model.Title);
            var res = await _db.QueryMultipleAsync("[TermsConditionFiles_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            
            return 1;
        }

        public async Task<TermsConditionViewModel> GetTermsConditionById(long Id)
        {
            TermsConditionViewModel response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id",Id);
            var res = await _db.QueryMultipleAsync("[Get_TermsCondition_ById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<TermsConditionViewModel>().FirstOrDefault();
            return response;
            //var result = await _db.QueryFirstOrDefaultAsync<TermsConditionViewModel>(
            //@"SELECT Id,TermsConditionText,CountryId,FileUrl,AutoFill,Title FROM TermsCondition  WHERE @Id IS NULL OR Id = @Id ",
            //new
            //{
            //    Id
            //});

            //return result;
        }
        public async Task<List<TCMultipleFilViewModel>> GetTermsConditionFilesById(long Id )
        {
            List<TCMultipleFilViewModel> response = new List<TCMultipleFilViewModel>();
            var vParams = new DynamicParameters();
            vParams.Add("@TermsConditionId", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            
            var res = await _db.QueryMultipleAsync("[TermsConditionFiles_ById]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<TCMultipleFilViewModel>());
            var tuple = new List<TCMultipleFilViewModel>(response);
            return tuple;
        }

        public List<ResponseViewModel> DeleteTermsCondition(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[TermsCondition_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }
        public async Task<List<TermsConditionViewModel>> GetTermsCondition_ByCountryId(int CountryId)
        {
            List<TermsConditionViewModel> response = new List<TermsConditionViewModel>();
            var vParams = new DynamicParameters();            
            vParams.Add("@CountryId", CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[TermsCondition_GetByCountryId]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<TermsConditionViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new List<TermsConditionViewModel>(response);
            return tuple;
        }

        public async Task<List<ResponseViewModel>> RemovePdfFile(long Id,string FileName)
        {
            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@TermsConditionId", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);
            param.Add("@FileName", FileName, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[dbo].[TermsConditionFiles_Delete]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }

        public async Task<TermsConditionViewModelSelect> GetTermsConditionBy_Id()
        {
            TermsConditionViewModel response = new();

            var res = await _db.QueryMultipleAsync("[Get_TermsConditionBy_Id]", commandType: CommandType.StoredProcedure);

            response = res.Read<TermsConditionViewModel>().FirstOrDefault();
            return response;
            //var res = await _db.QueryFirstOrDefaultAsync<TermsConditionViewModelSelect>(
            // @"SELECT Id,TermsConditionText,FileUrl FROM TermsCondition WHERE CountryId = 0",
            // new
            // {
            // });

            //return res;
        }
        public async Task<List<TermsConditionViewModelTitle>> GetTermsConditionByCountry(int CountryId)
        {
            List<TermsConditionViewModelTitle> response = new List<TermsConditionViewModelTitle>();
            var vParams = new DynamicParameters();
            vParams.Add("@CountryId", CountryId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[TermsCondition_GetTermsConditionById]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<TermsConditionViewModelTitle>());
           // long no = res.Read<long>().SingleOrDefault();
            var tuple = new List<TermsConditionViewModelTitle>(response);
            return tuple;
        }

        public Task<TermsConditionViewModelSelect> GetTermsCondition_ByCountry_Id()
        {
            throw new NotImplementedException();
        }
    }
}
