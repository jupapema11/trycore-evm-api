# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Trycore.EVM.API/Trycore.EVM.API.csproj Trycore.EVM.API/
COPY Trycore.EVM.Application/Trycore.EVM.Application.csproj Trycore.EVM.Application/
COPY Trycore.EVM.Domain/Trycore.EVM.Domain.csproj Trycore.EVM.Domain/
COPY Trycore.EVM.Infrastructure/Trycore.EVM.Infrastructure.csproj Trycore.EVM.Infrastructure/

RUN dotnet restore Trycore.EVM.API/Trycore.EVM.API.csproj

COPY . .
RUN dotnet publish Trycore.EVM.API/Trycore.EVM.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

EXPOSE 8080
ENTRYPOINT ["/docker-entrypoint.sh"]
