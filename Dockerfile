# Use official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore dependencies
COPY ProductCatalog.Api.sln ./
COPY ProductCatalog.Api/ProductCatalog.Api.csproj ProductCatalog.Api/
COPY ProductCatalog.Application/ProductCatalog.Application.csproj ProductCatalog.Application/
COPY ProductCatalog.Domain/ProductCatalog.Domain.csproj ProductCatalog.Domain/
COPY ProductCatalog.Infrastructure/ProductCatalog.Infrastructure.csproj ProductCatalog.Infrastructure/
RUN dotnet restore

# Copy everything and build
COPY . .
WORKDIR /src/ProductCatalog.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.Api.dll"]
