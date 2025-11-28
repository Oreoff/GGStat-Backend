FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base

# Install Kerberos library (REQUIRED for PostgreSQL)
USER root
RUN apt-get update && apt-get install -y krb5-user libkrb5-3

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/GGStat-Backend.ImporterService/GGStat-Backend.ImporterService.csproj", "src/GGStat-Backend.ImporterService/"]
COPY ["GGStat-Backend.Data/GGStat-Backend.Data.csproj", "GGStat-Backend.Data/"]
COPY ["GGStat-Backend.Infrastructure/GGStat-Backend.Infrastructure.csproj", "GGStat-Backend.Infrastructure/"]

RUN dotnet restore "src/GGStat-Backend.ImporterService/GGStat-Backend.ImporterService.csproj"

COPY . .

WORKDIR "/src/src/GGStat-Backend.ImporterService"
RUN dotnet build "./GGStat-Backend.ImporterService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GGStat-Backend.ImporterService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GGStat-Backend.ImporterService.dll"]