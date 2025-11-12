# WorkNest API Documentation

Esta es la documentación oficial para la API de WorkNest, un backend construido para gestionar tableros de tareas, autenticación y usuarios.

---

## ?? Tabla de Contenidos

* [Autenticación (`api/auth`)](#autenticación-apiauth)
* [Tableros (`api/boards`)](#tableros-apiboards)
* [Tareas (`api/tasks`)](#tareas-apitasks)
* [Estadísticas (`api/statistics`)](#estadísticas-apistatistics)

---

## Autenticación (`api/auth`)

Endpoints para el registro, inicio de sesión y gestión de usuarios.

#### `POST /api/auth/register`

* **Descripción:** Registra un nuevo usuario en el sistema. Por defecto, se le asigna el rol `User`.
* **Autorización:** Pública.
* **Body (Request):**
    ```json
    {
      "userName": "nuevoUsuario",
      "email": "usuario@correo.com",
      "password": "password123",
      "confirmPassword": "password123"
    }
    ```
* **Respuesta (Exitosa `201 Created`):**
    ```json
    {
      "id": 1,
      "userName": "nuevoUsuario",
      "email": "usuario@correo.com",
      "roles": ["User"]
    }
    ```

#### `POST /api/auth/login`

* **Descripción:** Inicia sesión con un usuario existente.
* **Autorización:** Pública.
* **Body (Request):**
    ```json
    {
      "emailOrUsername": "nuevoUsuario",
      "password": "password123"
    }
    ```
* **Respuesta (Exitosa `200 OK`):**
    ```json
    {
      "token": "jwt.token.aqui",
      "user": {
        "id": 1,
        "userName": "nuevoUsuario",
        "email": "usuario@correo.com",
        "roles": ["User"]
      }
    }
    ```

#### `POST /api/auth/logout`

* **Descripción:** Cierra la sesión del usuario (invalida la cookie de sesión).
* **Autorización:** Requiere autenticación (Token JWT).
* **Respuesta (Exitosa `200 OK`):** (Sin contenido)

#### `GET /api/auth/users`

* **Descripción:** Obtiene una lista de todos los usuarios registrados.
* **Autorización:** Solo `Admin` o `Mod`.
* **Respuesta (Exitosa `200 OK`):**
    ```json
    [
      {
        "id": 1,
        "userName": "nuevoUsuario",
        "email": "usuario@correo.com",
        "roles": ["User"]
      },
      {
        "id": 2,
        "userName": "admin",
        "email": "admin@correo.com",
        "roles": ["Admin", "User"]
      }
    ]
    ```

---

## Tableros (`api/boards`)

Endpoints para gestionar los tableros.

#### `GET /api/boards`

* **Descripción:** Obtiene una lista de todos los tableros.
* **Autorización:** Requiere autenticación (Cualquier rol).
* **Respuesta (Exitosa `200 OK`):**
    ```json
    [
      {
        "id": 1,
        "name": "Proyecto WorkNest"
      },
      {
        "id": 2,
        "name": "Proyecto Secundario"
      }
    ]
    ```

#### `GET /api/boards/{id}`

* **Descripción:** Obtiene los detalles completos de un tablero, incluyendo sus columnas y tareas.
* **Autorización:** Requiere autenticación (Cualquier rol).
* **Respuesta (Exitosa `200 OK`):**
    ```json
    {
      "id": 1,
      "name": "Proyecto WorkNest",
      "columns": [
        {
          "id": 1,
          "name": "Backlog",
          "order": 1,
          "tasks": []
        },
        {
          "id": 2,
          "name": "To Do",
          "order": 2,
          "tasks": [
            {
              "id": 101,
              "name": "Configurar API",
              "description": "...",
              "dueDate": "2025-11-15T10:00:00Z",
              "columnId": 2,
              "assignedUser": {
                "id": 1,
                "userName": "nuevoUsuario",
                "email": "usuario@correo.com",
                "roles": ["User"]
              }
            }
          ]
        }
      ]
    }
    ```

#### `POST /api/boards`

* **Descripción:** Crea un nuevo tablero. Automáticamente crea las columnas por defecto (Backlog, To Do, In Progress, Done).
* **Autorización:** Solo `Admin` o `Mod`.
* **Body (Request):**
    ```json
    {
      "name": "Nuevo Proyecto Marketing"
    }
    ```
* **Respuesta (Exitosa `201 Created`):** (Devuelve el `BoardDetailsDTO` completo, ver `GET /api/boards/{id}`)

#### `DELETE /api/boards/{id}`

* **Descripción:** Elimina un tablero y todas sus columnas y tareas asociadas.
* **Autorización:** Solo `Admin` o `Mod`.
* **Respuesta (Exitosa `204 No Content`):** (Sin contenido)

---

## Tareas (`api/tasks`)

Endpoints para la creación y gestión de tareas individuales.

#### `POST /api/tasks`

* **Descripción:** Crea una nueva tarea y la asigna a un usuario y una columna.
* **Autorización:** Solo `Admin` o `Mod`.
* **Body (Request):**
    ```json
    {
      "name": "Revisar documentación",
      "description": "Asegurarse que Swagger esté bien.",
      "dueDate": "2025-11-20T18:00:00Z",
      "columnId": 2, 
      "assignedUserId": 1 
    }
    ```
* **Respuesta (Exitosa `200 OK`):**
    ```json
    {
      "id": 102,
      "name": "Revisar documentación",
      "description": "Asegurarse que Swagger esté bien.",
      "dueDate": "2025-11-20T18:00:00Z",
      "columnId": 2,
      "assignedUser": {
        "id": 1,
        "userName": "nuevoUsuario",
        "email": "usuario@correo.com",
        "roles": ["User"]
      }
    }
    ```

#### `DELETE /api/tasks/{id}`

* **Descripción:** Elimina una tarea específica.
* **Autorización:** Solo `Admin` o `Mod`.
* **Respuesta (Exitosa `204 No Content`):** (Sin contenido)

#### `PATCH /api/tasks/{id}/move`

* **Descripción:** Mueve una tarea a una columna diferente (ej. de "To Do" a "In Progress").
* **Autorización:** Requiere autenticación. El servicio valida que el usuario sea el **asignado** a la tarea, o que sea `Admin` o `Mod`.
* **Body (Request):**
    ```json
    {
      "targetColumnId": 3 
    }
    ```
* **Respuesta (Exitosa `200 OK`):** (Devuelve el `TaskDTO` actualizado, ver `POST /api/tasks`)

---

## Estadísticas (`api/statistics`)

Endpoints para obtener métricas del dashboard.

#### `GET /api/statistics/dashboard`

* **Descripción:** Obtiene un resumen de estadísticas sobre las tareas.
* **Autorización:** Solo `Admin` o `Mod`.
* **Respuesta (Exitosa `200 OK`):**
    ```json
    {
      "totalTasks": 50,
      "tasksDone": 20,
      "tasksInProgress": 5,
      "tasksOverdue": 3,
      "tasksPerUser": [
        {
          "userName": "nuevoUsuario",
          "email": "usuario@correo.com",
          "activeTasks": 4
        },
        {
          "userName": "otroUsuario",
          "email": "otro@correo.com",
          "activeTasks": 2
        }
      ]
    }
    ```