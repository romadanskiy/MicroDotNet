FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.sln .
COPY RuOverflow.Questions/*.csproj ./RuOverflow.Questions/
COPY RuOverflow.Questions.Infrastructure/*.csproj ./RuOverflow.Questions.Infrastructure/

RUN dotnet restore

# copy everything else and build app
COPY RuOverflow.Questions/. ./RuOverflow.Questions/
COPY RuOverflow.Questions.Infrastructure/. ./RuOverflow.Questions.Infrastructure/

WORKDIR /app/RuOverflow.Questions
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/RuOverflow.Questions/out ./
ENTRYPOINT ["dotnet", "RuOverflow.Questions.dll"]
