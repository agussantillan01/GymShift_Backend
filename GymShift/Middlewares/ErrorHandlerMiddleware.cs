using Business.Exceptions;
using Domain.Wrappers;
using Newtonsoft.Json;
using System.Net;

namespace GymShift.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var responseModel = new Response<string>
                {
                    Succeeded = false,
                    Message = "Ha ocurrido un error inesperado. Por favor, Comuniquese."
                };

                switch (error)
                {
                    case ApiException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = ex.Message;
                        _logger.LogWarning($"API Error: {ex.Message}");
                        break;

                    case ValidationException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Message = "Se encontraron uno o más errores.";
                        responseModel.Errors = ex.Errors; 
                        _logger.LogWarning($"Validation Error: {ex.Message} - {JsonConvert.SerializeObject(ex.Errors)}");
                        break;

                    case KeyNotFoundException ex:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Message = "El recurso solicitado no fue encontrado.";
                        _logger.LogInformation($"Not Found: {ex.Message}");
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogError(error, "Unhandled Exception: " + error.Message);
                        break;
                }

                var result = JsonConvert.SerializeObject(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
