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

        public NovaVenda(string nomeCliente, string cpfCliente)
        {
            InitializeComponent();
            blockNomeCliente.Text = nomeCliente;
            blockCpfCliente.Text = cpfCliente;
        }

        private async void boxCodProduto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(boxCodProduto.Text))
                {
                    MessageBox.Show("Digite um código de produto!");
                    return;
                }

                int codigoProduto;

                if (!int.TryParse(boxCodProduto.Text, out codigoProduto))
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

        private async void boxQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrWhiteSpace(boxCodProduto.Text) || string.IsNullOrWhiteSpace(boxQuantidade.Text))
                {
                    MessageBox.Show("Digite o código e a quantidade!");
                    return;
                }

                int codigoProduto;
                int quantidade;

                if (!int.TryParse(boxCodProduto.Text, out codigoProduto))
                {
                    MessageBox.Show("Código inválido!");
                    return;
                }

                if (!int.TryParse(boxQuantidade.Text, out quantidade))
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

                VendaProduto vendaProduto = new VendaProduto();
                vendaProduto.ProdutoId = produto.Id;
                vendaProduto.Produto = produto;
                vendaProduto.Quantidade = quantidade;
                vendaProduto.PrecoVenda = produto.PrecoVenda;

                produtos.Add(vendaProduto);

                NovaVendaCollection vendas = new NovaVendaCollection(
                    produto.Id,
                    produto.Descricao,
                    produto.UnidadeDeMedida,
                    produto.PrecoVenda,
                    quantidade
                );
                gridVendaProduto.Items.Add(vendas);

                decimal totalAtual = decimal.Parse(blockTotal.Text);
                blockTotal.Text = (totalAtual + vendas.Total).ToString();

                // limpar campos
                boxCodProduto.Text = "";
                boxQuantidade.Text = "";
                blockNomeProduto.Text = "PRODUTO PESQUISADO";
            }
        }

        private async void btnConfirmarVenda(object sender, RoutedEventArgs e)
        {
            if (produtos.Count == 0)
            {
                MessageBox.Show("Adicione produtos antes de confirmar a venda!");
                return;
            }

            // Criar lista de VendaProduto
            List<VendaProduto> vendaProdutos = new List<VendaProduto>();

            foreach (var p in produtos)
            {
                vendaProdutos.Add(new VendaProduto
                {
                    ProdutoId = p.ProdutoId,       // apenas o Id
                    PrecoVenda = p.PrecoVenda,     // preço do produto
                    Quantidade = p.Quantidade  // quantidade vendida
                });
            }

            // Chama o model para salvar a venda
            bool status = await vModel.NovaVenda(
                blockCpfCliente.Text,
                blockNomeCliente.Text,
                decimal.Parse(blockTotal.Text),
                boxObs.Text,
                vendaProdutos // passe VendaProduto, não Produto
            );

            if (status)
            {
                MessageBox.Show("Venda cadastrada com sucesso!");
                Close();
            }
            else
            {
                MessageBox.Show("Erro ao cadastrar venda!", "ERRO", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void gridVendaProduto_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }

    class NovaVendaCollection
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public UnidadeMedida UnidadeDeMedida { get; set; }
        public decimal PrecoVenda { get; set; }
        public int QuantidadeProduto { get; set; }
        public decimal Total { get; set; }

        public NovaVendaCollection(int produtoId, string produtoNome, UnidadeMedida unidadeDeMedida, decimal precoVenda, int quantidadeProduto)
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