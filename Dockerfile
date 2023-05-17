FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY RemoraDiscordBot.Worker.csproj .
RUN dotnet restore

COPY . .

RUN dotnet build -c Release --no-restore

RUN dotnet publish -c Release --no-restore -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

ENV PROJECT_NAME RemoraDiscordBot.Worker

CMD dotnet $PROJECT_NAME.dll
