using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.LegalEntity
{
    public class LegalEntityRepository : RepositoryBase, ILegalEntityRepository
    {
        public LegalEntityRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)  
        {
        }

        public List<ResponseViewModel> Delete(int Id)
        {

            List<ResponseViewModel> response = new List<ResponseViewModel>();

            var param = new DynamicParameters();
            param.Add("@Id", Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Delete_Entity]", param, commandType: CommandType.StoredProcedure);


            response = res.Read<ResponseViewModel>().ToList();

            return response;
        }



        //public async Task<ResponseViewModel> Delete(int id)
        //{
        //    try 
        //    {
        //        ResponseViewModel response = new ResponseViewModel();
        //        bool retval;
        //        var vParams = new DynamicParameters();
        //        vParams.Add("@param_id", id);
        //        //retval = await _db.QuerySingleAsync<bool>("[Delete_Entity]", vParams, commandType: CommandType.StoredProcedure);
        //        var res = _db.QueryMultiple("[Delete_Entity]", vParams, commandType: CommandType.StoredProcedure);
        //        //retval = await _db.QuerySingleAsync<bool>(
        //        //        @"DELETE FROM LegalEntity where Id=@param_id
        //        //     ;", new
        //        //        {~
        //        //            @param_id = id
        //        //        }).ConfigureAwait(false);

        //        response = res.Read<ResponseViewModel>().SingleOrDefault();
        //        return response;
        //    }
        //    catch (Exception ex) 
        //    {
        //        throw ex;
        //    }
            
        //}

        //public async Task<List<LegalEntityViewModel>> GetLegalEntity()
        //{
        //    var result = await _db.QueryAsync<LegalEntityViewModel>(@"SELECT * FROM LegalEntity");
        //    return result.ToList();
        //}

        public async Task<long> UpsertLegalEntity(LegalEntityViewModel legalEntityViewModel)
        {
            long retval = 0;
            List<LegalEntityViewModel> response = new List<LegalEntityViewModel>();
            var vParams = new DynamicParameters();
            vParams.Add("@param_id", legalEntityViewModel.Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@param_name", legalEntityViewModel.Name, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@param_phone", legalEntityViewModel.Phone, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@param_email", legalEntityViewModel.Email, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@param_address", legalEntityViewModel.Address, dbType: DbType.String, direction: ParameterDirection.Input);
            vParams.Add("@param_country", legalEntityViewModel.Country, dbType: DbType.Int64, direction: ParameterDirection.Input);

            //if (legalEntityViewModel.Id > 0)
            //{
                try
                {
                    retval = await _db.QuerySingleAsync<long>("[LegalEntity_Upsert]", vParams, commandType: CommandType.StoredProcedure);
                    //retval = await _db.QuerySingleAsync<long>(@"UPDATE [dbo].[LegalEntity] SET 
                    //                                    Name=@param_name,
                    //                                    Phone=@param_phone,
                    //                                    Email=@param_email,
                    //                                    Address=@param_address,
                    //                                    Country=@param_country
                    //                                    OUTPUT inserted.Id
                    //                                    WHERE Id=@param_id ;",
                    //                                               new
                    //                                               {
                    //                                                   @param_name = legalEntityViewModel.Name,
                    //                                                   @param_phone = legalEntityViewModel.Phone,
                    //                                                   @param_email = legalEntityViewModel.Email,
                    //                                                   @param_address = legalEntityViewModel.Address,
                    //                                                   @param_country = legalEntityViewModel.Country,
                    //                                                   @param_id = legalEntityViewModel.Id
                    //                                               }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {

                    throw;
                }
            //}
            //else
            //{
            //    retval = await _db.QuerySingleAsync<long>(
            //            @"INSERT INTO [dbo].[LegalEntity]
            //       (Name
            //       ,Phone
            //       ,Email
            //       ,Address
            //       ,Country)
            //    OUTPUT inserted.Id
            //       VALUES
            //       (@param_name
            //       ,@param_phone
            //       ,@param_email
            //       ,@param_address
            //       ,@param_country)
            //     ;", new
            //            {
            //                @param_name = legalEntityViewModel.Name,
            //                @param_phone = legalEntityViewModel.Phone,
            //                @param_email = legalEntityViewModel.Email,
            //                @param_address = legalEntityViewModel.Address,
            //                @param_country = legalEntityViewModel.Country
            //            }).ConfigureAwait(false);
            //}

            return retval;
        }

        public async Task<Tuple<List<LegalEntityViewModel>, long>> GetLegalEntity_All(PageParam pageParam, string search)
        {
            List<LegalEntityViewModel> response = new List<LegalEntityViewModel>();
            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = await _db.QueryMultipleAsync("[LegalEntity_Get]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<LegalEntityViewModel>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<LegalEntityViewModel>, long>(response, no);
            return tuple;
        }

        public async Task<List<LegalEntityViewModel>> GetLegalEntityListALL()
        {
            var result = await _db.QueryAsync<LegalEntityViewModel>(
               @"SELECT Id, Name FROM LegalEntity"
               );

            return result.ToList();
        }

        public async Task<bool> CheckNameExist(string name)
        {

            var exists = await _db.ExecuteScalarAsync<bool>(
                      @"
                            SELECT 1 from 
                            LegalEntity
                            WHERE LOWER(Name) = LOWER(@Name)
                            ",
                      new
                      {
                          Name = name,
                      }).ConfigureAwait(false);

            return exists;

        }



    }
}
