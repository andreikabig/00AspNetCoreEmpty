/*
    *********************************************************************************
    Program.cs - запускает и настраивает веб-хост, содержит логику обработки запросов
    *********************************************************************************
    . В ASP.NET Core 6 добавили Minimal API - упрощенная минимализированная модель для запуска веб-приложения
 */


/*
 Приложение представляет собой объект WebApplication:
 */


using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;

// Создаем объект builder приложения WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// Создаем объект WebApplication на основе билдера
var app = builder.Build();

/*
    .MapWhen() - на основании некоторого условия позволяет создавать ответвления конвейера при обработке запроса
        
        Параметры:
                    1. делегат Func<HttpContext, bool> - некоторое условие, которому должен соответствовать запрос
                                    Делегат принимает: HttpContext
                                    Делегат возвращает: bool - соответствие запроса условию
                    2. делегат Action<IApplicationBuilder> - действия над объектом IApplicationBuilder, который передается в делегат в качестве параметра
        
        ПРИМЕЧАНИЕ: В отличие от .UseWhen() использует терминальный компонент middleware
       
 */

app.MapWhen(context => context.Request.Path == "/time",
    appBuilder => appBuilder.Run(async context =>
    {
        var time = DateTime.Now.ToShortTimeString();
        await context.Response.WriteAsync(time);
    })
    );

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Welcome to RubyOnBrain!");
});


app.Run();





