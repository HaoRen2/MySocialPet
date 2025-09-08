MySocialPet
<p align="center">
<img src="https://www.google.com/search?q=https://i.imgur.com/8a3n3tC.png" alt="Logo de MySocialPet" width="200"/>
</p>

<p align="center">
<strong>Una plataforma web integral para el cuidado de tus mascotas y la conexión con una comunidad de amantes de los animales.</strong>
</p>

<p align="center">
<a href="#características-principales">Características</a> •
<a href="#arquitectura-y-tecnologías">Tecnologías</a> •
<a href="#instalación-y-puesta-en-marcha">Instalación</a> •
<a href="#licencia">Licencia</a> •
<a href="#contribuidores">Contribuidores</a>
</p>

MySocialPet es una plataforma web integral diseñada para dueños de mascotas, protectoras y amantes de los animales. Permite a los usuarios registrar y administrar toda la información de salud y bienestar de sus mascotas, crear álbumes de fotos, organizar eventos, participar en foros comunitarios y facilitar procesos de adopción. Este proyecto fue desarrollado por Alexis Godoy, Lei Wang, Pol Nebot y Juan Pablo Guerrero en agosto de 2025.

Características Principales
MySocialPet combina funcionalidades de red social con herramientas de gestión para fomentar el bienestar animal. El objetivo es cubrir las necesidades de los dueños de mascotas y protectoras, ofreciendo un espacio para compartir experiencias, resolver dudas y fomentar la adopción responsable.

Para Dueños de Mascotas 🐾
Gestión de Mascotas: Registra a tus mascotas con su nombre, fecha de nacimiento, peso, longitud, género, foto y raza. Añade notas personalizadas para un seguimiento detallado.

Registro de Salud: Lleva un historial de salud con datos como peso, longitud y condición corporal (BCS). La aplicación incluye una calculadora de calorías para ayudar a mantener el peso ideal de tu mascota.

Vacunación y Eventos: Registra las vacunas y mantén un calendario de eventos veterinarios o cuidados importantes con notificaciones.

Álbumes de Fotos: Crea álbumes personalizados para almacenar recuerdos y etiqueta a tus mascotas en las imágenes.

Comunidad y Soporte 👨‍👩‍👧‍👦
Foros Comunitarios: Participa en foros organizados por especie, inicia discusiones y responde a otros usuarios con mensajes que pueden incluir texto e imágenes.

Adopción y Protectoras: Explora perfiles de protectoras y asociaciones de animales, consulta su información de contacto y, próximamente, los animales disponibles para adopción.

Sugerencias y Consejos: Obtén consejos personalizados sobre cuidados, salud y nutrición, filtrados por especie, raza y categoría.

Arquitectura y Tecnologías
La arquitectura de MySocialPet sigue el patrón ASP.NET MVC, apoyado en una separación por capas que favorece la mantenibilidad y la evolución del producto.

Backend:

ASP.NET MVC

Entity Framework Core

Base de Datos:

SQL Server

Arquitectura:

Dominio: Modelos de negocio (Entidades).

Infraestructura: Configuración de EF Core y servicios transversales (email, etc.).

Lógica de Negocio: Servicios que encapsulan las reglas de negocio.

Presentación: Controladores, Vistas (Razor) y ViewModels.

Instalación y Puesta en Marcha
Para ejecutar este proyecto en tu entorno local, sigue estos pasos:

Prerrequisitos:

.NET SDK

SQL Server

Clonar el repositorio:

git clone [https://github.com/tu-usuario/MySocialPet.git](https://github.com/tu-usuario/MySocialPet.git)
cd MySocialPet

Configurar la Base de Datos:

Abre el archivo appsettings.json.

Modifica la cadena de conexión (DefaultConnection) para que apunte a tu instancia local de SQL Server.

Aplicar Migraciones:

Ejecuta las migraciones de Entity Framework para crear el esquema de la base de datos.

dotnet ef database update

Ejecutar la aplicación:

dotnet run

La aplicación estará disponible en https://localhost:5001 o la URL que indique tu consola.

Registro e Inicio de Sesión
Una vez que la aplicación esté en funcionamiento, haz clic en el botón “Login” y luego en "Registrarse" en la esquina superior derecha.

Completa el formulario con tu nombre de usuario, correo electrónico y contraseña.

Al enviar el formulario, serás redirigido a la página de inicio ya como usuario de la plataforma.

Licencia
Este proyecto está bajo la Licencia MIT. Consulta el archivo LICENSE para más detalles.

Contribuidores
Alexis Godoy

Lei Wang

Pol Nebot

Juan Pablo Guerrero
