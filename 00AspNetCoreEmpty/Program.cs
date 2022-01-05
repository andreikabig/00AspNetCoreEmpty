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
    .MapWhen() - �� ��������� ���������� ������� ��������� ��������� ����������� ��������� ��� ��������� �������
        
        ���������:
                    1. ������� Func<HttpContext, bool> - ��������� �������, �������� ������ ��������������� ������
                                    ������� ���������: HttpContext
                                    ������� ����������: bool - ������������ ������� �������
                    2. ������� Action<IApplicationBuilder> - �������� ��� �������� IApplicationBuilder, ������� ���������� � ������� � �������� ���������
        
        ����������: � ������� �� .UseWhen() ���������� ������������ ��������� middleware
       
 */

app.MapWhen(context => context.Request.Path == "/time",
    appBuilder => appBuilder.Run(async context =>
    {
        var time = DateTime.Now.ToShortTimeString();
        await context.Response.WriteAsync(time);
    })
    );

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Welcome to RubyOnBrain!");
});


app.Run();





