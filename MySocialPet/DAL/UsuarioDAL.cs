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

        public Usuario GetUsuarioLogin(string userName, string pwd)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Username == userName);

            if (usuario != null && PasswordHelper.VerifyPasswordHash(pwd, usuario.PasswordHash, usuario.PasswordSalt))
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
    }
}
