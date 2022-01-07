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
using _00AspNetCoreEmpty.Middlewares;
using _00AspNetCoreEmpty.Extensions;

// Создаем объект builder приложения WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// Регистрация сервиса с помощью метода .AddНазваниеСервиса();
builder.Services.AddMvc(); // добавляет в проект сервисы MVC, благодаря чему мы сможем их использовать в приложении

// Создаем объект WebApplication на основе билдера
var app = builder.Build();

/*
    Для установки зависимостей используются специальные контейнеры - IoC-контейнеры (Inversion of Control)
        Цель контеннйеров: установка зависимостей между абстракциями и конкретными объекта, управление созданием этих объектов

    ASP.NET Core имеет встроенный контейнер внедрения зависимостей - представлен интерфейсом IServiceProvieder
    Зависимость = сервис
    
    Цель контейнера: отвечать за сопоставление зависимостей с конкретными типами,
                     внедрнеие зависимостей в различные объекты
    
    WebApplicationBuilder - свойство Services (объект IServiceCollection)
    Задача: управление сервисами
 
 */

var services = builder.Services;

// Сервисы по умолчанию
app.Run(async (context) =>
{
    var sb = new StringBuilder();
    sb.Append("<h1>Все сервисы</h1>");
    sb.Append("<table>");
    sb.Append("<tr><th>Тип</th><th>Lifetime</th><th>Реализация</th></tr>");
    foreach (var svc in services)
    {
        sb.Append("<tr>");
        sb.Append($"<td>{svc.ServiceType.FullName}</td>");
        sb.Append($"<td>{svc.Lifetime}</td>");
        sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
        sb.Append("</tr>");
    }
    sb.Append("</table>");
    context.Response.ContentType = "text/html;charset=utf-8";
    await context.Response.WriteAsync(sb.ToString());
});



app.Run();




