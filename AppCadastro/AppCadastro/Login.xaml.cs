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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;

namespace AppCadastro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        public Login()
        {
            InitializeComponent();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["AppCadastro.Properties.Settings.ConnectionString"].ConnectionString.ToString();
        }

        #region Executa o Método Verificador no Clique de Login
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            if(VerifyUser(textBoxUser.Text, textBoxPassword.Password.ToUpper()))
            {
                con.Close();
                Sistema sistema = new Sistema();
                sistema.lblUser.Content = $"Usuário: {textBoxUser.Text}";
                sistema.Show();
                this.Close();
                
            }
            else
            {
                MessageBox.Show("Usuário ou Senha inválidos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Método para Verificar as Credenciais
        private bool VerifyUser(string username, string password)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "select Usuario_Cod from tblUsuario where Usuario_Nome='"+username+"' and Usuario_Senha='"+password+"'";
            dr = com.ExecuteReader();

            if (dr.Read())
            {
                if(Convert.ToBoolean(dr["Usuario_Cod"]) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region Encerra a Aplicação no Clique de Sair
        private void SairButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

    }
}
