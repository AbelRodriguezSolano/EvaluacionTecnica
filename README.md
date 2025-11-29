# EvaluacionTecnica - Gestión de Usuarios y Roles

Sistema de gestión de usuarios y roles con autenticación, construido con Clean Architecture y ASP.NET Core 8 MVC.

## Descripción

Aplicación web que permite administrar usuarios y roles con las siguientes funcionalidades:
- Autenticación con usuario y contraseña
- CRUD completo de Usuarios y Roles
- Filtros de búsqueda avanzados
- Validaciones de unicidad (cédula, usuario, nombre de rol)
- Auditoría de cambios (usuario y fecha de transacción)

## Estructura del Proyecto

```
src/
├── EvaluacionTecnica.Domain/          # Entidades y contratos
├── EvaluacionTecnica.Application/     # Lógica de negocio, DTOs, servicios
├── EvaluacionTecnica.Infrastructure/  # Acceso a datos con EF Core
└── EvaluacionTecnica.Web/            # ASP.NET Core MVC

tests/
├── EvaluacionTecnica.UnitTests/       # Pruebas unitarias
└── EvaluacionTecnica.IntegrationTests/ # Pruebas de integración
```

## Tecnologías

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core 8
- SQL Server 2022
- BCrypt.Net (hash de contraseñas)
- xUnit (testing)
- Docker & Docker Compose

## Cómo Ejecutar

### Con Docker (Recomendado)

```bash
# Iniciar contenedores
docker-compose up -d

# Ejecutar la aplicación
docker-compose exec -u root app dotnet run --project src/EvaluacionTecnica.Web --urls "http://0.0.0.0:5000"

# Aplicar migraciones (desde tu máquina local)
dotnet ef database update --project src/EvaluacionTecnica.Infrastructure --startup-project src/EvaluacionTecnica.Web --connection "Server=localhost,1433;Database=EvaluacionTecnicaDB;User Id=sa;Password=MyP@ssw0rd;TrustServerCertificate=True"

# Acceder desde el navegador: http://localhost:5000
```

### Local (sin Docker)

**Opción 1: Usando Entity Framework Migrations**
```bash
# Restaurar dependencias
dotnet restore

# Aplicar migraciones
dotnet ef database update --project src/EvaluacionTecnica.Infrastructure --startup-project src/EvaluacionTecnica.Web

# Ejecutar
dotnet run --project src/EvaluacionTecnica.Web

# Acceder: http://localhost:5000
```

**Opción 2: Ejecutando el Script SQL**
```bash
# Ejecutar el script SQL en SQL Server
sqlcmd -S localhost,1433 -U sa -P YourPassword -i db/EvaluacionTecnicaDB.sql

# O usar SQL Server Management Studio (SSMS) para ejecutar db/EvaluacionTecnicaDB.sql

# Luego ejecutar la aplicación
dotnet run --project src/EvaluacionTecnica.Web

# Acceder: http://localhost:5000
```

## Credenciales de Acceso

**Usuario Admin:**
- Usuario: `ADMIN`
- Contraseña: `admin123`

**Usuario Desarrollador:**
- Usuario: `DESARROLLADOR`
- Contraseña: `aplicante123`

## Ejecutar Tests

```bash
# Todos los tests
dotnet test

# Solo unitarios
dotnet test tests/EvaluacionTecnica.UnitTests

# Solo integración
dotnet test tests/EvaluacionTecnica.IntegrationTests
```