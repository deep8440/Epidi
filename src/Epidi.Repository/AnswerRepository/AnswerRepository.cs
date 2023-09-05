using Dapper;
using Epidi.Models.DBConnection;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Question;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using System.Data;

namespace Epidi.Repository.AnswerRepository
{
    public class AnswerRepository : RepositoryBase, IAnswerRepository
    {
        public AnswerRepository(IOptions<ConnectionSettings> connectionOptions,
           IConnectionProvider connectionProvider
          ) : base(connectionOptions: connectionOptions,
               connectionProvider: connectionProvider)
        {
        }
        public async Task<List<AnswerViewModel>> GetAnswerByQuestionId(long QuestionId)
        {
			List<AnswerViewModel> response = new List<AnswerViewModel>();
			var param = new DynamicParameters();
			param.Add("@QuestionId", QuestionId);
			var res = _db.QueryMultiple("[dbo].[GetAnswerBy_QuestionId]", param, commandType: CommandType.StoredProcedure);
			response = res.Read<AnswerViewModel>().ToList();

			return response;
			//        var result = await _db.QueryAsync<AnswerViewModel>(
			//@"SELECT AnswerId, QuestionId, AnswerDesc FROM Answer
			//           WHERE QuestionId = @QuestionId AND IsActive = 1", new { QuestionId }
			//         );

			//        return result.ToList();
		}
		public List<ResponseViewModel> SaveAnswer(AnswerViewModel model)
		{

			List<ResponseViewModel> response = new List<ResponseViewModel>();

			var param = new DynamicParameters();
			param.Add("@AnswerId", model.AnswerId, dbType: DbType.Int32, direction: ParameterDirection.Input);
			param.Add("@QuestionId", model.QuestionId, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@AnswerDesc", model.AnswerDesc, dbType: DbType.String, direction: ParameterDirection.Input);
			param.Add("@Weightage", model.Weightage, dbType: DbType.Int32, direction: ParameterDirection.Input);

            var res = _db.QueryMultiple("[dbo].[Answer_Upsert]", param, commandType: CommandType.StoredProcedure);


			response = res.Read<ResponseViewModel>().ToList();

			return response;

		}
		public async Task<AnswerViewModel> GetAnswerByAnswerId(long answerId)
		{
            try 
			{
				AnswerViewModel response = new();
				var param = new DynamicParameters();
				param.Add("@AnswerId", answerId);
				var res = _db.QueryMultiple("[dbo].[GetAnswerBy_AnswerId]", param, commandType: CommandType.StoredProcedure);
				response = res.Read<AnswerViewModel>().FirstOrDefault();

				return response;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
			//var result = await _db.QueryFirstOrDefaultAsync<AnswerViewModel>(
			//@"SELECT AnswerId,QuestionId, AnswerDesc FROM Answer
			//            WHERE (@AnswerId IS NULL OR AnswerId = @AnswerId)",
			//new
			//{
			//	answerId
			//});

			//return result;
			// FirstOrDefault
		}
		public List<ResponseViewModel> DeleteAnswer(int answerId)
		{

			List<ResponseViewModel> response = new List<ResponseViewModel>();

			var param = new DynamicParameters();
			param.Add("@AnswerId", answerId, dbType: DbType.Int32, direction: ParameterDirection.Input);

			var res = _db.QueryMultiple("[dbo].[Answer_Delete]", param, commandType: CommandType.StoredProcedure);


			response = res.Read<ResponseViewModel>().ToList();

			return response;

		}
	}
}
