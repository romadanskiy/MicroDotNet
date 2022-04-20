FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Scanner/Scanner.csproj", "Scanner/"]
RUN dotnet restore "Scanner/Scanner.csproj"
COPY . .
WORKDIR "/src/Scanner"
RUN dotnet build "Scanner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Scanner.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scanner.dll"]
