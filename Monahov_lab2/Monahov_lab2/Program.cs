using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();
// получаем строку подключени€ из файла конфигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
// добавл€ем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
// аутентификаци€ с помощью куки
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    // html-форма дл€ ввода логина/парол€
    string loginForm = @"<!DOCTYPE html>
    <html>
    <head>
        <meta charset='utf-8' />
        <title>METANIT.COM</title>
    </head>
    <body>
        <h2>Login Form</h2>
        <form method='post'>
            <p>
                <label>Name</label><br />
                <input name='name' />
            </p>
            <p>
                <label>Password</label><br />
                <input type='password' name='password' />
            </p>
            <input type='submit' value='Login' />
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(loginForm);
});

app.MapPost("/login", async (string? returnUrl, ApplicationContext db, HttpContext context) =>
{
    // получаем из формы email и пароль
    var form = context.Request.Form;
    // если email и/или пароль не установлены, посылаем статусный код ошибки 400
    if (!form.ContainsKey("name") || !form.ContainsKey("password"))
        return Results.BadRequest("Name и/или пароль не установлены");
 
    string name = form["name"];
    string password = form["password"];

    // находим пользовател€ 
    Person? person = await db.People.FirstOrDefaultAsync(p => p.Name == name && p.Password == password);
    // если пользователь не найден, отправл€ем статусный код 401
    if (person is null) return Results.Unauthorized();
 
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Name) };
    // создаем объект ClaimsIdentity
    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    // установка аутентификационных куки
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl??"/");
});
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

app.Map("/test", [Authorize] () => $"Hello World!");

app.MapGet("/api/users", [Authorize] async (ApplicationContext db) => await db.Users.ToListAsync());

app.MapGet("/api/users/{id:int}",  async (int id, ApplicationContext db) =>
{
    // получаем пользовател€ по id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "ѕользователь не найден" });

    // если пользователь найден, отправл€ем его
    return Results.Json(user);
});

app.MapDelete("/api/users/{id:int}",  async (int id, ApplicationContext db) =>
{
    // получаем пользовател€ по id
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "ѕользователь не найден" });

    // если пользователь найден, удал€ем его
    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.MapPost("/api/users", async (User user, ApplicationContext db) =>
{
    // добавл€ем пользовател€ в массив
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return user;
});

app.MapPut("/api/users", async (User userData, ApplicationContext db) =>
{
    // получаем пользовател€ по id
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userData.Id);

    // если не найден, отправл€ем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "ѕользователь не найден" });

    // если пользователь найден, измен€ем его данные и отправл€ем обратно клиенту
    user.Age = userData.Age;
    user.Name = userData.Name;
    await db.SaveChangesAsync();
    return Results.Json(user);
});

app.Run();