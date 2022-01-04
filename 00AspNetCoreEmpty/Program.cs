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

// Создаем объект builder приложения WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// Создаем объект WebApplication на основе билдера
var app = builder.Build();


// Добавление middleware приложения
app.Run();

// Запуск приложения - метод .Run()
app.Run();


