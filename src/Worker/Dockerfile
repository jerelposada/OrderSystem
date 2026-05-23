FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY src/Domain/OrderSystem.Domain.csproj src/Domain/
COPY src/application/OrderSystem.Application.csproj src/application/
COPY src/Infrastructure/OrderSystem.Infrastructure.csproj src/Infrastructure/
COPY src/Worker/OrderSystem.Worker.csproj src/Worker/

RUN dotnet restore src/Worker/OrderSystem.Worker.csproj

COPY src/ src/

RUN dotnet publish src/Worker/OrderSystem.Worker.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser
USER appuser

COPY --from=build /out .

ENTRYPOINT ["dotnet", "OrderSystem.Worker.dll"]