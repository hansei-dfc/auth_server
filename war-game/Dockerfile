#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["war-game/war-game.csproj", "war-game/"]
RUN dotnet restore "war-game/war-game.csproj"
COPY . .
WORKDIR "/src/war-game"
RUN dotnet build "war-game.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "war-game.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "war-game.dll"]