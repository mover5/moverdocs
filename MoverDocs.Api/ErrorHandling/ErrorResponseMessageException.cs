using System;
using System.Collections.Generic;
using System.Net;

namespace MoverDocs.Api.ErrorHandling
{
    public class ErrorResponseMessageException : Exception
    {
        public HttpStatusCode HttpStatus { get; private set; }

        public ErrorResponseCode ErrorCode { get; private set; }

        public IDictionary<string, string> ResponseHeaders { get; set; }

        public ErrorResponseMessageException(HttpStatusCode httpStatus, ErrorResponseCode errorCode, string errorMessage, Exception innerException = null, IDictionary<string, string> headers = null)
            : base(errorMessage, innerException)
        {
            this.HttpStatus = httpStatus;
            this.ErrorCode = errorCode;
            this.ResponseHeaders = headers;
        }
    }
}