using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Models.ExceptionHelper
{

    /// <summary>
    /// EntityNotFoundException, ex:-
    /// Workflow details not found
    /// Status Code-409 (Conflict)
    /// </summary>
    public class EntityNotFoundException : Exception
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
        public EntityNotFoundException(string fieldName, string fieldValue, string errorCode, string errorMessage)
            : base($"{fieldValue} {fieldName} not found")
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
        public EntityNotFoundException(string fieldName, string fieldValue, string errorCode, HttpStatusCode statusCode)
            : base($"{fieldValue} {fieldName} not found")
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.ErrorCode = errorCode;
            this.StatusCode = statusCode;
        }
    }
}
