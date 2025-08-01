# MicroServicio Tareas Colaborador

## 🎯 Enfoque de la Prueba Técnica – Desarrollador HASSQL

Este proyecto se trata de un **microservicio Full Stack basada en arquitectura cliente-servidor**, construida con **React** en el cliente, **ASP.NET Core 8** en el backend, y **SQL Server** como base de datos.

La funcionalidad se centra en la gestión de tareas asociadas a colaboradores, permitiendo su creación, consulta, actualización, eliminación y control de estado, incluyendo seguridad mediante autenticación de usuarios con **JWT**.

## 🛠 Tecnologías utilizadas

### 🖥️ Frontend
- JavaScript con [React](https://react.dev/)
- Bootstrap 5
- fetch API(Js Vanilla) para solicitud de requests HTTP

### 🔗 Backend
- .NET Core 8 (ASP.NET Web API)
- Autenticación con JWT Bearer Token
- Arquitectura en capas: Controller → Service → Repository

### 🗄️ Base de Datos
- SQL Server
- Script SQL personalizado para estructura (DDL) y datos iniciales (DML)

---

## ✅ Requisitos previos

Antes de comenzar, asegúrate de tener configurado en tu sistema:

* [Docker](https://www.docker.com/) y Docker Compose instalados correctamente
* Git
* Puerto `1435` disponible (para contenedor SQL Server)

---

## 🛠 Instalación y Configuración

### ⚙️ Levantar la aplicación con Docker

1. 📥 Clona el repositorio:

   ```bash
   git clone https://github.com/diegoslh/MicroService_Tasks.git
   cd MicroServicio_TareasColaboradores
   ```

2. 🐳 Ejecutar con Docker Compose:

   ```bash
   docker compose up --build
   ```

   Este comando levantará los siguientes servicios:

   | Servicio   | Descripción                   | Puerto local                                       |
   | ---------- | ----------------------------- | -------------------------------------------------- |
   | `frontend` | Aplicación React (UI cliente) | [http://localhost:3000](http://localhost:3000)     |
   | `api`      | Web API ASP.NET Core          | [https://localhost:44357](https://localhost:44357) |
   | `db`       | Contenedor SQL Server         | `1435`                                             |
   
---

### 📌 NOTA IMPORTANTE SOBRE LA BASE DE DATOS

> ⚠️ **La base de datos no se crea automáticamente.**
> Para que la aplicación funcione correctamente, debes ejecutar el script `database.sql` que se encuentra en la ruta:
> `Services/Scripts/database.sql`

Tienes **dos opciones** para hacerlo:

---

#### 🅰️ Opción 1: Ejecutar el script directamente dentro del contenedor

1. Abre una terminal y accede al contenedor de SQL Server:

```bash
docker exec -it BdTareasColaborador /bin/bash
```

2. Una vez dentro, ejecuta el siguiente comando para correr el script:

```bash
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Temporal1**" -i /app/Services/Scripts/database.sql
```

---

#### 🅱️ Opción 2: Conectarte desde un cliente externo como **SSMS**, **Azure Data Studio** o **DataGrip**

Puedes conectarte al contenedor de SQL Server como si fuera una instancia remota usando los siguientes datos:

| Parámetro           | Valor                       |
| ------------------- | --------------------------- |
| **Servidor (host)** | `localhost`                 |
| **Puerto**          | `1435`                      |
| **Usuario**         | `sa`                        |
| **Contraseña**      | `Temporal1**`               |
| **Base de datos**   | *(ejecutar el script)*      |

Una vez conectado, puedes ejecutar el script `database.sql` directamente desde el editor del cliente.

> 📁 El script está ubicado en: `Services/Scripts/database.sql`

---

## 🧪 Autenticación y Seguridad

* El backend protege rutas con el decorador `[Authorize]`.
* Los tokens se generan al autenticar un usuario y deben ser enviados en cada petición como:

```http
Authorization: Bearer <tu_token_jwt>
```

🔒 **Autenticación en Swagger**

Swagger muestra claramente qué endpoints requieren autenticación mediante un ícono de **candado 🔒** junto a la ruta protegida.

Además, puedes ingresar tu **token JWT** directamente en la interfaz de Swagger usando el botón **"Authorize"**, lo cual permitirá que todas las solicitudes autenticadas se realicen correctamente sin necesidad de configurar el encabezado manualmente en cada petición.

---

## 🧬 Estructura del Proyecto

```
├── Apps/                          # Aplicaciones del sistema (backend y frontend)
│   ├── api/                       # Proyecto backend .NET Core API
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Repositories/
│   │   ├── Models/
│   │   └── Dockerfile/
│   │
│   └── web/                       # Proyecto frontend React
│       ├── public/
│       ├── source/
│       ├── index.html
│       └── Dockerfile
│
├── Services/                      # Scripts, configuraciones, utilidades externas
│   └── Scripts/
│       └── database.sql           # Script de creación/configuración de base de datos
│
├── docker-compose.yml            # Orquestación de servicios con Docker Compose
├── MER                           # Modelo Entidad Relación de la Base de Datos utilizada
└── README.md                     # Documentación del proyecto
```

---

## 🔗 Endpoints principales del CRUD para tabla Tarea

### 🧾 Tareas

| Método | Ruta                             | Descripción                      |
| ------ | -------------------------------- | -------------------------------- |
| GET    | `/api/v1/Tarea?fullPayload=true` | Listado completo de tareas       |
| POST   | `/api/v1/Tarea`                  | Crear tarea                      |
| PUT    | `/api/v1/Tarea/{id}`             | Editar tarea                     |
| DELETE | `/api/v1/Tarea/{id}`             | Eliminar tarea                   |
| PATCH  | `/api/v1/Tarea/{id}/estado`      | Cambiar estado (activo/inactivo) |

> 💡 Nota: La documentación completa de los endpoints se encontrará en: [http://localhost:44357/swagger/index.html](http://localhost:44357/swagger/index.html)
---

## 🧠 Funcionalidades implementadas

| Requisito                               | Implementación                                                                                             |
| --------------------------------------- | ---------------------------------------------------------------------------------------------------------- |
| ✅ **API RESTful en C#**                 | Desarrollada con **ASP.NET Core 8**, organizada en capas: Controller, Service y Repository.                |
| ✅ **Entidad definida por el candidato** | Se implementó la entidad **Tarea**, con todos los endpoints necesarios para su gestión.                    |
| ✅ **Acceso sin ORM**                    | Se utiliza **ADO.NET** mediante una clase genérica personalizada para ejecutar consultas y comandos SQL.   |
| ✅ **Validaciones obligatorias**         | Validaciones aplicadas tanto en los modelos como en la lógica de negocio para asegurar datos consistentes. |
| ✅ **Cliente web HTML + JS**             | Frontend creado con **React** y **Bootstrap**, permitiendo consumir toda la API desde la interfaz.         |
| ✅ **Autenticación y autorización**      | Implementación de **JWT** para proteger rutas, incluyendo integración con Swagger para probar con token.   |
| ✅ **Buenas prácticas**                  | Código organizado, reutilizable, limpio, con manejo centralizado de errores y estructura clara.            |
| ✅ **Versionado en Git**                 | Todo el código fuente está versionado en este repositorio público.                                         |
| ✅ **Instrucciones completas**           | El `README.md` incluye pasos claros para levantar los servicios, ejecutar el script SQL y probar la app.   |

---
