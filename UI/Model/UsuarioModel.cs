using Dominio;
using Repositorio;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    public class UsuarioModel
    {
        private UsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();

        public async Task<bool> CriarUsuario(string nome, string email, string senha)
        {
            Usuario novoUsuario = new Usuario(nome, email, Codificar(senha));
            return await _usuarioRepositorio.AddIfEmailNotExist(novoUsuario);
        }

        public async Task<Usuario> Entrar(string email, string senha)
        {
            using (var context = new Context())
            {
                string senhaCodificada = Codificar(senha);

                var usuario = context.Usuarios
                    .FirstOrDefault(u => u.Email == email && u.Senha == senhaCodificada);

                return usuario;
            }
        }

        public static string Codificar(string texto)
        {
            var md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(texto);
            byte[] hash = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
