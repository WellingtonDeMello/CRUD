using Dominio;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Model;
using UI.Utils;

namespace UI
{
    public partial class Principal : Window
    {
        // Guarda o tipo do usuário logado (Admin ou Funcionario)
        private string tipoUsuarioAtual;

        public Principal(string usuarioAtual, string tipoUsuario)
        {
            InitializeComponent();

            // Exibe usuário logado no topo da tela
            BoxUsuarioAtual.Text = $"{usuarioAtual} ({tipoUsuario})";

            // Define tipo para controle de permissões
            tipoUsuarioAtual = tipoUsuario;
        }

        //CONTROLE DE BOTÕES (MENU LATERAL)

        // Reseta todos os botões para cor padrão 
        private void ResetarBotoes()
        {
            var corPadrao = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#374151"));

            BtnProdutos.Background = corPadrao;
            BtnVendas.Background = corPadrao;
            BtnUsuarios.Background = corPadrao;
            BtnFornecedores.Background = corPadrao;
        }

        //MOSTRAR PAINÉIS

        private void MostrarProdutos(object sender, RoutedEventArgs e)
        {
            ResetarBotoes(); // remove seleção anterior

            // destaca botão ativo
            BtnProdutos.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));

            // exibe painel correto
            PainelProdutos.Visibility = Visibility.Visible;

            // oculta os outros
            PainelVendas.Visibility = Visibility.Collapsed;
            PainelUsuarios.Visibility = Visibility.Collapsed;
            PainelFornecedores.Visibility = Visibility.Collapsed;
        }

        private void MostrarVendas(object sender, RoutedEventArgs e)
        {
            ResetarBotoes();
            BtnVendas.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));

            PainelVendas.Visibility = Visibility.Visible;
            PainelProdutos.Visibility = Visibility.Collapsed;
            PainelUsuarios.Visibility = Visibility.Collapsed;
            PainelFornecedores.Visibility = Visibility.Collapsed;
        }

        private void MostrarUsuarios(object sender, RoutedEventArgs e)
        {
            // Apenas Admin pode acessar usuários
            if (tipoUsuarioAtual != "Admin")
            {
                MessageBox.Show("Apenas administradores podem acessar usuários.");
                return;
            }

            ResetarBotoes();
            BtnUsuarios.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));

            PainelUsuarios.Visibility = Visibility.Visible;
            PainelProdutos.Visibility = Visibility.Collapsed;
            PainelVendas.Visibility = Visibility.Collapsed;
            PainelFornecedores.Visibility = Visibility.Collapsed;
        }

        private void MostrarFornecedores(object sender, RoutedEventArgs e)
        {
            ResetarBotoes();
            BtnFornecedores.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6900"));

            PainelFornecedores.Visibility = Visibility.Visible;
            PainelProdutos.Visibility = Visibility.Collapsed;
            PainelVendas.Visibility = Visibility.Collapsed;
            PainelUsuarios.Visibility = Visibility.Collapsed;
        }

        //USUÁRIOS

        private void BtnConsultarUsuarios_Click(object sender, RoutedEventArgs e)
        {
            // Busca todos os usuários no banco e exibe no grid
            using (var context = new Context())
            {
                gridUsuarios.ItemsSource = context.Usuarios.ToList();
            }
        }

        private void BtnSalvarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Validação
            if (Validacoes.CampoVazio(txtNomeUsuario.Text) ||
                Validacoes.CampoVazio(txtEmailUsuario.Text) ||
                Validacoes.CampoVazio(txtSenhaUsuario.Password))
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            // Validação do email
            if (!Validacoes.EmailValido(txtEmailUsuario.Text))
            {
                MessageBox.Show("Email inválido.");
                return;
            }

            // Validação de seleção do tipo de usuário
            if (comboTipoUsuario.SelectedItem == null)
            {
                MessageBox.Show("Selecione o tipo de usuário.");
                return;
            }

            using (var context = new Context())
            {
                //não permitir email duplicado
                if (context.Usuarios.Any(u => u.Email == txtEmailUsuario.Text))
                {
                    MessageBox.Show("Já existe um usuário com este email.");
                    return;
                }

                // Criação do objeto usuário
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

            //limpa os campos após salvar
            txtNomeUsuario.Clear();
            txtEmailUsuario.Clear();
            txtSenhaUsuario.Clear();
            comboTipoUsuario.SelectedIndex = -1;

            BtnConsultarUsuarios_Click(null, null);
        }

        private void BtnExcluirUsuario_Click(object sender, RoutedEventArgs e)
        {
            
            if (gridUsuarios.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            var usuario = (Usuario)gridUsuarios.SelectedItem;

            
            var confirm = MessageBox.Show("Deseja excluir este usuário?", "Confirmar", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes) return;

            using (var context = new Context())
            {
                context.Usuarios.Remove(usuario);
                context.SaveChanges();
            }

            MessageBox.Show("Usuário excluído.");
            BtnConsultarUsuarios_Click(null, null);
        }

        //PRODUTOS

        private void BtnCadastroProduto(object sender, RoutedEventArgs e)
        {
            
            if (tipoUsuarioAtual == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem cadastrar produtos.");
                return;
            }

            new NovoProduto().ShowDialog();

            
            BtnConsultarProduto(null, null);
        }

        private void btnEditarProduto(object sender, RoutedEventArgs e)
        {
            if (tipoUsuarioAtual == "Funcionario")
            {
                MessageBox.Show("Funcionários não podem editar produtos.");
                return;
            }

            if (gridProdutos.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto.");
                return;
            }

            // Abre tela passando produto selecionado
            new NovoProduto((Produto)gridProdutos.SelectedItem).ShowDialog();

            BtnConsultarProduto(null, null);
        }

        private async void BtnConsultarProduto(object sender, RoutedEventArgs e)
        {
           
            ProdutoModel model = new ProdutoModel();
            gridProdutos.ItemsSource = await model.ListarProdutos();
        }

        private void BtnExcluirProduto(object sender, RoutedEventArgs e)
        {
            if (gridProdutos.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto.");
                return;
            }

            var confirm = MessageBox.Show("Deseja excluir este produto?", "Confirmar", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes) return;

            using (var context = new Context())
            {
                var produto = context.Produtos.Find(((Produto)gridProdutos.SelectedItem).Id);

                if (produto != null)
                {
                    context.Produtos.Remove(produto);
                    context.SaveChanges();
                }
            }

            MessageBox.Show("Produto excluído!");
            BtnConsultarProduto(null, null);
        }

        //VENDAS

        private void BtnNovaVendaDialog(object sender, RoutedEventArgs e)
        {
            // Abre tela para informar dados do cliente
            ClienteDialog dialog = new ClienteDialog();

            // Se usuário confirmou, abre tela de venda
            if (dialog.ShowDialog() == true)
            {
                new NovaVenda(dialog.NomeCliente, dialog.CpfCliente).ShowDialog();
            }
        }

        private void BtnConsultarVenda(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                // Carrega vendas + produtos relacionados
                gridVendas.ItemsSource = context.Vendas
                    .Include(v => v.VendaProdutos)
                    .ThenInclude(vp => vp.Produto)
                    .ToList();
            }
        }

        private void gridVendas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridVendas.SelectedItem == null) return;

            using (var context = new Context())
            {
                // Busca venda completa com produtos
                var venda = context.Vendas
                    .Include(v => v.VendaProdutos)
                    .ThenInclude(vp => vp.Produto)
                    .FirstOrDefault(v => v.Id == ((Venda)gridVendas.SelectedItem).Id);

                // Exibe produtos da venda
                gridVendaProdutos.ItemsSource = venda?.VendaProdutos;
            }
        }

        private void BtnExcluir(object sender, RoutedEventArgs e)
        {
            if (gridVendas.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma venda.");
                return;
            }

            var confirm = MessageBox.Show("Deseja excluir esta venda?", "Confirmar", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes) return;

            using (var context = new Context())
            {
                context.Vendas.Remove((Venda)gridVendas.SelectedItem);
                context.SaveChanges();
            }

            MessageBox.Show("Venda excluída!");
            BtnConsultarVenda(null, null);
        }

        private void editar(object sender, RoutedEventArgs e)
        {
            if (gridVendas.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma venda.");
                return;
            }

            // Abre tela de edição da venda
            new EditarVenda((Venda)gridVendas.SelectedItem).ShowDialog();

            BtnConsultarVenda(null, null);
        }

        //FORNECEDORES

        private void BtnConsultarFornecedor(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                gridFornecedores.ItemsSource = context.Fornecedores.ToList();
            }
        }

        private void BtnCadastrarFornecedor(object sender, RoutedEventArgs e)
        {
            new NovoFornecedor().ShowDialog();

            BtnConsultarFornecedor(null, null);
        }

        private void BtnEditarFornecedor(object sender, RoutedEventArgs e)
        {
            if (gridFornecedores.SelectedItem == null)
            {
                MessageBox.Show("Selecione um fornecedor.");
                return;
            }

            new NovoFornecedor((Fornecedor)gridFornecedores.SelectedItem).ShowDialog();

            BtnConsultarFornecedor(null, null);
        }

        private void BtnExcluirFornecedor(object sender, RoutedEventArgs e)
        {
            if (gridFornecedores.SelectedItem == null)
            {
                MessageBox.Show("Selecione um fornecedor.");
                return;
            }

            var confirm = MessageBox.Show("Deseja excluir este fornecedor?", "Confirmar", MessageBoxButton.YesNo);
            if (confirm != MessageBoxResult.Yes) return;

            using (var context = new Context())
            {
                context.Fornecedores.Remove((Fornecedor)gridFornecedores.SelectedItem);
                context.SaveChanges();
            }

            MessageBox.Show("Fornecedor excluído!");
            BtnConsultarFornecedor(null, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Iniciar iniciar = new Iniciar();
            iniciar.Show();
            this.Close();
        }
    }
}