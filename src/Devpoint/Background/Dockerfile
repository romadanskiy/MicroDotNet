FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Background/Background.csproj", "Background/"]
RUN dotnet restore "Background/Background.csproj"
COPY . .
WORKDIR "/src/Background"
RUN dotnet build "Background.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Background.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Background.dll"]
