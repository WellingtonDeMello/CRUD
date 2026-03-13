using Dominio;
using Dominio.Enum;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using UI.Model;

namespace UI
{
    public partial class NovaVenda : Window
    {
        VendaModel vModel = new VendaModel();
        List<VendaProduto> produtos = new List<VendaProduto>();

        decimal totalVenda = 0;

        public NovaVenda(string nomeCliente, string cpfCliente)
        {
            InitializeComponent();

            blockNomeCliente.Text = nomeCliente;
            blockCpfCliente.Text = cpfCliente;

            blockTotal.Text = "0";
        }

        // ===================== BUSCAR PRODUTO =====================

        private async void boxCodProduto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(boxCodProduto.Text))
                {
                    MessageBox.Show("Digite um código de produto!");
                    return;
                }

                if (!int.TryParse(boxCodProduto.Text, out int codigoProduto))
                {
                    MessageBox.Show("Código inválido!");
                    return;
                }

                var produto = await vModel.ProcurarProduto(codigoProduto);

                if (produto != null)
                {
                    blockNomeProduto.Text = produto.Descricao;
                }
                else
                {
                    blockNomeProduto.Text = "Produto não encontrado";
                    MessageBox.Show("Produto não encontrado!");
                }
            }
        }

        // ===================== ADICIONAR PRODUTO =====================

        private async void boxQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(boxCodProduto.Text) ||
                    string.IsNullOrWhiteSpace(boxQuantidade.Text))
                {
                    MessageBox.Show("Digite o código e a quantidade!");
                    return;
                }

                if (!int.TryParse(boxCodProduto.Text, out int codigoProduto))
                {
                    MessageBox.Show("Código inválido!");
                    return;
                }

                if (!int.TryParse(boxQuantidade.Text, out int quantidade))
                {
                    MessageBox.Show("Quantidade inválida!");
                    return;
                }

                Produto produto = await vModel.ProcurarProduto(codigoProduto);

                if (produto == null)
                {
                    MessageBox.Show("Produto não encontrado!");
                    return;
                }

                VendaProduto vendaProduto = new VendaProduto
                {
                    ProdutoId = produto.Id,
                    Produto = produto,
                    Quantidade = quantidade,
                    PrecoVenda = produto.PrecoVenda
                };

                produtos.Add(vendaProduto);

                NovaVendaCollection vendaGrid = new NovaVendaCollection(
                    produto.Id,
                    produto.Descricao,
                    produto.UnidadeDeMedida,
                    produto.PrecoVenda,
                    quantidade
                );

                gridVendaProduto.Items.Add(vendaGrid);

                totalVenda += vendaGrid.Total;
                blockTotal.Text = totalVenda.ToString("F2");

                // limpar campos
                boxCodProduto.Text = "";
                boxQuantidade.Text = "";
                blockNomeProduto.Text = "PRODUTO PESQUISADO";
                boxCodProduto.Focus();
            }
        }

        // ===================== CONFIRMAR VENDA =====================

        private async void btnConfirmarVenda(object sender, RoutedEventArgs e)
        {
            if (produtos.Count == 0)
            {
                MessageBox.Show("Adicione produtos antes de confirmar a venda!");
                return;
            }

            // abrir tela de pagamento
            FormaPagamento pagamento = new FormaPagamento(totalVenda);
            pagamento.ShowDialog();

            // montar lista final
            List<VendaProduto> vendaProdutos = new List<VendaProduto>();

            foreach (var p in produtos)
            {
                vendaProdutos.Add(new VendaProduto
                {
                    ProdutoId = p.ProdutoId,
                    PrecoVenda = p.PrecoVenda,
                    Quantidade = p.Quantidade
                });
            }

            bool status = await vModel.NovaVenda(
                blockCpfCliente.Text,
                blockNomeCliente.Text,
                totalVenda,
                boxObs.Text,
                vendaProdutos
            );

            if (status)
            {
                MessageBox.Show("Venda cadastrada com sucesso!");
                Close();
            }
            else
            {
                MessageBox.Show("Erro ao cadastrar venda!", "ERRO",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void gridVendaProduto_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void boxQuantidade_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }

    // ===================== CLASSE DA GRID =====================

    class NovaVendaCollection
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public UnidadeMedida UnidadeDeMedida { get; set; }
        public decimal PrecoVenda { get; set; }
        public int QuantidadeProduto { get; set; }
        public decimal Total { get; set; }

        public NovaVendaCollection(
            int produtoId,
            string produtoNome,
            UnidadeMedida unidadeDeMedida,
            decimal precoVenda,
            int quantidadeProduto)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            UnidadeDeMedida = unidadeDeMedida;
            PrecoVenda = precoVenda;
            QuantidadeProduto = quantidadeProduto;
            Total = quantidadeProduto * precoVenda;
        }
    }
}