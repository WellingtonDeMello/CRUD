using Dominio;
using Repositorio;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UI.Model
{
    public class UsuarioModel
    {
        private UsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();

        public async Task<bool> CriarUsuario(string nome, string email, string senha)
        {
            Usuario novoUsuario = new Usuario(
                nome.Trim(),
                email.Trim(),
                Codificar(senha)
            );

            return await _usuarioRepositorio.AddIfEmailNotExist(novoUsuario);
        }

        public async Task<Usuario> Entrar(string email, string senha)
        {
            using (var context = new Context())
            {
                string senhaCodificada = Codificar(senha);

                var usuario = await context.Usuarios
                    .FirstOrDefaultAsync(u =>
                        u.Email == email.Trim() &&
                        u.Senha == senhaCodificada
                    );

                return usuario;
            }
        }

        public static string Codificar(string texto)
        {
            using (var md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = md5.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}