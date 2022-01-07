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
using _00AspNetCoreEmpty.Extensions;

// ������� ������ builder ���������� WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// ������� ������ WebApplication �� ������ �������
var app = builder.Build();

//app.UseMiddleware<TokenMiddleware>();

//app.UseToken("12345678"); // ���������� ����� ����������

app.UseTokenAuthentication();
app.UseToken("12345");
app.UseMyRouting();


app.Run();




