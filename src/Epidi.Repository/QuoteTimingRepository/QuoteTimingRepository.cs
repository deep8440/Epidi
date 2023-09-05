using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Instrument;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.QuoteTiming;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Epidi.Repository.QuoteTimingRepository
{
    public class QuoteTimingRepository : RepositoryBase, IQuoteTimingRepository
    {
        public QuoteTimingRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }

        public async Task<List<QuoteTiming>> GetQuoteTiming()
        {
            try 
            {
                List<QuoteTiming> response = new();

                var res = _db.QueryMultiple("[Get_QuoteTiming]", commandType: CommandType.StoredProcedure);

                response = res.Read<QuoteTiming>().ToList();
                return response;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            

            //          var result = await _db.QueryAsync<QuoteTiming>(@"SELECT QT.[Id]
            // ,IM.Name AS MasterInstrumentName	
            //    ,QT.[MasterInstrumentId]
            //    ,QT.[StartTime]
            //    ,QT.[EndTime]
            //    ,QT.[Day]
            //    ,QT.[CreatedDate]
            //    ,QT.[CreatedBy]
            //    ,QT.[DeletedDate]
            //    ,QT.[IsDelete]
            //FROM [Epidi_Dev].[dbo].[QuoteTiming] AS QT WITH(NOLOCK) 
            //INNER JOIN InstrumentMaster AS IM ON QT.MasterInstrumentId = IM.id
            //WHERE QT.[IsDelete] = 0");
            //          return result.ToList();
        }
        public async Task<QuoteTiming> GetQuoteTimingById(int id)
        {
            QuoteTiming response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@Id", id);
            var res = await _db.QueryMultipleAsync("[Get_QuoteTiming_ById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<QuoteTiming>().FirstOrDefault();
            return response;
            //var result = await _db.QueryAsync<QuoteTiming>(@"SELECT * FROM QuoteTiming Where Id=" + id);
            //return result.FirstOrDefault();
        }
        public async Task<long> AddQuoteTiming(QuoteTiming quoteTiming)
        {
            try
            {
                long retval = -1;

                var vParams = new DynamicParameters();
                vParams.Add("@MasterInstrumentId", quoteTiming.MasterInstrumentId);
                vParams.Add("@StartTime", quoteTiming.StartTime);
                vParams.Add("@EndTime", quoteTiming.EndTime);
                vParams.Add("@Day", quoteTiming.Day);
                vParams.Add("@CreatedDate", quoteTiming.CreatedDate);
                vParams.Add("@CreatedBy", quoteTiming.CreatedBy);

                var res = await _db.QueryMultipleAsync("[Add_QuoteTiming]", vParams, commandType: CommandType.StoredProcedure);

                retval = res.Read<long>().FirstOrDefault();

                return retval;

                //retval = await _db.QuerySingleAsync<long>(
                //        @"INSERT INTO [dbo].[quoteTiming]
                //   (MasterInstrumentId
                //   ,StartTime
                //   ,EndTime
                //   ,Day
                //   ,CreatedDate
                //   ,CreatedBy
                //   ,[IsDelete])
                //OUTPUT inserted.Id
                //   VALUES
                //   (@MasterInstrumentId
                //   ,@StartTime
                //   ,@EndTime
                //   ,@Day
                //   ,@CreatedDate
                //   ,@CreatedBy
                //   ,0)
                // ;", new
                //        {
                //            @MasterInstrumentId = quoteTiming.MasterInstrumentId,
                //            @StartTime = quoteTiming.StartTime,
                //            @EndTime = quoteTiming.EndTime,
                //            @Day = quoteTiming.Day,
                //            @CreatedDate = quoteTiming.CreatedDate,
                //            @CreatedBy = quoteTiming.CreatedBy,
                //        }).ConfigureAwait(false);

                //return retval;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<long> EditQuoteTiming(QuoteTiming quoteTiming)
        {
            long retval = -1;

            var vParams = new DynamicParameters();
            vParams.Add("@MasterInstrumentId", quoteTiming.MasterInstrumentId);
            vParams.Add("@StartTime", quoteTiming.StartTime);
            vParams.Add("@EndTime", quoteTiming.EndTime);
            vParams.Add("@Day", quoteTiming.Day);
            vParams.Add("@Id", quoteTiming.Id);

            var res = await _db.QueryMultipleAsync("[Edit_QuoteTiming]", vParams, commandType: CommandType.StoredProcedure);

            retval = res.Read<long>().FirstOrDefault();

            return retval;

            //retval = await _db.QuerySingleAsync<long>(
            //        @"UPDATE [dbo].[QuoteTiming] SET 
            //           [MasterInstrumentId] = @MasterInstrumentId
            //          ,[StartTime] = @StartTime
            //          ,[EndTime] = @EndTime
            //          ,[Day] = @Day
            //      OUTPUT inserted.Id
            //        WHERE Id= @Id
            //     ", new
            //        {
            //            @MasterInstrumentId = quoteTiming.MasterInstrumentId,
            //            @StartTime = quoteTiming.StartTime,
            //            @EndTime = quoteTiming.EndTime,
            //            @Day = quoteTiming.Day,
            //            @Id = quoteTiming.Id
            //        }).ConfigureAwait(false);
            //retval = quoteTiming.Id;
            //return retval;
        }
        public async Task<long> RemoveQuoteTiming(int id)
        {
            try
            {
                long retval = -1;

                var vParams = new DynamicParameters();
                vParams.Add("@Id", id);

                var res = await _db.QueryMultipleAsync("[Remove_QuoteTiming]", vParams, commandType: CommandType.StoredProcedure);

                retval = res.Read<long>().FirstOrDefault();

                return retval;

                //retval = await _db.QuerySingleAsync<long>(
                //        @"UPDATE QuoteTiming SET [IsDelete] = 1,[DeletedDate] =GETDATE() OUTPUT inserted.Id where Id=@id;", new
                //        {
                //            @id = id
                //        }).ConfigureAwait(false);

                //return retval;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
