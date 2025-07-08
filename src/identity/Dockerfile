FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/identity/IdentityServer/IdentityServer.csproj ./identity/IdentityServer/
COPY src/shared/Protos/Protos.csproj ./shared/Protos/

RUN dotnet restore ./identity/IdentityServer/IdentityServer.csproj

COPY src/identity/IdentityServer/ ./identity/IdentityServer/
COPY src/shared/Protos/ ./shared//Protos/

RUN dotnet publish ./identity/IdentityServer/IdentityServer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000
ENTRYPOINT ["dotnet", "IdentityServer.dll"]
