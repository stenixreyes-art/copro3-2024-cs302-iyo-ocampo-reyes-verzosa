FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Morgan Thieves/Morgan Thieves.csproj", "Morgan Thieves/"]

RUN dotnet restore "Morgan Thieves/Morgan Thieves.csproj"

COPY . .

WORKDIR "/src/Morgan Thieves"
RUN dotnet build "Morgan Thieves.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Morgan Thieves.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Morgan Thieves.dll"]
