using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProductsServer.DTO;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using ProductsServer.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsServer.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        private ProductsDBContext Context { get; set; }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                Context = httpContext.RequestServices.GetRequiredService<ProductsDBContext>();
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode httpStatusCode, string message)
        {
            _logger.LogError(ex.Message);

            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            ErrorDTO errorDTO = new()
            {
                Message = message,
                StatusCode = (int)httpStatusCode,
            };

            string result = JsonSerializer.Serialize(errorDTO);

            await response.WriteAsJsonAsync(result);

            string idError = $"{DateTime.UtcNow.AddHours(3):dd.MM.yyyy_HH:mm:ss:ff}_{ex.GetType()}";
            byte[] error = Encoding.Unicode.GetBytes(ex.ToString());

            await Context.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO `LogErrors` VALUES({idError}, {"server"}, {error})");
        }
    }
}
