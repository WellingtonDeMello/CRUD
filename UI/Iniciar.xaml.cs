using SeuProjeto;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UI.Model;
using UI.Utils;

namespace UI
{
    public partial class Iniciar : Window
    {
        public Iniciar()
        {
            InitializeComponent();
        }

        private void txtNome_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholderNome.Visibility =
                string.IsNullOrWhiteSpace(txtNome.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }


        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholderEmail.Visibility =
                string.IsNullOrWhiteSpace(txtEmail.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }


        private void txtSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            placeholderSenha.Visibility =
                string.IsNullOrEmpty(txtSenha.Password)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private void txtRepetirSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            placeholderRepetirSenha.Visibility =
                string.IsNullOrEmpty(txtRepetirSenha.Password)
                ? Visibility.Visible
                : Visibility.Hidden;
        }



        private void entrarButton_Click(object sender, RoutedEventArgs e)
        {
            Login logingestex = new Login();
            logingestex.Show(); // abre a tela de login

            this.Close(); // fecha a tela iniciar
        }

        private async void btnCadastro(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            UsuarioModel uModel = new UsuarioModel();

            // validar campos vazios
            if (Validacoes.CampoVazio(txtNome.Text) ||
                Validacoes.CampoVazio(txtEmail.Text) ||
                Validacoes.CampoVazio(txtSenha.Password))
            {
                MessageBox.Show("Preencha todos os campos.",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                IsEnabled = true;
                return;
            }

            // validar email
            if (!Validacoes.EmailValido(txtEmail.Text))
            {
                MessageBox.Show("Email inválido.",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                IsEnabled = true;
                return;
            }

            // validar senha
            if (txtSenha.Password != txtRepetirSenha.Password)
            {
                MessageBox.Show("As senhas são diferentes.",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                IsEnabled = true;
                return;
            }

            bool emailValido = await uModel.CriarUsuario(
                txtNome.Text,
                txtEmail.Text,
                txtSenha.Password
            );

            if (emailValido)
            {
                MessageBox.Show("Cadastrado com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Email já cadastrado!",
                    "Erro",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            IsEnabled = true;
        }
    }
}