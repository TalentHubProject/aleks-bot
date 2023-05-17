FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RemoraDiscordBot.Worker/RemoraDiscordBot.Worker.csproj", "RemoraDiscordBot.Worker/"]
COPY ["RemoraDiscordBot.Bot/RemoraDiscordBot.Bot.csproj", "RemoraDiscordBot.Bot/"]
COPY ["RemoraDiscordBot.Business/RemoraDiscordBot.Business.csproj", "RemoraDiscordBot.Business/"]
COPY ["RemoraDiscordBot.Data/RemoraDiscordBot.Data.csproj", "RemoraDiscordBot.Data/"]
COPY ["RemoraDiscordBot.Data.Domain/RemoraDiscordBot.Data.Domain.csproj", "RemoraDiscordBot.Data.Domain/"]
COPY ["RemoraDiscordBot.Plugins/RemoraDiscordBot.Plugins.csproj", "RemoraDiscordBot.Plugins/"]
RUN dotnet restore "RemoraDiscordBot.Worker/RemoraDiscordBot.Worker.csproj"
COPY . .
WORKDIR "/src/RemoraDiscordBot.Worker"
RUN dotnet build "RemoraDiscordBot.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RemoraDiscordBot.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RemoraDiscordBot.Worker.dll"]
