using Microsoft.AspNetCore.Http;

namespace QuarterEstate.APIS.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "a bad request , you have made",
                401 => "authorized , u r not",
                403 => "access is forbidden , it is",
                404 => "resource was not found",
                405 => "method not allowed , this is",
                408 => "request timeout , it has",
                409 => "conflict occurred , there is",
                415 => "unsupported media type , this is",
                422 => "unprocessable entity , it is",
                429 => "too many requests , sent you have",
                500 => "server error , happened it has",
                502 => "bad gateway , it is",
                503 => "service unavailable , currently it is",
                504 => "gateway timeout , it did",
                _ => null
            };
            return message;
        }
    }
}
