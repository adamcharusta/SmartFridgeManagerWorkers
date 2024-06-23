FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY Directory.Build.props ./
COPY Directory.Packages.props ./

COPY src/Application/Application.csproj ./src/Application/
COPY src/EmailWorker/EmailWorker.csproj ./src/EmailWorker/

RUN dotnet new sln --name SmartFridgeManagerAPI && \
    dotnet sln add src/Application/Application.csproj && \
    dotnet sln add src/EmailWorker/EmailWorker.csproj

RUN dotnet restore

COPY src/Application ./src/Application
COPY src/EmailWorker ./src/EmailWorker

RUN dotnet publish src/Application/Application.csproj -c Release -o out

RUN ls -la /app/out

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

RUN ls -la /app

ENTRYPOINT ["dotnet", "SmartFridgeManagerWorkers.Application.dll"]