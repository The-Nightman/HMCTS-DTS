FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HmctsDts.Server/HmctsDts.Server.csproj", "HmctsDts.Server/"]
RUN dotnet restore "HmctsDts.Server/HmctsDts.Server.csproj"
COPY . .
WORKDIR "/src/HmctsDts.Server"
RUN dotnet build "./HmctsDts.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HmctsDts.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HmctsDts.Server.dll"]
