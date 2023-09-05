using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using Epidi.Repository.CountryRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.CountryService
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;

        public CountryService(ICountryRepository repository)
        {
            _repository = repository;
        }
        public Task<List<CountryDDl>> GetAllCountries()
        {
           return _repository.GetAllCountries();
        }
        public Task<List<CountryViewModel>> GetCountryData()
        {
           return _repository.GetCountryData();
        }
        public async Task<ResponseViewModel> DeleteCountry(int Id)
        {
            return await _repository.DeleteCountry(Id);
        }

		public Tuple<List<CountryViewModel>, long> GetAllCountriesForAdmin(PageParam pageParam,string search, string ColumnName, string SortType)
		{
			return _repository.GetAllCountriesForAdmin(pageParam, search, ColumnName,SortType);
		}

        public async Task<CountryViewModel> GetById(int Id)
        {
            return await _repository.GetById(Id);
        }

        public async Task<CountryViewModel> Upsert(CountryViewModel model)
        {
            return await _repository.Upsert(model);
        }
        public async Task<long> ImportCountryData_UpsertByDt(DataTable dtCountryData)
        {
            return await _repository.ImportCountryData_UpsertByDt(dtCountryData);
        }

        public Task<List<CountryViewModel>> GetCountryListAll()
        {
            return _repository.GetCountryListAll();
        }



    }
}
