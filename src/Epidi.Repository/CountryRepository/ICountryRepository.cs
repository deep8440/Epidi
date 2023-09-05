using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.Country;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Repository.CountryRepository
{
    public interface ICountryRepository
    {
        Task<List<CountryDDl>> GetAllCountries();
        Task<List<CountryViewModel>> GetCountryData();
        Task<ResponseViewModel> DeleteCountry(int Id);
        Tuple<List<CountryViewModel>, long> GetAllCountriesForAdmin(PageParam pageParam, string search, string ColumnName, string SortType);
        Task<CountryViewModel> Upsert(CountryViewModel model);
        Task<CountryViewModel> GetById(int Id);
        Task<long> ImportCountryData_UpsertByDt(DataTable dtCountryData);

        Task<List<CountryViewModel>> GetCountryListAll();


    }
}
