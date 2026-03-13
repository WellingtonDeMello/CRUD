using System;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    public partial class FormaPagamento : Window
    {
        decimal totalVenda;

        public FormaPagamento(decimal total)
        {
            InitializeComponent();

            totalVenda = total;
            txtTotal.Text = "Total: R$ " + totalVenda.ToString("F2");
        }

        private void PagamentoDinheiro(object sender, RoutedEventArgs e)
        {
            areaDinheiro.Visibility = Visibility.Visible;
            areaPix.Visibility = Visibility.Collapsed;
        }

        private void PagamentoPix(object sender, RoutedEventArgs e)
        {
            areaPix.Visibility = Visibility.Visible;
            areaDinheiro.Visibility = Visibility.Collapsed;
        }

        private void PagamentoDebito(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pagamento no débito aprovado!");
            FinalizarVenda(null, null);
        }

        private void PagamentoCredito(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pagamento no crédito aprovado!");
            FinalizarVenda(null, null);
        }

        private void CalcularTroco(object sender, TextChangedEventArgs e)
        {
            if (decimal.TryParse(txtValorRecebido.Text, out decimal recebido))
            {
                decimal troco = recebido - totalVenda;

                if (troco >= 0)
                    txtTroco.Text = "Troco: R$ " + troco.ToString("F2");
                else
                    txtTroco.Text = "Valor insuficiente";
            }
        }

        private void FinalizarVenda(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Venda finalizada!");
            this.Close();
        }

        private void PagamentoCartao(object sender, RoutedEventArgs e)
        {

        }
    }
}