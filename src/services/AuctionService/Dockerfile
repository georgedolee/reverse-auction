FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/services/AuctionService/    ./services/AuctionService/
COPY src/shared/				  ./shared/

RUN dotnet restore services/AuctionService/AuctionService.API/AuctionService.API.csproj
RUN dotnet publish services/AuctionService/AuctionService.API/AuctionService.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8000
EXPOSE 8000

ENTRYPOINT ["dotnet", "AuctionService.API.dll"]
