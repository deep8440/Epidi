using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidi.Models.DBConnection;
using Epidi.Repository.ConnectionProvider;
using Microsoft.Extensions.Options;
using Dapper;
using Epidi.Models.ViewModel.Users;
using Epidi.Models.ViewModel.UsersDocuments;
using System.Data;

namespace Epidi.Repository.UsersDocumentsRepository
{
    public class UsersDocumentsRepository : RepositoryBase, IUsersDocumentsRepository
    {

        public UsersDocumentsRepository(IOptions<ConnectionSettings> connectionOptions,
            IConnectionProvider connectionProvider) : base(connectionOptions: connectionOptions,
                connectionProvider: connectionProvider)
        {
        }
        public async Task<UsersDocumentViewModel> Upsert(UsersDocumentViewModel model)
        {
            UsersDocumentViewModel response = null;

            var vParams = new DynamicParameters();
            vParams.Add("@Id", model.Id);
            vParams.Add("@UserId", model.UserId);
            vParams.Add("@DocumentPath", model.DocumentPath);
            vParams.Add("@DocumentName", model.DocumentName);
            vParams.Add("@CreatedBy", model.CreatedBy);
            vParams.Add("@UpdatedBy", model.UpdatedBy);
            vParams.Add("@DocumentType", model.DocumentType);

            var res = await _db.QueryMultipleAsync("[UsersDocument_Upsert]", vParams, commandType: CommandType.StoredProcedure);

            response = res.Read<UsersDocumentViewModel>().FirstOrDefault();
            return response;
        }
        public async Task<List<UsersDocumentViewModel>> GetByUserId(int UserId)
        {
            List<UsersDocumentViewModel> response = new List<UsersDocumentViewModel>();

            var vParams = new DynamicParameters();
            vParams.Add("@UserId", UserId);

            var res = await _db.QueryMultipleAsync("[UsersDocument_ByUserId]", vParams, commandType: CommandType.StoredProcedure);
            response = res.Read<UsersDocumentViewModel>().ToList();
            return response;

        }

		public async Task<int> DeleteDocument(int Id,int UserId)
		{
			var vParams = new DynamicParameters();
			vParams.Add("@UserId", UserId);
			vParams.Add("@Id", Id);

			var res = await _db.QueryMultipleAsync("[UsersDocument_Delete]", vParams, commandType: CommandType.StoredProcedure);
			int resp = res.Read<int>().FirstOrDefault();
			return resp;
		}
	}
}
