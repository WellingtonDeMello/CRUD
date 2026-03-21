using Dominio;
using Dominio.Enum;
using System;
using System.Linq;
using System.Windows;
using UI.Model;

namespace UI
{
    public partial class NovoProduto : Window
    {
        // Model responsável por acessar o banco 
        ProdutoModel _pModel = new ProdutoModel();

        // Guarda o produto quando for edição
        private Produto _produto;

        // construtor usado para cadastrar novo produto
        public NovoProduto()
        {
            InitializeComponent(); 

            // Preenche os ComboBox com valores dos enums
            // Ex Alimento, Bebida, Limpeza
            boxGrupo.ItemsSource = Enum.GetValues(typeof(ProdutoGrupo)).Cast<ProdutoGrupo>();

            // Ex Kg, Litro, Unidade
            boxUnMedida.ItemsSource = Enum.GetValues(typeof(UnidadeMedida)).Cast<UnidadeMedida>();
        }

        // construtor usado para editar
        public NovoProduto(Produto produto) : this()
        {
            _produto = produto;

            if (_produto != null)
            {
                // Preenche os campos com os dados do produto selecionado

                boxDescricao.Text = _produto.Descricao;

                // Converte enum para texto para mostrar no ComboBox
                boxUnMedida.Text = _produto.UnidadeDeMedida.ToString();

                boxCodBarras.Text = _produto.CodBarras;

                // Converte número para texto
                boxPrecoCusto.Text = _produto.PrecoCusto.ToString();
                boxPrecoVenda.Text = _produto.PrecoVenda.ToString();

                // Checkbox 
                boxAtivo.IsChecked = _produto.Ativo;

                // Enum para texto
                boxGrupo.Text = _produto.ProdutoGrupo.ToString();
            }
        }

        // BOTÃO CONFIRMAR 
        private void btnConfirmarProduto(object sender, RoutedEventArgs e)
        {
            // verifica se os preços são números válidos
            if (!decimal.TryParse(boxPrecoCusto.Text, out decimal precoCusto) ||
                !decimal.TryParse(boxPrecoVenda.Text, out decimal precoVenda))
            {
                MessageBox.Show("Digite preços válidos.");
                return;
            }

            try
            {
                // Se não existe produto = cadastro
                if (_produto == null)
                {
                    _pModel.AdicionarProduto(
                        boxDescricao.Text.Trim(), // remove espaços

                        // Converte texto do ComboBox para enum
                        Enum.Parse<UnidadeMedida>(boxUnMedida.Text),

                        boxCodBarras.Text.Trim(),

                        precoCusto,
                        precoVenda,
                        boxAtivo.IsChecked ?? false,

                        // Converte texto para enum
                        Enum.Parse<ProdutoGrupo>(boxGrupo.Text)
                    );

                    MessageBox.Show("Produto adicionado com sucesso!");
                }
                else
                {
                    // Se existe produto=edição
                    _pModel.EditarProduto(
                        _produto.Id,

                        boxDescricao.Text.Trim(),
                        Enum.Parse<UnidadeMedida>(boxUnMedida.Text),
                        boxCodBarras.Text.Trim(),

                        precoCusto,
                        precoVenda,

                        boxAtivo.IsChecked ?? false,
                        Enum.Parse<ProdutoGrupo>(boxGrupo.Text)
                    );

                    MessageBox.Show("Produto atualizado com sucesso!");
                }

                Close(); // fecha a tela após salvar
            }
            catch (Exception ex)
            {
                // Caso de erro, mostra mensagem
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
    }
}