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
        await response.SendFileAsync(@"Views\Index.html");
    //await response.WriteAsync("<h1>����� ���������� � It-����� Ruby on Brain!</h1>", System.Text.Encoding.UTF8); // ��������� �������������, �.�. �� ������� �� ����� � ������
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
    else if (request.Path == "/download")
    {
        response.Headers.ContentType = "image/png; charset=utf-8";
        response.Headers.ContentDisposition = "attachment; filename=image.png";
        await response.SendFileAsync(@"Images\logo.png");
    }
    else if (request.Path == "/register")
    {
        await response.SendFileAsync(@"Views\Register.html");
    }
    else if (request.Path == "/postuser")
    {
        var form = request.Form;

        string name = form["name"];
        string surname = form["surname"];
        string password = form["password"];
        string email = form["email"];
        string[] topics = form["topics"];
        string[] languages = form["languages"];

        StringBuilder stringBuilder = new StringBuilder("<table>");

        for (int i = 0; i < topics.Count(); i++)
        {
            stringBuilder.Append($"<tr><td>{i + 1}.</td><td>{topics[i]}</td></tr>");
        }

        stringBuilder.Append("</table>");

        string langs = "����� ����������������: ";

        foreach (string language in languages)
            langs += $"{language} ";


        await response.WriteAsync($"<p><h1>������������ ������: </h1></p>" +
            $"<p>Name: {name}</p>" +
            $"<p>Surname: {surname}</p>" +
            $"<p>Password: {password}</p>" +
            $"<p>Login: {email}</p><br />" +
            $"������������ ��������: <br />" +
            $"{stringBuilder}<br />" +
            $"{langs}");

    }
    else if (request.Path == "/api/person")
    {
        Person person = new ("������", 21);

        // ����� � json (person)
        await response.WriteAsJsonAsync(person);
    }
    else
        response.Redirect("/");
}

// ������ ���������� - ����� .Run()
app.Run();


// record - ������������ ����� (������ �������� ����� �������� ��������, ��� ��������� ����� ����������� ������ set -> init)
public record Person(string Name, int Age); 