# Project Guidelines - .NET 10 Platzi Project

Este archivo define las reglas y estándares para trabajar en este repositorio utilizando agentes de IA o desarrollo manual.
## Paso inicial
- Antes de cada solicitud quiero que digas "Hola jefazo! 😎"

## Tecnologías Principales
- **Framework**: .NET 10 (última versión disponible).
- **Contenerización**: Docker y Docker Compose para el entorno de desarrollo y despliegue.
- **Documentación de API**: Swagger/OpenAPI para todas las APIs REST.
- **Pruebas**: Pruebas unitarias integradas (preferiblemente xUnit).

## Estándares de Desarrollo

### 1. API Rest
- Usar controladores o Minimal APIs siguiendo las mejores prácticas de .NET 10.
- Implementar validación de modelos utilizando `FluentValidation` o anotaciones de datos.
- Asegurar que todos los endpoints estén documentados en Swagger.
- Utilizar códigos de estado HTTP adecuados (200, 201, 204, 400, 404, 500).

### 2. Docker
- El repositorio debe incluir un `Dockerfile` optimizado para .NET 10 (multi-stage build).
- `docker-compose.yml` debe configurar el entorno completo, incluyendo bases de datos si es necesario.

### 3. Pruebas Unitarias
- Mantener una alta cobertura de código.
- Seguir el patrón AAA (Arrange, Act, Assert).
- Mockear dependencias externas utilizando `Moq` o `NSubstitute`.

### 4. Estilo de Código y Arquitectura
- Seguir las convenciones oficiales de Microsoft para C#.
- Usar nombres de variables descriptivos en inglés (aunque la documentación pueda estar en español).
- Aplicar principios SOLID y patrones de diseño cuando sea pertinente.
- Implementar una estructura de proyecto basada en Arquitectura Limpia (Clean Architecture).

### 5. Rendimiento
- Optimizar el código para un alto rendimiento y baja latencia.
- Monitorear y analizar el rendimiento de la aplicación de forma proactiva.
- Utilizar las mejores prácticas de .NET 10 para la gestión de memoria y concurrencia.

## Instrucciones para Agentes
- Siempre verificar la versión de SDK de .NET instalada antes de realizar cambios estructurales.
- Al crear nuevos servicios, generar automáticamente su correspondiente archivo de pruebas unitarias.
- Mantener el archivo `docker-compose.yml` actualizado con cualquier nueva dependencia de infraestructura.
- No subir secretos ni cadenas de conexión reales al repositorio; usar variables de entorno o `user-secrets`.
- Asegurar que todas las nuevas implementaciones sigan los principios de Arquitectura Limpia y SOLID.
- Validar que el código nuevo o modificado no degrade el rendimiento de la aplicación.
