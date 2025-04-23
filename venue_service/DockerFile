# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia a solução e o projeto
COPY venue_service/venue_service.csproj ./venue_service/
RUN dotnet restore ./venue_service/venue_service.csproj

# Copia tudo e publica
COPY . ./
RUN dotnet publish ./venue_service/venue_service.csproj -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "venue_service.dll"]
