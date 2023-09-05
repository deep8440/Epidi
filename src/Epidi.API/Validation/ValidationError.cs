using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Epidi.API.Validation
{
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string ErrorMessage { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            ErrorMessage = message;
        }

    }
    public class ValidationResultModel
    {
        public string Message { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation Failed";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }

        public ValidationResultModel(List<ValidationError> errors)
        {
            Errors = errors;
        }
    }

    //
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
