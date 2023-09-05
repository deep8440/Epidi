using Epidi.Models.Model;
using Epidi.Models.ViewModel.UsersDocuments;
using Epidi.Repository.UsersDocumentsRepository;
using Epidi.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.UsersDocumentsService
{
    public class UsersDocumentsService : IUsersDocumentsService
    {
        private readonly IUsersDocumentsRepository _usersDocumentsRepository;

        public UsersDocumentsService(IUsersDocumentsRepository usersDocumentsRepository)
        { 
            _usersDocumentsRepository = usersDocumentsRepository;
        }
        public Task<List<UsersDocumentViewModel>> GetByUserId(int UserId)
        {
            return _usersDocumentsRepository.GetByUserId(UserId);
        }

        public Task<UsersDocumentViewModel> Upsert(UsersDocumentViewModel model)
        {
            return _usersDocumentsRepository.Upsert(model);
        }
		public Task<int> DeleteDocument(int Id, int UserId)
		{
            return _usersDocumentsRepository.DeleteDocument(Id, UserId);
        }
    }
}
