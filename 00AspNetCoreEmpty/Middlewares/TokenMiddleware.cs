namespace _00AspNetCoreEmpty.Middlewares
{
    // MiddleWare отдельным классом
    
    /*
        ТРЕБОВАНИЯ
        ----------
        1. Конструктор, который принимает параметр типа RequestDelegate - через этот параметр можно получить ссылку на тот делегат запроса, который стоит следующим
                                                                          в ковейере обработки запроса
        2. Метод Invoke (либо InvokeAsync) - возвращает Task и принимает HttpContext; данный метод обрабатывает наш запрос
     
     */
    public class TokenMiddleware
    {
        private readonly RequestDelegate next;
        public TokenMiddleware(RequestDelegate next)
        {
            this.next = next;   
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];
            if (token != "12345678")
            {
                context.Response.ContentType = "text/html; charset=UTF-8";
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Неправильный token!");
            }
            else
                await next.Invoke(context);
        }
    }
}
