using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using MyLittlePony_Conexy.Application.Services;
using MyLittlePony_Conexy.Infrastructure;
using MyLittlePony_Conexy.Infrastructure.Repositories;
using MyLittlePony_Conexy.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddDbContext<QuizDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection")
                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuizService, QuizService>();

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
    await dbContext.Database.MigrateAsync();

    var seedData = configuration.GetValue("SeedData", true);
    if (seedData)
    {
        await QuizSeeder.SeedAsync(dbContext);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();