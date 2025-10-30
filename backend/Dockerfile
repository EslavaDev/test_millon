# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["RealEstate.sln", "./"]
COPY ["src/RealEstate.Api/RealEstate.Api.csproj", "src/RealEstate.Api/"]
COPY ["src/RealEstate.Application/RealEstate.Application.csproj", "src/RealEstate.Application/"]
COPY ["src/RealEstate.Domain/RealEstate.Domain.csproj", "src/RealEstate.Domain/"]
COPY ["src/RealEstate.Infrastructure/RealEstate.Infrastructure.csproj", "src/RealEstate.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/RealEstate.Api/RealEstate.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/src/RealEstate.Api"
RUN dotnet build "RealEstate.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "RealEstate.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "RealEstate.Api.dll"]
