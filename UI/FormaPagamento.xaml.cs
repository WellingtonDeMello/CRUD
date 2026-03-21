using System;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    public partial class FormaPagamento : Window
    {
        // Variável que guarda o total da venda
        decimal totalVenda;

        // Construtor da tela de pagamento
        
        public FormaPagamento(decimal total)
        {
            InitializeComponent();

            // Armazena o total da venda
            totalVenda = total;

            // Mostra o total na tela 
            txtTotal.Text = "Total: R$ " + totalVenda.ToString("F2");
        }

      
        // PAGAMENTO EM DINHEIRO
        
        private void PagamentoDinheiro(object sender, RoutedEventArgs e)
        {
            // Mostra a área de pagamento em dinheiro
            areaDinheiro.Visibility = Visibility.Visible;

            // Esconde a área de Pix
            areaPix.Visibility = Visibility.Collapsed;
        }

    
        // PAGAMENTO EM PIX
        private void PagamentoPix(object sender, RoutedEventArgs e)
        {
            // Mostra o QR Code do Pix
            areaPix.Visibility = Visibility.Visible;

            // Esconde a área de dinheiro
            areaDinheiro.Visibility = Visibility.Collapsed;
        }

       
        // CALCULAR TROCO
       
        private void CalcularTroco(object sender, TextChangedEventArgs e)
        {
            // Tenta converter o valor digitado para decimal
            if (decimal.TryParse(txtValorRecebido.Text, out decimal recebido))
            {
                // Calcula o troco
                decimal troco = recebido - totalVenda;

                // Se o valor recebido for suficiente
                if (troco >= 0)
                {
                    // Mostra o troco formatado
                    txtTroco.Text = "Troco: R$ " + troco.ToString("F2");
                }
                else
                {
                    // Caso o valor não seja suficiente
                    txtTroco.Text = "Valor insuficiente";
                }
            }
        }

   
        // FINALIZAR VENDA
      
        private void FinalizarVenda(object sender, RoutedEventArgs e)
        {
            // Mostra mensagem confirmando a venda
            MessageBox.Show("Venda finalizada com sucesso!");

            // Fecha a tela de pagamento
            this.Close();
        }

       
        // PAGAMENTO COM CARTÃO
       
        private void PagamentoCartao(object sender, RoutedEventArgs e)
        {
            // Cria a janela de pagamento com cartão
            Window janelaCartao = new Window();
            janelaCartao.Title = "Pagamento no Cartão";
            janelaCartao.Width = 420;
            janelaCartao.Height = 260;
            janelaCartao.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            janelaCartao.ResizeMode = ResizeMode.NoResize;
            janelaCartao.Background = System.Windows.Media.Brushes.White;

            // Layout principal
            StackPanel painel = new StackPanel();
            painel.Margin = new Thickness(30);

            // Texto de título
            TextBlock texto = new TextBlock();
            texto.Text = "Escolha o tipo de cartão";
            texto.FontSize = 20;
            texto.FontWeight = FontWeights.Bold;
            texto.Margin = new Thickness(0, 0, 0, 25);
            texto.HorizontalAlignment = HorizontalAlignment.Center;

            // BOTÃO DÉBITO
            Button btnDebito = new Button();
            btnDebito.Content = "Débito";
            btnDebito.Height = 45;
            btnDebito.Margin = new Thickness(0, 5, 0, 10);
            btnDebito.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#FF6900");
            btnDebito.Foreground = System.Windows.Media.Brushes.White;
            btnDebito.FontSize = 16;

            // BOTÃO CRÉDITO
            Button btnCredito = new Button();
            btnCredito.Content = "Crédito";
            btnCredito.Height = 45;
            btnCredito.Margin = new Thickness(0, 5, 0, 10);
            btnCredito.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#FF6900");
            btnCredito.Foreground = System.Windows.Media.Brushes.White;
            btnCredito.FontSize = 16;

            // BOTÃO CANCELAR
            Button btnCancelar = new Button();
            btnCancelar.Content = "Cancelar";
            btnCancelar.Height = 40;
            btnCancelar.Margin = new Thickness(0, 15, 0, 0);
            btnCancelar.Background = System.Windows.Media.Brushes.Gray;
            btnCancelar.Foreground = System.Windows.Media.Brushes.White;

            // Evento botão Débito
            btnDebito.Click += (s, ev) =>
            {
                MessageBox.Show("Pagamento no débito aprovado!");
                janelaCartao.Close();
                FinalizarVenda(null, null);
            };

            // Evento botão Crédito
            btnCredito.Click += (s, ev) =>
            {
                MessageBox.Show("Pagamento no crédito aprovado!");
                janelaCartao.Close();
                FinalizarVenda(null, null);
            };

            // Evento botão Cancelar
            btnCancelar.Click += (s, ev) =>
            {
                janelaCartao.Close();
            };

            // Adiciona os elementos ao painel
            painel.Children.Add(texto);
            painel.Children.Add(btnDebito);
            painel.Children.Add(btnCredito);
            painel.Children.Add(btnCancelar);

            // Define o conteúdo da janela
            janelaCartao.Content = painel;

            // Abre a janela
            janelaCartao.ShowDialog();
        }
    }
}