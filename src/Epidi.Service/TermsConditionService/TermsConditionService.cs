using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.IBCommissionMarkUpSettingInstrumentWise;
using Epidi.Models.ViewModel.Question;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Repository.TermsConditionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.TermsConditionService
{
    public class TermsConditionService : ITermsConditionService
    {
        private readonly ITermsConditionRepository _termsConditionRepository;

        public TermsConditionService(ITermsConditionRepository termsConditionRepository)
        {
            _termsConditionRepository = termsConditionRepository;

        }
        public async Task<Tuple<List<TermsConditionViewModel>, long>> Get(PageParam pageParam, string search)
        {
            return await _termsConditionRepository.Get(pageParam, search);
        }
        public async Task<TermsConditionViewModel> Upsert(TermsConditionViewModel model)
        {
            return await _termsConditionRepository.Upsert(model);
        }
        public async Task<long> TermsConditionFile_Upsert(TCMultipleFilViewModel model)
        {
            return await _termsConditionRepository.TermsConditionFile_Upsert(model);
        }
        public async Task<TermsConditionViewModel> GetTermsConditionById(long Id)
        {
            return await _termsConditionRepository.GetTermsConditionById(Id);
        }
        public async Task<List<TCMultipleFilViewModel>> GetTermsConditionFilesById(long Id)
        {
            return await _termsConditionRepository.GetTermsConditionFilesById(Id);
        }
        public List<ResponseViewModel> DeleteTermsCondition(int Id)
        {
            return _termsConditionRepository.DeleteTermsCondition(Id);
        }
        public async Task<List<TermsConditionViewModel>> GetTermsCondition_ByCountryId(int CountryId)
        {
            return await _termsConditionRepository.GetTermsCondition_ByCountryId(CountryId);
        }
        public async Task<List<ResponseViewModel>> RemovePdfFile(long Id, string FileName)
        {
            return await _termsConditionRepository.RemovePdfFile(Id, FileName);
        }
        public async Task<TermsConditionViewModelSelect> GetTermsConditionBy_Id()
        {
            return await _termsConditionRepository.GetTermsConditionBy_Id();
        }
        public async Task<List<TermsConditionViewModelTitle>> GetTermsConditionByCountry(int CountryId)
        {
            return await _termsConditionRepository.GetTermsConditionByCountry(CountryId);
        }

        
    }
}
