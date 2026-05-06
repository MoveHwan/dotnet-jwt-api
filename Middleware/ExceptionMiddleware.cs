using System.Net;
using System.Text.Json;
using Test.DTOs.Common;

namespace Test.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleException(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleException(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = ApiResponse<List<string>>.FailResponse("Validation failed", errors);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await HandleException(context, HttpStatusCode.InternalServerError, "서버 오류");
            }
        }

        private static async Task HandleException(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                message,
                status = context.Response.StatusCode
                
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}