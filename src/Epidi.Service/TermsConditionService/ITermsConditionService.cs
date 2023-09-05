using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.TermsCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TermsConditionService
{
    public interface ITermsConditionService
    {
        Task<Tuple<List<TermsConditionViewModel>, long>> Get(PageParam pageParam, string search);
        Task<TermsConditionViewModel> Upsert(TermsConditionViewModel model);
        Task<TermsConditionViewModel> GetTermsConditionById(long Id);
        List<ResponseViewModel> DeleteTermsCondition(int Id);
        Task<List<TermsConditionViewModel>> GetTermsCondition_ByCountryId(int CountryId);
        Task<List<ResponseViewModel>> RemovePdfFile(long Id, string FileName);
        Task<TermsConditionViewModelSelect> GetTermsConditionBy_Id();
        Task<List<TermsConditionViewModelTitle>> GetTermsConditionByCountry(int CountryId);
        Task<long> TermsConditionFile_Upsert(TCMultipleFilViewModel model);
        Task<List<TCMultipleFilViewModel>> GetTermsConditionFilesById(long Id);
    }
}
