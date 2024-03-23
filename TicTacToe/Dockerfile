FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TicTacToe/TicTacToe.csproj", "TicTacToe/"]
RUN dotnet restore "TicTacToe/TicTacToe.csproj"
COPY . .
WORKDIR "/src/TicTacToe"
RUN dotnet build "TicTacToe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicTacToe.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicTacToe.dll"]
