FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY Directory.Build.props ./
COPY Directory.Packages.props ./

COPY src/Application/Application.csproj ./src/Application/
COPY src/EmailWorker/EmailWorker.csproj ./src/EmailWorker/
COPY src/Domain/Domain.csproj ./src/Domain/

RUN dotnet new sln --name SmartFridgeManagerAPI && \
    dotnet sln add src/Application/Application.csproj && \
    dotnet sln add src/EmailWorker/EmailWorker.csproj && \
    dotnet sln add src/Domain/Domain.csproj

RUN dotnet restore

COPY src/Application ./src/Application
COPY src/EmailWorker ./src/EmailWorker
COPY src/Domain ./src/Domain

RUN dotnet publish src/Application/Application.csproj -c Release -o out

RUN ls -la /app/out

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update && apt-get install -y wget && \
    wget -q https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh && \
    chmod +x wait-for-it.sh

COPY --from=build-env /app/out .

ENV WAIT_TIMEOUT=60

RUN ls -la /app

ENTRYPOINT ["sh", "-c", "\
    ./wait-for-it.sh rabbitmq:${RABBITMQ_PORT} --timeout=${WAIT_TIMEOUT} --strict && \
    dotnet SmartFridgeManagerWorkers.Application.dll"]