using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.AnswerRepository;
using Epidi.Repository.CountryWiseDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.CountryWiseDocumentService
{
    public class CountryWiseDocumentService : ICountryWiseDocumentService
    {
        private readonly ICountryWiseDocumentRepository _countryWiseDocumentRepository;
        public CountryWiseDocumentService(ICountryWiseDocumentRepository countryWiseDocumentRepository)
        {
            _countryWiseDocumentRepository = countryWiseDocumentRepository;
        }

        public List<ResponseViewModel> DeleteCountryWiseDocument(int Id)
        {
            return _countryWiseDocumentRepository.DeleteCountryWiseDocument(Id);
        }

        public async Task<CountryWiseDocumentViewModel> GetCountryWiseDocumentByCountryId(int CountryId)
        {
            return await _countryWiseDocumentRepository.GetCountryWiseDocumentByCountryId(CountryId);
        }

        public Task<CountryWiseDocumentViewModel> GetCountryWiseDocumentById(int Id)
        {
            return _countryWiseDocumentRepository.GetCountryWiseDocumentById(Id);
        }

        public Tuple<List<CountryWiseDocumentViewModel>, long> GetCountryWiseDocumentList(PageParam pageParam, string search, string ColumnName, string SortType)
        {
            return _countryWiseDocumentRepository.GetCountryWiseDocumentList(pageParam,search, ColumnName, SortType);

        }

        public List<ResponseViewModel> SaveCountryWiseDocument(CountryWiseDocumentViewModel model)
        {
            return _countryWiseDocumentRepository.SaveCountryWiseDocument(model);
        }
        public async Task<List<ResponseViewModel>> CountryWiseDocument_Upsert(List<CountryWiseDocumentViewModel> model)
        {
            return await _countryWiseDocumentRepository.CountryWiseDocument_Upsert(model);
        }
        public async Task<List<ResponseViewModel>> CountryWiseDocument_CopyData(CopyCountryDataViewModel model)
        {
            return await _countryWiseDocumentRepository.CountryWiseDocument_CopyData(model);
        }
    }
}
