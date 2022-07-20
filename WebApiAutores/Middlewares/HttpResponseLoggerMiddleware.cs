namespace WebApiAutores.Middlewares
{
    public static class HttpResponseLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpResponseLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpResponseLoggerMiddleware>();
        }
    }

    public class HttpResponseLoggerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<HttpResponseLoggerMiddleware> logger;

        public HttpResponseLoggerMiddleware(RequestDelegate next, ILogger<HttpResponseLoggerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        //Invoke or InvokeAsync
        public async Task InvokeAsync(HttpContext context)
        {
            using (var ms = new MemoryStream())
            {
                var originalBodyResponse = context.Response.Body;
                context.Response.Body = ms;

                //await next.Invoke();
                await next(context);
                //after the above line, this code below will be executed when all the middlewares was executed and returned to the client a response.

                ms.Seek(0, SeekOrigin.Begin);
                string response = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(originalBodyResponse);
                context.Response.Body = originalBodyResponse;

                logger.LogInformation(response);
            }
        }
    }
}
