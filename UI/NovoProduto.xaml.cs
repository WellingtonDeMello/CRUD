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
        ProdutoModel _pModel = new ProdutoModel();
        private Produto _produto;

        public NovoProduto()
        {
            InitializeComponent();

            // Carrega enums nos comboboxes
            boxGrupo.ItemsSource = Enum.GetValues(typeof(ProdutoGrupo)).Cast<ProdutoGrupo>();
            boxUnMedida.ItemsSource = Enum.GetValues(typeof(UnidadeMedida)).Cast<UnidadeMedida>();
        }

        public NovoProduto(Produto produto) : this()
        {
            _produto = produto;

            if (_produto != null)
            {
                // Preenche os campos com os valores do produto para edição
                boxDescricao.Text = _produto.Descricao;
                boxUnMedida.Text = _produto.UnidadeDeMedida.ToString();
                boxCodBarras.Text = _produto.CodBarras;
                boxPrecoCusto.Text = _produto.PrecoCusto.ToString();
                boxPrecoVenda.Text = _produto.PrecoVenda.ToString();
                boxAtivo.IsChecked = _produto.Ativo;
                boxGrupo.Text = _produto.ProdutoGrupo.ToString();
            }
        }

        private void btnConfirmarProduto(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_produto == null)
                {
                    // Cadastro de novo produto
                    _pModel.AdicionarProduto(
                        boxDescricao.Text.Trim(),
                        Enum.Parse<UnidadeMedida>(boxUnMedida.Text),
                        boxCodBarras.Text.Trim(),
                        decimal.Parse(boxPrecoCusto.Text),
                        decimal.Parse(boxPrecoVenda.Text),
                        boxAtivo.IsChecked ?? false,
                        Enum.Parse<ProdutoGrupo>(boxGrupo.Text)
                    );

                    MessageBox.Show("Produto adicionado com sucesso!");
                }
                else
                {
                    // Edição do produto existente
                    _pModel.EditarProduto(
                        _produto.Id,
                        boxDescricao.Text.Trim(),
                        Enum.Parse<UnidadeMedida>(boxUnMedida.Text),
                        boxCodBarras.Text.Trim(),
                        decimal.Parse(boxPrecoCusto.Text),
                        decimal.Parse(boxPrecoVenda.Text),
                        boxAtivo.IsChecked ?? false,
                        Enum.Parse<ProdutoGrupo>(boxGrupo.Text)
                    );

                    MessageBox.Show("Produto atualizado com sucesso!");
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Atenção", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}