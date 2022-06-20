FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN mkdir Resources
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Web.API/Web.API.csproj", "Web.API/"]
COPY ["Data.Core/Data.Core.csproj", "Data.Core/"]
COPY ["Domain.Developers/Domain.Developers.csproj", "Domain.Developers/"]
COPY ["Domain.Content/Domain.Content.csproj", "Domain.Content/"]
COPY ["Domain.Subscriptions/Domain.Subscriptions.csproj", "Domain.Subscriptions/"]
COPY ["Domain.Payments/Domain.Payments.csproj", "Domain.Payments/"]
COPY ["Domain.Users/Domain.Users.csproj", "Domain.Users/"]
COPY ["Services.Developers/Services.Developers.csproj", "Services.Developers/"]
COPY ["Services.Subscriptions/Services.Subscriptions.csproj", "Services.Subscriptions/"]
COPY ["Services.Payments/Services.Payments.csproj", "Services.Payments/"]
COPY ["Services.Content/Services.Content.csproj", "Services.Content/"]
COPY ["Services.Users/Services.Users.csproj", "Services.Users/"]
RUN dotnet restore "Web.API/Web.API.csproj"
COPY . .
WORKDIR /src/Web.API
RUN dotnet build "Web.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.API.dll"]
