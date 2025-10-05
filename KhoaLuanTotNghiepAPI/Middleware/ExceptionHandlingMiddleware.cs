namespace KhoaLuanTotNghiepAPI.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access attempt");
                await HandleExceptionAsync(context, ex, StatusCodes.Status401Unauthorized, "Unauthorized");
            }
            catch (ForbiddenException ex)
            {
                _logger.LogWarning(ex, "Forbidden access attempt");
                await HandleExceptionAsync(context, ex, StatusCodes.Status403Forbidden, "Forbidden");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Resource not found");
                await HandleExceptionAsync(context, ex, StatusCodes.Status404NotFound, "Not Found");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error");
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, "Validation Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string title)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                succeeded = false,
                code = statusCode,
                messages = new List<string> { exception.Message }
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }




}
