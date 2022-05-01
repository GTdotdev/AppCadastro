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

namespace AppCadastro.Fornecedor
{
    /// <summary>
    /// Interaction logic for ConsultaFornecedor.xaml
    /// </summary>
    public partial class ConsultaFornecedor : Window
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        public ConsultaFornecedor()
        {
            InitializeComponent();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["AppCadastro.Properties.Settings.ConnectionString"].ConnectionString.ToString();
        }

        #region Preenche o DataGrid de Acordo com a Tabela Fornecedor
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            con.Open();
            com.Connection = con;

            com.CommandText = "select * from tblFornecedor";
            DataTable dt = new DataTable("tblFornecedor");
            SqlDataAdapter dataAdp = new SqlDataAdapter(com);
            dataAdp.Fill(dt);
            DataGrid.ItemsSource = dt.DefaultView;
            dataAdp.Update(dt);
            con.Close();
        }

        #endregion

        #region Método para Enviar o Fornecedor ao Pressionar a Tecla Enter
        public class FornCod
        {
            public string? id { get; set; }
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               
                foreach (DataRowView row in DataGrid.SelectedItems)
                {
                    FornCod codfornecedor = new FornCod();
                    codfornecedor.id = row.Row.ItemArray[0].ToString();
                    foreach (Window item in Application.Current.Windows)
                    {
                        if (item.Name == "FornWindow")
                        {
                            ((Fornecedores)item).txtCodFornecedor.Text = codfornecedor.id;

                        }
                    }
                    
                }

                this.Close();


            }
            
        }

        #endregion
    }
}
