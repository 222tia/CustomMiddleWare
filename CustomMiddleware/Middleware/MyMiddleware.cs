namespace CustomMiddleware.Middleware {
    public class MyMiddleware {
        
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next) {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context) {
            var username = context.Request.Query["username"];
            var password = context.Request.Query["password"];

            // enter /?username=user1&password=password1 after localhost:XXXX
            if (username != "user1" || password != "password1") {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Not authorized.");
            } else {
                await _next(context);
            }
        }
    }
}
