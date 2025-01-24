FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 9000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/", "."]
RUN dotnet msbuild "./T3.Web/T3.Web.csproj" -t:restore /p:DefineConstants=DOCKER_BUILD
COPY . .
WORKDIR "/src/T3.Web"

RUN dotnet clean "./T3.Web.csproj" -c $BUILD_CONFIGURATION
RUN dotnet msbuild /p:Configuration=$BUILD_CONFIGURATION /p:DefineConstants=DOCKER_BUILD /p:RestorePackages=false

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./T3.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "T3.Web.dll"]