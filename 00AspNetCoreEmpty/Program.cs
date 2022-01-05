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
    .Map() - создание ветки конвейера, которая обрабатывает запрос по определенному пути
        
        Параметры:
                    1. Путь запроса, с которым сопоставляется ветка
                    2. делегат Action<IApplicationBuilder> - действия над объектом IApplicationBuilder, в котором создается ветка конвейера
       
 */

app.Map("/time", appBuilder =>
    {
        var time = DateTime.Now.ToShortTimeString();

        appBuilder.Use(async (context, next) =>
        {
            Console.WriteLine(time);
            await next.Invoke();
        });

        appBuilder.Run(async (context) =>
        {
            await context.Response.WriteAsync(time);
        });
    });

// Примеры вложенных методов Map
app.Map("/home", appBuilder =>
{
    // /home/index
    appBuilder.Map("/index", IndexPage);
    appBuilder.Map("/about", AboutPage);

    // /home
    appBuilder.Run(async (context) => await context.Response.WriteAsync("Home page")) ;

});

// Ветки для разных путей
// app.Map("/index", ...)

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Welcome to RubyOnBrain!");
});


app.Run();


void IndexPage(IApplicationBuilder appBuilder)
{
    appBuilder.Run(async context => await context.Response.WriteAsync("Index page"));
}

void AboutPage(IApplicationBuilder appBuilder)
{
    appBuilder.Run(async context => await context.Response.WriteAsync("About page"));
}




