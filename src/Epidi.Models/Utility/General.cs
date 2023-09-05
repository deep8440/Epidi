using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.Utility
{
    public static class General
    {
        
        public const string ContentType = "applicaton/json";

        public const string AccountCreated = "Account {0} created successfully";
        public const string AccountUpdated = "Account {0} updated successfully";
        public const string AccountActivated = "Account {0} activated successfully";
        public const string AccountDeleted = "Account {0} deleted successfully";

        #region CommonMessage
        public const string NoDataFound = "{0} not found";
        public const string Incorrect = "{0} incorrect";
        public const string Unauthorized = "You are unauthorized to use this feature";
        public const string Forbidden = "You are not allowed to use this application";
        public const string BadRequest = "Please check your request format";
        public const string OperationNotDone = "Something went wrong, please try again after some time";
        public const string Success = "Success";
        public const string NotificationNotSent = "Fail";
        public const string Duplicate = "Record already exists.";
        public const string Invalid = "InValid Workflow.";
        public const string PasswordMismatch = "Password mismatch";
        public const string OldPasswordNotFound = "Old Password not match";
        public const string Inserted = "Personal Information Inserted";
        public const string Updated = "Personal Information Updated";
        public const string NoAccountFound = "You are not registred with us";
        public const string GreaterthenZero = "Please enter countryId greater then 0";
        public const string OTPNotMatched = "Oops! Wrong OTP. Please resend or try again";

        #endregion
    }
}
