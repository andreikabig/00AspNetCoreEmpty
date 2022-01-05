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
    .UseWhen() - �� ��������� ���������� ������� ��������� ��������� ����������� ��������� ��� ��������� �������
        
        ���������:
                    1. ������� Func<HttpContext, bool> - ��������� �������, �������� ������ ��������������� ������
                                    ������� ���������: HttpContext
                                    ������� ����������: bool - ������������ ������� �������
                    2. ������� Action<IApplicationBuilder> - �������� ��� �������� IApplicationBuilder, ������� ���������� � ������� � �������� ���������
       
 */


app.UseWhen(context => context.Request.Path == "/time",
    appBuilder =>   // ����������� ��������� ��� ���������� �������� �������
    {
        var time = DateTime.Now.ToShortTimeString();

        appBuilder.Use(async (context, next) =>
        {
            Console.WriteLine($"Time: {time}");
            await next.Invoke();
        });

        appBuilder.Run(async (context) =>
        {
            await context.Response.WriteAsync($"Time: {time}");
        });
    }
    );

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Welcome to RubyOnBrain!");
});


app.Run();





