using Epidi.Models.ViewModel.UsersDocuments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ViewModel.Users
{
    public class UsersViewModel
    {
        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string Name { get; set; }
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the Surname value.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the Address value.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Address2 value.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the PostCode value.
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Gets or sets the CountryId value.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the Email value.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Mobile value.
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the CurrentBalance value.
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// Gets or sets the Leverage value.
        /// </summary>
        public string Leverage { get; set; }

        /// <summary>
        /// Gets or sets the IsActive value.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the CreatedAt value.
        /// </summary>
        public int CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedAt value.
        /// </summary>
        public int UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedBy value.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the OnboardingEntity value.
        /// </summary>
        public int OnboardingEntity { get; set; }

        /// <summary>
        /// Gets or sets the LegalEntityName value.
        /// </summary>
        public int LegalEntityName { get; set; }

        /// <summary>
        /// Gets or sets the IBId value.
        /// </summary>
        public String LName { get; set; }

        /// <summary>
        /// Gets or sets the IBId value.
        /// </summary>
        public int IBId { get; set; }

        /// <summary>
        /// Gets or sets the BDMId value.
        /// </summary>
        public int BDMId { get; set; }

        /// <summary>
        /// Gets or sets the Nationlity value.
        /// </summary>
        public int NationlityId { get; set; }
        /// <summary>
        /// Gets or sets the TaxResidence value.
        /// </summary>
        public int TaxResidenceId { get; set; }

        /// <summary>
        /// Gets or sets the ResidentArea value.
        /// </summary>
        public string ResidentArea { get; set; }

        /// <summary>
        /// Gets or sets the IDNumber value.
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// Gets or sets the PEP value.
        /// </summary>
        public bool PEP { get; set; }

        /// <summary>
        /// Gets or sets the TINNumber value.
        /// </summary>
        public string TINNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string CountryCode { get; set; }

        [NotMapped]
        public bool UsersVsRulesIsDeleted { get; set; }
        [NotMapped]
        public int UsersVsRulesId { get; set; }
        public string? UserName { get; set; }
        public string? CustomerId { get; set; }
        public int? MobileCountryCode { get; set; }
        public int Role { get; set; }
        public string VerificationCode { get; set; }
        public bool IsEmailConfirm { get;set; }
        public int StepCompleted { get; set; }
        public string Promocode { get; set; }
        public bool IsTermsConditionAgreed { get;set; }
        public bool RememberMe { get; set; }
        public bool IsDesclaimer { get; set; }
        public bool IsPersonalInfoCompleted { get; set; }
        public bool IsDocumentVerified { get; set; }
        public bool IsTermsConditionStepAgreed { get; set; }
        public decimal Amount { get; set; }
        public int RequestStatusId { get; set; }
        public int RequestBy { get; set; }
        public string RequestType { get; set; }
        public string CustomerNote { get; set; }
        public string AdminNote { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDelete { get; set; }

        public List <UsersDocumentViewModel> UsersDocumentViewModels{ get; set; }

    }

    public class Userpromocodemodel
    {
        public string Promocode { get; set; }
    }
    public class UserFieldsList
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string ForeignKey { get; set; }
    }
    public class UsersFavouriteInsert
    {
        public int UserId { get; set; }
        public int RuleId { get; set; }
        public int MasterInstrumentId { get; set; }
        public int Day { get; set; }
        public int Hours { get; set; }
    }
    
    public class PayoutRequest
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int RequestStatusId { get; set; }
        public int RequestBy { get; set; }
        public string RequestType { get; set; }
        public string CustomerNote { get; set; }
        public string AdminNote { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDelete { get; set; }
    }

    public class PayoutRequestData
    {
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
    }

	public class QuestionAnswerList
	{
		public int Id { get; set; }
		public int UserId { get; set; }
        public string AnswerId { get; set; }
        public int questionId { get; set; }
		public string QuestionInfo { get; set; }
		public string QuesDesc { get; set; }
		public string AnswerDesc { get; set; }
        public int AnsTypeId { get; set; }
	    public string AnsTypeName { get; set; }

}


	public class UsersFavourite
    {
        public int UserId { get; set; }
        public int MasterInstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public string GroupName { get; set; }
        public int LpId { get; set; }
        public int LPInstrumentId { get; set; }
        public string LPInstrumentName { get; set; }
        public string LPName { get; set; }
    }

    public class PersonalInfomation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }

        public string Address2 { get; set; }
        public string ResidentArea { get; set; }

        public string PostCode { get; set; }

        public int MobileCountryCode { get; set; }
        public bool IsPersonalInfoCompleted { get; set; }
        public int NationlityId { get; set; }
        public int TaxResidenceId { get; set; }
        public string IDNumber { get; set; }
        public bool PEP { get; set; }
        public string TINNumber { get; set; }
    }
    public class PaymentRequestList
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int RequestStatusId { get; set; }
        public string Status { get; set; }
        public int RequestBy { get; set; }
        public string UserName { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestType { get; set; }
        public string PayoutRequestTypeName { get; set; }
        public string CustomerNote { get; set; }
        public string AdminNote { get; set; }
    }
    
}
