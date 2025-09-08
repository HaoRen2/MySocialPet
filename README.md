# MySocialPet

  <img src="https://i.imgur.com/8a3n3tC.png" alt="Logo de MySocialPet" width="200"/>

  <strong>Una plataforma web integral para el cuidado de tus mascotas y la conexión con una comunidad de amantes de los animales.</strong>

  <a href="#características-principales">Características</a> •
  <a href="#arquitectura-y-tecnologías">Tecnologías</a> •
  <a href="#instalación-y-puesta-en-marcha">Instalación</a> •
  <a href="#licencia">Licencia</a> •
  <a href="#contribuidores">Contribuidores</a>

---

## Descripción

**MySocialPet** es una plataforma web integral diseñada para dueños de mascotas, protectoras y amantes de los animales.  
Permite registrar y administrar toda la información de salud y bienestar de las mascotas, crear álbumes de fotos, organizar eventos, participar en foros comunitarios y facilitar procesos de adopción.  

Este proyecto fue desarrollado por **Alexis Godoy, Lei Wang, Pol Nebot y Juan Pablo Guerrero** en **agosto de 2025**.

---

## Características Principales

MySocialPet combina funcionalidades de red social con herramientas de gestión para fomentar el bienestar animal.  
El objetivo es cubrir las necesidades de los dueños de mascotas y protectoras, ofreciendo un espacio para compartir experiencias, resolver dudas y fomentar la adopción responsable.

### Para Dueños de Mascotas 🐾
- **Gestión de Mascotas:** Registro con nombre, fecha de nacimiento, peso, longitud, género, foto y raza.  
- **Notas personalizadas** para un seguimiento detallado.  
- **Registro de Salud:** Historial con datos de peso, longitud y condición corporal (BCS). Incluye calculadora de calorías.  
- **Vacunación y Eventos:** Calendario con notificaciones de vacunas y cuidados importantes.  
- **Álbumes de Fotos:** Almacena recuerdos y etiqueta a tus mascotas.  

### Comunidad y Soporte 👨‍👩‍👧‍👦
- **Foros Comunitarios:** Discusiones por especie, con soporte para texto e imágenes.  
- **Adopción y Protectoras:** Explora asociaciones y consulta su información de contacto (animales en adopción próximamente).  
- **Sugerencias y Consejos:** Recomendaciones personalizadas según especie, raza y categoría.  

---

## Arquitectura y Tecnologías

La arquitectura sigue el patrón **ASP.NET MVC**, organizada en capas para favorecer mantenibilidad y evolución.

- **Backend:** ASP.NET MVC + Entity Framework Core  
- **Base de Datos:** SQL Server  
- **Arquitectura en Capas:**  
  - **Dominio:** Modelos de negocio (Entidades)  
  - **Infraestructura:** Configuración de EF Core y servicios transversales (email, etc.)  
  - **Lógica de Negocio:** Servicios con reglas de negocio  
  - **Presentación:** Controladores, Vistas (Razor) y ViewModels  

---

## Instalación y Puesta en Marcha

### Prerrequisitos
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/es-es/sql-server)

### Pasos

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/tu-usuario/MySocialPet.git
   cd MySocialPet
   
2. **Configurar la Base de Datos**
Abre appsettings.json.
Modifica la cadena de conexión (DefaultConnection) para apuntar a tu instancia local de SQL Server.

3. **Aplicar Migraciones**

4. **Ejecutar la aplicación**
dotnet run o utilizar en visual studio 2022

---

## Paso 5: **Registro e Inicio de Sesión**

## Registro e Inicio de Sesión

1. Haz clic en **Login** y luego en **Registrarse**.  
2. Completa el formulario con usuario, email y contraseña.  
3. Al confirmar, accederás automáticamente a la plataforma.
---
## Licencia

Este proyecto está bajo la **Licencia MIT**.  
Consulta el archivo [LICENSE](LICENSE) para más detalles.
---
## Contribuidores

- **Alexis Godoy**  
- **Lei Wang**  
- **Pol Nebot**  
- **Juan Pablo Guerrero**
