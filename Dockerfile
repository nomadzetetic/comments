FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY app/ ./app
COPY data/ ./data
COPY core/ ./core
COPY services/ ./services
COPY comments.sln .
RUN dotnet restore ./comments.sln
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app
COPY --from=build-env /app/dist .
ENTRYPOINT ["dotnet", "comments.app.dll"]
