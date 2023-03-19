FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
EXPOSE 80
EXPOSE 443

WORKDIR /app/src
COPY ["src/WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

WORKDIR /app 
RUN dotnet restore "src/WebAPI/WebAPI.csproj"

WORKDIR /app/src
COPY ["src/WebAPI/*", "WebAPI/"]
COPY ["src/Domain/*", "Domain/"]
COPY ["src/Application/*", "Application/"]
COPY ["src/Infrastructure/*", "Infrastructure/"]

WORKDIR /app/src/WebAPI
FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]