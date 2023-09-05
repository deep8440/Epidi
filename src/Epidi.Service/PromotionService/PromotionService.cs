using Epidi.Models.Model;
using Epidi.Models.Paging;
using Epidi.Models.ViewModel.Common;
using Epidi.Models.ViewModel.RuleCondition;
using Epidi.Repository.PromotionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Service.PromotionService
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        public PromotionService(IPromotionRepository promotionRepository) 
        { 
            _promotionRepository = promotionRepository;
        }
        public async Task<List<RuleConditionViewModel_Dt>> GetRuleConditions(int Id)
        {
            return await _promotionRepository.GetRuleConditions(Id);
        }
        public async Task<ResponseViewModel> UpsertPromotion(Promotion promotion)
        {
            return await _promotionRepository.UpsertPromotion(promotion);
        }
        public async Task<ResponseViewModel> UpsertPromotionRuleWithRuleCondition(Promotion promotion)
        {
            return await _promotionRepository.UpsertPromotionRuleWithRuleCondition(promotion);
        }
        public async Task<ResponseViewModel> UpsertRuleConditions(Promotion promotion)
        {
            return await _promotionRepository.UpsertRuleConditions(promotion);
        }
        public ResponseViewModel DeleteCreditPromotion(int Id)
        {
            return _promotionRepository.DeleteCreditPromotion(Id);
        }
        public async Task<ResponseViewModel> UpsertReferFriendPromotion(ReferPromotion promotion)
        {
            return await _promotionRepository.UpsertReferFriendPromotion(promotion);
        }
        public async Task<ResponseViewModel> UpsertFreeAssetPromotion(FreeAssetPromotion promotion)
        {
            return await _promotionRepository.UpsertFreeAssetPromotion(promotion);
        }
        public Tuple<List<ReferPromotion>, long> refPromotions(PageParam pageParam, string search, int Id)
        {
            return _promotionRepository.refPromotions(pageParam, search, Id);
        }
        public Tuple<List<CreditPromotion>, long> creditPromotions(PageParam pageParam, string search, int Id)
        {
            return _promotionRepository.creditPromotions(pageParam, search, Id);
        }
        public Tuple<List<FreeAssetPromotion>, long> freeAssetPromotions(PageParam pageParam, string search, int Id)
        {
            return _promotionRepository.freeAssetPromotions(pageParam, search, Id);
        }
        public Promotion GetPromotion(int id)
        {
            return _promotionRepository.GetPromotion(id);
        }
        public Tuple<List<Promotion>, long> GetAllPromotion(PageParam pageParam, string search)
        {
            return  _promotionRepository.GetAllPromotion(pageParam, search);
        }

        public ResponseViewModel DeleteFreeAssetPromotion(int Id)
        {
            return _promotionRepository.DeleteFreeAssetPromotion(Id);
        }

        public ResponseViewModel DeleteReferFriendPromotion(int Id)
        {
            return _promotionRepository.DeleteReferFriendPromotion(Id);
        }

        public ResponseViewModel DeleteVoucherPromotion(int Id)
        {
            return _promotionRepository.DeleteVoucherPromotion(Id);
        }

        public Task<ResponseViewModel> UpsertVoucherPromotion(VoucherPromotion promotion)
        {
            return _promotionRepository.UpsertVoucherPromotion(promotion);
        }

        public Tuple<List<VoucherPromotion>, long> voucherPromotions(PageParam pageParam, string search, int Id)
        {
            return _promotionRepository.voucherPromotions(pageParam,search,Id);
        }

        public ResponseViewModel DeleteRebatePromotion(int Id)
        {
            return _promotionRepository.DeleteRebatePromotion(Id);
        }

        public async Task<ResponseViewModel> UpsertRebatePromotion(RebatePromotion promotion)
        {
            return await _promotionRepository.UpsertRebatePromotion(promotion);
        }

        public Tuple<List<RebatePromotion>, long> rebatePromotions(PageParam pageParam, string search, int Id)
        {
            return _promotionRepository.rebatePromotions(pageParam, search,Id);
        }
    }
}
