namespace E_Commerce_API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    Exceptions.NotFoundException => StatusCodes.Status404NotFound,
                    Exceptions.ValidationException => StatusCodes.Status400BadRequest,
                    Exceptions.NotUniqueEmail => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
        }
    }
}
