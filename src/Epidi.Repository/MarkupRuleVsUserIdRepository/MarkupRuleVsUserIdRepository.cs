using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.QuoteTiming;
using Epidi.Models.ViewModel.MarkupRuleVsUserId;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Users;
using System.Data;

namespace Epidi.Repository.MarkupRuleVsUserIdRepository
{
    public class MarkupRuleVsUserIdRepository : RepositoryBase,IMarkupRuleVsUserIdRepository
    {
        public MarkupRuleVsUserIdRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public async Task<long> AddMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId)
        {
            try
            {
                long retval = -1;

                List<MarkupRuleVsUserId> response = new List<MarkupRuleVsUserId>();
                var vParams = new DynamicParameters();
                vParams.Add("@UserId", MarkupRuleVsUserId.UserId);
                vParams.Add("@MarkUpRuleId", MarkupRuleVsUserId.MarkUpRuleId);
                vParams.Add("@CreatedDate", MarkupRuleVsUserId.CreatedDate);
                vParams.Add("@CreatedBy", MarkupRuleVsUserId.CreatedBy);

                try
                {
                    retval = await _db.QuerySingleAsync<long>("[Add_MarkupRuleVsUserId]", vParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }
                return retval;

                //retval = await _db.QuerySingleAsync<long>(
                //        @"INSERT INTO [dbo].[MarkupRuleVsUserId]
                //   ([UserId]
                //    ,[MarkUpRuleId]
                //    ,[CreatedDate]
                //    ,[CreatedBy]
                //    ,[DeletedBy])
                //OUTPUT inserted.Id
                //   VALUES
                //   (@UserId
                //   ,@MarkUpRuleId
                //   ,@CreatedDate
                //   ,@CreatedBy
                //    ,0
                //   )
                // ;", new
                //        {
                //            @UserId = MarkupRuleVsUserId.UserId,
                //            @MarkUpRuleId = MarkupRuleVsUserId.MarkUpRuleId,
                //            @CreatedDate = MarkupRuleVsUserId.CreatedDate,
                //            @CreatedBy = MarkupRuleVsUserId.CreatedBy
                //        }).ConfigureAwait(false);

                //return retval;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<long> EditMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId)
        {
            try
            {
                long retval = -1;

                List<MarkupRuleVsUserId> response = new List<MarkupRuleVsUserId>();
                var vParams = new DynamicParameters();
                vParams.Add("@Id", MarkupRuleVsUserId.Id, dbType: DbType.Int64, direction: ParameterDirection.Input);
                vParams.Add("@MarkUpRuleId", MarkupRuleVsUserId.MarkUpRuleId, dbType: DbType.String, direction: ParameterDirection.Input);
                vParams.Add("@UserId", MarkupRuleVsUserId.UserId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                try
                {
                    retval = await _db.QuerySingleAsync<long>("[Edit_MarkupRuleVsUserId]", vParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }
                return retval;

                //retval = await _db.QuerySingleAsync<long>(
                //        @"UPDATE [dbo].[MarkupRuleVsUserId]
                //       SET [UserId] = @UserId
                //          ,[MarkUpRuleId] = @MarkUpRuleId
                //    OUTPUT inserted.id
                //        WHERE Id=@Id
                //     ;", new
                //        {
                //                @UserId = MarkupRuleVsUserId.UserId,
                //                @MarkUpRuleId = MarkupRuleVsUserId.MarkUpRuleId,

                //            @Id = MarkupRuleVsUserId.Id
                //        }).ConfigureAwait(false);
                //return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       public async Task<List<MarkupRuleVsUserId>> GetMarkupRuleVsUserId()
        {
            List<MarkupRuleVsUserId> response = new List<MarkupRuleVsUserId>();

            var res = await _db.QueryMultipleAsync("[Get_MarkupRuleVsUserId]", commandType: CommandType.StoredProcedure);

            response = res.Read<MarkupRuleVsUserId>().ToList();

            return response;
            //var result = await _db.QueryAsync<MarkupRuleVsUserId>(@"SELECT MRU.[Id]
            //      ,MRU.[UserId]
            //	  ,U.Name +' '+ U.Surname AS UserName	
            //      ,MRU.[MarkUpRuleId]
            //	  ,QMR.Name AS Name	
            //      ,MRU.[CreatedDate]
            //      ,MRU.[CreatedBy]
            //      ,MRU.[DeletedDate]
            //      ,MRU.[DeletedBy]
            //  FROM [Epidi_Dev].[dbo].[MarkupRuleVsUserId] AS MRU WITH(NOLOCK) 
            //  INNER JOIN Users AS U ON MRU.UserId = U.Id
            //  INNER JOIN QuoteMarkUpRule QMR ON MRU.MarkUpRuleId = QMR.Id WHERE MRU.DeletedBy = 0");
            //return result.ToList();
        }

        public async Task<long> RemoveMarkupRuleVsUserId(int Id)
        {
            try
            {
                long retval = -1;
                var vParams = new DynamicParameters();
                vParams.Add("@Id", Id, dbType: DbType.Int64, direction: ParameterDirection.Input);

                retval = await _db.QuerySingleAsync<long>("[Remove_MarkupRuleVsUserId]", vParams, commandType: CommandType.StoredProcedure);
                return retval;

                //retval = await _db.QuerySingleAsync<long>(
                //        @"UPDATE MarkupRuleVsUserId SET DeletedBy = 1 ,DeletedDate = GETDATE() OUTPUT inserted.Id WHERE Id = @Id;", 
                //        new
                //        {
                //            @Id = Id
                //        }).ConfigureAwait(false);
                //return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<MarkupRuleVsUserId> GetMarkupRuleVsUserIdById(int Id)
        {
            var result = await _db.QueryAsync<MarkupRuleVsUserId>(@"SELECT * FROM MarkupRuleVsUserId Where Id=" + Id);
            return result.FirstOrDefault();
        }

        public Tuple<List<MarkupRuleVsUserId>, long> QuoteMarkUpRule_GetAll(PageParam pageParam, string search)
        {
            List<MarkupRuleVsUserId> response = new List<MarkupRuleVsUserId>();

            var vParams = new DynamicParameters();
            vParams.Add("@Offset", pageParam.Offset, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Limit", pageParam.Limit, dbType: DbType.Int64, direction: ParameterDirection.Input);
            vParams.Add("@Search", search, dbType: DbType.String, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[QuoteMarkUpRule_GetAll]", vParams, commandType: CommandType.StoredProcedure);

            response.AddRange(res.Read<MarkupRuleVsUserId>());
            long no = res.Read<long>().SingleOrDefault();
            var tuple = new Tuple<List<MarkupRuleVsUserId>, long>(response, no);
            return tuple;
        }
    }
}
