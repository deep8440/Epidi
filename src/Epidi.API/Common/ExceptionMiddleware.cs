using Epidi.Models.Utility;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using Epidi.Models.ExceptionHelper;

namespace Epidi.API.Common
{

    /// <summary>
    /// Global Exception handler
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next">Process an HTTP request.</param>
        /// <param name="logger">Logging the error</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DuplicateFoundException duplicateException)
            {
                _logger.LogError($"FileName: {duplicateException.FieldName}");
                _logger.LogError($"FieldValue: {duplicateException.FieldValue}");
                await HandleExceptionAsync(httpContext, duplicateException);
            }
            catch (EntityCanNotDeletedException entityCanNotDeleted)
            {
                _logger.LogError($"FileName: {entityCanNotDeleted.FieldName}");
                _logger.LogError($"FieldValue: {entityCanNotDeleted.FieldValue}");
                await HandleExceptionAsync(httpContext, entityCanNotDeleted);
            }
            catch (EntityNotFoundException entityNotFoundException)
            {
                _logger.LogError($"FileName: {entityNotFoundException.FieldName}");
                _logger.LogError($"FieldValue: {entityNotFoundException.FieldValue}");
                await HandleExceptionAsync(httpContext, entityNotFoundException);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                _logger.LogError($"Inner Exception: {ex.InnerException}");
                await HandleExceptionAsync(httpContext);
            }
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="context">Handling the HTTP exception</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = General.ContentType;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new Error()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = General.OperationNotDone,
            }.ToString());
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="context">Handling the HTTP exception</param>
        /// <param name="exception">Handling the DuplicateFoundException exception</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, DuplicateFoundException exception)
        {
            context.Response.ContentType = General.ContentType;
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            BaseResponse baseResponse = new BaseResponse();

            baseResponse.ResponseCode = context.Response.StatusCode;
            baseResponse.Error = new List<Error>() {
              new Error()
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                ErrorMessage = exception.Message,
                ErrorCode = exception.ErrorCode
            }
            };
            baseResponse.ResponseMessage = exception.Message;
            var responseBody = JsonConvert.SerializeObject(baseResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return context.Response.WriteAsync(responseBody);
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="context">Handling the HTTP exception</param>
        /// <param name="exception">Handling the DuplicateFoundException exception</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, EntityNotFoundException exception)
        {
            context.Response.ContentType = General.ContentType;
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            BaseResponse baseResponse = new BaseResponse();

            baseResponse.ResponseCode = context.Response.StatusCode;
            baseResponse.Error = new List<Error>() {
              new Error()
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ErrorMessage = exception.Message,
                 ErrorCode = exception.ErrorCode
            }
            };
            baseResponse.ResponseMessage = exception.Message;
            var responseBody = JsonConvert.SerializeObject(baseResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return context.Response.WriteAsync(responseBody);
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="context">Handling the HTTP exception</param>
        /// <param name="exception">Handling the DuplicateFoundException exception</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, EntityCanNotDeletedException exception)
        {
            context.Response.ContentType = General.ContentType;
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            BaseResponse baseResponse = new BaseResponse();

            baseResponse.ResponseCode = context.Response.StatusCode;
            baseResponse.Error = new List<Error>() {
              new Error()
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                ErrorMessage = exception.Message,
                 ErrorCode = exception.ErrorCode
            }
            };
            baseResponse.ResponseMessage = exception.Message;
            var responseBody = JsonConvert.SerializeObject(baseResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return context.Response.WriteAsync(responseBody);
        }
    }

}
