FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER root
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GGStat-Backend.ParsingDataService/GGStat-Backend.ParsingDataService.csproj", "src/GGStat-Backend.ParsingDataService/"]
COPY ["GGStat-Backend.Infrastructure/GGStat-Backend.Infrastructure.csproj", "GGStat-Backend.Infrastructure/"]
RUN dotnet restore "src/GGStat-Backend.ParsingDataService/GGStat-Backend.ParsingDataService.csproj"
COPY . .
WORKDIR "/src/src/GGStat-Backend.ParsingDataService"
RUN dotnet build "./GGStat-Backend.ParsingDataService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GGStat-Backend.ParsingDataService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN mkdir -p /app/db && chmod 777 /app/db
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GGStat-Backend.ParsingDataService.dll"]
