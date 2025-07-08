FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/services/UserService/    ./services/UserService/
COPY src/shared/                  ./shared/

RUN dotnet restore services/UserService/UserService.API/UserService.API.csproj
RUN dotnet publish services/UserService/UserService.API/UserService.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000

ENTRYPOINT ["dotnet", "UserService.API.dll"]
