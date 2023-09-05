using Epidi.Notifications;
using Epidi.Notifications.Interface;
using Epidi.Repository.AnswerRepository;
using Epidi.Repository.ApplicationUserRepository;
using Epidi.Repository.ConnectionProvider;
using Epidi.Repository.CountryRepository;
using Epidi.Repository.DeviceTokensRepository;
using Epidi.Repository.InstrumentRepository;
using Epidi.Repository.IBCommisionMarkUpSettingInstrumentWise;
using Epidi.Repository.IBRepository;
using Epidi.Repository.MobileUserTracking;
using Epidi.Repository.PayInoutRequestRepository;
using Epidi.Repository.QuestionsRepository;
using Epidi.Repository.UsersDocumentsRepository;
using Epidi.Repository.UsersRepository;
using Epidi.Repository.UsersTransactionsRepository;
using Epidi.Service.AnswerService;
using Epidi.Service.ApplicationUserService;
using Epidi.Service.CountryService;
using Epidi.Service.DeviceTokensService;
using Epidi.Service.InstrumentService;
using Epidi.Service.IBCommisionMarkUpSettingInstrumentWise;
using Epidi.Service.IBService;
using Epidi.Service.MobileUserTrackingService;
using Epidi.Service.PayInoutRequestService;
using Epidi.Service.QuestionService;
using Epidi.Service.TokenService;
using Epidi.Service.UsersDocumentsService;
using Epidi.Service.UsersService;
using Epidi.Service.UsersTransactionsService;
using IdentityServer4.AccessTokenValidation;
using Epidi.Service.TradeTimingService;
using Epidi.Repository.TradeTimingRepository;
using Epidi.Service.QuoteTimingService;
using Epidi.Repository.QuoteTimingRepository;
using Epidi.Service.LPWiseInstrumentPriorityService;
using Epidi.Repository.LPWiseInstrumentPriorityRepository;
using Epidi.Service.MarkupRuleVsUserIdService;
using Epidi.Repository.MathRepository;
using Epidi.Service.MathService;
using Epidi.Repository.MarkupRuleVsUserIdRepository;
using Epidi.Service.TermsConditionService;
using Epidi.Repository.TermsConditionRepository;
using Epidi.Service.StepsService;
using Epidi.Repository.StepsRepository;
using Epidi.Service.CountryWiseDocumentService;
using Epidi.Repository.CountryWiseDocument;
using Epidi.Service.UserQuestionAnswerService;
using Epidi.Repository.UserQuestionAnswerRepository;
using Epidi.Service.PartnerSetting;
using Epidi.Repository.PartnerSettingRepository;
using Epidi.Repository.CRMRepository;
using Epidi.Service.CRMService;

namespace Epidi.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = configuration["Jwt:Issuer"];
                options.RequireHttpsMetadata = false;
            });
        }

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

            service.AddTransient<IPayInoutRequestService, PayInoutRequestService>();
            service.AddTransient<IPayInoutRequestRepository, PayInoutRequestRepository>();

            service.AddTransient<IMarkupRuleVsUserIdService, MarkupRuleVsUserIdService>();
            service.AddTransient<ITradeTimingRepository, TradeTimingRepository>();

            service.AddTransient<ILPWiseInstrumentPriorityService, LPWiseInstrumentPriorityService>();
            service.AddTransient<ILPWiseInstrumentPriorityRepository, LPWiseInstrumentPriorityRepository>();

            service.AddTransient<IIBService, IBService>();
            service.AddTransient<IIBRepository, IBRepository>();

            service.AddTransient<IIBCommisionMarkUpSettingInstWiseService, IBCommisionMarkUpSettingInstWiseService>();
            service.AddTransient<IIBCommisionMarkUpSettingInstrumentWise, IBCommisionMarkUpSettingInstrumentWise>();

            service.AddScoped<IMessages, Messages>();

            service.AddTransient<IQuoteTimingService, QuoteTimingService>();
            service.AddTransient<IQuoteTimingRepository, QuoteTimingRepository>();

            service.AddTransient<IMathService, MathService>();
            service.AddTransient<IMathRepository, MathRepository>();

            service.AddTransient<IMarkupRuleVsUserIdService, MarkupRuleVsUserIdService>();
            service.AddTransient<IMarkupRuleVsUserIdRepository, MarkupRuleVsUserIdRepository>();

            service.AddTransient<ITermsConditionService, TermsConditionService>();
            service.AddTransient<ITermsConditionRepository, TermsConditionRepository>();

            service.AddTransient<IStepsService, StepsService>();
            service.AddTransient<IStepsRepository, StepsRepository>();

            service.AddTransient<ICountryWiseDocumentService, CountryWiseDocumentService>();
            service.AddTransient<ICountryWiseDocumentRepository, CountryWiseDocumentRepository>();

            service.AddTransient<IInstrumentService, InstrumentService>();
            service.AddTransient<IInstrumentRepository, InstrumentRepository>();

            service.AddTransient<IUserQuestionAnswerService, UserQuestionAnswerService>();
            service.AddTransient<IUserQuestionAnswerRepository, UserQuestionAnswerRepository>();

            service.AddTransient<IPartnerSettingService, PartnerSettingService>();
            service.AddTransient<IPartnerSettingRepository, PartnerSettingRepository>();

            return service;
        }
    }
}
