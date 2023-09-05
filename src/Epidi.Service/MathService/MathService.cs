using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.SkipRule;
using Epidi.Repository.MathRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MathService
{
    public class MathService : IMathService
    {
        private readonly IMathRepository _repository;

        public MathService(IMathRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> AddMath(SkipRuleModelView Math)
        {
            return await _repository.AddMath(Math);
        }

        public async Task<long> EditMath(SkipRuleModelView Math)
        {
            return await _repository.EditMath(Math);
        }

        
        public Tuple<List<SkipRuleModelView>, long> GetMath(PageParam pageParam, string search)
        {
           return _repository.GetMath(pageParam, search);
        }



        public List<ResponseViewModel> RemoveMath(int Id)
        {
            return _repository.RemoveMath(Id);
        }

      
        public async Task<SkipRuleModelView> GetMathById(int id)
        {
            return await _repository.GetMathById(id);
        }

        public async Task<long> SkipRule_InsertByImport(DataTable dataTable)
        {
            return await _repository.SkipRule_InsertByImport(dataTable);
        
        }

        public async Task<ResponseViewModel> CopyMathRule(int Id)
        {
            return await _repository.CopyMathRule(Id);
        }

    }
}
