using Dominio;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Model;

namespace UI
{
    public partial class Principal : Window
    {
        private string tipoUsuarioAtual;

        public Principal(string usuarioAtual, string tipoUsuario)
        {
            InitializeComponent();

            BoxUsuarioAtual.Text = $"{usuarioAtual} ({tipoUsuario})";
            tipoUsuarioAtual = tipoUsuario;
        }

        // ===================== MOSTRAR PAINÉIS =====================
        private void MostrarUsuarios(object sender, RoutedEventArgs e)
        {
            if (tipoUsuarioAtual != "Admin")
            {
                MessageBox.Show("Apenas administradores podem acessar usuários.");
                return;
            }

            PainelUsuarios.Visibility = Visibility.Visible;
            PainelProdutos.Visibility = Visibility.Collapsed;
            PainelVendas.Visibility = Visibility.Collapsed;

            BtnUsuarios.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));
            BtnProdutos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
            BtnVendas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
        }

        private void MostrarProdutos(object sender, RoutedEventArgs e)
        {
            PainelProdutos.Visibility = Visibility.Visible;
            PainelVendas.Visibility = Visibility.Collapsed;
            PainelUsuarios.Visibility = Visibility.Collapsed;

            BtnProdutos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));
            BtnVendas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
            BtnUsuarios.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
        }

        private void MostrarVendas(object sender, RoutedEventArgs e)
        {
            PainelVendas.Visibility = Visibility.Visible;
            PainelProdutos.Visibility = Visibility.Collapsed;
            PainelUsuarios.Visibility = Visibility.Collapsed;

            BtnVendas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));
            BtnProdutos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
            BtnUsuarios.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
        }

        // ===================== USUÁRIOS =====================
      
        private void BtnConsultarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                gridUsuarios.ItemsSource = context.Usuarios.ToList();
            }
        }

        private void BtnSalvarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmailUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtSenhaUsuario.Password) ||
                comboTipoUsuario.SelectedItem == null)
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            using (var context = new Context())
            {
                Usuario u = new Usuario
                {
                    Nome = txtNomeUsuario.Text.Trim(),
                    Email = txtEmailUsuario.Text.Trim(),
                    Senha = UsuarioModel.Codificar(txtSenhaUsuario.Password),
                    TipoUsuario = (comboTipoUsuario.SelectedItem as ComboBoxItem).Content.ToString()
                };

                context.Usuarios.Add(u);
                context.SaveChanges();
            }

            MessageBox.Show("Usuário criado com sucesso!");

          

            // Limpar campos
            txtNomeUsuario.Clear();
            txtEmailUsuario.Clear();
            txtSenhaUsuario.Clear();
            comboTipoUsuario.SelectedIndex = -1;
        }

        private void BtnExcluirUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (gridUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            var usuario = (Usuario)gridUsuarios.SelectedItem;

            var confirm = MessageBox.Show(
                "Deseja excluir este usuário?",
                "Confirmar",
                MessageBoxButton.YesNo);

            if (confirm != MessageBoxResult.Yes)
                return;

            using (var context = new Context())
            {
                context.Usuarios.Remove(usuario);
                context.SaveChanges();
            }

            MessageBox.Show("Usuário excluído.");
            BtnConsultarUsuarios_Click(null, null);
        }

        private void BtnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (gridUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            var usuario = (Usuario)gridUsuarios.SelectedItem;

            // Preencher a telinha com os dados existentes
            txtNomeUsuario.Text = usuario.Nome;
            txtEmailUsuario.Text = usuario.Email;
            txtSenhaUsuario.Password = ""; // Sempre pedir nova senha
            comboTipoUsuario.SelectedItem = comboTipoUsuario.Items.Cast<ComboBoxItem>()
                                             .FirstOrDefault(i => i.Content.ToString() == usuario.TipoUsuario);

           

            // Ao salvar, você pode diferenciar edição de criação pelo ID (isso precisaria de um campo auxiliar se quiser editar)
        }

        private void BtnExcluirUsuarioo_Click(object sender, RoutedEventArgs e)
        {
            if (gridUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            var usuario = (Usuario)gridUsuarios.SelectedItem;

            var confirm = MessageBox.Show(
                "Deseja excluir este usuário?",
                "Confirmar",
                MessageBoxButton.YesNo);

            if (confirm != MessageBoxResult.Yes)
                return;

            using (var context = new Context())
            {
                context.Usuarios.Remove(usuario);
                context.SaveChanges();
            }

            MessageBox.Show("Usuário excluído.");
            BtnConsultarUsuarios_Click(null, null);
        }

        // ===================== PRODUTOS =====================
        private void BtnCadastroProduto(object sender, RoutedEventArgs e)
        {
            if (tipoUsuarioAtual == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem cadastrar produtos.");
                return;
            }

            NovoProduto novoProduto = new NovoProduto();
            novoProduto.ShowDialog();
        }

        private void btnEditarProduto(object sender, RoutedEventArgs e)
        {
            if (tipoUsuarioAtual == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem editar produtos.");
                return;
            }

            Produto produto = (Produto)gridProdutos.SelectedItem;

            if (produto != null)
            {
                NovoProduto novoProduto = new NovoProduto(produto);
                novoProduto.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selecione um produto.");
            }
        }

        private async void BtnConsultarProduto(object sender, RoutedEventArgs e)
        {
            ProdutoModel _pModel = new ProdutoModel();
            gridProdutos.ItemsSource = await _pModel.ListarProdutos();
        }

        private void BtnExcluirProduto(object sender, RoutedEventArgs e)
        {
            if (tipoUsuarioAtual == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem excluir produtos.");
                return;
            }

            if (gridProdutos.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto para excluir.");
                return;
            }

            var produtoSelecionado = (Produto)gridProdutos.SelectedItem;

            using (var context = new Context())
            {
                var produto = context.Produtos.Find(produtoSelecionado.Id);
                if (produto != null)
                {
                    context.Produtos.Remove(produto);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Produto excluído com sucesso!");
            BtnConsultarProduto(null, null);
        }

        // ===================== VENDAS =====================
        private void BtnNovaVendaDialog(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Deseja incluir nome e CPF do cliente?",
                "Nome e CPF do Cliente",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    NomeCpf nomeCpf = new NomeCpf();
                    nomeCpf.ShowDialog();
                    break;

                case MessageBoxResult.No:
                    NovaVenda novaVenda = new NovaVenda("Não informado", "Não informado");
                    novaVenda.ShowDialog();
                    break;
            }
        }

        private async void BtnConsultarVenda(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                gridVendas.ItemsSource = context.Vendas
                    .Include(v => v.VendaProdutos)
                    .ThenInclude(vp => vp.Produto)
                    .ToList();
            }
        }

        private void gridVendas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridVendas.SelectedItem == null) return;

            var vendaSelecionada = (Venda)gridVendas.SelectedItem;

            using (var context = new Context())
            {
                var vendaCompleta = context.Vendas
                    .Include(v => v.VendaProdutos)
                    .ThenInclude(vp => vp.Produto)
                    .FirstOrDefault(v => v.Id == vendaSelecionada.Id);

                if (vendaCompleta != null)
                    gridVendaProdutos.ItemsSource = vendaCompleta.VendaProdutos;
            }
        }

        private void BtnExcluir(object sender, RoutedEventArgs e)
        {
            if (gridVendas.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma venda para excluir.");
                return;
            }

            var vendaSelecionada = (Venda)gridVendas.SelectedItem;

            var confirmacao = MessageBox.Show(
                "Tem certeza que deseja excluir esta venda?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacao != MessageBoxResult.Yes)
                return;

            using (var context = new Context())
            {
                var venda = context.Vendas
                    .Include(v => v.VendaProdutos)
                    .FirstOrDefault(v => v.Id == vendaSelecionada.Id);

                if (venda != null)
                {
                    context.VendaProdutos.RemoveRange(venda.VendaProdutos);
                    context.Vendas.Remove(venda);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Venda excluída com sucesso!");
            BtnConsultarVenda(null, null);
        }

        private void editar(object sender, RoutedEventArgs e)
        {
            if (gridVendas.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma venda para editar.");
                return;
            }

            var vendaSelecionada = (Venda)gridVendas.SelectedItem;
            EditarVenda tela = new EditarVenda(vendaSelecionada);
            tela.ShowDialog();
            BtnConsultarVenda(null, null);
        }
    }
}