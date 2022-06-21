ARG image_dotnet_sdk='mcr.microsoft.com/dotnet/sdk:6.0'
ARG image_dotnet_aspnet='mcr.microsoft.com/dotnet/aspnet:6.0.1-alpine3.14'

# syntax=docker/dockerfile:1
FROM $image_dotnet_sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM $image_dotnet_aspnet
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Auth.Web.dll"]