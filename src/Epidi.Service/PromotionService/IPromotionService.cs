using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PromotionService
{
    public interface IPromotionService
    {
        Task<ResponseViewModel> UpsertPromotion(Promotion promotion);
        Task<ResponseViewModel> UpsertPromotionRuleWithRuleCondition(Promotion promotion);
        Task<ResponseViewModel> UpsertRuleConditions(Promotion promotion);
        Task<List<RuleConditionViewModel_Dt>> GetRuleConditions(int Id);
        ResponseViewModel DeleteCreditPromotion(int Id);
        ResponseViewModel DeleteFreeAssetPromotion(int Id);
        ResponseViewModel DeleteReferFriendPromotion(int Id);
        ResponseViewModel DeleteVoucherPromotion(int Id);
        ResponseViewModel DeleteRebatePromotion(int Id);
        Task<ResponseViewModel> UpsertReferFriendPromotion(ReferPromotion promotion);
        Task<ResponseViewModel> UpsertFreeAssetPromotion(FreeAssetPromotion promotion);
        Task<ResponseViewModel> UpsertVoucherPromotion(VoucherPromotion promotion);
        Task<ResponseViewModel> UpsertRebatePromotion(RebatePromotion promotion);
        Promotion GetPromotion(int id);
        Tuple<List<Promotion>, long> GetAllPromotion(PageParam pageParam, string search);
        Tuple<List<CreditPromotion>, long> creditPromotions(PageParam pageParam, string search, int Id);
        Tuple<List<FreeAssetPromotion>, long> freeAssetPromotions(PageParam pageParam, string search, int Id);
        Tuple<List<ReferPromotion>, long> refPromotions(PageParam pageParam, string search, int Id);
        Tuple<List<VoucherPromotion>, long> voucherPromotions(PageParam pageParam, string search, int Id);
        Tuple<List<RebatePromotion>, long> rebatePromotions(PageParam pageParam, string search, int Id);
    }
}
