# MySocialPet

[cite_start]**MySocialPet** es una plataforma web integral diseñada para dueños de mascotas, protectoras y amantes de los animales[cite: 387]. [cite_start]Permite registrar y administrar toda la información de salud y bienestar de las mascotas, crear álbumes de fotos, organizar eventos, participar en foros comunitarios y facilitar procesos de adopción[cite: 388].

[cite_start]Este proyecto fue desarrollado por **Alexis Godoy, Lei Wang, Pol Nebot y Juan Pablo Guerrero** en **agosto de 2025**[cite: 3, 4, 5, 6, 7].

## 🚀 Descripción del Proyecto

[cite_start]MySocialPet es una plataforma digital que combina funcionalidades de red social con herramientas de gestión para el bienestar animal[cite: 94]. [cite_start]El objetivo es centralizar en un único lugar el registro de mascotas, su historial de salud, vacunación, álbumes fotográficos y foros de comunidad, además de servir como un canal para fomentar la adopción responsable a través de la colaboración con protectoras[cite: 62, 131].

[cite_start]La aplicación está concebida para ser modular y escalable, buscando convertirse en una red social especializada en el cuidado animal[cite: 83].

## ✨ Características Principales

El sistema se estructura en varios módulos clave para ofrecer una experiencia completa:

* [cite_start]**Gestión de Usuarios y Autenticación**[cite: 17, 25]:
    * [cite_start]Registro y autenticación segura para usuarios[cite: 67].
    * [cite_start]Roles diferenciados para dueños de mascotas y protectoras[cite: 68].
    * [cite_start]Perfiles de usuario personalizables con avatar e información de contacto[cite: 102, 145].
    * [cite_start]Funcionalidad para recuperar y restablecer la contraseña de forma segura mediante tokens[cite: 142, 144].

* [cite_start]**Gestión Integral de Mascotas**[cite: 18, 26]:
    * [cite_start]Registro detallado de mascotas: nombre, especie, raza, fecha de nacimiento, peso, etc.[cite: 70, 104, 147].
    * [cite_start]**Historial Clínico**: Seguimiento de salud (peso, condición corporal), registro de vacunas y calendario de eventos (citas veterinarias, tratamientos)[cite: 71, 105, 106, 149].
    * [cite_start]Notas personalizadas para cada mascota[cite: 154].

* [cite_start]**Álbumes Multimedia y Recuerdos**[cite: 19, 27]:
    * [cite_start]Creación de álbumes personalizados para organizar fotos[cite: 109, 156].
    * [cite_start]Subida de fotos con título, descripción y fecha[cite: 110].
    * [cite_start]Etiquetado de mascotas en las fotos para una fácil identificación[cite: 111, 158].

* [cite_start]**Comunidad y Foros**[cite: 20, 28]:
    * [cite_start]Foros temáticos organizados por especie para compartir consejos y resolver dudas[cite: 74, 114, 162].
    * [cite_start]Creación de hilos de discusión y envío de mensajes con soporte para imágenes[cite: 116, 117].
    * [cite_start]Sistema de hilos y respuestas encadenadas para seguir conversaciones[cite: 167].
    * [cite_start]Visualización de temas en tendencia (trending)[cite: 118, 168].

* [cite_start]**Protectoras y Adopción**[cite: 21, 29]:
    * [cite_start]Perfiles para protectoras con su información y listado de animales en adopción[cite: 77, 78, 170].
    * [cite_start]Actualmente, el alta de protectoras es gestionada por administradores, con planes de permitir el registro autónomo[cite: 121, 172].

* [cite_start]**Sugerencias y Recomendaciones**[cite: 22, 30]:
    * [cite_start]Sistema de consejos personalizados según la especie, raza y categoría de la mascota[cite: 124, 175, 176, 177].
    * [cite_start]Recomendaciones sobre cuidados, nutrición y salud para ayudar a los dueños[cite: 125].

## 🛠️ Arquitectura y Tecnología

[cite_start]La aplicación está desarrollada siguiendo el patrón **ASP.NET MVC**, con una arquitectura por capas bien definida para garantizar la mantenibilidad y escalabilidad del sistema[cite: 207].

* [cite_start]**Patrón Arquitectónico**: Model-View-Controller (MVC)[cite: 89].
* [cite_start]**Base de Datos**: **SQL Server** [cite: 219][cite_start], gestionado a través del ORM **Entity Framework Core (EF Core)**[cite: 88, 224].
* **Componentes Principales**:
    * [cite_start]**Domain**: Contiene las entidades de negocio (modelos)[cite: 209].
    * [cite_start]**Infrastructure**: Gestiona el acceso a datos con EF Core y servicios transversales como el envío de correos[cite: 41, 223].
    * [cite_start]**DTOs (Data Transfer Objects)**: Objetos para transportar datos entre capas de forma eficiente[cite: 42, 86].
    * [cite_start]**Business Logic (Servicios)**: Encapsula las reglas de negocio y la lógica de la aplicación[cite: 43].
    * [cite_start]**Controllers**: Reciben las peticiones HTTP, invocan a los servicios y devuelven las vistas[cite: 44].
    * [cite_start]**Views y ViewModels**: Vistas Razor para la UI y ViewModels para adaptar los datos a las necesidades de la presentación[cite: 45].

## 📋 Requisitos No Funcionales

Se ha puesto especial atención en la calidad del sistema, cumpliendo con los siguientes requisitos:

* [cite_start]**Rendimiento**: Tiempos de respuesta optimizados (<3 segundos para operaciones comunes) y paginación eficiente de datos[cite: 185, 186].
* [cite_start]**Seguridad**: Almacenamiento de contraseñas con hash y salt, tokens con expiración para recuperación de cuenta y comunicación bajo HTTPS[cite: 188, 189, 191].
* [cite_start]**Usabilidad**: Interfaz intuitiva, de diseño responsivo y accesible desde ordenadores y dispositivos móviles[cite: 193, 194].
* [cite_start]**Mantenibilidad y Escalabilidad**: Código limpio con separación de capas, base de datos normalizada y una arquitectura preparada para crecer y añadir nuevas funcionalidades[cite: 198, 203].

## 🚀 Despliegue

La arquitectura está preparada para un despliegue flexible:

* [cite_start]Compatible con **SQL Server** y gestionado con **EF Core Migrations**[cite: 248].
* [cite_start]Preparado para entornos en la nube como **Azure** o despliegues on-premise[cite: 249].
* [cite_start]Uso obligatorio de **HTTPS** para garantizar la seguridad en las comunicaciones[cite: 250].

## 👨‍💻 Autores

Este proyecto fue desarrollado por:

* [cite_start]**Alexis Godoy** [cite: 3]
* [cite_start]**Lei Wang** [cite: 4]
* [cite_start]**Pol Nebot** [cite: 5]
* [cite_start]**Juan Pablo Guerrero** [cite: 6]

[cite_start]*Fecha de finalización: Agosto de 2025*[cite: 7].
