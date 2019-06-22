FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.5-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2.300-stretch AS build
WORKDIR /src
COPY ["src/PiLights/PiLights.csproj", "src/PiLights/"]
RUN dotnet restore "src/PiLights/PiLights.csproj"
COPY . .
WORKDIR "/src/src/PiLights"
RUN dotnet build "PiLights.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PiLights.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PiLights.dll"]