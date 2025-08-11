using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySocialPet.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurandoRelacionesSugerencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Especie",
                columns: table => new
                {
                    IdEspecie = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especie", x => x.IdEspecie);
                });

            migrationBuilder.CreateTable(
                name: "Sugerencia",
                columns: table => new
                {
                    IdSugerencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tema = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sugerencia", x => x.IdSugerencia);
                });

            migrationBuilder.CreateTable(
                name: "TipoUsuario",
                columns: table => new
                {
                    IdTipoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUsuario", x => x.IdTipoUsuario);
                });

            migrationBuilder.CreateTable(
                name: "TipoVacuna",
                columns: table => new
                {
                    IdTipoVacuna = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoVacuna", x => x.IdTipoVacuna);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdEspecie = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.IdCategoria);
                    table.ForeignKey(
                        name: "FK_Categoria_Especie_IdEspecie",
                        column: x => x.IdEspecie,
                        principalTable: "Especie",
                        principalColumn: "IdEspecie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Foro",
                columns: table => new
                {
                    IdForo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdEspecie = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foro", x => x.IdForo);
                    table.ForeignKey(
                        name: "FK_Foro_Especie_IdEspecie",
                        column: x => x.IdEspecie,
                        principalTable: "Especie",
                        principalColumn: "IdEspecie");
                });

            migrationBuilder.CreateTable(
                name: "EspecieSugerencia",
                columns: table => new
                {
                    IdEspecie = table.Column<int>(type: "int", nullable: false),
                    IdSugerencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EspecieSugerencia", x => new { x.IdEspecie, x.IdSugerencia });
                    table.ForeignKey(
                        name: "FK_EspecieSugerencia_Especie_IdEspecie",
                        column: x => x.IdEspecie,
                        principalTable: "Especie",
                        principalColumn: "IdEspecie",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EspecieSugerencia_Sugerencia_IdSugerencia",
                        column: x => x.IdSugerencia,
                        principalTable: "Sugerencia",
                        principalColumn: "IdSugerencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AvatarFoto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdTipoUsuario = table.Column<int>(type: "int", nullable: false),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_TipoUsuario_IdTipoUsuario",
                        column: x => x.IdTipoUsuario,
                        principalTable: "TipoUsuario",
                        principalColumn: "IdTipoUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListaVacuna",
                columns: table => new
                {
                    IdListaVacuna = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEspecie = table.Column<int>(type: "int", nullable: false),
                    IdTipoVacuna = table.Column<int>(type: "int", nullable: false),
                    EdadRecomendada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsRefuerzo = table.Column<bool>(type: "bit", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Esencial = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaVacuna", x => x.IdListaVacuna);
                    table.ForeignKey(
                        name: "FK_ListaVacuna_Especie_IdEspecie",
                        column: x => x.IdEspecie,
                        principalTable: "Especie",
                        principalColumn: "IdEspecie",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaVacuna_TipoVacuna_IdTipoVacuna",
                        column: x => x.IdTipoVacuna,
                        principalTable: "TipoVacuna",
                        principalColumn: "IdTipoVacuna",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaSugerencias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    IdSugerencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaSugerencias", x => new { x.IdCategoria, x.IdSugerencia });
                    table.ForeignKey(
                        name: "FK_CategoriaSugerencias_Categoria_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaSugerencias_Sugerencia_IdSugerencia",
                        column: x => x.IdSugerencia,
                        principalTable: "Sugerencia",
                        principalColumn: "IdSugerencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Raza",
                columns: table => new
                {
                    IdRaza = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRaza = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Informacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tamanyo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatioIdeal = table.Column<int>(type: "int", nullable: true),
                    IdEspecie = table.Column<int>(type: "int", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raza", x => x.IdRaza);
                    table.ForeignKey(
                        name: "FK_Raza_Categoria_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categoria",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Raza_Especie_IdEspecie",
                        column: x => x.IdEspecie,
                        principalTable: "Especie",
                        principalColumn: "IdEspecie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    IdAlbum = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreAlbum = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.IdAlbum);
                    table.ForeignKey(
                        name: "FK_Album_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discusion",
                columns: table => new
                {
                    IdDiscusion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstaFinalizado = table.Column<bool>(type: "bit", nullable: false),
                    IdForo = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioCreador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discusion", x => x.IdDiscusion);
                    table.ForeignKey(
                        name: "FK_Discusion_Foro_IdForo",
                        column: x => x.IdForo,
                        principalTable: "Foro",
                        principalColumn: "IdForo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Discusion_Usuario_IdUsuarioCreador",
                        column: x => x.IdUsuarioCreador,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Protectora",
                columns: table => new
                {
                    IdProtectora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Web = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protectora", x => x.IdProtectora);
                    table.ForeignKey(
                        name: "FK_Protectora_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mascota",
                columns: table => new
                {
                    IdMascota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PesoKg = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LongitudCm = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Foto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BCS = table.Column<int>(type: "int", nullable: true),
                    Esterilizada = table.Column<bool>(type: "bit", nullable: false),
                    EstadoAdopcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdRaza = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mascota", x => x.IdMascota);
                    table.ForeignKey(
                        name: "FK_Mascota_Raza_IdRaza",
                        column: x => x.IdRaza,
                        principalTable: "Raza",
                        principalColumn: "IdRaza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mascota_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RazaSugerencia",
                columns: table => new
                {
                    IdRaza = table.Column<int>(type: "int", nullable: false),
                    IdSugerencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RazaSugerencia", x => new { x.IdRaza, x.IdSugerencia });
                    table.ForeignKey(
                        name: "FK_RazaSugerencia_Raza_IdRaza",
                        column: x => x.IdRaza,
                        principalTable: "Raza",
                        principalColumn: "IdRaza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RazaSugerencia_Sugerencia_IdSugerencia",
                        column: x => x.IdSugerencia,
                        principalTable: "Sugerencia",
                        principalColumn: "IdSugerencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FotoAlbum",
                columns: table => new
                {
                    IdFoto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Foto = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdAlbum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoAlbum", x => x.IdFoto);
                    table.ForeignKey(
                        name: "FK_FotoAlbum_Album_IdAlbum",
                        column: x => x.IdAlbum,
                        principalTable: "Album",
                        principalColumn: "IdAlbum",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensaje",
                columns: table => new
                {
                    IdMensaje = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContenidoMensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdDiscusion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensaje", x => x.IdMensaje);
                    table.ForeignKey(
                        name: "FK_Mensaje_Discusion_IdDiscusion",
                        column: x => x.IdDiscusion,
                        principalTable: "Discusion",
                        principalColumn: "IdDiscusion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mensaje_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoEvento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificacionEnviada = table.Column<bool>(type: "bit", nullable: false),
                    IdMascota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.IdEvento);
                    table.ForeignKey(
                        name: "FK_Evento_Mascota_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "Mascota",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nota",
                columns: table => new
                {
                    IdNota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IdMascota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nota", x => x.IdNota);
                    table.ForeignKey(
                        name: "FK_Nota_Mascota_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "Mascota",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaludRegistro",
                columns: table => new
                {
                    IdSalud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PesoKg = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    BCS = table.Column<int>(type: "int", nullable: true),
                    LongitudCm = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IdMascota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaludRegistro", x => x.IdSalud);
                    table.ForeignKey(
                        name: "FK_SaludRegistro_Mascota_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "Mascota",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacunaRegistro",
                columns: table => new
                {
                    IdVacunaRegistro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdMascota = table.Column<int>(type: "int", nullable: false),
                    IdTipoVacuna = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacunaRegistro", x => x.IdVacunaRegistro);
                    table.ForeignKey(
                        name: "FK_VacunaRegistro_Mascota_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "Mascota",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacunaRegistro_TipoVacuna_IdTipoVacuna",
                        column: x => x.IdTipoVacuna,
                        principalTable: "TipoVacuna",
                        principalColumn: "IdTipoVacuna",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FotoEtiquetaMascota",
                columns: table => new
                {
                    IdFoto = table.Column<int>(type: "int", nullable: false),
                    IdMascota = table.Column<int>(type: "int", nullable: false),
                    FotoAlbumIdFoto = table.Column<int>(type: "int", nullable: false),
                    MascotaIdMascota = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoEtiquetaMascota", x => new { x.IdFoto, x.IdMascota });
                    table.ForeignKey(
                        name: "FK_FotoEtiquetaMascota_FotoAlbum_FotoAlbumIdFoto",
                        column: x => x.FotoAlbumIdFoto,
                        principalTable: "FotoAlbum",
                        principalColumn: "IdFoto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FotoEtiquetaMascota_Mascota_MascotaIdMascota",
                        column: x => x.MascotaIdMascota,
                        principalTable: "Mascota",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_IdUsuario",
                table: "Album",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_IdEspecie",
                table: "Categoria",
                column: "IdEspecie");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaSugerencias_IdSugerencia",
                table: "CategoriaSugerencias",
                column: "IdSugerencia");

            migrationBuilder.CreateIndex(
                name: "IX_Discusion_IdForo",
                table: "Discusion",
                column: "IdForo");

            migrationBuilder.CreateIndex(
                name: "IX_Discusion_IdUsuarioCreador",
                table: "Discusion",
                column: "IdUsuarioCreador");

            migrationBuilder.CreateIndex(
                name: "IX_EspecieSugerencia_IdSugerencia",
                table: "EspecieSugerencia",
                column: "IdSugerencia");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_IdMascota",
                table: "Evento",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Foro_IdEspecie",
                table: "Foro",
                column: "IdEspecie");

            migrationBuilder.CreateIndex(
                name: "IX_FotoAlbum_IdAlbum",
                table: "FotoAlbum",
                column: "IdAlbum");

            migrationBuilder.CreateIndex(
                name: "IX_FotoEtiquetaMascota_FotoAlbumIdFoto",
                table: "FotoEtiquetaMascota",
                column: "FotoAlbumIdFoto");

            migrationBuilder.CreateIndex(
                name: "IX_FotoEtiquetaMascota_MascotaIdMascota",
                table: "FotoEtiquetaMascota",
                column: "MascotaIdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_ListaVacuna_IdEspecie",
                table: "ListaVacuna",
                column: "IdEspecie");

            migrationBuilder.CreateIndex(
                name: "IX_ListaVacuna_IdTipoVacuna",
                table: "ListaVacuna",
                column: "IdTipoVacuna");

            migrationBuilder.CreateIndex(
                name: "IX_Mascota_IdRaza",
                table: "Mascota",
                column: "IdRaza");

            migrationBuilder.CreateIndex(
                name: "IX_Mascota_IdUsuario",
                table: "Mascota",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_IdDiscusion",
                table: "Mensaje",
                column: "IdDiscusion");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_IdUsuario",
                table: "Mensaje",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Nota_IdMascota",
                table: "Nota",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Protectora_IdUsuario",
                table: "Protectora",
                column: "IdUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Raza_IdCategoria",
                table: "Raza",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Raza_IdEspecie",
                table: "Raza",
                column: "IdEspecie");

            migrationBuilder.CreateIndex(
                name: "IX_RazaSugerencia_IdSugerencia",
                table: "RazaSugerencia",
                column: "IdSugerencia");

            migrationBuilder.CreateIndex(
                name: "IX_SaludRegistro_IdMascota",
                table: "SaludRegistro",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdTipoUsuario",
                table: "Usuario",
                column: "IdTipoUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_VacunaRegistro_IdMascota",
                table: "VacunaRegistro",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_VacunaRegistro_IdTipoVacuna",
                table: "VacunaRegistro",
                column: "IdTipoVacuna");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaSugerencias");

            migrationBuilder.DropTable(
                name: "EspecieSugerencia");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "FotoEtiquetaMascota");

            migrationBuilder.DropTable(
                name: "ListaVacuna");

            migrationBuilder.DropTable(
                name: "Mensaje");

            migrationBuilder.DropTable(
                name: "Nota");

            migrationBuilder.DropTable(
                name: "Protectora");

            migrationBuilder.DropTable(
                name: "RazaSugerencia");

            migrationBuilder.DropTable(
                name: "SaludRegistro");

            migrationBuilder.DropTable(
                name: "VacunaRegistro");

            migrationBuilder.DropTable(
                name: "FotoAlbum");

            migrationBuilder.DropTable(
                name: "Discusion");

            migrationBuilder.DropTable(
                name: "Sugerencia");

            migrationBuilder.DropTable(
                name: "Mascota");

            migrationBuilder.DropTable(
                name: "TipoVacuna");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Foro");

            migrationBuilder.DropTable(
                name: "Raza");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "TipoUsuario");

            migrationBuilder.DropTable(
                name: "Especie");
        }
    }
}
