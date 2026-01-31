FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5120

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GGStat-Web-Api/GGStat-Backend.Web-Api.csproj", "src/GGStat-Web-Api/"]
COPY ["GGStat-Backend.Data/GGStat-Backend.Data.csproj", "GGStat-Backend.Data/"]
COPY ["GGStat-Backend.Infrastructure/GGStat-Backend.Infrastructure.csproj", "GGStat-Backend.Infrastructure/"]

RUN dotnet restore "src/GGStat-Web-Api/GGStat-Backend.Web-Api.csproj"
COPY . .
WORKDIR "/src/src/GGStat-Web-Api"
RUN dotnet build "./GGStat-Backend.Web-Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GGStat-Backend.Web-Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GGStat-Backend.Web-Api.dll"]
