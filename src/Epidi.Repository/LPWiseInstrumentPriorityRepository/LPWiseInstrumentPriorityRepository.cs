using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.QuoteTiming;
using Epidi.Models.ViewModel.LPWiseInstrumentPriority;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.ViewModel.InstrumentMaster;
using System.Data;
using Epidi.Models.ViewModel.LP;

namespace Epidi.Repository.LPWiseInstrumentPriorityRepository
{
    public class LPWiseInstrumentPriorityRepository : RepositoryBase,ILPWiseInstrumentPriorityRepository
    {
        public LPWiseInstrumentPriorityRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public async Task<long> AddLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority)
        {
            try
            {
                long retval = -1;
                var vParams = new DynamicParameters();
                vParams.Add("@Priority", lPWiseInstrumentPriority.Priority);
                vParams.Add("@LPId", lPWiseInstrumentPriority.LPId);
                vParams.Add("@StartDate", lPWiseInstrumentPriority.StartDate);
                vParams.Add("@EndDate", lPWiseInstrumentPriority.EndDate);
                vParams.Add("@CreatedBy", lPWiseInstrumentPriority.CreatedBy);
                vParams.Add("@CreatedDate", lPWiseInstrumentPriority.CreatedDate);

                retval = await _db.QuerySingleAsync<long>("[Add_LPWiseInstrument_Priority]", vParams, commandType: CommandType.StoredProcedure);

                return retval;

                //retval = await _db.QuerySingleAsync<long>(
                //        @"INSERT INTO [dbo].[LPWiseInstrumentPriority]
                //   ([Priority]
                //    ,[LPId]
                //    ,[StartDate]
                //    ,[EndDate]
                //    ,[IsActive]
                //    ,[CreatedBy]
                //    ,[CreatedDate]
                //    ,[DeletedBy])
                //OUTPUT inserted.Id
                //   VALUES
                //   (@Priority
                //   ,@LPId
                //   ,@StartDate
                //   ,@EndDate
                //   ,1
                //   ,@CreatedBy
                //   ,@CreatedDate
                //   ,0
                //   )
                // ;", new
                //        {
                //            @Priority = lPWiseInstrumentPriority.Priority,
                //            @LPId = lPWiseInstrumentPriority.LPId,
                //            @StartDate = lPWiseInstrumentPriority.StartDate,
                //            @EndDate = lPWiseInstrumentPriority.EndDate,
                //            @CreatedDate = lPWiseInstrumentPriority.CreatedDate,
                //            @CreatedBy = lPWiseInstrumentPriority.CreatedBy
                //        }).ConfigureAwait(false);

                //return retval;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<long> EditLPWiseInstrumentPriority(LPWiseInstrumentPriority lPWiseInstrumentPriority)
        {
            try
            {
            long retval = -1;

                var vParams = new DynamicParameters();
                vParams.Add("@Priority", lPWiseInstrumentPriority.Priority);
                vParams.Add("@LPId", lPWiseInstrumentPriority.LPId);
                vParams.Add("@StartDate", lPWiseInstrumentPriority.StartDate);
                vParams.Add("@EndDate", lPWiseInstrumentPriority.EndDate);
                vParams.Add("@UpdatedDate", lPWiseInstrumentPriority.UpdatedDate);
                vParams.Add("@UpdatedBy", lPWiseInstrumentPriority.UpdatedBy);
                vParams.Add("@id", lPWiseInstrumentPriority.id);

                retval = await _db.QuerySingleAsync<long>("[Edit_LPWiseInstrument_Priorityy]", vParams, commandType: CommandType.StoredProcedure);
            //retval = await _db.QuerySingleAsync<long>(
            //        @"UPDATE [dbo].[LPWiseInstrumentPriority]
            //       SET [Priority] = @Priority
            //          ,[StartDate] = @StartDate
            //          ,[EndDate] = @EndDate
            //          ,[UpdatedDate] = @UpdatedDate
            //          ,[UpdatedBy] = @UpdatedBy
            //          ,[LPId] = @LPId
            //    OUTPUT inserted.Id
            //        WHERE Id=@id
            //     ;", new
            //        {
            //            @Priority = lPWiseInstrumentPriority.Priority,
            //            @StartDate = lPWiseInstrumentPriority.StartDate,
            //            @EndDate = lPWiseInstrumentPriority.EndDate,
            //            @LPId = lPWiseInstrumentPriority.LPId,
            //            @UpdatedDate = lPWiseInstrumentPriority.UpdatedDate,
            //            @UpdatedBy = lPWiseInstrumentPriority.UpdatedBy,
            //            @id = lPWiseInstrumentPriority.id
            //        }).ConfigureAwait(false);
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       public async Task<List<LPWiseInstrumentPriority>> GetLPWiseInstrumentPriority()
        {
            List<LPWiseInstrumentPriority> response = new List<LPWiseInstrumentPriority>();
            var result = await _db.QueryMultipleAsync("[Get_LPWiseInstrument_Priority]", commandType: CommandType.StoredProcedure);
            response.AddRange(result.Read<LPWiseInstrumentPriority>());
            return response.ToList();
            //          var result = await _db.QueryAsync<LPWiseInstrumentPriority>(@"SELECT LIP.[Id]
            //    ,LIP.[Priority]
            // ,MLP.[Name] As LPName
            //    ,LIP.[LPId]
            //    ,LIP.[StartDate]
            //    ,LIP.[EndDate]
            //    ,LIP.[IsActive]
            //    ,LIP.[CreatedBy]
            //    ,LIP.[CreatedDate]
            //    ,LIP.[UpdatedBy]
            //    ,LIP.[UpdatedDate]
            //    ,LIP.[DeletedBy]
            //    ,LIP.[DeletedDate]
            //FROM [Epidi_Dev].[dbo].[LPWiseInstrumentPriority]AS LIP WITH(NOLOCK) 
            //INNER JOIN MasterLP AS MLP ON LIP.LPId = MLP.id WHERE LIP.DeletedBy = 0");
            //return result.ToList();
        }

        public async Task<long> RemoveLPWiseInstrumentPriority(int id,int DeletedBy)
        {
            try
            {
                long retval = -1;

                var vParams = new DynamicParameters();
                vParams.Add("@id", id);
                vParams.Add("@DeletedBy", DeletedBy);

                retval = await _db.QuerySingleAsync<long>("[Remove_LPWiseInstrument_Priority]", vParams, commandType: CommandType.StoredProcedure);

                return retval;
                //retval = await _db.QuerySingleAsync<long>(
                //        @"UPDATE LPWiseInstrumentPriority SET DeletedDate = GETDATE(),DeletedBy = @DeletedBy OUTPUT inserted.Id WHERE Id = @id;", 
                //        new
                //        {
                //            @id = id,
                //            @DeletedBy = DeletedBy
                //        }).ConfigureAwait(false);
                //return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<long> ActiveInActiveLPWiseInstrumentPriority(int id,bool IsActive)
        {
            try
            {
                long retval = -1;
                var vParams = new DynamicParameters();
                vParams.Add("@id", id);
                vParams.Add("@IsActive", IsActive);

                retval = await _db.QuerySingleAsync<long>("[Active_InActive_LPWiseInstrument_Priority]", vParams, commandType: CommandType.StoredProcedure);

                return retval;
                //retval = await _db.QuerySingleAsync<long>(
                //        @"UPDATE LPWiseInstrumentPriority SET IsActive = @IsActive OUTPUT inserted.Id WHERE Id = @id;", 
                //        new
                //        {
                //            @id = id,
                //            @IsActive = IsActive
                //        }).ConfigureAwait(false);
                //return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<LPWiseInstrumentPriority> GetLPWiseInstrumentPriorityById(int id)
        {
            var result = await _db.QueryAsync<LPWiseInstrumentPriority>(@"SELECT * FROM LPWiseInstrumentPriority Where Id=" + id);
            return result.FirstOrDefault();
        }
        public async Task<List<LPViewModel>> Get_All_LP()
        {
            List<LPViewModel> response = new List<LPViewModel>();

            var res = await _db.QueryMultipleAsync("[LP_GetAll]", commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<LPViewModel>());
            return response.ToList();
        }
    }
}
