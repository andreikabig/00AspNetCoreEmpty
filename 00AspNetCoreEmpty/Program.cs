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

/*
    .Map() - �������� ����� ���������, ������� ������������ ������ �� ������������� ����
        
        ���������:
                    1. ���� �������, � ������� �������������� �����
                    2. ������� Action<IApplicationBuilder> - �������� ��� �������� IApplicationBuilder, � ������� ��������� ����� ���������
       
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

// ����� ��� ������ �����
// app.Map("/index", ...)

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Welcome to RubyOnBrain!");
});


app.Run();





