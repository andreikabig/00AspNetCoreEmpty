/*
    *********************************************************************************
    Program.cs - запускает и настраивает веб-хост, содержит логику обработки запросов
    *********************************************************************************
    . В ASP.NET Core 6 добавили Minimal API - упрощенная минимализированная модель для запуска веб-приложения
 */


/*
 Приложение представляет собой объект WebApplication:
 */

using _00AspNetCoreEmpty;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;

// Создаем объект builder приложения WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// Создаем объект WebApplication на основе билдера
var app = builder.Build();

// Список пользоваталей
List<User> users = new List<User>() 
{
    new User() {Id = Guid.NewGuid().ToString(), FirstName = "Платон", LastName = "Хинчин", Age = 10, Email = "platon@gmail.com", PhoneNumber = "+79392392000", ProgLanguage = "Python Starter", Password = "FF3axF93d" },    
    new User() {Id = Guid.NewGuid().ToString(), FirstName = "Максим", LastName = "Андреев", Age = 10, PhoneNumber = "+73942994920", ProgLanguage = "Python Starter", Password = "FF9xLL03sddd" },    
    new User() {Id = Guid.NewGuid().ToString(), FirstName = "Михаил", LastName = "Иванов", Age = 12, PhoneNumber = "+7000994920", ProgLanguage = "Python Beginer", Password = "FLffc8993jd" },    
    new User() {Id = Guid.NewGuid().ToString(), FirstName = "Михаил", LastName = "Медведев", Age = 12, Email = "rusbear@gmail.com", PhoneNumber = "+7948394920", ProgLanguage = "Python Starter", Password = "Jfffff93kd" },    
    new User() {Id = Guid.NewGuid().ToString(), FirstName = "Диана", LastName = "Баранова", Age = 7, Email = "crowndiana@gmail.com", PhoneNumber = "+1948394920", ProgLanguage = "Scratch", Password = "3889383hdfff" },    
    new User() {Id = Guid.NewGuid().ToString(), FirstName = "Иван", LastName = "Панов", Age = 13, Language = "Английский", Email = "test03@gmail.com", PhoneNumber = "+7948393420", ProgLanguage = "Python Starter", Password = "39ifjkddd" }    
};


// Добавление middleware приложения (который в зависимости от типа запроса делает те или иные дейтсвия)
app.Run(UserTable);

async Task UserTable(HttpContext context)
{
    var request = context.Request;
    var response = context.Response;

    var path = request.Path;

    // Регулярное выражение для сравнения запроса - id пользователея
    string expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

    if (request.Method == "GET" && path == "/api/users")
    {
        await GetAllUsers(response);
    }
    else if (request.Method == "GET" && Regex.IsMatch(path, expressionForGuid))
    {
        string? id = path.Value?.Split("/")[3];
        await GetUser(id, response, request);
    }
    else if (request.Method == "POST" && path == "/api/users")
    {
        await CreateUser(response, request);
    }
    else if (path == "/api/users" && request.Method == "PUT")
    {
        await UpdateUser(response, request);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "DELETE")
    {
        string? id = path.Value?.Split("/")[3];
        await DeleteUser(id, response, request);
    }
    else 
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("Views/index.html");
    }
}








// Запуск приложения - метод .Run()
app.Run();


// GET: /api/users
async Task GetAllUsers(HttpResponse response)
{
    await response.WriteAsJsonAsync(users);
}


// GET: /api/users/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx (id)
async Task GetUser(string? id, HttpResponse response, HttpRequest request)
{
    User? user = users.FirstOrDefault(x => x.Id == id);

    if (user != null)
        await response.WriteAsJsonAsync(user);
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Пользователь не найден." });
    }
}

// POST: /api/users
async Task CreateUser(HttpResponse response, HttpRequest request)
{
    try
    {
        // Получаем данные пользователя
        var user = await request.ReadFromJsonAsync<User>();
        if (user != null)
        {
            user.Id = Guid.NewGuid().ToString();
            users.Add(user);

            await response.WriteAsJsonAsync(user);
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new {message = "Некорректные данные!" });
    }
}


// PUT: /api/users
async Task UpdateUser(HttpResponse response, HttpRequest request)
{
    try
    {
        User? userData = await request.ReadFromJsonAsync<User>();

        if (userData != null)
        {
            // Получаем пользователя по id
            var user = users.FirstOrDefault(x => x.Id == userData.Id);

            if (user != null)
            {
                user.Age = userData.Age;
                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;
                user.Email = userData.Email;
                user.Password = userData.Password;
                user.Language = userData.Language;
                user.ProgLanguage = userData.ProgLanguage;
                user.PhoneNumber = userData.PhoneNumber;
                await response.WriteAsJsonAsync(user);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "Пользователь не найден!" });
            }
        }
        else
            throw new Exception();
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные!" });
    }
}


// DELETE: /api/users/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxx (id)
async Task DeleteUser(string? id, HttpResponse response, HttpRequest request)
{
    // Получаем пользователя по id
    User? user = users.FirstOrDefault(x => x.Id == id);

    if (user != null)
    {
        users.Remove(user);
        await response.WriteAsJsonAsync(user);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Пользователь не найден!" });
    }
}
