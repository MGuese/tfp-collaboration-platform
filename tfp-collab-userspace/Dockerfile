FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["tfp-collab-userspace/tfp-collab-userspace.csproj", "tfp-collab-userspace/"]
RUN dotnet restore "tfp-collab-userspace/tfp-collab-userspace.csproj"
COPY . .
WORKDIR "/src/tfp-collab-userspace"
RUN dotnet build "./tfp-collab-userspace.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./tfp-collab-userspace.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "tfp-collab-userspace.dll"]
