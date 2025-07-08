FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/services/FileService/    ./services/FileService/
COPY src/shared/Protos/           ./shared/Protos

RUN dotnet restore services/FileService/FileService.API/FileService.API.csproj
RUN dotnet publish services/FileService/FileService.API/FileService.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000

ENTRYPOINT ["dotnet", "FileService.API.dll"]
