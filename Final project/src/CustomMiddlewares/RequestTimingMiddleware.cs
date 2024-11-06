namespace AndreiKorbut.CareerChoiceBackend.CustomMiddlewares
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _nextMiddleware;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate requestDelegate, ILogger<RequestTimingMiddleware> logger)
        {
            _nextMiddleware = requestDelegate;
            _logger = logger;
        }

        public async Task InvokAsync(HttpContext httpContext)
        {
            var startTime = DateTime.Now;

            await _nextMiddleware(httpContext);

            var endTime = DateTime.Now;
            var processingTime = endTime - startTime;

            _logger.LogInformation("Request for {Path} took {Duration} ms", httpContext.Request.Path, processingTime.TotalMilliseconds);
        }
    }
}
