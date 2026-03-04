using Microsoft.EntityFrameworkCore;
using MyLittlePony_Conexy.Application.Services;
using MyLittlePony_Conexy.Infrastructure;
using MyLittlePony_Conexy.Infrastructure.Repositories;
using MyLittlePony_Conexy.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
    await QuizSeeder.SeedAsync(dbContext);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
