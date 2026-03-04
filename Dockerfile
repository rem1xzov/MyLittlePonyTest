# Стейдж сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем .csproj из корня
COPY ["MyLittlePony_Conexy.csproj", "./"]

# Восстанавливаем зависимости
RUN dotnet restore "MyLittlePony_Conexy.csproj"

# Копируем вообще все файлы проекта
COPY . .

# Собираем проект
RUN dotnet build "MyLittlePony_Conexy.csproj" -c Release -o /app/build

# --- ВОТ ЭТОТ БЛОК МЫ ОБНОВИЛИ ---
# Публикуем с явным указанием фреймворка net9.0
FROM build AS publish
RUN dotnet publish "MyLittlePony_Conexy.csproj" -c Release -f net9.0 -o /app/publish /p:UseAppHost=false
# --------------------------------

# Финальный образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render дает порт динамически
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MyLittlePony_Conexy.dll"]
