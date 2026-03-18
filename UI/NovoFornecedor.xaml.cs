using Dominio;
using Repositorio;
using System.Text.RegularExpressions;
using System.Windows;

namespace UI
{
    public partial class NovoFornecedor : Window
    {
        // guarda fornecedor quando for edição
        private Fornecedor fornecedorEditar;

        public NovoFornecedor()
        {
            InitializeComponent();
        }

        // construtor para edição
        public NovoFornecedor(Fornecedor fornecedor)
        {
            InitializeComponent();

            fornecedorEditar = fornecedor;

            txtNome.Text = fornecedor.Nome;
            txtCnpj.Text = fornecedor.CNPJ;
            txtTelefone.Text = fornecedor.Telefone;
            txtEmail.Text = fornecedor.Email;
        }

        // permite apenas números
        private void NumeroSomente(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SalvarFornecedor(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                // 🔥 SE FOR EDIÇÃO
                if (fornecedorEditar != null)
                {
                    var f = context.Fornecedores.Find(fornecedorEditar.Id);

                    if (f != null)
                    {
                        f.Nome = txtNome.Text;
                        f.CNPJ = txtCnpj.Text;
                        f.Telefone = txtTelefone.Text;
                        f.Email = txtEmail.Text;
                    }
                }
                else
                {
                    // 🔥 NOVO CADASTRO
                    Fornecedor f = new Fornecedor
                    {
                        Nome = txtNome.Text,
                        CNPJ = txtCnpj.Text,
                        Telefone = txtTelefone.Text,
                        Email = txtEmail.Text
                    };

                    context.Fornecedores.Add(f);
                }

                context.SaveChanges();
            }

            MessageBox.Show("Fornecedor salvo com sucesso!");
            Close();
        }
    }
}