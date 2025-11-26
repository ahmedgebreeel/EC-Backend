using System.Net;
using System.Text.Json;

namespace Presentation.API.Midllewares
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        //public string? Details { get; set; } // optional (for dev)
    }
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _env);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, IWebHostEnvironment env)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = new ApiError
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                //Details = env.IsDevelopment() ? ex.StackTrace : null
            };

            var result = JsonSerializer.Serialize(error);
            return context.Response.WriteAsync(result);
        }
    }
}
