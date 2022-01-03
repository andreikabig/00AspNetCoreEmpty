/*
    *********************************************************************************
    Program.cs - ��������� � ����������� ���-����, �������� ������ ��������� ��������
    *********************************************************************************
    . � ASP.NET Core 6 �������� Minimal API - ���������� ������������������ ������ ��� ������� ���-����������
 */


/*
 ���������� ������������ ����� ������ WebApplication:

 ������: ��������� ���� ������������ ����������
         * ���������
         * �������������� ������������ � �.�.
 -----------------------------------------------------------------------
 ����� .CreateBuilder(args):
 ������: �������� ������� WebApplicationBuilder �� ������ ���������� ���������� (������ �������� ����������)
 
 �������� WebApplicationBuilder:
        * Configuration - ������ ConfigurationManager - ���������� ������������ � ����������
        * Environment - ���������� �� ���������, � ������� �������� ���������� (����������, ����� � �.�.)
        * Host - ������ IHostBuilder - ��������� ��� ��������� �����
        * Logging - ����������� �������� ����������� � ����������
        * Services - ��������� �������� + ���������� �������� � ����������
        * WebHost - ������ IWebHostBuilder - ��������� ��������� �������� �������
 */

using System.Text;

var builder = WebApplication.CreateBuilder(args);


/*
    ����� .Build() - �������� ������� WebApplication (������ ������ ����������)
    
    ������� ��������� WebApplication:
            * IHost - ����������� ��� ������� � ��������� �����
            * IApplicationBuilder - ��������� �����������, ������� ��������� � ��������� ��������
            * IEndpointRouteBuilder - ��������� ���������, ������� �������������� � ���������
    
    ��������:
            * Configuration - ������������ ���������� - IConfiguration
            * Environment - ��������� ���������� - ������ IWebHostEnvironment
            * Lifetime - ����������� � �������� ���������� ����� ����������
            * Logger - ������ ���������� �� ���������
            * Services - ������� ����������
            * Urls - ����� �������, ������� ���������� ������
 */

var app = builder.Build();

/*
    ��������� ��������
    ------------------
    �������: ������� ���������
    ������ ���������: ���������� middleware
    ������������: ������ WebApplication
    
    �������� ����������� middleware
    -------------------------------
        1. ������ (�������� ����������� middleware): .Run(), .Map(), .Use()
        2. ��������� ������
            * ����� - ������� RequestDelegate
            * ��������� ������ HttpContext (��� ��������� ������� ����������� ������ HtppContext � ����������� � �������)
            * ���������� - ������ Task
                    
                    �������� HttpContext:
                        * Connection - ���������� �� �������������� �����������
                        * Features - ��������� HTTP-����������������� ��� �������
                        * Items - ��������� ��� ����-�������� ��� �������� ������ �������
                        * Request - ���������� ������ HttpRequest, ������� ������ ���-�� � �������
                        * RequestAborted - ����������� ����������, ����� ����������� �����������
                        * RequestServices
                        * Response - ���������� ������ HttpResponse, ������� ��������� ��������� ������� �������
                        * User - ������������, ������� ������������� � ���� ��������
                        * WebSocket
 
        3. ������������ ���������� ���������� middleware (����� Use��������, ��������, UseWelcomePage())
    
 */


// ���������� ����������� ���������� middleware
// app.UseWelcomePage();


/*
    ���������� middleware � �������������� ������ .Run()
    ----------------------------------------------------
    ���������: ������������ ���������� - ��������� ��������� �������
    ��������: ������� RequstDelegate, ����������� HttpRequest, ������������ Task

    ����������: �� �������� ������ �� ��������� �� ���������! 
*/

// app.Run(async (context) => await context.Response.WriteAsync("Welcome to It-school Ruby on Brain!"));

// async Task HandleRequst(HttpContext context) => await context.Response.WriteAsync("Welcome to It-school Ruby on Brain!");
// app.Run(HandleRequst);  // ���������� ���������� middleware � ������� ���������� ������

/*
    �������� ������ (��������� ������ HttpResponse)
    ------------------------------
    ������: context.Response.WriteAsync() 
    ���������: ������, ���������
 */

app.Run(WelcomeRequst);
async Task WelcomeRequst(HttpContext context)
{
    var response = context.Response;
    var request = context.Request;

    response.Headers.ContentLanguage = "ru-RU";
    response.Headers.ContentType = "text/html; charset=utf-8";

    if (request.Path == "/request")
    {
        StringBuilder stringBuilder = new StringBuilder("<table>");

        foreach (var header in request.Headers)
            stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");

        stringBuilder.Append("</table>");

        await response.WriteAsync(stringBuilder.ToString());
    }
    else if (request.Path == "/")
        await response.WriteAsync("<h1>����� ���������� � It-����� Ruby on Brain!</h1>", System.Text.Encoding.UTF8); // ��������� �������������, �.�. �� ������� �� ����� � ������
    else if (request.Path == "/show")
    {
        var query = request.Query;

        StringBuilder stringBuilder = new StringBuilder("<h2>��������� ������ �������</h2><table>");
        stringBuilder.Append($"<tr><td>��������</td><td>��������</td></tr>");

        foreach (var param in request.Query)
            stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");

        stringBuilder.Append("</table>");

        await response.WriteAsync(stringBuilder.ToString());
    }
    else if (request.Path == "/image")
    {
        response.Headers.ContentType = "image/png; charset=utf-8";
        await response.SendFileAsync(@"Images\logo.png");
    }
        
    else
        await response.WriteAsync($"��������, �� �������� {request.Path} ��� �� ����� �����!");
}

// ������ ���������� - ����� .Run()
app.Run();
