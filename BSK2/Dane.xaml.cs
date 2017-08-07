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
using System.Data;

namespace BSK2
{

    public partial class Dane : Window
    {
        private String tablicaUprawnien;
        private String connString;
        public int idUzytkownika;
        private MySqlConnection connection;
        private MySqlDataAdapter adapter;
        private DataTable dt;
        private Tables currentTable = Tables.zwierze;
        private string currentTableString = "Zwierze";
        private int currentNumberOfRows = 0;
       // private String connString = "server=localhost;uid=root;pwd=piesnicpon;database=zwierzyniec;";
        //private String connString = "server='192.168.0.8';uid=root;pwd=piesnicpon;database=zwierzyniec;";

        private bool adding = false;
        private DataGridRow addingToRow;
        private List<string> namesOfColumns;
        private bool usuwanie = true;
        private enum Tables
        {
            zwierze,
            klient,
            weterynarz,
            pracownik,
            lecznica,
            usluga,
            wizyta
        }
        public Dane()
        {

            InitializeComponent();
            populateComboBox();
            connectToDataBase();



            adapter = new MySqlDataAdapter("SELECT * FROM Zwierze", connection);


            dt = new DataTable();

            adapter.Fill(dt);
            dataGrid1.ItemsSource = dt.DefaultView;
            currentNumberOfRows = dataGrid1.Items.Count - 2;
            connection.Close();

        }
        public Dane(int id,string connStr)
        {
            connString = connStr;
            idUzytkownika = id;
            InitializeComponent();
            populateComboBox();
            connectToDataBase();
           // adapter = new MySqlDataAdapter("SELECT * FROM Zwierze", connection);

            dt = new DataTable();

            //adapter.Fill(dt);
            dataGrid1.ItemsSource = dt.DefaultView;
            currentNumberOfRows = dataGrid1.Items.Count - 2;
            connection.Close();

        }
        private void populateComboBox()
        {
            foreach (Tables table in Enum.GetValues(typeof(Tables)))
            {
                comboBox.Items.Add(table);
               // comboBox.SelectedIndex = 0; //domyslnie Zwierze
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1(idUzytkownika,connString);
            win2.Show();
            this.Close();
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }
        private void connectToDataBase()
        {
            try {
                connection = new MySqlConnection(connString);
                connection.Open();
            }
            catch (MySqlException er)
            {
                MessageBox.Show("Problem z polaczeniem z baza danych");
            }
        }
        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {

            pokazTabele();
        }
        private void pokazTabele() {
            switch (comboBox.Text)
            {
                case nameof(Tables.klient):
                    showTable(Tables.klient);
                    break;
                case nameof(Tables.zwierze):
                    showTable(Tables.zwierze);
                    break;
                case nameof(Tables.lecznica):
                    showTable(Tables.lecznica);
                    break;
                case nameof(Tables.pracownik):
                    showTable(Tables.pracownik);
                    break;
                case nameof(Tables.usluga):
                    showTable(Tables.usluga);
                    break;
                case nameof(Tables.weterynarz):
                    showTable(Tables.weterynarz);
                    break;
                case nameof(Tables.wizyta):
                    showTable(Tables.wizyta);
                    break;
            }
        }
        private void dodajKrotkeOnClick(object sender, EventArgs e)
        {
            if (weryfikacja_uprawnien(currentTable, 1) == true)
            {
                DataGrid dataGrid2 = new DataGrid();
                string command = "SELECT * FROM " + comboBox.Text;
                connectToDataBase();
                adapter = new MySqlDataAdapter(command, connection);


                DataTable dt2 = new DataTable();
                adapter.Fill(dt2);
                // dataGrid2.ItemsSource = dt2.DefaultView;
                // currentNumberOfRows = dataGrid1.Items.Count;
                connection.Close();


                //addingToRow = dataGrid2.ItemContainerGenerator.ContainerFromItem(dataGrid2.Items[dataGrid2.Items.Count - 1]) as DataGridRow;

                namesOfColumns = new List<string>();


                foreach (DataColumn cl in dt2.Columns)
                    namesOfColumns.Add(cl.ColumnName);

                InsertWindow win2 = new InsertWindow(namesOfColumns, this, idUzytkownika,connString);
                win2.Show();
                // this.Close();
            }
            else
                MessageBox.Show("Nie masz uprawnien do dodawania do tej tablicy");

        }
        private bool weryfikacja_uprawnien(Tables table,int ktoreUprawnienie)
        {
            
            connectToDataBase();
            string tablica = comboBox.Text;
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT " + tablica + " FROM uprawnienia WHERE fk_uzytkownika='" + idUzytkownika + "';";
            string am="";
            try
            {
                am = cmd.ExecuteScalar().ToString();
            }
            catch (MySqlException er)
            {
                MessageBox.Show("Problem z polaczeniem z baza danych");
            }
          
            tablicaUprawnien = am;
            connection.Close();
            try {
                if (am[ktoreUprawnienie] == '1')
                    return true;
                else
                    return false;
            }
            catch(System.IndexOutOfRangeException)
            {
                return false;     
           }

        }
        private void showTable(Tables table)
        {
            if (weryfikacja_uprawnien(table,0) == true)
            {

                string chosenTable = "";
                string command = "SELECT * FROM ";
                switch (table)
                {
                    case Tables.klient:
                        chosenTable = "klient";
                        break;
                    case Tables.zwierze:
                        chosenTable = "zwierze";
                        break;
                    case Tables.lecznica:
                        chosenTable = "lecznica";
                        break;
                    case Tables.pracownik:
                        chosenTable = "pracownik";
                        break;
                    case Tables.usluga:
                        chosenTable = "usluga";
                        break;
                    case Tables.weterynarz:
                        chosenTable = "weterynarz";
                        break;
                    case Tables.wizyta:
                        chosenTable = "wizyta";
                        break;

                }
                try
                {
                    command += chosenTable;
                    currentTable = table;
                    currentTableString = chosenTable;
                    connectToDataBase();
                    adapter = new MySqlDataAdapter(command, connection);
                }
                catch (MySqlException er)
                {
                    MessageBox.Show("Problem z polaczeniem z baza danych");
                }

                dt = new DataTable();
                adapter.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                currentNumberOfRows = dataGrid1.Items.Count;
                connection.Close();
                if (weryfikacja_uprawnien(currentTable, 2) == false)
                    dataGrid1.IsEnabled = false;
            }
            else
            {
                dt = new DataTable();
               // adapter.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                MessageBox.Show("Nie masz uprawnien do ogladania tej tablicy");
            }
        }

        //UPDATE
        private void Datagrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
            //int i=dataGrid1.Items.Count;
            var currentRowIndex = dataGrid1.Items.IndexOf(dataGrid1.SelectedItem);
            // MessageBox.Show("" + currentRowIndex.ToString() + "  " + currentNumberOfRows.ToString());
            if (currentRowIndex <= currentNumberOfRows)
            {
                TextBox t = e.EditingElement as TextBox;

                DataGridRow row1 = e.Row;
                int rowIndex = ((DataGrid)sender).ItemContainerGenerator.IndexFromContainer(row1);
                DataRowView dataRow = (DataRowView)dataGrid1.Items[rowIndex];
                string columnName = e.Column.Header.ToString();
                string keyname = dataGrid1.Columns[0].Header.ToString();
                string newValue = t.Text;
                string query;
                if (keyname == "pesel")
                    query = "UPDATE " + currentTableString + " SET " + columnName + "=\"" + newValue + "\" WHERE " + keyname + "=" + dataRow.Row.ItemArray[0].ToString() + ";";
                else
                    query = "UPDATE " + currentTableString + " SET " + columnName + "=\"" + newValue + "\" WHERE " + keyname + "=" + dataRow.Row.ItemArray[0].ToString() + ";";


                connection = new MySqlConnection(connString);

                connection.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = connection;
                cmd.CommandText = query;
                try
                {
                    int numRowsUpdated = cmd.ExecuteNonQuery();
                }
                catch (MySqlException er)
                {
                    MessageBox.Show("Nie mozesz zmienic tego pola");
                }

                connection.Close();
                showTable(currentTable);
            }
        }



        //INSERT
        public void getValues(List<string> values)
        {
            //insercik

            string query = "";

            string keyname = dataGrid1.Columns[0].Header.ToString();
            query = "INSERT INTO " + currentTableString + " VALUES (";
            //  query += "null, ";
            for (int i = 0; i < values.Count(); i++)
            {
                if (i == 0)
                {
                    if (keyname == "pesel")
                    {
                        query += "'";

                        query += values[i];
                        query += "'";
                        if (i != values.Count() - 1)
                            query += ", ";
                    }
                    else
                    {
                        query += "null, ";
                        continue;
                    }

                }
                else
                {
                    query += "'";

                    query += values[i];
                    query += "'";
                    if (i != values.Count() - 1)
                        query += ", ";
                }


            }
            query += ");";

            connection = new MySqlConnection(connString);

            connection.Open();
            MySqlCommand cmd = new MySqlCommand();

            cmd.Connection = connection;
            cmd.CommandText = query;

            try
            {
                int numRowsUpdated = cmd.ExecuteNonQuery();
            }
            catch (MySqlException er)
            {
                MessageBox.Show("Nie mozna dodac krotki z takimi parametrami. Byc moze problem z kluczem obcym?");
            }
            connection.Close();
            pokazTabele();
        }
        //DELETE
        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            if (weryfikacja_uprawnien(currentTable, 3) == true)
            {
                usuwanie = true;
                DataRowView dataRow = (DataRowView)dataGrid1.SelectedItem;
                //  int index = dataGrid1.CurrentCell.Column.DisplayIndex;
                //string columnName = dataGrid1.CurrentCell.Column.Header.ToString();
                string keyname = dataGrid1.Columns[0].Header.ToString();
                //   string newValue = t.Text;

                string query;

                if (keyname == "pesel")
                    query = "DELETE FROM " + currentTableString + " WHERE " + keyname + "=" + dataRow.Row.ItemArray[0].ToString() + ";";
                else
                    query = "DELETE FROM " + currentTableString + " WHERE " + keyname + "=" + (int)dataRow.Row.ItemArray[0] + ";";

             
                connection = new MySqlConnection(connString);

                connection.Open();
                MySqlCommand cmd = new MySqlCommand();
                try
                {
                    cmd.Connection = connection;
                    cmd.CommandText = query;
                    int numRowsUpdated = cmd.ExecuteNonQuery();

                    connection.Close();

                }
                catch (MySqlException er)
                {
                    MessageBox.Show(er.Data.ToString());
                }
                showTable(currentTable);
            }
            else
                MessageBox.Show("Nie masz uprawnien do usuwania z tej tablicy");
        }
    }
}
