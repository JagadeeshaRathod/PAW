namespace swas.UI.Helpers
{
    public class MyModule
    {
        private readonly RequestDelegate _next;

        public MyModule(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do something with context near the beginning of request processing.
            var myHeader = context.Request.Path.ToString();
            string referer = context.Request.Headers["Referer"].ToString();
            if (referer != "" && myHeader != "/")
                await _next.Invoke(context);
            else if (myHeader == "/")
                await _next.Invoke(context);
            else if (myHeader == "/Account/InvalidPage" || myHeader.Contains("Login") || myHeader.Contains("Account") || myHeader.Contains("Identity"))
                await _next.Invoke(context);
            else
                context.Response.Redirect("/Account/InvalidPage");

            // Clean up.  
        }
    }

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyModule>();
        }
    }
}
