/*
    *********************************************************************************
    Program.cs - запускает и настраивает веб-хост, содержит логику обработки запросов
    *********************************************************************************
    . В ASP.NET Core 6 добавили Minimal API - упрощенная минимализированная модель для запуска веб-приложения
 */


/*
 Приложение представляет собой объект WebApplication:

 Задача: настройка всей конфигурации приложения
         * маршрутов
         * использоваемых зависимостей и т.д.
 -----------------------------------------------------------------------
 Метод .CreateBuilder(args):
 Задача: создание объекта WebApplicationBuilder на основе переданных параметров (объект паттерна приложения)
 
 Свойства WebApplicationBuilder:
        * Configuration - объект ConfigurationManager - добавление конфигураций к приложению
        * Environment - информация об окружении, в котором запущено приложение (разработка, релиз и т.п.)
        * Host - объект IHostBuilder - необходим для настройки хоста
        * Logging - определение настроек логирования в приложении
        * Services - коллекция сервисов + добавление сервисов в приложение
        * WebHost - объект IWebHostBuilder - настройка отдельных настроек сервера
 */

using System.Text;

var builder = WebApplication.CreateBuilder(args);


/*
    Метод .Build() - создание объекта WebApplication (объект самого приложения)
    
    Входные параметры WebApplication:
            * IHost - применяется для запуска и остановки хоста
            * IApplicationBuilder - установка компонентов, которые участвует в обработке запросов
            * IEndpointRouteBuilder - установка маршрутов, которые сопоставляются с запросами
    
    Свойства:
            * Configuration - конфигурация приложения - IConfiguration
            * Environment - окружение приложения - объект IWebHostEnvironment
            * Lifetime - уведомления о событиях жизненного цикла приложения
            * Logger - логгер приложения по умолчанию
            * Services - сервисы приложения
            * Urls - набор адресов, которые использует сервер
 */

var app = builder.Build();

/*
    ОБРАБОТКА ЗАПРОСОВ
    ------------------
    Принцип: принцип конвейера
    Состав конвейера: компоненты middleware
    Расположение: объект WebApplication
    
    Создание компонентов middleware
    -------------------------------
        1. Методы (создания компонентов middleware): .Run(), .Map(), .Use()
        2. Отдельные классы
            * класс - делегат RequestDelegate
            * принимает объект HttpContext (при получении запроса формируется объект HtppContext с информацией о запросе)
            * возвращает - объект Task
                    
                    Свойства HttpContext:
                        * Connection - информация об установившемся подключении
                        * Features - коллекция HTTP-функциональностей для запроса
                        * Items - коллекция пар ключ-значение для хранения данных запроса
                        * Request - возвращает объект HttpRequest, которая хранит инф-ию о запросе
                        * RequestAborted - уведомление приложения, когда подключение прерывается
                        * RequestServices
                        * Response - возвращает объект HttpResponse, который позволяет управлять ответом клиенту
                        * User - пользователь, который ассоциируется с этим запросом
                        * WebSocket
 
        3. Использовать встроенные компоненты middleware (через UseНазвание, например, UseWelcomePage())
    
 */


// Добавление встроенного компонента middleware
// app.UseWelcomePage();


/*
    ДОБАВЛЕНИЕ middleware с использованием метода .Run()
    ----------------------------------------------------
    Добавляет: терминальные компоненты - завершают обработку запроса
    Параметр: делегат RequstDelegate, принимающий HttpRequest, возвращающий Task

    ПРИМЕЧАНИЕ: НЕ ПЕРЕДАЕТ ЗАПРОС НА ОБРАБОТКУ ПО КОНВЕЙЕРУ! 
*/

// app.Run(async (context) => await context.Response.WriteAsync("Welcome to It-school Ruby on Brain!"));

// async Task HandleRequst(HttpContext context) => await context.Response.WriteAsync("Welcome to It-school Ruby on Brain!");
// app.Run(HandleRequst);  // Добавление компонента middleware с помощью отдельного метода

/*
    ОТПРАВКА ОТВЕТА (используя объект HttpResponse)
    ------------------------------
    Способ: context.Response.WriteAsync() 
    Параметры: данные, кодировка
 */

app.Run(WelcomeRequst);
async Task WelcomeRequst(HttpContext context)
{
    var response = context.Response;
    var request = context.Request;

    response.Headers.ContentLanguage = "ru-RU";
    response.Headers.ContentType = "text/html; charset=utf-8";

    if (request.Path == "/request")
    {
        StringBuilder stringBuilder = new StringBuilder("<table>");

        foreach (var header in request.Headers)
            stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");

        stringBuilder.Append("</table>");

        await response.WriteAsync(stringBuilder.ToString());
    }
    else if (request.Path == "/")
        await response.WriteAsync("<h1>Добро пожаловать в It-школу Ruby on Brain!</h1>", System.Text.Encoding.UTF8); // кодировка необязательно, т.к. мы указали ее ранее в ответе
    else if (request.Path == "/show")
    {
        var query = request.Query;

        StringBuilder stringBuilder = new StringBuilder("<h2>Параметры строки запроса</h2><table>");
        stringBuilder.Append($"<tr><td>Параметр</td><td>Значение</td></tr>");

        foreach (var param in request.Query)
            stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");

        stringBuilder.Append("</table>");

        await response.WriteAsync(stringBuilder.ToString());
    }
    else if (request.Path == "/image")
    {
        response.Headers.ContentType = "image/png; charset=utf-8";
        await response.SendFileAsync(@"Images\logo.png");
    }
        
    else
        await response.WriteAsync($"Извините, но маршрута {request.Path} нет на нашем сайте!");
}

// Запуск приложения - метод .Run()
app.Run();
