using Dominio;
using Repositorio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Model
{
    public class FornecedorModel
    {
        Context _context = new Context();

        // Lista todos fornecedores
        public async Task<List<Fornecedor>> ListarFornecedores()
        {
            return _context.Fornecedores.ToList();
        }

        // Adiciona fornecedor
        public async Task<bool> AdicionarFornecedor(
            string nome,
            string cnpj,
            string telefone,
            string email,
            bool ativo)
        {
            Fornecedor fornecedor = new Fornecedor();

            fornecedor.Nome = nome;
            fornecedor.CNPJ = cnpj;
            fornecedor.Telefone = telefone;
            fornecedor.Email = email;
            fornecedor.Ativo = ativo;

            _context.Fornecedores.Add(fornecedor);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}