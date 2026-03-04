FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy project file for better restore caching
COPY ["MyLittlePony_Conexy/MyLittlePony_Conexy.csproj", "MyLittlePony_Conexy/"]

# Restore dependencies explicitly for the csproj
RUN dotnet restore "MyLittlePony_Conexy/MyLittlePony_Conexy.csproj"

# Copy the rest of the source
COPY . .

# Publish the application
RUN dotnet publish "MyLittlePony_Conexy/MyLittlePony_Conexy.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Let Render (or the platform) inject connection strings via env vars
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "MyLittlePony_Conexy.dll"]

