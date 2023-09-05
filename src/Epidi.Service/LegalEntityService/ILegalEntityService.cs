using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Models.ViewModel.TermsCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.LegalEntityService
{
    public interface ILegalEntityService
    {
        //  Task<List<LegalEntityViewModel>> GetLegal_Entity();
        Task<bool> CheckNameExist(string name);
        Task<long> UpsertLegalEntity(LegalEntityViewModel legalEntityViewModel);
        List<ResponseViewModel> Delete(int Id);
       


        Task<Tuple<List<LegalEntityViewModel>, long>> GetLegalEntity_All(PageParam pageParam, string search);
        


        Task<List<LegalEntityViewModel>> GetLegalEntityListALL();


    }
}
