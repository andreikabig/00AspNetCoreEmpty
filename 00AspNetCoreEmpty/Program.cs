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
using _00AspNetCoreEmpty.Middlewares;

// ������� ������ builder ���������� WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// ������� ������ WebApplication �� ������ �������
var app = builder.Build();

app.UseMiddleware<TokenMiddleware>();

app.Run(async (context) => {
    context.Response.ContentType = "text/html; charset=UTF-8";
    await context.Response.WriteAsync("�������� ���� �� ������ �� ����!"); });

app.Run();




