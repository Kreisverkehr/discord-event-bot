FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY ./app .
ENTRYPOINT ["dotnet", "DiscordEventBot.Service.dll"]
