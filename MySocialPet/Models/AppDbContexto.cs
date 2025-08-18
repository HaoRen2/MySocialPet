using Microsoft.EntityFrameworkCore;
using MySocialPet.Models.Albums;
using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.Foros;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.Salud;
using MySocialPet.Models.Sugerencias;

public class AppDbContexto : DbContext
{
    public AppDbContexto(DbContextOptions<AppDbContexto> options) : base(options) { }

    // Registrar cada modelo como un DbSet
    public DbSet<TipoUsuario> TipoUsuarios { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Protectora> Protectoras { get; set; }
    public DbSet<Especie> Especies { get; set; }
    public DbSet<Raza> Razas { get; set; }
    public DbSet<Mascota> Mascotas { get; set; }
    public DbSet<SaludRegistro> SaludRegistros { get; set; }
    public DbSet<TipoVacuna> TipoVacunas { get; set; }
    public DbSet<ListaVacuna> PlanVacunaciones { get; set; }
    public DbSet<VacunaRegistro> VacunaRegistros { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Sugerencia> Sugerencias { get; set; }
    public DbSet<EspecieSugerencia> EspecieSugerencias { get; set; }
    public DbSet<RazaSugerencia> RazaSugerencias { get; set; }
    public DbSet<Album> Albumes { get; set; }
    public DbSet<FotoAlbum> FotoAlbumes { get; set; }
    public DbSet<FotoEtiquetaMascota> FotoEtiquetaMascotas { get; set; }
    public DbSet<Foro> Foros { get; set; }
    public DbSet<Discusion> Discusiones { get; set; }
    public DbSet<Mensaje> Mensajes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Clave compuesta para la tabla de relación EspecieSugerencia
        modelBuilder.Entity<EspecieSugerencia>()
            .HasKey(es => new { es.IdEspecie, es.IdSugerencia });

        // Clave compuesta para la tabla de relación RazaSugerencia
        modelBuilder.Entity<RazaSugerencia>()
            .HasKey(rs => new { rs.IdRaza, rs.IdSugerencia });

        // Clave compuesta para la tabla de relación FotoEtiquetaMascota
        modelBuilder.Entity<FotoEtiquetaMascota>()
            .HasKey(fe => new { fe.IdFoto, fe.IdMascota });

        // Relaciones explícitas para FotoEtiquetaMascota
        modelBuilder.Entity<FotoEtiquetaMascota>()
            .HasOne(fe => fe.FotoAlbum)
            .WithMany(f => f.MascotasEtiquetadas)
            .HasForeignKey(fe => fe.IdFoto);

        modelBuilder.Entity<FotoEtiquetaMascota>()
            .HasOne(fe => fe.Mascota)
            .WithMany(m => m.FotosEtiquetadas)
            .HasForeignKey(fe => fe.IdMascota);

        // Clave compuesta para CategoriaSugerencia
        modelBuilder.Entity<CategoriaSugerencia>()
        .HasKey(cs => new { cs.IdCategoria, cs.IdSugerencia });


    }
}
