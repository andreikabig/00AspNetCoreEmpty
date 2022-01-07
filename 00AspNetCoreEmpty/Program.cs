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

// ����������� ������� � ������� ������ .Add���������������();
builder.Services.AddMvc(); // ��������� � ������ ������� MVC, ��������� ���� �� ������ �� ������������ � ����������

// ������� ������ WebApplication �� ������ �������
var app = builder.Build();

/*
    ��� ��������� ������������ ������������ ����������� ���������� - IoC-���������� (Inversion of Control)
        ���� ������������: ��������� ������������ ����� ������������ � ����������� �������, ���������� ��������� ���� ��������

    ASP.NET Core ����� ���������� ��������� ��������� ������������ - ����������� ����������� IServiceProvieder
    ����������� = ������
    
    ���� ����������: �������� �� ������������� ������������ � ����������� ������,
                     ��������� ������������ � ��������� �������
    
    WebApplicationBuilder - �������� Services (������ IServiceCollection)
    ������: ���������� ���������
 
 */

var services = builder.Services;

// ������� �� ���������
app.Run(async (context) =>
{
    var sb = new StringBuilder();
    sb.Append("<h1>��� �������</h1>");
    sb.Append("<table>");
    sb.Append("<tr><th>���</th><th>Lifetime</th><th>����������</th></tr>");
    foreach (var svc in services)
    {
        sb.Append("<tr>");
        sb.Append($"<td>{svc.ServiceType.FullName}</td>");
        sb.Append($"<td>{svc.Lifetime}</td>");
        sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
        sb.Append("</tr>");
    }
    sb.Append("</table>");
    context.Response.ContentType = "text/html;charset=utf-8";
    await context.Response.WriteAsync(sb.ToString());
});



app.Run();




