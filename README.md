# Dirección General de Impuestos Internos (DGII) - Prueba Técnica

Solución para la consulta de contribuyentes, visualización de comprobantes fiscales y sumatoria exacta del ITBIS (18%).

---

## Tecnologías utilizadas

- **Backend:** .NET 9 (C# 13), ASP.NET Core Web API.
- **Acceso a Datos:** Entity Framework Core 9 con SQLite .
- **Frontend:** React , TypeScript, Vite, Tailwind CSS, Lucide Icons.
- **Testing:** xUnit, Moq, FluentAssertions y SQLite In-Memory.


## Arquitectura del Sistema


```
prueba-tecnica/
├── DGII.sln
├── src/
│   ├── DGII.Domain/          # Núcleo: Entidades puras e Interfaces de repositorio
│   ├── DGII.Application/     # Casos de uso, DTOs, Servicios de cálculo y Excepciones
│   ├── DGII.Infrastructure/  # EF Core SQLite, Mapeos, Repositorios y Seed Data
│   ├── DGII.API/             # Controllers REST, Middleware y Swagger
│   └── client-app/           # Frontend React + TypeScript + Vite + Tailwind CSS
└── tests/
    └── DGII.UnitTests/       # Pruebas unitarias
```

---


##  Guía de Ejecución y Pruebas

### 1. Pruebas Unitarias

```bash
dotnet test
```

### 2. Backend 
Desde la raíz del proyecto:
```bash
dotnet run --project src/DGII.API
```
- Probar API
- **Swagger :** [http://localhost:5000/swagger](http://localhost:5000/swagger).

#### Endpoints Principales:
- `GET /api/contribuyentes`: Lista de todos los contribuyentes.
- `GET /api/contribuyentes/98754321012`: Detalle a partir del rnc/cedula especifico.
- `GET /api/comprobantes-fiscales`: Todos los comprobantes reportados.
- `GET /api/contribuyentes/98754321012/comprobantes-fiscales`: Comprobantes a partir del rnc/cedula especifico.
- `GET /api/contribuyentes/98754321012/resumen`: Resumen completo con la sumatoria del ITBIS

### 3. Frontend 

```bash
cd src/client-app
npm install
npm run dev
```
**Abrir en el navegador:** [http://localhost:5173](http://localhost:5173). 

