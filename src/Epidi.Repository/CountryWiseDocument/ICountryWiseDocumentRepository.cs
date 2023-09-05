using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.CountryWiseDocument
{
    public interface ICountryWiseDocumentRepository
    {

        Tuple<List<CountryWiseDocumentViewModel>, long> GetCountryWiseDocumentList(PageParam pageParam, string search, string ColumnName, string SortType);
        public List<ResponseViewModel> SaveCountryWiseDocument(CountryWiseDocumentViewModel model);
        public Task<List<ResponseViewModel>> CountryWiseDocument_Upsert(List<CountryWiseDocumentViewModel> model);
        public Task<CountryWiseDocumentViewModel> GetCountryWiseDocumentById(int Id);
        public List<ResponseViewModel> DeleteCountryWiseDocument(int Id);
        public Task<CountryWiseDocumentViewModel> GetCountryWiseDocumentByCountryId(int CountryId);
        Task<List<ResponseViewModel>> CountryWiseDocument_CopyData(CopyCountryDataViewModel model);
    }
}
