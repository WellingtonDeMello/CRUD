using Dominio;
using Repositorio;
using System.Linq;
using System.Windows;

namespace UI
{
    public partial class EditarVenda : Window
    {
        private int _vendaId;

        public EditarVenda(Venda venda)
        {
            InitializeComponent();

            _vendaId = venda.Id;

            boxNome.Text = venda.ClienteNome;
            boxCpf.Text = venda.ClienteDocumento;
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                var venda = context.Vendas
                    .FirstOrDefault(v => v.Id == _vendaId);

                if (venda != null)
                {
                    venda.ClienteNome = boxNome.Text;
                    venda.ClienteDocumento = boxCpf.Text;

                    context.SaveChanges();
                }
            }

            MessageBox.Show("Venda editada com sucesso!");
            Close();
        }
    }
}