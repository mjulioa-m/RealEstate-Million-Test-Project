<<<<<<< HEAD
# Prueba Técnica: Aplicación Full-Stack de Bienes Raíces

Este proyecto es una aplicación web Full-Stack construida como parte de una prueba técnica. La solución incluye una API RESTful desarrollada en .NET 8 y un frontend en Next.js (React) que consume dicha API.

La aplicación permite a los usuarios ver, filtrar y buscar propiedades de bienes raíces.

## 🚀 Stack Tecnológico

* **Backend:**
    * .NET 8 (C#)
    * API Mínima / Web API
    * MongoDB (usando el driver oficial de MongoDB)
    * NUnit (para Pruebas Unitarias)
* **Frontend:**
    * Next.js 13+ (App Router)
    * React
    * TypeScript
    * `react-hot-toast` (para notificaciones)
* **Base de Datos:**
    * MongoDB (local, Docker, o Atlas)

## ✨ Características

### Backend (API)
* **Arquitectura Limpia:** Separación de responsabilidades usando `Domain`, `Repositories`, `DTOs` y `Controllers`.
* **Filtrado Avanzado:** El endpoint `GET /api/properties` permite filtrar por `name`, `address`, `minPrice` y `maxPrice`.
* **Paginación:** Soporte para paginación (`page` y `pageSize`) para manejar grandes volúmenes de datos.
* **Operaciones CRUD:** Funcionalidad completa para Crear, Leer, Actualizar y Eliminar propiedades.
* **Endpoints Específicos:** Incluye un endpoint `POST /batch` para inserción masiva.
* **Pruebas Unitarias:** Cobertura de tests (NUnit) para el `PropertiesController`, asegurando la lógica de negocio y el mapeo de DTOs.

### Frontend (Web)
* **Listado y Filtros:** Página principal que muestra propiedades y permite filtrar en tiempo real.
* **Carga "Infinita":** Botón "Cargar más" que trae la siguiente página de resultados sin recargar la página.
* **Página de Detalles:** Vista de detalles de la propiedad (ruta `property/[id]`).
* **Diseño Responsivo:** Interfaz adaptable a dispositivos móviles y de escritorio.
* **Notificaciones Modernas:** Uso de `react-hot-toast` para feedback no intrusivo al usuario (ej. al agregar propiedades).
* **Población de Datos:** Incluye un botón para agregar 100 propiedades de prueba (`/batch`) para demostración.

## 📋 Prerrequisitos

Para ejecutar este proyecto, necesitarás tener instalado:

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Node.js (v18 o superior)](https://nodejs.org/en)
* Un servidor de [MongoDB](https://www.mongodb.com/try/download/community) (local, Docker) o una cadena de conexión a [MongoDB Atlas](https://www.mongodb.com/cloud/atlas).

---

## ⚡ Puesta en Marcha

Sigue estos pasos para ejecutar el proyecto localmente.

### 1. Backend (.NET API)

1.  **Navega a la carpeta de la API:**
    ```bash
    cd ruta/al/proyecto/RealEstate.Api
    ```

2.  **Configura la Base de Datos:**
    * Abre el archivo `RealEstate.Api/appsettings.json`.
    * Modifica la sección `MongoDbSettings` con tu cadena de conexión y los nombres de tu base de datos/colecciones.
    ```json
    "MongoDbSettings": {
      "ConnectionString": "mongodb://localhost:27017",
      "DatabaseName": "RealEstateDb",
      "PropertiesCollectionName": "properties"
    }
    ```

3.  **Restaura las dependencias y ejecuta:**
    ```bash
    dotnet restore
    dotnet run
    ```
    * La API estará corriendo (generalmente en `http://localhost:5116` o `https://localhost:7123`). Anota la URL base.

### 2. Frontend (Next.js)

1.  **Navega a la carpeta del frontend:**
    ```bash
    cd ruta/al/proyecto/RealEstate.Frontend
    ```

2.  **Configura la URL de la API:**
    * Crea un archivo `.env.local` en la raíz de la carpeta del frontend.
    * Añade la URL base de tu API (la que anotaste en el paso anterior):
    ```.env.local
    NEXT_PUBLIC_API_URL=http://localhost:5116/api
    ```
    *(Asegúrate de incluir `/api` al final de la URL)*

3.  **Instala las dependencias y ejecuta:**
    ```bash
    npm install
    npm run dev
    ```
    * La aplicación web estará disponible en `http://localhost:3000`.

---

## 🧪 Pruebas Unitarias

Para ejecutar las pruebas del backend, navega a la carpeta de pruebas y usa el comando `test`:

```bash
cd ruta/al/proyecto/RealEstate.Tests
dotnet test
```

## 🗺️ Endpoints de la API

* `GET /api/properties`: Lista y filtra propiedades (con paginación).
* `GET /api/properties/{id}`: Obtiene una propiedad por su ID.
* `POST /api/properties`: Crea una nueva propiedad.
* `POST /api/properties/batch`: Crea múltiples propiedades (usado por el botón de prueba).
* `PUT /api/properties/{id}`: Actualiza una propiedad (reemplazo completo).
* `PUT /api/properties/{id}/image`: Actualiza solo la imagen de una propiedad.
* `DELETE /api/properties/{id}`: Elimina una propiedad.
=======
# RealEstate-Million-Test-Project
>>>>>>> 74db0cf5aa8cbd4e1972c8e70ba5ced8334ad25e
