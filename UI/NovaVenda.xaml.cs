using Dominio;
using Dominio.Enum;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using UI.Model;
using UI.Utils;

namespace UI
{
    public partial class NovaVenda : Window
    {
        // Model responsável pelas regras de negócio da venda 
        VendaModel vModel = new VendaModel();

        // Lista produtos adicionados na venda
        List<VendaProduto> produtos = new List<VendaProduto>();

        // Guardar o total da venda
        decimal totalVenda = 0;

        
        Produto produtoSelecionado = null;

        public NovaVenda(string nomeCliente, string cpfCliente)
        {
            InitializeComponent();

            //Pedir Dados do Cliente
            blockNomeCliente.Text = nomeCliente;
            blockCpfCliente.Text = cpfCliente;

            // Inicializa total como zero
            blockTotal.Text = "0";
        }

        //BUSCAR PRODUTO 

        private async void boxCodProduto_KeyUp(object sender, KeyEventArgs e)
        {
            // Executa apenas quando o usuário pressiona enter
            if (e.Key == Key.Enter)
            {
                // não permite buscar vazio
                if (string.IsNullOrWhiteSpace(boxCodProduto.Text))
                {
                    MessageBox.Show("Digite o código ou nome do produto!");
                    return;
                }

                Produto produto = null;

                // Se for número, busca por ID
                if (int.TryParse(boxCodProduto.Text, out int codigo))
                {
                    produto = await vModel.ProcurarProduto(codigo);
                }
                else
                {
                    // Caso contrário, busca por nome
                    produto = await vModel.ProcurarProdutoNome(boxCodProduto.Text);
                }

                //encontrou produto
                if (produto != null)
                {
                    produtoSelecionado = produto;
                    blockNomeProduto.Text = produto.Descricao;
                }
                else
                {
                    //não encontrou
                    produtoSelecionado = null;
                    blockNomeProduto.Text = "Produto não encontrado";
                }
            }
        }

        //ADICIONAR PRODUTO

        private async void boxQuantidade_KeyUp(object sender, KeyEventArgs e)
        {
            // Executa ao pressionar enter
            if (e.Key == Key.Enter)
            {
                // Impede adicionar sem selecionar produto
                if (produtoSelecionado == null)
                {
                    MessageBox.Show("Busque um produto primeiro!");
                    return;
                }

                // Valida se quantidade é número
                if (!int.TryParse(boxQuantidade.Text, out int quantidade))
                {
                    MessageBox.Show("Quantidade inválida!");
                    return;
                }

                // Cria objeto que será salvo no banco
                VendaProduto vendaProduto = new VendaProduto
                {
                    ProdutoId = produtoSelecionado.Id,
                    Produto = produtoSelecionado,
                    Quantidade = quantidade,
                    PrecoVenda = produtoSelecionado.PrecoVenda
                };

                // Adiciona na lista da venda
                produtos.Add(vendaProduto);

                // Cria objeto para exibição na grid
                NovaVendaCollection vendas = new NovaVendaCollection(
                    produtoSelecionado.Id,
                    produtoSelecionado.Descricao,
                    produtoSelecionado.UnidadeDeMedida,
                    produtoSelecionado.PrecoVenda,
                    quantidade
                );

                // Adiciona na tabela visual
                gridVendaProduto.Items.Add(vendas);

                // Atualiza o total da venda somando o novo item
                decimal totalAtual = decimal.Parse(blockTotal.Text);
                blockTotal.Text = (totalAtual + vendas.Total).ToString();

                // Limpa os campos
                boxCodProduto.Text = "";
                boxQuantidade.Text = "";
                blockNomeProduto.Text = "PRODUTO PESQUISADO";
                produtoSelecionado = null;
            }
        }

        //CONFIRMAR VENDA

        private async void btnConfirmarVenda(object sender, RoutedEventArgs e)
        {
            // Validação que impede finalizar venda sem produtos
            if (produtos.Count == 0)
            {
                MessageBox.Show("Adicione produtos antes de confirmar a venda!");
                return;
            }

            // Converte total da tela para decimal
            decimal totalVenda = decimal.Parse(blockTotal.Text);

            // Abre tela de pagamento passando o total da venda
            FormaPagamento pagamento = new FormaPagamento(totalVenda);
            pagamento.ShowDialog();

            // Cria lista final que será enviada ao banco
            List<VendaProduto> vendaProdutos = new List<VendaProduto>();

            // Copia os dados 
            foreach (var p in produtos)
            {
                vendaProdutos.Add(new VendaProduto
                {
                    ProdutoId = p.ProdutoId,
                    PrecoVenda = p.PrecoVenda,
                    Quantidade = p.Quantidade
                });
            }

            // Envia dados da venda para o banco
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
                MessageBox.Show("Erro ao cadastrar venda!");
            }
        }
    }


    class NovaVendaCollection
    {
        // Dados exibidos no grid
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

            // Calcula o total do item
            Total = quantidadeProduto * precoVenda;
        }
    }
}