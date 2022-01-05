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


// Добавление middleware приложения (который загружает файлы на сервер)
app.Run(UploadFiles);

async Task UploadFiles(HttpContext context)
{
    var request = context.Request;
    var response = context.Response;

    response.ContentType = "text/html; charset=utf-8";

    var path = request.Path;


    if (request.Method == "POST" && path == "/upload")
    {
        IFormFileCollection files = request.Form.Files;

        // Путь к папке, в которую будут сохраняться файлы
        var uploadPath = $"{Directory.GetCurrentDirectory()}/Uploads";

        // Если такой папки нет, то создаем ее
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        foreach (var file in files)
        {
            // Путь к файлу
            string fullPath = $"{uploadPath}/{file.FileName}";

            // Сохраняем файл в папку
            using (var fileStram = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStram);  // Копирует файл в поток
            }
        }

        await response.WriteAsync("Файл успешно загружен!");
    }
    else 
    {
        await response.SendFileAsync("Views/index.html");
    }
}

app.Run();





