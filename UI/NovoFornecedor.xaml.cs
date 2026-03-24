using Dominio;
using Repositorio;
using System.Text.RegularExpressions;
using System.Windows;

namespace UI
{
    public partial class NovoFornecedor : Window
    {
        // Guarda fornecedor quando for edição
        private Fornecedor fornecedorEditar;

      
        public NovoFornecedor()
        {
            InitializeComponent();
        }

        //CONSTRUTOR PARA EDIÇÃO
        public NovoFornecedor(Fornecedor fornecedor) : this()
        {
            fornecedorEditar = fornecedor;

            txtNome.Text = fornecedor.Nome;
            txtCnpj.Text = fornecedor.CNPJ;
            txtTelefone.Text = fornecedor.Telefone;
            txtEmail.Text = fornecedor.Email;
        }

        // Permite apenas números (CNPJ e telefone)
        private void NumeroSomente(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        // SALVAR (CRUD)
        private void SalvarFornecedor(object sender, RoutedEventArgs e)
        {
            //  VALIDAÇÕES 

            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtCnpj.Text) ||
                string.IsNullOrWhiteSpace(txtTelefone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            // Email 
            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Email inválido.");
                return;
            }

            // CNPJ deve ter 14 números
            if (txtCnpj.Text.Length != 14)
            {
                MessageBox.Show("CNPJ deve ter 14 números.");
                return;
            }

            // Telefone válido (10 ou 11 números)
            if (txtTelefone.Text.Length < 10 || txtTelefone.Text.Length > 11)
            {
                MessageBox.Show("Telefone inválido.");
                return;
            }

            //BANCO 

            using (var context = new Context())
            {
                //  EDIÇÃO
                if (fornecedorEditar != null)
                {
                    var f = context.Fornecedores.Find(fornecedorEditar.Id);

                    if (f != null)
                    {
                        f.Nome = txtNome.Text.Trim();
                        f.CNPJ = txtCnpj.Text.Trim();
                        f.Telefone = txtTelefone.Text.Trim();
                        f.Email = txtEmail.Text.Trim();
                    }
                }
                else
                {
                    //  CADASTRO
                    Fornecedor f = new Fornecedor
                    {
                        Nome = txtNome.Text.Trim(),
                        CNPJ = txtCnpj.Text.Trim(),
                        Telefone = txtTelefone.Text.Trim(),
                        Email = txtEmail.Text.Trim()
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