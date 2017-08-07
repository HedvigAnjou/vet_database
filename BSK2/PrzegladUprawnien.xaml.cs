using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

namespace BSK2
{
    /// <summary>
    /// Interaction logic for PrzegladUprawnien.xaml
    /// </summary>
    public partial class PrzegladUprawnien : Window
    {
        private String tablicaUprawnien;
        private String connString;
        public int idUzytkownika;
        private MySqlConnection connection;
        private MySqlDataAdapter adapter;
        private DataTable dt;


        public PrzegladUprawnien()
        {
            InitializeComponent();
        }

        public PrzegladUprawnien(int id, string connStr)
        {
            connString = connStr;
            idUzytkownika = id;
            InitializeComponent();
    
            connectToDataBase();

            FillGrid();

            connection.Close();
     
        }

        private void connectToDataBase()
        {
            try
            {
                connection = new MySqlConnection(connString);
                connection.Open();
            }
            catch (MySqlException er)
            {
                MessageBox.Show("Problem z polaczeniem z baza danych");
            }
        }

        public void FillGrid()
        {


            string sql = "SELECT  uzytkownicy.login, uzytkownicy.przejmij,  uprawnienia.zwierze , uprawnienia.klient , uprawnienia.usluga, uprawnienia.wizyta, uprawnienia.lecznica, uprawnienia.pracownik, uprawnienia.weterynarz FROM uzytkownicy INNER JOIN uprawnienia ON uzytkownicy.id_uzytkownika = uprawnienia.fk_uzytkownika;";
            MySqlCommand cmdSel = new MySqlCommand(sql, connection);
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
          
            
            da.Fill(dt);
           

            for  (int i =2; i<dt.Columns.Count; i++)
            {
                for (int j =0; j<dt.Rows.Count; j++)
                {
                    string cell = dt.Rows[j][i].ToString();
                    string result = "";

                    if (cell[5] == '1')
                        result += 'C';
                    else if (cell[1] == '1')
                        result += 'c';
                    else result += '-';

                    if (cell[4] == '1')
                        result += 'R';
                    else if (cell[0] == '1')
                        result += 'r';
                    else result += '-';

                    if (cell[6] == '1')
                        result += 'U';
                    else if (cell[2] == '1')
                        result += 'u';
                    else result += '-';

                    if (cell[7] == '1')
                        result += 'D';
                    else if (cell[3] == '1')
                        result += 'd';
                    else result += '-';


                    dt.Rows[j][i] = result;
                    
                }
            }


            DataColumn p = new DataColumn("p");
            dt.Columns.Add(p);
            for (int k =0; k<dt.Rows.Count;k++)
            {
                bool cell = (bool)dt.Rows[k][1];
                if (cell == true)
                    dt.Rows[k]["p"] = "przejmij";
                else
                    dt.Rows[k]["p"] = "";


            }
            dt.Columns.RemoveAt(1);
            dt.Columns["p"].ColumnName = "przejmij";
            


            dataGrid.ItemsSource = dt.DefaultView;
           
        }




        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
            string user = row.Row.ItemArray[0].ToString();

            //var index = dataGrid.SelectedCells[0].Column.Header;
            int numer = dataGrid.CurrentColumn.DisplayIndex;
            string zmienna = dataGrid.CurrentColumn.Header.ToString();

            if (numer > 0 && numer <8)
            {
                Uprawnienia win2 = new Uprawnienia(idUzytkownika, connString, zmienna, user);
                win2.Show();
                this.Close();
            }
        }

        private void Powrot_Click(object sender, RoutedEventArgs e)
        {
            Window1 win = new Window1(idUzytkownika, connString);
            win.Show();
            this.Close();
        }
    }
}


