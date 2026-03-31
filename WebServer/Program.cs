using System.Linq.Expressions;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// app.Use(async (context, next) => {
//     Console.WriteLine($"[LOG] {context.Request.Method} {context.Request.Path}");
//     await next(context); 
//     Console.WriteLine($"[LOG] Ответ отправлен: {context.Response.StatusCode}");
// });

// app.Use(async (context, next) => {
//     context.Response.Headers.Append("X-Powered-By", "ASP.NET Core Lab27");
//     await next(context);
// });

// app.Use(async (context, next) => {
//     var key = context.Request.Query["key"];
//     if (key != "secret") {
//         context.Response.StatusCode = 401; 
//         await context.Response.WriteAsync("error");
//         return; 
//     }
//     await next(context);
// });

// app.MapGet("/", () => "Добро пожаловать на сервер!");

// app.MapGet("/about", () => "Это мой первый ASP.NET Core сервер");

// app.MapGet("/time", () => $"Время на сервере: {DateTime.Now}");

// app.MapGet("/hello/{name}", (string name) => $"Привет, {name}!");

// app.MapGet("/sum/{a}/{b}", (int a, int b) => $"{a+b}");

// app.MapGet("/student", () => new {
//     Name = "Иван шмаль",
//     Group = "ИСП-52",
//     Year = -1000000,
//     IsActive = true
// });

// app.MapGet("/subjects", () => new[] {
//     "РПМ",
//     "РМП",
//     "ИСРПО",
//     "СП"
// });

// app.MapGet("/product/{id}", (int id) => new Product(
//     Id: id,
//     Name: $"Товар #{id}",
//     Price: id * 99.99m,
//     InStock: id % 2 == 0
// ));

// Middleware - логирование
app.Use(async (context, next) => {
    var method = context.Request.Method;
    var path = context.Request.Path;
    Console.WriteLine($"{method} {path}");
    await next();
});

// Маршрут 1: Главная
app.MapGet("/", () => Results.Ok(new { 
    Message = "Добро пожаловать!", 
    Version = "1.0", 
    Time = DateTime.Now.ToString("HH:mm:ss") 
}));

// Маршрут 2: Информация о студенте
app.MapGet("/me", () => Results.Ok(new { 
    Name = "Иванов Иван", 
    Group = "ИСП-234", 
    Course = 3, 
    Skills = new[] { "C#", "HTML", "CSS", "JS", "ASP.NET" } 
}));

// Маршрут 3: Калькулятор
app.MapGet("/calc/{a}/{b}", (double a, double b) => Results.Ok(new { 
    A = a, 
    B = b, 
    Sum = a + b, 
    Diff = a - b, 
    Mul = a * b, 
    Div = b != 0 ? a / b : 0 
}));

app.MapFallback(() => Results.NotFound(new { 
    Error = "Маршрут не найден", 
    Code = 404 
}));

app.Run();

// record Product(int Id, string Name, decimal Price, bool InStock);