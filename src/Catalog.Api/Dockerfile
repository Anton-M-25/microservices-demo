# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["src/Catalog.Api/Catalog.Api.csproj", "src/Catalog.Api/"]
RUN dotnet restore "src/Catalog.Api/Catalog.Api.csproj"

# Copy everything else and publish
COPY . .
RUN dotnet publish "src/Catalog.Api/Catalog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# ASP.NET Core listens on 8080 inside container (best practice)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]