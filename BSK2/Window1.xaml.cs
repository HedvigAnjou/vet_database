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
using MySql.Data.MySqlClient;

namespace BSK2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        String connString;
        public int idUzytkownika;
        public Window1()
        {
            InitializeComponent();
        }
        private string doAQuery(string query, bool czySpodziewacSieWyniku)
        {
            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            string result = "";
            try
            {
                if (czySpodziewacSieWyniku == true)
                {
                    var zm = cmd.ExecuteScalar();
                    if (zm != null)
                        result = zm.ToString();
                    else
                        result = null;


                }
                else
                    cmd.ExecuteScalar();
            }
            catch (MySqlException er)
            {
                MessageBox.Show("Wystąpił błąd z bazą danych");
            }
            connection.Close();
            return result;
        }
        public Window1(int id, string connString)
        {
            this.connString = connString;
            InitializeComponent();
            idUzytkownika = id;
            //populateComboBox();
            if (doAQuery("SELECT admin FROM uzytkownicy WHERE id_uzytkownika='" + idUzytkownika + "';", true) == "True")
            { 
            dodawanieUzytkownika.Visibility = System.Windows.Visibility.Visible;
            przegladUprawnien.Visibility = System.Windows.Visibility.Visible;

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            Uprawnienia win2 = new Uprawnienia(idUzytkownika,connString);
            win2.Show();
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dane win2 = new Dane(idUzytkownika,connString);
            win2.Show();
            this.Close();
        }

        private void dodawanieUzytkownika_Click(object sender, RoutedEventArgs e)
        {
            PanelAdmina win = new PanelAdmina(idUzytkownika, connString);
            win.Show();
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        private void przegladUprawnien_Click(object sender, RoutedEventArgs e)
        {
            PrzegladUprawnien pu = new PrzegladUprawnien(idUzytkownika,connString);
            pu.Show();
            this.Close();
        }
    }
}
