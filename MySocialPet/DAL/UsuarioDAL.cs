using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySocialPet.Models;
using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.ViewModel.Perfil;
using MySocialPet.Tools;

namespace MySocialPet.DAL
{
    public class UsuarioDAL
    {
        private readonly AppDbContexto _context;

        public UsuarioDAL(AppDbContexto context)
        {
            _context = context;
        }

        public Usuario GetUsuarioLogin(string email, string pwd)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null && PasswordHelper.VerifyPasswordHash(pwd, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return usuario;
            }

            return null;
        }

        public void CambiarContrasenya(Usuario user, string nuevaPassword)
        {
            if (user == null) return;

            PasswordHelper.CreatePasswordfHash(nuevaPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.SaveChanges();
        }

        public Usuario GetUsuarioByEmail(string email)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {
                return usuario;
            }

            return null;
        }

        public Usuario CreateUsuario(Usuario user, string password)
        {
            PasswordHelper.CreatePasswordfHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.FechaRegistro = DateTime.Now;
            user.IdTipoUsuario = 1;

            _context.Usuarios.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void GuardarTokenRecuperacion(int idUsuario, string token, DateTime expiracion)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            if (usuario != null)
            {
                usuario.ResetToken = token;
                usuario.TokenExpiration = expiracion.ToUniversalTime(); // 🔄 Guardar en UTC
                _context.SaveChanges();
            }

            Console.WriteLine("TokenExpiration guardado (UTC): " + expiracion.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public Usuario GetUsuarioByToken(string token)
        {
            var nowUtc = DateTime.UtcNow;

            Console.WriteLine("Ahora (UTC): " + nowUtc.ToString("yyyy-MM-dd HH:mm:ss"));

            return _context.Usuarios.FirstOrDefault(u =>
                u.ResetToken == token && u.TokenExpiration >= nowUtc); // 🔍 Comparar en UTC
        }

        public void EliminarTokenRecuperacion(int idUsuario)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            if (usuario != null)
            {
                usuario.ResetToken = null;
                usuario.TokenExpiration = null;
                _context.SaveChanges();
            }
        }
        // --- Lectura ---
        public Task<Usuario?> GetUsuarioByIdAsync(int idUsuario)
            => _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

        public async Task<PerfilViewModel?> GetPerfilByIdAsync(int idUsuario)
        {
            return await _context.Usuarios
                .Where(u => u.IdUsuario == idUsuario)
                .Select(u => new PerfilViewModel
                {
                    IdUsuario = u.IdUsuario,
                    Username = u.Username,
                    Email = u.Email,
                    AvatarFoto = u.AvatarFoto,
                    FechaRegistro = u.FechaRegistro,
                    // Usa el nombre correcto según tu entidad TipoUsuario:
                    // TipoUsuarioNombre = u.TipoUsuario != null ? u.TipoUsuario.NombreTipo : null,
                    TipoUsuarioNombre = null,
                    ProtectoraNombre = u.Protectora != null ? u.Protectora.Nombre : null,

                    // Datos personales
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Telefono = u.Telefono,
                    Direccion = u.Direccion,
                    Ciudad = u.Ciudad,
                    Provincia = u.Provincia,
                    CodigoPostal = u.CodigoPostal,

                    // Contadores
                    MascotasCount = _context.Mascotas.Count(m => m.IdUsuario == u.IdUsuario),
                    AlbumesCount = _context.Albumes.Count(a => a.IdUsuario == u.IdUsuario),
                    DiscusionesCount = _context.Discusiones.Count(d => d.IdUsuarioCreador == u.IdUsuario),
                    MensajesCount = _context.Mensajes.Count(ms => ms.IdUsuario == u.IdUsuario)
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        // --- Actualización perfil (sin email/contraseña) ---
        public async Task<bool> UpdatePerfilBasicoAsync(
            int idUsuario,
            string nuevoUsername,
            string? nombre,
            string? apellido,
            string? telefono,
            string? direccion,
            string? ciudad,
            string? provincia,
            string? codigoPostal)
        {
            var u = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);
            if (u == null) return false;

            // (Opcional) validar unicidad del username aquí si lo necesitas
            u.Username = nuevoUsername;
            u.Nombre = nombre;
            u.Apellido = apellido;
            u.Telefono = telefono;
            u.Direccion = direccion;
            u.Ciudad = ciudad;
            u.Provincia = provincia;
            u.CodigoPostal = codigoPostal;

            await _context.SaveChangesAsync();
            return true;
        }

        // --- Avatar ---
        public async Task<bool> UpdateAvatarAsync(int idUsuario, byte[] avatarBytes)
        {
            var u = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);
            if (u == null) return false;
            u.AvatarFoto = avatarBytes;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAvatarAsync(int idUsuario)
        {
            var u = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);
            if (u == null) return false;
            u.AvatarFoto = null;
            await _context.SaveChangesAsync();
            return true;
        }

        // --- Email ---
        public Task<bool> EmailExistsExceptAsync(int idUsuario, string email)
            => _context.Usuarios.AnyAsync(u => u.Email == email && u.IdUsuario != idUsuario);

        public async Task<bool> ChangeEmailAsync(int idUsuario, string newEmail)
        {
            var u = await _context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);
            if (u == null) return false;
            u.Email = newEmail;
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
