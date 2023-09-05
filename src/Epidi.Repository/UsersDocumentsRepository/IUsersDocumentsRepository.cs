﻿using Epidi.Models.ViewModel.UsersDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.UsersDocumentsRepository
{
    public interface IUsersDocumentsRepository
    {
        Task<UsersDocumentViewModel> Upsert(UsersDocumentViewModel model);
        Task<List<UsersDocumentViewModel>> GetByUserId(int UserId);
        Task<int> DeleteDocument(int Id, int UserId);

	}
}
