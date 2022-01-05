/*
    *********************************************************************************
    Program.cs - ��������� � ����������� ���-����, �������� ������ ��������� ��������
    *********************************************************************************
    . � ASP.NET Core 6 �������� Minimal API - ���������� ������������������ ������ ��� ������� ���-����������
 */


/*
 ���������� ������������ ����� ������ WebApplication:
 */


using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;

// ������� ������ builder ���������� WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// ������� ������ WebApplication �� ������ �������
var app = builder.Build();


// ���������� middleware ���������� (������� ��������� ����� �� ������)
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

        // ���� � �����, � ������� ����� ����������� �����
        var uploadPath = $"{Directory.GetCurrentDirectory()}/Uploads";

        // ���� ����� ����� ���, �� ������� ��
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        foreach (var file in files)
        {
            // ���� � �����
            string fullPath = $"{uploadPath}/{file.FileName}";

            // ��������� ���� � �����
            using (var fileStram = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStram);  // �������� ���� � �����
            }
        }

        await response.WriteAsync("���� ������� ��������!");
    }
    else 
    {
        await response.SendFileAsync("Views/index.html");
    }
}

app.Run();





