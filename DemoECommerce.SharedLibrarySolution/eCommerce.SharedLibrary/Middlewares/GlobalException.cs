using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace eCommerce.SharedLibrary.Middlewares
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string title = "Error";
            int status = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";

            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Unauthorized";
                    status = StatusCodes.Status401Unauthorized;
                    message = "You are not authenticated";
                    await ModifyHeader(context, title, status, message);
                }

                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Too many requests";
                    status = StatusCodes.Status429TooManyRequests;
                    message = "Too many requests that exceed rate limiting";
                    await ModifyHeader(context, title, status, message);
                }

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbidden";
                    status = StatusCodes.Status403Forbidden;
                    message = "You don't have enough authorization";
                    await ModifyHeader(context, title, status, message);
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of Time";
                    status = StatusCodes.Status408RequestTimeout;
                    message = "Request time out...try again";
                }
                await ModifyHeader(context, title, status, message);
            }
        }

        private async Task ModifyHeader(HttpContext context, string title, int status, string message)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Title = title,
                Detail = message,
                Status = status
            }), CancellationToken.None);
            return;
        }
    }
}
