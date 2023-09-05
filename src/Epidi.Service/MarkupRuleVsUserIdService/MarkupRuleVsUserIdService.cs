using Epidi.Models.Paging;
using Epidi.Models.ViewModel.MarkupRuleVsUserId;
using Epidi.Models.ViewModel.Users;
using Epidi.Repository.MarkupRuleVsUserIdRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MarkupRuleVsUserIdService
{
    public class MarkupRuleVsUserIdService : IMarkupRuleVsUserIdService
    {
        private readonly IMarkupRuleVsUserIdRepository _repository;

        public MarkupRuleVsUserIdService(IMarkupRuleVsUserIdRepository repository)
        {
            _repository = repository;
        }
        public async Task<long> AddMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId)
        {
            return await _repository.AddMarkupRuleVsUserId(MarkupRuleVsUserId);
        }

        public async Task<long> EditMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId)
        {
            return await _repository.EditMarkupRuleVsUserId(MarkupRuleVsUserId);
        }

        
        public Task<List<MarkupRuleVsUserId>> GetMarkupRuleVsUserId()
        {
           return _repository.GetMarkupRuleVsUserId();
        }

        public async Task<long> RemoveMarkupRuleVsUserId(int Id)
        {
            return await _repository.RemoveMarkupRuleVsUserId(Id);
        }
        public async Task<MarkupRuleVsUserId> GetMarkupRuleVsUserIdById(int Id)
        {
            return await _repository.GetMarkupRuleVsUserIdById(Id);
        }

        public Tuple<List<MarkupRuleVsUserId>, long> QuoteMarkUpRule_GetAll(PageParam pageParam, string search)
        {
            return _repository.QuoteMarkUpRule_GetAll(pageParam, search);
        }

    }
}
