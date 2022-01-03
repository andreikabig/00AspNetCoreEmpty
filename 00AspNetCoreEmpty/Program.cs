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
using System.Text.Json;

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
        await response.SendFileAsync(@"Views\Index.html");
    //await response.WriteAsync("<h1>Добро пожаловать в It-школу Ruby on Brain!</h1>", System.Text.Encoding.UTF8); // кодировка необязательно, т.к. мы указали ее ранее в ответе
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
    else if (request.Path == "/download")
    {
        response.Headers.ContentType = "image/png; charset=utf-8";
        response.Headers.ContentDisposition = "attachment; filename=image.png";
        await response.SendFileAsync(@"Images\logo.png");
    }
    else if (request.Path == "/register")
    {
        await response.SendFileAsync(@"Views\Register.html");
    }
    else if (request.Path == "/postuser")
    {
        var form = request.Form;

        string name = form["name"];
        string surname = form["surname"];
        string password = form["password"];
        string email = form["email"];
        string[] topics = form["topics"];
        string[] languages = form["languages"];

        StringBuilder stringBuilder = new StringBuilder("<table>");

        for (int i = 0; i < topics.Count(); i++)
        {
            stringBuilder.Append($"<tr><td>{i + 1}.</td><td>{topics[i]}</td></tr>");
        }

        stringBuilder.Append("</table>");

        string langs = "Языки программирования: ";

        foreach (string language in languages)
            langs += $"{language} ";


        await response.WriteAsync($"<p><h1>Отправленные данные: </h1></p>" +
            $"<p>Name: {name}</p>" +
            $"<p>Surname: {surname}</p>" +
            $"<p>Password: {password}</p>" +
            $"<p>Login: {email}</p><br />" +
            $"Интересующие тематики: <br />" +
            $"{stringBuilder}<br />" +
            $"{langs}");

    }
    else if (request.Path == "/api/person/post")    // Обработка пост-запроса
    {
        var responseText = "Некорректные данные";   // Сообщение по умолчанию

        // Если в запросе нам отправляется json файл
        if (request.HasJsonContentType())
        {
            // Определим параметры сериализации/десериализации
            var jsonoptions = new JsonSerializerOptions();

            // Добавляем конвертер кода .json в объект типа Person. в коллекцию ковертеров
            jsonoptions.Converters.Add(new PersonConverter());

            // Десерилизация данных (форма -> в объект) с помощью нашего конвертера
            var person = await request.ReadFromJsonAsync<Person>(jsonoptions);
            /*
             ВОЗМОЖНЫЕ ПРОБЛЕМЫ:
                Невозможно десирилизовать полученные данные - например, ошибка формата данных или несовпадение ключей полученного файла и свойств объекта
                РЕШЕНИЕ: настройка серилизации и десерилизации - обработка исключений, и т.п.
             */

            // Если класс не пуст
            if (person != null)
                responseText = $"Name: {person.Name}, Age: {person.Age}";   // меняем сообщение на данные
        }
        await response.WriteAsJsonAsync(new {text = responseText}); // отправляем анонимный класс в json формате
    }
    else if (request.Path == "/api/person") // Обработка get-запроса 
        await response.SendFileAsync(@"Views\User.html");
    else
        response.Redirect("/");
}

// Запуск приложения - метод .Run()
app.Run();


// record - неизменяемый класс (однако свойства можно спокойно изменять, для обратного нужно прописывать вместо set -> init)
public record Person(string Name, int Age); 