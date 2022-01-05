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
    .Use() - ��������� ���������� MiddleWare, ������� ��������� �������� ��������� ������� ����� ��������� � ��������� �����������
       
        ���������: �������� � ����� �����������, ������������ ������ Task
                
                ��������� ��������:
                                    * ������ HttpContext
                                    * ������ Func<Task> ��� RequestDelegate - ������������ ��������� � ��������� ��������� middleware, �������� ����� �������� ��������� �������
                                            
                                        P.S. ���� ���������� RequestDelegate, �� � Invoke ���������� �������� ������ HttpContext
 */

string date = "";

app.Use(ForUse);


app.Run(ForRun);

app.Run();


async Task ForUse(HttpContext context, Func<Task> next)
{
    /*
            ����������: �� ������������� �������� ����� next.Invoke() ����� response.WriteAsync()
                        ��������� ������ ���� �������� ��������� �������, ���� ������������ ����� � ������� .WriteAsync(),
                        �� �� ��� �������� ������������!!!
     */
    date = DateTime.Now.ToShortDateString();
    await next.Invoke();    // Invoke - ����� �������� (��������� � ��������� ��������� - ForRun � .Run())
    Console.WriteLine("Date " + date);
}

async Task ForRun(HttpContext context) => await context.Response.WriteAsync($"Date: {date}");

