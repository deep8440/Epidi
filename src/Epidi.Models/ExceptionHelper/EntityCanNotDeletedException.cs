using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ExceptionHelper
{
    /// <summary>
    /// EntityCanNotDeletedException
    /// Active Workflow can not deleted
    /// Status Code
    /// </summary>
    public class EntityCanNotDeletedException : Exception
    {
        /// <summary>
        /// fieldName
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// statusCode
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// fieldName
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// StatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// DuplicateFoundException
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public EntityCanNotDeletedException(string fieldName, string fieldValue, string errorCode, string errorMessage)
            : base($"{fieldValue} {fieldName} already exists")
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// DuplicateFoundException
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public EntityCanNotDeletedException(string fieldName, string fieldValue, string errorCode, HttpStatusCode statusCode)
            : base($"{fieldValue} {fieldName} already exists")
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.ErrorCode = errorCode;
            this.StatusCode = statusCode;
        }
    }
}
