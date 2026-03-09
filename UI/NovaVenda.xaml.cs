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
        List<Produto> produtos = new List<Produto>();

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

                produtos.Add(produto);

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

            bool status = await vModel.NovaVenda(
                blockCpfCliente.Text,
                blockNomeCliente.Text,
                decimal.Parse(blockTotal.Text),
                boxObs.Text,
                produtos
            );

            if (status == true)
            {
                MessageBox.Show("Venda cadastrada com sucesso!");
                Close();

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