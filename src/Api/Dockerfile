# Etapa 1 — build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia solo los .csproj primero (aprovecha caché de Docker)
COPY src/Domain/OrderSystem.Domain.csproj src/Domain/
COPY src/application/OrderSystem.Application.csproj src/application/
COPY src/Infrastructure/OrderSystem.Infrastructure.csproj src/Infrastructure/
COPY src/Api/OrderSystem.Api.csproj src/Api/

RUN dotnet restore src/Api/OrderSystem.Api.csproj

# Copia el resto del código
COPY src/ src/

RUN dotnet publish src/Api/OrderSystem.Api.csproj -c Release -o /out

# Etapa 2 — runtime (imagen más liviana)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Buena práctica: no correr como root
RUN adduser --disabled-password --gecos "" appuser
USER appuser

COPY --from=build /out .

ENTRYPOINT ["dotnet", "OrderSystem.Api.dll"]