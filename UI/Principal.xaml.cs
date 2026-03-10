using Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
        private string tipoUsuario;

        private void MostrarUsuarios(object sender, RoutedEventArgs e)
        {
            if (tipoUsuarioAtual != "Admin")
            {
                MessageBox.Show("Apenas administradores podem acessar usuários.");
                return;
            }

            PainelProdutos.Visibility = Visibility.Collapsed;
            PainelVendas.Visibility = Visibility.Collapsed;
            PainelUsuarios.Visibility = Visibility.Visible;

            FormularioUsuario.Visibility = Visibility.Visible;
        }

        
        public Principal(string usuarioAtual, string tipoUsuario)
        {
            InitializeComponent();

            BoxUsuarioAtual.Text = usuarioAtual + " (" + tipoUsuario + ")";

            tipoUsuarioAtual = tipoUsuario;
            this.tipoUsuario = tipoUsuario;
        }

        // ===================== CONTROLE DE TELA =====================

        private string tipoUsuarioAtual;

        // ===================== USUARIOS =====================

        private void BtnConsultarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                gridUsuarios.ItemsSource = context.Usuarios.ToList();
            }
        }

        private void BtnNovoUsuario_Click(object sender, RoutedEventArgs e)
        {
            FormularioUsuario.Visibility = Visibility.Visible;
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

        private void BtnSalvarUsuario_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                Usuario u = new Usuario();

                u.Nome = txtNomeUsuario.Text.Trim();
                u.Email = txtEmailUsuario.Text.Trim();
                u.Senha = UsuarioModel.Codificar(txtSenhaUsuario.Password); 
                u.TipoUsuario = (comboTipoUsuario.SelectedItem as ComboBoxItem).Content.ToString();

                context.Usuarios.Add(u);
                context.SaveChanges();
            }

            MessageBox.Show("Usuário criado!");

            BtnConsultarUsuarios_Click(null, null);
        }

        private void BtnEditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            if (gridUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            gridUsuarios.BeginEdit();
        }

        private void MostrarProdutos(object sender, RoutedEventArgs e)
        {
            PainelProdutos.Visibility = Visibility.Visible;
            PainelVendas.Visibility = Visibility.Collapsed;
            PainelUsuarios.Visibility = Visibility.Collapsed;

            BtnProdutos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));
            BtnVendas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
        }

        private void MostrarVendas(object sender, RoutedEventArgs e)
        {
            PainelProdutos.Visibility = Visibility.Collapsed;
            PainelVendas.Visibility = Visibility.Visible;
            PainelUsuarios.Visibility = Visibility.Collapsed;

            BtnVendas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));
            BtnProdutos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));
        }

        // ===================== PRODUTOS =====================

        private void BtnCadastroProduto(object sender, RoutedEventArgs e)
        {
            if (tipoUsuario == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem cadastrar produtos.",
                                "Acesso negado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            NovoProduto novoProduto = new NovoProduto();
            novoProduto.ShowDialog();
        }

        private void btnEditarProduto(object sender, RoutedEventArgs e)
        {
            if (tipoUsuario == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem editar produtos.",
                                "Acesso negado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
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
                MessageBox.Show("Selecione um item");
            }
        }

        private async void BtnConsultarProduto(object sender, RoutedEventArgs e)
        {
            ProdutoModel _pModel = new ProdutoModel();
            gridProdutos.ItemsSource = await _pModel.ListarProdutos();
        }

        private void BtnExcluirProduto(object sender, RoutedEventArgs e)
        {
            if (tipoUsuario == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem excluir produtos.",
                                "Acesso negado",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            if (gridProdutos.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto para excluir");
                return;
            }

            var produtoSelecionado = (Produto)gridProdutos.SelectedItem;

            var confirmacao = MessageBox.Show(
                "Tem certeza que deseja excluir este produto?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacao != MessageBoxResult.Yes)
                return;

            using (var context = new Context())
            {
                var produto = context.Produtos
                    .FirstOrDefault(p => p.Id == produtoSelecionado.Id);

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
                "Deseja incluir nome e cpf do cliente?",
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
            VendaModel _vModel = new VendaModel();
            gridVendas.ItemsSource = await _vModel.ListarVendas();
        }

        private void BtnExcluir(object sender, RoutedEventArgs e)
        {
            if (gridVendas.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma venda para excluir");
                return;
            }

            var vendaselecionada = (Venda)gridVendas.SelectedItem;

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
                    .FirstOrDefault(v => v.Id == vendaselecionada.Id);

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
                MessageBox.Show("Selecione uma venda para editar");
                return;
            }

            var vendaSelecionada = (Venda)gridVendas.SelectedItem;

            EditarVenda tela = new EditarVenda(vendaSelecionada);
            tela.ShowDialog();

            BtnConsultarVenda(null, null);
        }
    }
}