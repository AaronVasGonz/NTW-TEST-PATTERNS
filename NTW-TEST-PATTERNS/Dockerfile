# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NTW-TEST-PATTERNS/NTW-TEST-PATTERNS.csproj", "NTW-TEST-PATTERNS/"]
COPY ["Data/NWT-TEST-PATTERNS.Data.csproj", "Data/"]
COPY ["NTW-TEST-PATTERNS.Arquitecture/NTW-TEST-PATTERNS.Arquitecture.csproj", "NTW-TEST-PATTERNS.Arquitecture/"]
COPY ["NTW-TEST-PATTERNS.Models/NTW-TEST-PATTERNS.Models.csproj", "NTW-TEST-PATTERNS.Models/"]
COPY ["Service/NTW-TEST-PATTERNS.Service.csproj", "Service/"]
RUN dotnet restore "./NTW-TEST-PATTERNS/NTW-TEST-PATTERNS.csproj"
COPY . .
WORKDIR "/src/NTW-TEST-PATTERNS"
RUN dotnet build "./NTW-TEST-PATTERNS.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NTW-TEST-PATTERNS.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NTW-TEST-PATTERNS.dll"]