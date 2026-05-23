# OrderSystem

Proyecto de ejemplo para gestión de órdenes (API + Worker) desarrollado en .NET 8.

**Descripción**
- `OrderSystem` expone una API web (en `src/Api`) que permite crear y gestionar órdenes.
- Un `Worker` (en `src/Worker`) consume mensajes (por ejemplo via bus) para procesar órdenes en background.
- La capa `application` contiene DTOs e interfaces, `domain` las entidades y `infrastructure` la persistencia con EF Core.

**Estructura principal**
- [src/Api](src/Api): proyecto ASP.NET Core para la API.
- [src/Worker](src/Worker): servicio en background/consumer.
- [src/application](src/application): lógica de aplicación, DTOs e interfaces.
- [src/Domain](src/Domain): entidades y enums.
- [src/Infrastructure](src/Infrastructure): EF Core, repositorios y migraciones.
- [docker-compose.yml](docker-compose.yml): orquestación Docker (si aplica).

**Requisitos**
- .NET 8 SDK
- Docker (opcional, para ejecutar contenedores)
- (Opcional) `gh` CLI para interacción con GitHub

**Configuración**
- Copia `.env.example` a `.env` y ajusta las variables necesarias.
- Los archivos de configuración de la API y Worker están en `src/Api/appsettings.json` y `src/Worker/appsettings.json`.

**Ejecutar local (desarrollo)**
- Restaurar y ejecutar la API desde la raíz:

```bash
# desde la raíz del repo
dotnet restore src/Api/OrderSystem.Api.csproj
dotnet run --project src/Api/OrderSystem.Api.csproj
```

- Ejecutar el Worker:

```bash
dotnet run --project src/Worker/OrderSystem.Worker.csproj
```

**Usando Docker**
- Construir y ejecutar con Docker Compose (usa los `Dockerfile` en `src/Api` y `src/Worker`):

```bash
docker compose up --build
```

- Para construir sólo la imagen de la API:

```bash
docker build -f src/Api/Dockerfile -t ordersystem-api:latest .
```

**Migraciones y base de datos**
- Las migraciones EF Core están en `src/Infrastructure/Migrations`.
- Para aplicar migraciones (requiere dotnet-ef o ejecutar desde el contexto del proyecto):

```bash
dotnet ef database update --project src/Infrastructure/OrderSystem.Infrastructure.csproj --startup-project src/Api/OrderSystem.Api.csproj
```

(Si no tienes `dotnet-ef` globalmente instalado, instala con `dotnet tool install --global dotnet-ef`.)

**Prácticas y notas**
- Se han añadido `Dockerfile` optimizados por etapas en [src/Api/Dockerfile](src/Api/Dockerfile).
- Evita subir secretos: revisa `.env` y usa variables de entorno en CI.

**Contribuir**
1. Haz fork o crea una rama nueva (`git switch -c feat/mi-cambio`).
2. Abre PR describiendo los cambios.

**Contacto**
- Responsable: Jerel Posada

---

Si quieres puedo añadir badges, un ejemplo de petición curl o explicar las variables de `.env`.