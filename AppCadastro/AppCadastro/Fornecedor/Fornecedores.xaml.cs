using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace AppCadastro.Fornecedor
{
    /// <summary>
    /// Interaction logic for Fornecedores.xaml
    /// </summary>
    public partial class Fornecedores : Window
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader? dr;
        public Fornecedores()
        {
            InitializeComponent();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["AppCadastro.Properties.Settings.ConnectionString"].ConnectionString.ToString();
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Cadastrar Fornecedor no Banco de Dados
        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (txtNomeFornecedor.Text.Length == 0 || txtEndFornecedor.Text.Length == 0 || txtCelFornecedor.Text.Length == 0)
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            con.Open();
            com.Connection = con;

            com.CommandText = "select COUNT(*) from tblFornecedor where Fornecedor_Nome = '" + txtNomeFornecedor.Text + "'";
            dr = com.ExecuteReader();

            if (dr.Read())
            {
                int nomeExiste = dr.GetInt32(0);
                if (nomeExiste > 0)
                {
                    MessageBox.Show("Já existe um fornecedor com esse nome");
                    return;

                }
            }
            dr.Close();

            com.CommandText = "insert into tblFornecedor (Fornecedor_Nome, Fornecedor_End, Fornecedor_Cel) values ('" + txtNomeFornecedor.Text + "', '" + txtEndFornecedor.Text + "', '" + txtCelFornecedor.Text + "')";
            dr = com.ExecuteReader();
            MessageBox.Show("Fornecedor cadastrado com sucesso!", "Cadastro", MessageBoxButton.OK, MessageBoxImage.Information);
            con.Close();

            txtCodFornecedor.Clear();
            txtNomeFornecedor.Clear();
            txtEndFornecedor.Clear();
            txtCelFornecedor.Clear();
        }
        #endregion

        #region Excluir Fornecedor no Banco de Dados
        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (txtCodFornecedor.Text.Length == 0)
            {
                MessageBox.Show("Selecione um Fornecedor para excluir.");
                return;
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            if (MessageBox.Show("Ao excluir esse fornecedor, todos os produtos associados a ele serão excluidos, deseja continuar?", "Aviso", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
            {
                return;
            }

            con.Open();
            com.Connection = con;

            com.CommandText = "delete from tblFornecedor where Fornecedor_Cod = '" + txtCodFornecedor.Text + "'";
            dr = com.ExecuteReader();
            MessageBox.Show("Fornecedor excluido com sucesso!", "Cadastro", MessageBoxButton.OK, MessageBoxImage.Information);
            con.Close();

            txtCodFornecedor.Clear();
            txtNomeFornecedor.Clear();
            txtEndFornecedor.Clear();
            txtCelFornecedor.Clear();
        }
        #endregion

        #region Editar Fornecedor no Banco de Dados
        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (txtCodFornecedor.Text.Length == 0)
            {
                MessageBox.Show("Selecione um Fornecedor para editar.");
                return;
            }

            if (txtNomeFornecedor.Text.Length == 0 || txtEndFornecedor.Text.Length == 0 || txtCelFornecedor.Text.Length == 0)
            {
                MessageBox.Show("Preencha todos os campos");
                return;
            }

            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            con.Open();
            com.Connection = con;

            com.CommandText = "select COUNT(*) from tblFornecedor where Fornecedor_Nome = '" + txtNomeFornecedor.Text + "' and not Fornecedor_Cod = '" + txtCodFornecedor.Text + "'";
            dr = com.ExecuteReader();

            if (dr.Read())
            {
                int nomeExiste = dr.GetInt32(0);
                if (nomeExiste > 0)
                {
                    MessageBox.Show("Já existe um fornecedor com esse nome");
                    return;

                }
            }
            dr.Close();

            com.CommandText = "update tblFornecedor set Fornecedor_Nome = '" + txtNomeFornecedor.Text + "', Fornecedor_End = '" + txtEndFornecedor.Text + "', Fornecedor_Cel = '" + txtCelFornecedor.Text + "' where Fornecedor_Cod = '" + txtCodFornecedor.Text + "' ";
            dr = com.ExecuteReader();
            MessageBox.Show("Fornecedor editado com sucesso!", "Cadastro", MessageBoxButton.OK, MessageBoxImage.Information);
            con.Close();
        }
        #endregion

        #region Abrir aba Consulta Fornecedor
        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {
            txtCelFornecedor.Focus();
            ConsultaFornecedor consultaFornecedor = new ConsultaFornecedor();
            consultaFornecedor.ShowDialog();
        }
        #endregion

        #region Preencher Campos de Acordo com Codigo
        private void txtCodFornecedor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            con.Open();
            com.Connection = con;

            com.CommandText = "Select Fornecedor_Nome, Fornecedor_End, Fornecedor_Cel from tblFornecedor where Fornecedor_Cod = '" + txtCodFornecedor.Text + "'";
            dr = com.ExecuteReader();

            if (dr.Read())
            {
                string nome = dr.GetString(0);
                string endereco = dr.GetString(1);
                string celular = dr.GetString(2);

                txtNomeFornecedor.Text = nome;
                txtEndFornecedor.Text = endereco;
                txtCelFornecedor.Text = celular;
            }
            dr.Close();

            com.CommandText = "select Produto_Cod, Produto_Nome, Produto_Marca, Fornecedor_Nome from tblProduto inner join tblFornecedor on Fornecedor_Cod = Cod_Fornecedor where Fornecedor_Cod = '" + txtCodFornecedor.Text + "'";
            DataTable dt = new DataTable("tblProduto");
            SqlDataAdapter dataAdp = new SqlDataAdapter(com);
            dataAdp.Fill(dt);
            DataGridFornProd.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);


            con.Close();
            
        }
        #endregion

        #region Limpar os Campos de Texto
        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            txtCodFornecedor.Clear();
            txtNomeFornecedor.Clear();
            txtEndFornecedor.Clear();
            txtCelFornecedor.Clear();
        }
        #endregion

        #region Verificar Entrada de Texto Celular Fornecedor
        private static readonly Regex _regexcel = new Regex(@"^[0-9]\d*?$");

        private static bool IsTextAllowedCel(string text)
        {  
            return _regexcel.IsMatch(text);
        }

        private bool IsAllowedCel(TextBox tb, string text)
        {
            bool isAllowed = true;
            if (tb != null)
            {
                string currentText = tb.Text;
                if (!string.IsNullOrEmpty(tb.SelectedText))
                    currentText = currentText.Remove(tb.CaretIndex, tb.SelectedText.Length);
                isAllowed = IsTextAllowedCel(currentText.Insert(tb.CaretIndex, text));
            }
            return isAllowed;
        }

        private void Txt_PreviewTextInputCel(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsAllowedCel(sender as TextBox, e.Text);
        }

        private void TextBoxPastingCel(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsAllowedCel(sender as TextBox, text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }
        private void txtCelFornecedor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }
        #endregion

    }
}
