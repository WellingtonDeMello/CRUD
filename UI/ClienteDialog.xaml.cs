using System.Text.RegularExpressions;
using System.Windows;

namespace UI
{
    public partial class ClienteDialog : Window
    {
        public string NomeCliente { get; set; }
        public string CpfCliente { get; set; }

        public ClienteDialog()
        {
            InitializeComponent();
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            string cpf = Regex.Replace(txtCpf.Text, @"\D", "");

            if (txtNome.Text == "" || !Regex.IsMatch(cpf, @"^\d{11}$"))
            {
                MessageBox.Show("Dados inválidos.");
                return;
            }

            NomeCliente = txtNome.Text;
            CpfCliente = cpf;

            DialogResult = true;
            Close();
        }
    }
}