using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using MyLittlePony_Conexy.Application.Services;
using MyLittlePony_Conexy.Infrastructure;
using MyLittlePony_Conexy.Infrastructure.Repositories;
using MyLittlePony_Conexy.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация уже встроена в builder.Configuration
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuizDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

// На Render Swagger тоже можно оставить включенным для тестов, если хочешь
if (app.Environment.IsDevelopment() || true) // Удали "|| true" позже для безопасности
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Блок автоматического создания таблиц и сидирования
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<QuizDbContext>();
        
        // 1. Создаем таблицы (применяем миграции), если их нет в базе Render
        await dbContext.Database.MigrateAsync();
        
        // 2. Наполняем базу данными
        await QuizSeeder.SeedAsync(dbContext);
        
        Console.WriteLine("База данных успешно обновлена и заполнена!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при подготовке базы: {ex.Message}");
    }
}
// 1. Говорим серверу, что index.html в корне — это файл по умолчанию
app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
    DefaultFileNames = new List<string> { "index.html" }
});

// 2. Разрешаем отдавать статические файлы из корня
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
