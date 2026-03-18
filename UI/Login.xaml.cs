using System.Windows;
using UI.Model;
using UI.Utils;

namespace UI
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        // CONTROLE PLACEHOLDER EMAIL
        private void emailBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            emailPlaceholder.Visibility =
                string.IsNullOrWhiteSpace(emailBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        // CONTROLE PLACEHOLDER SENHA
        private void passBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            senhaPlaceholder.Visibility =
                string.IsNullOrWhiteSpace(passBox.Password)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private async void btnEntrar(object sender, RoutedEventArgs e)
        {


            IsEnabled = false;

            UsuarioModel uModel = new UsuarioModel();

            var usuario = await uModel.Entrar(emailBox.Text, passBox.Password);

            if (usuario != null)
            {
                Principal principalWindow = new Principal(usuario.Nome, usuario.TipoUsuario);

                principalWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Email ou Senha inválido");
                IsEnabled = true;
            }
        }

        private void btnNovo(object sender, RoutedEventArgs e)
        {
            Iniciar iniciarWindow = new Iniciar();
            iniciarWindow.ShowDialog();
        }

        private void btnLista(object sender, RoutedEventArgs e)
        {
            ListaUsuariosWindow listaUsuariosWindow = new ListaUsuariosWindow();
            listaUsuariosWindow.ShowDialog();
        }
    }
}