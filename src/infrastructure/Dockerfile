FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/infrastructure/ApiGateway/ApiGateway.csproj ./infrastructure/ApiGateway/

RUN dotnet restore ./infrastructure/ApiGateway/ApiGateway.csproj

COPY src/infrastructure/ApiGateway/ ./infrastructure/ApiGateway/

RUN dotnet publish ./infrastructure/ApiGateway/ApiGateway.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
