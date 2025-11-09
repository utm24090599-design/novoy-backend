# Novoy Backend - Documentación Completa

## 📋 Tabla de Contenidos
- [Descripción General](#descripción-general)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Arquitectura del Sistema](#arquitectura-del-sistema)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Modelos de Datos](#modelos-de-datos)
- [Endpoints API](#endpoints-api)
- [Configuración e Instalación](#configuración-e-instalación)
- [Base de Datos](#base-de-datos)
- [Lógica de Negocio](#lógica-de-negocio)

---

## 🎯 Descripción General

**Novoy Backend** es una API RESTful desarrollada en ASP.NET Core que gestiona un sistema de transporte público inteligente. El sistema simula y gestiona paradas de autobús con generación aleatoria de pasajeros clasificados por categorías (estudiantes, niños, adultos mayores y personas regulares), además de manejar información de autobuses y tarifas.

### Características Principales
- Generación automática de escenarios de paradas de autobús
- Clasificación inteligente de pasajeros por edad
- Gestión de precios diferenciados por categoría
- Administración de flota de autobuses
- Integración con PostgreSQL
- API RESTful con documentación Swagger

---

## 🛠 Tecnologías Utilizadas

### Backend Framework
- **ASP.NET Core** - Framework principal
- **.NET 6/7/8** - Plataforma de desarrollo
- **Entity Framework Core** - ORM para base de datos

### Base de Datos
- **PostgreSQL** - Sistema de gestión de base de datos
- **Npgsql** - Provider para PostgreSQL

### Documentación y Desarrollo
- **Swagger/OpenAPI** - Documentación interactiva de API
- **CORS** - Configurado para frontend en localhost:5173

---

## 🏗 Arquitectura del Sistema

El proyecto sigue una arquitectura en capas:

```
┌─────────────────────────────────────┐
│         Frontend (React)            │
│      http://localhost:5173          │
└───────────────┬─────────────────────┘
                │ HTTP/REST
┌───────────────▼─────────────────────┐
│        Controllers Layer            │
│  - BusStopGnController              │
│  - PeopleController                 │
│  - BusController                    │
│  - PriceController                  │
└───────────────┬─────────────────────┘
                │
┌───────────────▼─────────────────────┐
│      Business Logic Layer           │
│  - Generación de pasajeros          │
│  - Clasificación por categorías     │
│  - Cálculos de porcentajes          │
└───────────────┬─────────────────────┘
                │
┌───────────────▼─────────────────────┐
│        Data Access Layer            │
│      (Entity Framework Core)        │
│        - AppDbContext               │
└───────────────┬─────────────────────┘
                │
┌───────────────▼─────────────────────┐
│       PostgreSQL Database           │
│  - person                           │
│  - price                            │
│  - bus                              │
└─────────────────────────────────────┘
```

---

## 📁 Estructura del Proyecto

```
novoy-backend/
│
├── Controllers/
│   ├── BusStopGnController.cs       # Generación de paradas
│   ├── PeopleController.cs          # Gestión de personas
│   ├── BusController.cs             # Gestión de autobuses
│   └── PriceController.cs           # Gestión de precios
│
├── Models/
│   ├── Person.cs                    # Modelo de persona
│   ├── Price.cs                     # Modelo de precios
│   ├── Bus.cs                       # Modelo de autobús
│   └── BusStopGn.cs                 # Modelo de parada generada
│
├── Dtos/
│   └── PersonWithCategoryDto.cs     # DTO con categorización
│
├── Context/
│   └── AppDbContext.cs              # Contexto de base de datos
│
├── Program.cs                        # Punto de entrada
└── appsettings.json                 # Configuración
```

---

## 📊 Modelos de Datos

### Person (Persona)
Representa un pasajero en el sistema.

**Propiedades:**
- `Id` (long): Identificador único
- `Age` (int): Edad del pasajero
- `Gender` (string): Género (M/F)

**Tabla:** `person`

### Price (Precio)
Define las tarifas por categoría de pasajero.

**Propiedades:**
- `Id` (long): Identificador único
- `Children` (double): Tarifa para niños
- `Old` (double): Tarifa para adultos mayores
- `Student` (double): Tarifa para estudiantes
- `Any_Person` (double): Tarifa regular

**Tabla:** `price`

### Bus (Autobús)
Información de los autobuses de la flota.

**Propiedades:**
- `Id` (long): Identificador único
- `Max_Capacity` (int): Capacidad máxima
- `Number_Plate` (string): Placa del vehículo
- `Route` (string): Ruta asignada

**Tabla:** `bus`

### BusStopGn (Parada Generada)
Modelo para simular una parada de autobús.

**Propiedades:**
- `PeopleIn` (int): Total de personas
- `StudentsCantity` (int?): Cantidad de estudiantes
- `ChildrenCantity` (int?): Cantidad de niños
- `OldCantity` (int?): Cantidad de adultos mayores
- `AnyPersonCantity` (int?): Cantidad de personas regulares

### PersonWithCategoryDto
DTO que extiende Person con categorización.

**Propiedades:**
- `Id` (long)
- `Age` (int)
- `Gender` (string)
- `Category` (string): "Estudiante", "Niño", "Viejo", "Otro"

---

## 🔌 Endpoints API

### Resumen de Endpoints

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/BusStopGn` | Genera parada completa con distribución |
| GET | `/BusStopGn/GetBusStop` | Solo retorna total de personas |
| GET | `/BusStopGn/GetClassifiedPeople` | Genera personas desde BD con categorías |
| GET | `/People/GetPeople` | Lista todas las personas |
| GET | `/GetBuses` | Lista todos los autobuses |
| GET | `/GetPrices` | Obtiene tarifas |

### 1. BusStopGnController

**IMPORTANTE:** Este controlador genera valores aleatorios en el **constructor**, por lo que cada instancia del controlador (cada petición HTTP) tendrá valores diferentes.

#### `GET /BusStopGn` (Método: GenerateBusStop)
Genera una parada de autobús completa con distribución aleatoria de pasajeros. Este es el endpoint principal que retorna todos los detalles de la simulación.

**Response:**
```json
{
  "peopleIn": 45,
  "studentsCantity": 7,
  "childrenCantity": 3,
  "oldCantity": 12,
  "anyPersonCantity": 23
}
```

**Lógica:**
- Total aleatorio: 1-80 personas (generado en constructor)
- Estudiantes: máx 20%
- Niños: máx 10%
- Adultos mayores: máx 35%
- Normalización automática si excede 100%
- Los valores se calculan una vez por petición HTTP

**Nota técnica:** El loop `for (int i = 0; i < _peopleIn; i++)` en el código está vacío, posiblemente preparado para funcionalidad futura.

#### `GET /BusStopGn/GetBusStop`
Obtiene solo el total de personas en la parada.

**Response:**
```json
{
  "peopleIn": 45
}
```

#### `GET /BusStopGn/GetClassifiedPeople`
Genera personas clasificadas desde la base de datos.

**Response:**
```json
{
  "total": 45,
  "personas": [
    {
      "id": 1,
      "age": 22,
      "gender": "M",
      "category": "Estudiante"
    },
    ...
  ]
}
```

**Reglas de Clasificación:**
- **Niño**: edad < 16
- **Estudiante**: 16 ≤ edad ≤ 25
- **Viejo**: edad > 60
- **Otro**: resto

### 2. PeopleController

#### `GET /People/GetPeople`
Lista todas las personas registradas.

**Response:**
```json
{
  "total": 150,
  "people": [
    {
      "id": 1,
      "age": 25,
      "gender": "F"
    },
    ...
  ]
}
```

### 3. BusController

#### `GET /GetBuses`
Lista todos los autobuses de la flota.

**Response:**
```json
[
  {
    "id": 1,
    "max_Capacity": 80,
    "number_Plate": "ABC-1234",
    "route": "Ruta 1"
  },
  ...
]
```

### 4. PriceController

#### `GET /GetPrices`
Obtiene las tarifas actuales.

**Response:**
```json
[
  {
    "id": 1,
    "children": 5.50,
    "old": 7.00,
    "student": 8.00,
    "any_Person": 12.00
  }
]
```

---

## ⚙️ Configuración e Instalación

### Prerrequisitos
- .NET SDK 6.0 o superior
- PostgreSQL 12 o superior
- IDE: Visual Studio / VS Code / Rider

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/utm24090599-design/novoy-backend.git
cd novoy-backend
```

2. **Configurar la cadena de conexión**

Editar `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "PostgreSQLConnection": "Host=localhost;Database=novoy_db;Username=tu_usuario;Password=tu_password"
  }
}
```

3. **Crear la base de datos**
```bash
dotnet ef database update
```

4. **Restaurar dependencias**
```bash
dotnet restore
```

5. **Ejecutar el proyecto**
```bash
dotnet run
```

6. **Acceder a Swagger**
```
https://localhost:[puerto]/swagger
```

### Configuración CORS

El backend está configurado para aceptar peticiones desde:
- `http://localhost:5173` (Frontend React/Vite)

Para agregar más orígenes, modificar `Program.cs`:
```csharp
policy.WithOrigins("http://localhost:5173", "http://otro-origen")
```

---

## 🗄 Base de Datos

### Esquema de Tablas

#### Tabla: person
```sql
CREATE TABLE person (
    id BIGSERIAL PRIMARY KEY,
    age INTEGER NOT NULL,
    gender VARCHAR(10) NOT NULL
);
```

#### Tabla: price
```sql
CREATE TABLE price (
    id BIGSERIAL PRIMARY KEY,
    children DOUBLE PRECISION NOT NULL,
    old DOUBLE PRECISION NOT NULL,
    student DOUBLE PRECISION NOT NULL,
    any_person DOUBLE PRECISION NOT NULL
);
```

#### Tabla: bus
```sql
CREATE TABLE bus (
    id BIGSERIAL PRIMARY KEY,
    max_capacity INTEGER NOT NULL,
    number_plate VARCHAR(20) NOT NULL,
    route VARCHAR(100) NOT NULL
);
```

### Datos de Ejemplo

```sql
-- Precios iniciales
INSERT INTO price (children, old, student, any_person)
VALUES (5.50, 7.00, 8.00, 12.00);

-- Personas de ejemplo
INSERT INTO person (age, gender)
VALUES 
    (10, 'M'), (22, 'F'), (65, 'M'),
    (18, 'F'), (45, 'M'), (8, 'F');

-- Autobuses
INSERT INTO bus (max_capacity, number_plate, route)
VALUES 
    (80, 'ABC-1234', 'Ruta Centro'),
    (60, 'XYZ-5678', 'Ruta Norte');
```

---

## 🧮 Lógica de Negocio

### Arquitectura del Controlador BusStopGnController

**Característica Especial:** Este controlador implementa una arquitectura única donde los cálculos se realizan en el **constructor**, no en los métodos de acción. Esto significa:

1. **Por cada petición HTTP** se crea una nueva instancia del controlador
2. El constructor genera nuevos valores aleatorios
3. Los métodos HTTP simplemente retornan estos valores pre-calculados
4. Cada petición obtiene una simulación diferente

```csharp
public BusStopGnController(AppDbContext context)
{
    _context = context;
    _random = new Random();
    _peopleIn = _random.Next(1, 81); // Se genera AQUÍ
    // ... resto de cálculos
}
```

### Algoritmo de Generación de Paradas

El `BusStopGnController` implementa un algoritmo sofisticado:

1. **Generación de Total Aleatorio**
   - Rango: 1-80 personas
   - Usa `Random.Next(1, 81)`

2. **Asignación de Porcentajes**
   - Estudiantes: 0-20%
   - Niños: 0-10%
   - Adultos mayores: 0-35%
   - Otros: restante

3. **Normalización**
   ```csharp
   if (totalPercent > 1.0) {
       double factor = 1.0 / totalPercent;
       // Aplicar factor a cada porcentaje
   }
   ```

4. **Cálculo de Cantidades**
   ```csharp
   _students = (int)Math.Floor(_percentStudents * _peopleIn);
   // Repetir para cada categoría
   _others = _peopleIn - (suma de otros);
   ```

### Clasificación Automática

El endpoint `GetClassifiedPeople` clasifica personas según edad:

```csharp
string category = 
    age < 16 ? "Niño"
    : age > 60 ? "Viejo"
    : age >= 16 && age <= 25 ? "Estudiante"
    : "Otro";
```

---

## 🔐 Seguridad y Buenas Prácticas

### Implementadas
✅ Uso de DTO para transferencia de datos
✅ Entity Framework previene SQL Injection
✅ CORS configurado específicamente
✅ Separación de capas (Controllers/Models/Context)

### Recomendaciones Futuras
- [ ] Implementar autenticación JWT
- [ ] Agregar validación de modelos con FluentValidation
- [ ] Implementar logging con Serilog
- [ ] Agregar manejo de errores global
- [ ] Implementar paginación en listados
- [ ] Agregar unit tests
- [ ] Implementar cache con Redis

---

## 📈 Casos de Uso

### 1. Simulación de Parada
```
Usuario → GET /BusStopGn
    ↓
Sistema genera números aleatorios
    ↓
Clasifica por categorías
    ↓
Retorna distribución
```

### 2. Consulta de Tarifas
```
Usuario → GET /GetPrices
    ↓
Sistema consulta BD
    ↓
Retorna precios actuales
```

### 3. Generación de Pasajeros Clasificados
```
Usuario → GET /BusStopGn/GetClassifiedPeople
    ↓
Sistema obtiene personas de BD
    ↓
Selecciona aleatoriamente
    ↓
Clasifica por edad
    ↓
Retorna lista clasificada
```

---

## 🤝 Contribución

Para contribuir al proyecto:

1. Fork el repositorio
2. Crear rama feature (`git checkout -b feature/NuevaCaracteristica`)
3. Commit cambios (`git commit -m 'Agregar nueva característica'`)
4. Push a la rama (`git push origin feature/NuevaCaracteristica`)
5. Crear Pull Request

---

## 📞 Soporte

Para reportar bugs o solicitar características:
- Abrir un issue en GitHub
- Contactar al equipo de desarrollo

---

## 📄 Licencia

[Especificar licencia del proyecto]

---

**Última actualización:** Noviembre 2025
**Versión:** 1.0.0