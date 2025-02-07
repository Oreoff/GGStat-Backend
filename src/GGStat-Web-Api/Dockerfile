# 1. Базовий образ для виконання (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
USER app
EXPOSE 8080
EXPOSE 8081

# 2. Образ для зборки (з SDK, необхідним для побудови додатку)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 3. Встановлюємо dotnet-ef ПІД ROOT
USER root
RUN dotnet tool install --global dotnet-ef && \
    mkdir -p /home/app/.dotnet/tools && \
    chown -R app:app /root/.nuget /root/.dotnet /home/app/.dotnet

# Додаємо dotnet-ef в PATH
ENV PATH="$PATH:/home/app/.dotnet/tools"

# 4. Копіюємо .csproj файл (переконайтесь, що шлях вірний)
COPY ["src/GGStat-Web-Api/GGStat-Backend-Web-Api.csproj", "src/GGStat-Web-Api/"]

# 5. Відновлюємо пакети
RUN dotnet restore "src/GGStat-Web-Api/GGStat-Backend-Web-Api.csproj"

# 6. Копіюємо весь код (копіюється весь проект, включаючи .csproj, який вже скопійований)
COPY . .

# 7. Виконуємо збірку додатку
RUN dotnet build "src/GGStat-Web-Api/GGStat-Backend-Web-Api.csproj" -c Release -o /app/build

# 8. Публікуємо додаток
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/GGStat-Web-Api/GGStat-Backend-Web-Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 9. Фінальний контейнер для запуску додатку
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 10. Встановлюємо точку входу для запуску додатку
ENTRYPOINT ["sh", "-c", "dotnet GGStat-Backend-Web-Api.dll && dotnet ef database update"]