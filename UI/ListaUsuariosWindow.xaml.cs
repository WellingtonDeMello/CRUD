using Dominio;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Repositorio;
using System.Linq;





namespace UI
{
    /// <summary>
    /// Lógica interna para ListaUsuariosWindow.xaml
    /// </summary>
    public partial class ListaUsuariosWindow : Window
    {
        public ListaUsuariosWindow()
        {
            InitializeComponent();
            CarregarUsuarios();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CarregarUsuarios()
        {
            using (var context = new Context())
            {
                var lista = context.Usuarios.ToList();
                Usuarios.ItemsSource = lista;
            }
        }

        private void btnEditar(object sender, RoutedEventArgs e)
        {
            if (Usuarios.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            Usuarios.BeginEdit();
        }

        private void btnSalvar(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                foreach (var item in Usuarios.Items)
                {
                    if (item is Usuario usuario)
                    {
                        context.Usuarios.Update(usuario);
                    }

                    if (Usuarios.SelectedItem == null)
                    {
                        MessageBox.Show("Selecione um usuário.");
                        return;
                    }

                }

                context.SaveChanges();
            }

            MessageBox.Show("Alterações salvas com sucesso!");
            CarregarUsuarios();
        }

        private void Voltar(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void Deletar(object sender, RoutedEventArgs e)
        {
            if (Usuarios.SelectedItem == null)
            {
                MessageBox.Show("Selecione um usuário para excluir");
                return;
            }

            var usuarioSelecionado = (Usuario)Usuarios.SelectedItem;

            var confirmacao = MessageBox.Show(
                "Tem certeza que deseja excluir este usuário?",
                "Confirmar Exclusão",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmacao != MessageBoxResult.Yes)
                return;

            using (var context = new Context())
            {
                context.Usuarios.Remove(usuarioSelecionado);
                context.SaveChanges();
            }

            MessageBox.Show("Usuário excluído com sucesso!");

            CarregarUsuarios();
        }
    }
}
    



