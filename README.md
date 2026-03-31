# Лабораторная работа №27: Знакомство с ASP.NET Core — первый веб-сервер на C#

## Основная информация

**ФИО:** Грошев Никита Андреевич

**Группа:** ИСП-232

**Дата:** 31.03.2026

---

## Описание работы

В ходе лабораторной работы был создан первый веб-сервер на ASP.NET Core с использованием Minimal API. Изучены основы HTTP-протокола, маршрутизация, middleware и возврат JSON-данных.

### Что было изучено:
- Установка .NET SDK и настройка проекта в VS Code
- Создание веб-сервера с помощью `dotnet new web`
- Работа с маршрутами (`MapGet`)
- Параметры маршрута (`/hello/{name}`)
- Возврат JSON через анонимные объекты и record-классы
- Middleware — цепочка обработки запросов
- Логирование запросов и добавление HTTP-заголовков
- Защита API через проверку ключа в middleware

---

## Структура проекта
```
lab27/
│
├── WebServer/
│ ├── Program.cs # Главный файл приложения
│ ├── WebServer.csproj # Файл проекта
│ ├── appsettings.json # Конфигурация
│ └── Properties/
│ └── launchSettings.json # Настройки запуска
│
├── img/ # Папка со скриншотами
│ ├── gitPushLab27_Groshev.png
│ ├── step5_firstRunLab27_Groshev.png
│ ├── step6_routingLab27_Groshev.png
│ ├── step7_jsonLab27_Groshev.png
│ ├── step8_middlewareLab27_Groshev.png
│ ├── step9_finalLab27_Groshev_1.png
│ ├── step9_finalLab27_Groshev_2.png
│ └── step9_finalLab27_Groshev_3.png
│
├── README.md
├── .editorconfig
└── .gitignore

```


---

## Список реализованных маршрутов

| Маршрут | Метод | Описание | Пример ответа |
|---------|-------|----------|---------------|
| `/` | GET | Главная страница | `{ "message": "Добро пожаловать!", "version": "1.0", "time": "14:30:00" }` |
| `/about` | GET | Информация о сервере | `"Это мой первый ASP.NET Core сервер"` |
| `/time` | GET | Текущее время | `"Время на сервере: 31.03.2026 14:30:00"` |
| `/hello/{name}` | GET | Приветствие | `"Привет, Иван!"` |
| `/sum/{a}/{b}` | GET | Сумма двух чисел | `"35"` |
| `/student` | GET | JSON с данными студента | `{ "name": "Иван Иванов", "group": "ИСП-234", "year": 3, "isActive": true }` |
| `/subjects` | GET | JSON-массив предметов | `[ "РПМ", "РМП", "ИСРПО", "СП" ]` |
| `/product/{id}` | GET | Информация о товаре | `{ "id": 3, "name": "Товар #3", "price": 299.97, "inStock": false }` |
| `/me` | GET | Информация о студенте | `{ "name": "Иванов Иван", "group": "ИСП-234", "course": 3, "skills": ["C#", "HTML", "CSS", "JS", "ASP.NET"] }` |
| `/calc/{a}/{b}` | GET | Калькулятор | `{ "a": 15, "b": 3, "sum": 18, "diff": 12, "mul": 45, "div": 5 }` |
| Любой другой | GET | Маршрут не найден | `{ "error": "Маршрут не найден", "code": 404 }` |

---

## Middleware

В приложении реализованы два middleware:

### 1. Логирование запросов
```csharp
app.Use(async (context, next) => {
    Console.WriteLine($"[LOG] {context.Request.Method} {context.Request.Path}");
    await next(context);
    Console.WriteLine($"[LOG] Ответ отправлен: {context.Response.StatusCode}");
});
```

### 2. Добавление HTTP-заголовка
```csharp
app.Use(async (context, next) => {
    context.Response.Headers.Add("X-Powered-By", "ASP.NET Core Lab27");
    await next(context);
});
```

### 3. Проверка секретного ключа (практическое задание)
```csharp
app.Use(async (context, next) => {
    var key = context.Request.Query["key"];
    if (key != "secret")
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("{\"error\": \"Доступ запрещён\"}");
        return;
    }
    await next(context);
});
```

---

## Как запустить проект

### 1. Перейти в папку с проектом
```bash
cd WebServer
```

2. Восстановить зависимости
```bash
dotnet restore
```

3. Запустить сервер
```bash
dotnet run
или с автоперезагрузкой:
```

```bash
dotnet watch run
```

4. Открыть в браузере
```
http://localhost:5000/
```

---

## Интересные факты

* ASP.NET Core используется в крупных проектах по всему миру:

* Stack Overflow — один из крупнейших сайтов вопросов и ответов по программированию

* Microsoft — внутренние сервисы и Azure

* JetBrains — серверная часть Rider и TeamCity

* Тинькофф — часть банковских сервисов

* Ozon — часть e-commerce платформы

---

## Ответы на контрольные вопросы

1. Чем Minimal API отличается от MVC в ASP.NET Core?
Minimal API — компактный подход, где все маршруты описываются в Program.cs с помощью app.MapGet(), app.MapPost(). Подходит для микросервисов и небольших API.
MVC — более структурированный подход с контроллерами, моделями и представлениями. Подходит для крупных приложений.

2. Что произойдёт, если поставить app.MapGet(...) до app.Use(...)?
Middleware, объявленные после маршрута, не будут обрабатывать запросы к этому маршруту, так как запрос уже будет обработан маршрутом и не дойдёт до middleware.

3. Почему ASP.NET Core преобразует PascalCase в camelCase при сериализации JSON?
По умолчанию System.Text.Json использует политику именования CamelCase, чтобы соответствовать стандартам JavaScript и JSON API. Это можно изменить, задав PropertyNamingPolicy = null в настройках.

4. Что означает код ответа HTTP 401? А 404? А 200?
200 OK — запрос успешно выполнен

401 Unauthorized — требуется аутентификация (нет ключа доступа)

404 Not Found — ресурс не найден

5. Чем dotnet run отличается от dotnet watch run?
dotnet run — однократный запуск. dotnet watch run — запускает сервер и отслеживает изменения файлов, автоматически перезапуская его при сохранении.

---

## GitHub репозиторий
```
https://github.com/Hig-lime-simp
```

## Автор
ФИО: Грошев Никита Андреевич
Группа: ИСП-232
Дата: 31.03.2026