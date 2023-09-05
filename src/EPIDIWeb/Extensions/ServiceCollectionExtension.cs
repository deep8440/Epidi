using Epidi.Notifications;
using Epidi.Notifications.Interface;
using Epidi.Repository.AnswerRepository;
using Epidi.Repository.ApplicationUserRepository;
using Epidi.Repository.BDM;
using Epidi.Repository.ConnectionProvider;
using Epidi.Repository.CountryRepository;
using Epidi.Repository.DeviceTokensRepository;
using Epidi.Repository.InstrumentRepository;
using Epidi.Repository.IBCommisionMarkUpSettingInstrumentWise;
using Epidi.Repository.IBRepository;
using Epidi.Repository.LeverageRulesRepository;
using Epidi.Repository.MobileUserTracking;
using Epidi.Repository.PayInoutRequestRepository;
using Epidi.Repository.QuestionsRepository;
using Epidi.Repository.QuoteMarkUpRuleRepository;
using Epidi.Repository.StepsRepository;
using Epidi.Repository.UsersDocumentsRepository;
using Epidi.Repository.UsersRepository;
using Epidi.Repository.UsersTransactionsRepository;
using Epidi.Service;
using Epidi.Service.AnswerService;
using Epidi.Service.ApplicationUserService;
using Epidi.Service.BDM;
using Epidi.Service.CountryService;
using Epidi.Service.DeviceTokensService;
using Epidi.Service.InstrumentService;
using Epidi.Service.IBCommisionMarkUpSettingInstrumentWise;
using Epidi.Service.IBService;
using Epidi.Service.LeverageRulesService;
using Epidi.Service.MobileUserTrackingService;
using Epidi.Service.PayInoutRequestService;
using Epidi.Service.QuestionService;
using Epidi.Service.QuoteMarkUpRuleService;
using Epidi.Service.StepsService;
using Epidi.Service.TokenService;
using Epidi.Service.UsersDocumentsService;
using Epidi.Service.UsersService;
using Epidi.Service.UsersTransactionsService;
using Epidi.Service.TradeTimingService;
using Epidi.Repository.TradeTimingRepository;
using Epidi.Repository.QuoteTimingRepository;
using Epidi.Service.QuoteTimingService;
using Epidi.Service.LPWiseInstrumentPriorityService;
using Epidi.Repository.LPWiseInstrumentPriorityRepository;
using Epidi.Repository.MarkupRuleVsUserIdRepository;
using Epidi.Service.MarkupRuleVsUserIdService;
using Epidi.Service.MasterLPService;
using Epidi.Repository.MasterLP;
using Epidi.Service.RuleInstrumentService;
using Epidi.Repository.RuleInstrumentRepository;
using Epidi.Service.RuleLpPriorityService;
using Epidi.Repository.RuleLpPriorityRepository;
using Epidi.Service.RuleConditionService;
using Epidi.Repository.RuleConditionRepository;
using Epidi.Service.MathService;
using Epidi.Repository.MathRepository;
using Epidi.Service.TermsConditionService;
using Epidi.Repository.TermsConditionRepository;
using Epidi.Repository.CountryWiseDocument;
using Epidi.Service.CountryWiseDocumentService;
using Epidi.Service.LegalEntityService;
using Epidi.Repository.LegalEntity;
using Epidi.Service.MarginService;
using Epidi.Repository.MarginRepository;
using Epidi.Service.TradeRuleService;
using Epidi.Repository.TradeRuleRepository;
using Epidi.Service.PositionTypeService;
using Epidi.Repository.PositionTypeRepository;
using Epidi.Service.TradeRuleUniversalValuesService;
using Epidi.Repository.TradeRuleUniversalValuesRepository;
using Epidi.Service.MarginRuleService;
using Epidi.Repository.MarginRuleRepository;

using Epidi.Service.CorporateActionsService;
using Epidi.Repository.CorporateActionsRepository;

using Epidi.Repository.SwapRuleRepository;
using Epidi.Service.SwapRuleService;
using Epidi.Repository.CommissionRuleRepository;
using Epidi.Service.CommissionRuleService;
using Epidi.Repository.PromotionRepository;
using Epidi.Service.PromotionService;
using Epidi.Service.IBLimitService;
using Epidi.Repository.IBLimitRepository;
using Epidi.Repository.OrdersRepository;
using Epidi.Service.OrdersService;
using Epidi.Service.CRMService;
using Epidi.Repository.CRMRepository;

namespace Epidi.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection CreateDependencies(this IServiceCollection service)
        {
            service.AddTransient<IConnectionProvider, ConnectionProvider>();
            service.AddHttpClient<HttpClient>();
            service.AddTransient<ICountryService, CountryService>();
            service.AddTransient<ICountryRepository, CountryRepository>();           

            service.AddTransient<IApplicationUserService, ApplicationUserService>();
            service.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            service.AddTransient<ITokenService, TokenService>();

            service.AddTransient<IQuestionService, QuestionService>();
            service.AddTransient<IQuestionRepository, QuestionRepository>();

            service.AddTransient<IAnswerService, AnswerService>();
            service.AddTransient<IAnswerRepository, AnswerRepository>();

            service.AddTransient<IMobileUserTrackingService, MobileUserTrackingService>();
            service.AddTransient<IMobileUserTrackingRepository, MobileUserTrackingRepository>();

            service.AddTransient<IDeviceTokensService, DeviceTokensService>();
            service.AddTransient<IDeviceTokensRepository, DeviceTokensRepository>();

            service.AddTransient<IUsersDocumentsService, UsersDocumentsService>();
            service.AddTransient<IUsersDocumentsRepository, UsersDocumentsRepository>();

            service.AddTransient<IUsersTransactionsService, UsersTransactionsService>();
            service.AddTransient<IUsersTransactionsRepository, UsersTransactionsRepository>();

            service.AddTransient<IUsersService, UsersService>();
            service.AddTransient<IUsersRepository, UsersRepository>();

            service.AddTransient<ICRMService, CRMService>();
            service.AddTransient<ICRMRepository, CRMRepository>();

            service.AddTransient<ILeverageRulesService, LeverageRulesService>();
            service.AddTransient<ILeverageRulesRepository, LeverageRulesRepository>();

            service.AddTransient<IPayInoutRequestService, PayInoutRequestService>();
            service.AddTransient<IPayInoutRequestRepository, PayInoutRequestRepository>();

            service.AddTransient<IStepsService, StepsService>();
            service.AddTransient<IStepsRepository, StepsRepository>();

            service.AddTransient<ITradeTimingService, TradeTimingService>();
            service.AddTransient<ITradeTimingRepository, TradeTimingRepository>();
            
            service.AddTransient<IMarkupRuleVsUserIdService, MarkupRuleVsUserIdService>();
            service.AddTransient<IMarkupRuleVsUserIdRepository, MarkupRuleVsUserIdRepository>();

            service.AddTransient<ILPWiseInstrumentPriorityService, LPWiseInstrumentPriorityService>();
            service.AddTransient<ILPWiseInstrumentPriorityRepository, LPWiseInstrumentPriorityRepository>();

            service.AddTransient<IBDMService, BDMService>();
            service.AddTransient<IBDMRepository, BDMRepository>();

            service.AddTransient<IIBService, IBService>();
            service.AddTransient<IIBRepository, IBRepository>();

            service.AddTransient<IIBLimitService, IBLimitService>();
            service.AddTransient<IIBLimitRepository, IBLimitRepository>();

            service.AddTransient<IQuoteMarkUpRuleService, QuoteMarkUpRuleService>();
            service.AddTransient<IQuoteMarkUpRuleRepository, QuoteMarkUpRuleRepository>();

            service.AddTransient<IIBCommisionMarkUpSettingInstWiseService, IBCommisionMarkUpSettingInstWiseService>();
            service.AddTransient<IIBCommisionMarkUpSettingInstrumentWise, IBCommisionMarkUpSettingInstrumentWise>();

            service.AddTransient<IQuoteTimingService, QuoteTimingService>();
            service.AddTransient<IQuoteTimingRepository, QuoteTimingRepository>();

            service.AddTransient<IMathService, MathService>();
            service.AddTransient<IMathRepository, MathRepository>();

            service.AddTransient<IMasterLPService, MasterLPService>();
            service.AddTransient<IMasterLPRepository, MasterLPRepository>();

            service.AddTransient<IRuleInstrumentService, RuleInstrumentService>();
            service.AddTransient<IRuleInstrumentRepository, RuleInstrumentRepository>();

            service.AddTransient<IRuleLpPriorityService, RuleLpPriorityService>();
            service.AddTransient<IRuleLpPriorityRepository, RuleLpPriorityRepository>();

            service.AddTransient<IRuleConditionService, RuleConditionService>();
            service.AddTransient<IRuleConditionRepository, RuleConditionRepository>();            
            
            service.AddTransient<IInstrumentService, InstrumentService>();
            service.AddTransient<IInstrumentRepository, InstrumentRepository>();

            service.AddTransient<ICountryWiseDocumentService, CountryWiseDocumentService>();
            service.AddTransient<ICountryWiseDocumentRepository, CountryWiseDocumentRepository>();

            service.AddTransient<IMessages, Messages>();
            service.AddTransient<ILegalEntityService, LegalEntityService>();
            service.AddTransient<ILegalEntityRepository, LegalEntityRepository>();

            service.AddTransient<ITermsConditionService, TermsConditionService>();
            service.AddTransient<ITermsConditionRepository, TermsConditionRepository>();

            service.AddTransient<IStepsService, StepsService>();
            service.AddTransient<IStepsRepository, StepsRepository>();

            service.AddTransient<IMarginService, MarginService>();
            service.AddTransient<IMarginRepository, MarginRepository>();

            service.AddTransient<IInstrumentSpecificationRepository, InstrumentSpecificationRepository>();
            service.AddTransient<IInstrumentSpecificationService, InstrumentSpecificationService>();

            service.AddTransient<ITradeRuleService, TradeRuleService>();
            service.AddTransient<ITradeRuleRepository, TradeRuleRepository>();

            service.AddTransient<IPositionTypeService, PositionTypeService>();
            service.AddTransient<IPositionTypeRepository, PositionTypeRepository>();

            service.AddTransient<ITradeRuleUniversalValuesService, TradeRuleUniversalValuesService>();
            service.AddTransient<ITradeRuleUniversalValuesRepository, TradeRuleUniversalValuesRepository>();

            service.AddTransient<IMarginRuleService, MarginRuleService>();
            service.AddTransient<IMarginRuleRepository, MarginRuleRepository>();


            service.AddTransient<ICorporateActionsService, CorporateActionsService>();
            service.AddTransient<ICorporateActionsRepository, CorporateActionsRepository>();

            service.AddTransient<ISwapRuleRepository, SwapRuleRepository>();
            service.AddTransient<ISwapRuleService, SwapRuleService>();

            service.AddTransient<ICommissionRuleRepository, CommissionRuleRepository>();
            service.AddTransient<ICommissionRuleService, CommissionRuleService>();

            service.AddTransient<IPromotionRepository, PromotionRepository>();
            service.AddTransient<IPromotionService, PromotionService>();

            service.AddTransient<IOrderRepository, OrderRepository>();
            service.AddTransient<IOrderService, OrderService>();


            return service;
        }
    }
}
