using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.QuoteTiming;
using Epidi.Models.ViewModel.TradeTiming;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.TradeTimingRepository
{
    public class TradeTimingRepository : RepositoryBase,ITradeTimingRepository
    {
        public TradeTimingRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public async Task<long> AddTradeTiming(TradeTiming tradeTiming)
        {
            try
            {
                long retval = -1;

                var vParams = new DynamicParameters();
                vParams.Add("@MasterInstrumentId", tradeTiming.MasterInstrumentId);
                vParams.Add("@StartTime", tradeTiming.StartTime);
                vParams.Add("@EndTime", tradeTiming.EndTime);
                vParams.Add("@Day", tradeTiming.Day);
                vParams.Add("@CreatedDate", tradeTiming.CreatedDate);
                vParams.Add("@CreatedBy", tradeTiming.CreatedBy);

                var res = await _db.QueryMultipleAsync("[Add_TradeTimin]", vParams, commandType: CommandType.StoredProcedure);

                retval = res.Read<long>().FirstOrDefault();

                return retval;

                //retval = await _db.QuerySingleAsync<long>(
                //        @"INSERT INTO [dbo].[TradeTiming]
                //   ([MasterInstrumentId]
                //    ,[StartTime]
                //    ,[EndTime]
                //    ,[Day]
                //    ,[CreatedDate]
                //    ,[CreatedBy]
                //    ,[IsDelete])
                //OUTPUT inserted.Id
                //   VALUES
                //   (@MasterInstrumentId
                //   ,@StartTime
                //   ,@EndTime
                //   ,@Day
                //   ,@CreatedDate
                //   ,@CreatedBy
                //    ,0
                //   )
                // ;", new
                //        {
                //            @MasterInstrumentId = tradeTiming.MasterInstrumentId,
                //            @StartTime = tradeTiming.StartTime,
                //            @EndTime = tradeTiming.EndTime,
                //            @Day = tradeTiming.Day,
                //            @CreatedDate = tradeTiming.CreatedDate,
                //            @CreatedBy = tradeTiming.CreatedBy
                //        }).ConfigureAwait(false);

                //return retval;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<long> EditTradeTiming(TradeTiming tradeTiming)
        {
            try
            {
                long retval = -1;

                var vParams = new DynamicParameters();
                vParams.Add("@MasterInstrumentId", tradeTiming.MasterInstrumentId);
                vParams.Add("@StartTime", tradeTiming.StartTime);
                vParams.Add("@EndTime", tradeTiming.EndTime);
                vParams.Add("@Day", tradeTiming.Day);
                vParams.Add("@id", tradeTiming.id);

                var res = await _db.QueryMultipleAsync("[Edit_TradeTiming]", vParams, commandType: CommandType.StoredProcedure);

                retval = res.Read<long>().FirstOrDefault();

                return retval;
                //retval = await _db.QuerySingleAsync<long>(
                //        @"UPDATE [dbo].[TradeTiming]
                //       SET [MasterInstrumentId] = @MasterInstrumentId
                //          ,[StartTime] = @StartTime
                //          ,[EndTime] = @EndTime
                //          ,[Day] = @Day
                //    OUTPUT inserted.Id
                //        WHERE Id=@id
                //     ;", new
                //        {
                //                @MasterInstrumentId = tradeTiming.MasterInstrumentId,
                //                @StartTime = tradeTiming.StartTime,
                //                @EndTime = tradeTiming.EndTime,
                //                @Day = tradeTiming.Day,
                //            @id = tradeTiming.id
                //        }).ConfigureAwait(false);
                //return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       public async Task<List<TradeTiming>> GetTradeTiming()
        {
            try 
            {
                List<TradeTiming> response = new List<TradeTiming>();

                var res = await _db.QueryMultipleAsync("[Get_TradeTiming]", commandType: CommandType.StoredProcedure);

                response = res.Read<TradeTiming>().ToList();

                return response;
            }
            catch (Exception ex) 
            {
                throw ex;
            }

            

            //var result = await _db.QueryAsync<TradeTiming>(@"SELECT TT.[Id]
            //	  ,IM.Name AS MasterInstrumentName	
            //      ,TT.[MasterInstrumentId]
            //      ,TT.[StartTime]
            //      ,TT.[EndTime]
            //      ,TT.[Day]
            //      ,TT.[CreatedDate]
            //      ,TT.[CreatedBy]
            //      ,TT.[DeletedDate]
            //      ,TT.[IsDelete]
            //  FROM [Epidi_Dev].[dbo].[TradeTiming] AS TT WITH(NOLOCK) 
            //  INNER JOIN InstrumentMaster AS IM ON TT.MasterInstrumentId = IM.id WHERE TT.IsDelete = 0");
            //return result.ToList();
        }

        public async Task<long> RemoveTradeTiming(int id)
        {
            try
            {
                long retval = -1;
                var vParams = new DynamicParameters();
                vParams.Add("@id", id);

                var res = await _db.QueryMultipleAsync("[Remove_TradeTiming]", vParams, commandType: CommandType.StoredProcedure);

                retval = res.Read<long>().FirstOrDefault();

                return retval;
                //retval = await _db.QuerySingleAsync<long>(
                //        @"UPDATE TradeTiming SET IsDelete = 1 ,DeletedDate = GETDATE() OUTPUT inserted.Id WHERE Id = @id;", 
                //        new
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
        public async Task<TradeTiming> GetTradeTimingById(int id)
        {
            TradeTiming response = new();

            var vParams = new DynamicParameters();
            vParams.Add("@id", id);
            var res = await _db.QueryMultipleAsync("[Get_TradeTiming_ById]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<TradeTiming>().FirstOrDefault();
            return response;

            //var result = await _db.QueryAsync<TradeTiming>(@"SELECT * FROM TradeTiming Where Id=" + id);
            //return result.FirstOrDefault();
        }
    }
}
