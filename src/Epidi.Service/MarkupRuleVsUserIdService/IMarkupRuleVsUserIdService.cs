using Epidi.Models.Paging;
using Epidi.Models.ViewModel.MarkupRuleVsUserId;
using Epidi.Models.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.MarkupRuleVsUserIdService
{
    public interface IMarkupRuleVsUserIdService
    {
        Task<List<MarkupRuleVsUserId>> GetMarkupRuleVsUserId();
        Task<long> AddMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId);
        Task<long> RemoveMarkupRuleVsUserId(int id);
        Task<MarkupRuleVsUserId> GetMarkupRuleVsUserIdById(int id);
        Task<long> EditMarkupRuleVsUserId(MarkupRuleVsUserId MarkupRuleVsUserId);
        Tuple<List<MarkupRuleVsUserId>, long> QuoteMarkUpRule_GetAll(PageParam pageParam, string search);
    }
}
