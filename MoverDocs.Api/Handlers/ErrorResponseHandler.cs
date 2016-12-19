using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MoverDocs.Api.ErrorHandling;
using MoverSoft.Common.Extensions;

namespace MoverDocs.Api.Handlers
{
    public class ErrorResponseHandler : BaseHandler
    {
        public ErrorResponseHandler(HttpConfiguration httpConfiguration) : base(httpConfiguration)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception exception)
            {
                if (exception.IsFatal())
                {
                    throw;
                }

                ErrorResponse errorResponse = null;
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

                if (exception.GetType() == typeof(ErrorResponseMessageException))
                {
                    var errorException = exception as ErrorResponseMessageException;
                    errorResponse = new ErrorResponse
                    {
                        Error = new ErrorResponseMessage
                        {
                            Message = errorException.Message,
                            Code = errorException.ErrorCode,
                            Exception = errorException.InnerException
                        }
                    };

                    statusCode = errorException.HttpStatus;
                }
                else
                {
                    errorResponse = new ErrorResponse
                    {
                        Error = new ErrorResponseMessage
                        {
                            Message = "An error occured",
                            Code = ErrorResponseCode.InternalServerError,
                            Exception = exception
                        }
                    };
                }

                return request.CreateResponse(
                    statusCode: statusCode,
                    value: errorResponse,
                    configuration: this.HttpConfiguration);
            }
        }
    }
}