using MySocialPet.Models;
using MySocialPet.Tools;
using Microsoft.Data.SqlClient;
using MySocialPet.Models.Autenticacion;

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

    }
}
