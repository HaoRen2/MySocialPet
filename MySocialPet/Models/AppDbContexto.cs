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
    public DbSet<ListaVacuna> ListaVacunas { get; set; }
    public DbSet<VacunaRegistro> VacunaRegistros { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Sugerencia> Sugerencias { get; set; }
    public DbSet<EspecieSugerencia> EspecieSugerencias { get; set; }
    public DbSet<CategoriaSugerencia> CategoriaSugerencias { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<RazaSugerencia> RazaSugerencias { get; set; }
    public DbSet<Album> Albumes { get; set; }
    public DbSet<FotoAlbum> FotoAlbumes { get; set; }
    public DbSet<FotoEtiquetaMascota> FotoEtiquetaMascotas { get; set; }
    public DbSet<Foro> Foros { get; set; }
    public DbSet<Discusion> Discusiones { get; set; }
    public DbSet<Mensaje> Mensajes { get; set; }
    public DbSet<Nota> Notas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EspecieSugerencia>()
    .HasKey(es => new { es.IdEspecie, es.IdSugerencia });

        modelBuilder.Entity<EspecieSugerencia>()
            .HasOne(es => es.Especie) 
            .WithMany(e => e.EspeciesSugerencia)
            .HasForeignKey(es => es.IdEspecie); 

        modelBuilder.Entity<EspecieSugerencia>()
            .HasOne(es => es.Sugerencia) 
            .WithMany(s => s.EspeciesSugerencia) // ...y una Sugerencia tiene muchas EspeciesSugerencia
            .HasForeignKey(es => es.IdSugerencia); // La clave foránea en esta tabla es IdSugerencia


        modelBuilder.Entity<RazaSugerencia>()
            .HasKey(rs => new { rs.IdRaza, rs.IdSugerencia });

        modelBuilder.Entity<RazaSugerencia>()
            .HasOne(rs => rs.Raza)
            .WithMany(r => r.RazasSugerencia) // Asumo que en tu clase Raza tienes 'public ICollection<RazaSugerencia> RazasSugerencia { get; set; }'
            .HasForeignKey(rs => rs.IdRaza);

        modelBuilder.Entity<RazaSugerencia>()
            .HasOne(rs => rs.Sugerencia)
            .WithMany(s => s.RazasSugerencia)
            .HasForeignKey(rs => rs.IdSugerencia);


        // --- Configuración para CategoriaSugerencia (mismo patrón) ---
        modelBuilder.Entity<CategoriaSugerencia>()
            .HasKey(cs => new { cs.IdCategoria, cs.IdSugerencia });

        modelBuilder.Entity<CategoriaSugerencia>()
            .HasOne(cs => cs.Categoria)
            .WithMany(c => c.CategoriaSugerencias) // Asumo que en tu clase Categoria tienes 'public ICollection<CategoriaSugerencia> CategoriaSugerencias { get; set; }'
            .HasForeignKey(cs => cs.IdCategoria);

        modelBuilder.Entity<CategoriaSugerencia>()
            .HasOne(cs => cs.Sugerencia)
            .WithMany(s => s.CategoriaSugerencias)
            .HasForeignKey(cs => cs.IdSugerencia);

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
