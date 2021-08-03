#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim-arm32v7 AS base
FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DiscordEventBot.Service/DiscordEventBot.Service.csproj", "DiscordEventBot.Service/"]
COPY ["DiscordEventBot.Model/DiscordEventBot.Model.csproj", "DiscordEventBot.Model/"]
COPY ["DiscordEventBot.Common/DiscordEventBot.Common.csproj", "DiscordEventBot.Common/"]
COPY ["DiscordEventBot.Common/DiscordEventBot.Jobs.csproj", "DiscordEventBot.Jobs/"]
RUN dotnet restore "DiscordEventBot.Service/DiscordEventBot.Service.csproj"
COPY . .
WORKDIR "/src/DiscordEventBot.Service"
RUN dotnet build "DiscordEventBot.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordEventBot.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordEventBot.Service.dll"]
