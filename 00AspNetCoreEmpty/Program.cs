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
    .Use() - добавляет компоненнт MiddleWare, который позволяет передать обработку запроса далее следующим в конвейере компонентам
       
        Принимает: действие с двумя параметрами, возвращающее объект Task
                
                Параметры делегата:
                                    * объект HttpContext
                                    * объект Func<Task> или RequestDelegate - представляет следующий в конвейере компонент middleware, которому будет передана обработка запроса
 */

string date = "";

app.Use(ForUse);


app.Run(ForRun);

app.Run();


async Task ForUse(HttpContext context, Func<Task> next)
{
    date = DateTime.Now.ToShortDateString();
    await next.Invoke();    // Invoke - вызов делегата
    Console.WriteLine("Date " + date);
}

async Task ForRun(HttpContext context) => await context.Response.WriteAsync($"Date: {date}");

