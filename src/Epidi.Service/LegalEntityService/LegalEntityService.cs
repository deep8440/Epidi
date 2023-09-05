using Epidi.Models.ExceptionHelper;
using Epidi.Models.Paging;
using Epidi.Models.Utility;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.LegalEntity;
using Epidi.Models.ViewModel.TermsCondition;
using Epidi.Repository.InstrumentRepository;
using Epidi.Repository.LegalEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Epidi.Service.LegalEntityService
{
    public class LegalEntityService : ILegalEntityService
    {
        private readonly ILegalEntityRepository _repository;
        public LegalEntityService(ILegalEntityRepository repository)
        {
            _repository = repository;
        }
        public List<ResponseViewModel> Delete(int Id)
        {
            return _repository.Delete(Id);
        }

       

        //public async Task<List<LegalEntityViewModel>> GetLegal_Entity()
        //{
        //    return await _repository.GetLegal_Entity();
        //}

        public async Task<bool> CheckNameExist(string name)
        {
            if (await _repository.CheckNameExist(name))
                return true;
            return false;
        }

        public async Task<long> UpsertLegalEntity(LegalEntityViewModel legalEntityViewModel)
        {
            return await _repository.UpsertLegalEntity(legalEntityViewModel);
        }

        public async Task<Tuple<List<LegalEntityViewModel>, long>> GetLegalEntity_All(PageParam pageParam, string search)
        {
            return await _repository.GetLegalEntity_All(pageParam, search);
        }
        

        public async Task<List<LegalEntityViewModel>> GetLegalEntityListALL()
        {
            return await _repository.GetLegalEntityListALL();
        }


    }
}
