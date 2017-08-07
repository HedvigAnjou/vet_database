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
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace BSK2
{
    /// <summary>
    /// Interaction logic for PanelAdmina.xaml
    /// </summary>
    public partial class PanelAdmina : Window
    {
        private int idUzytkownika;
        private String connString;
        List<string> listaUprawnien = new List<string>();
        List<string> tablice = new List<string>();

        public PanelAdmina(int idUsera, string connStr)
        {
            idUzytkownika = idUsera;
            connString = connStr;
            InitializeComponent();

            populateComboBoxUsers();


            listaUprawnien.Add("fk_read");
            listaUprawnien.Add("fk_create");
            listaUprawnien.Add("fk_update");
            listaUprawnien.Add("fk_delete");
            listaUprawnien.Add("fk_read_przekaz");
            listaUprawnien.Add("fk_create_przekaz");
            listaUprawnien.Add("fk_update_przekaz");
            listaUprawnien.Add("fk_delete_przekaz");


            tablice.Add("zwierze");
            tablice.Add("klient");
            tablice.Add("usluga");
            tablice.Add("wizyta");
            tablice.Add("lecznica");
            tablice.Add("pracownik");
            tablice.Add("weterynarz");

        }
        private void usunRazemZDziecmi(string tablica, string id)// zabiera uprawnienia wszystkim dzieciom tego id , te ktore on im przekazal
        {

            //  doAQuery("UPDATE uprawnienia SET " + tablica + "='" + "00000000" + "' WHERE fk_uzytkownika='" + id + "';", false); //sobie usuwam
            //sobie usuwam
          
            //foreach (string a in listaUprawnien)
            //{
            //    doAQuery("UPDATE drzewo SET " + a + "=null WHERE fk_uzytkownika='" + id + "' AND tablica='" + tablica + "';", false);


            //}

            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();
            string temp = "";
            List<string> listaDoUsuniecia = new List<string>();
            foreach (string a in listaUprawnien)
            {
                // string b = doAQuery("SELECT fk_uzytkownika FROM drzewo WHERE " + a + "='" + id + "' AND tablica='" + tablica + "';", true);
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT fk_uzytkownika FROM drzewo WHERE " + a + "='" + id + "' AND tablica='" + tablica + "';";

                // List<string> loginy = new List<string>();

                MySqlDataReader reader = cmd.ExecuteReader();
              //  doAQuery("DELETE uprawnienia WHERE fk_uzytkownika='" + id + "';", false);
                while (reader.Read())
                {
                    temp = reader["fk_uzytkownika"].ToString();
                    listaDoUsuniecia.Add(temp);
                    zabierzUprawnienie(temp, a,tablica);
                }
                reader.Close();
                foreach (string m in listaDoUsuniecia)
                { usunDzieciomUprawnienie(tablica, m,a); }
                listaDoUsuniecia.Clear();
            }

        }
        private void usunDzieciomUprawnienie(string tablica, string id, string uprawnienie)
        {
            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();
            List<string> listaDoUsuniecia = new List<string>();

                // string b = doAQuery("SELECT fk_uzytkownika FROM drzewo WHERE " + a + "='" + id + "' AND tablica='" + tablica + "';", true);
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT fk_uzytkownika FROM drzewo WHERE " + uprawnienie + "='" + id + "' AND tablica='" + tablica + "';";

                // List<string> loginy = new List<string>();

                MySqlDataReader reader = cmd.ExecuteReader();
                //  doAQuery("DELETE uprawnienia WHERE fk_uzytkownika='" + id + "';", false);
                while (reader.Read())
                {
                    listaDoUsuniecia.Add(reader["fk_uzytkownika"].ToString());
                    zabierzUprawnienie(id, uprawnienie, tablica);
                }
                reader.Close();
                foreach (string m in listaDoUsuniecia)
                { usunDzieciomUprawnienie(tablica, m,uprawnienie); }
           

        }
        private void zabierzUprawnienie(string id, string uprawnienieDoZabrania,string tablica)// zabiera uprawnienie i info on kogo je dostal w tab drzewo
        {
            //listaUprawnien.Add("fk_read");
            //listaUprawnien.Add("fk_create");
            //listaUprawnien.Add("fk_update");
            //listaUprawnien.Add("fk_delete");
            //listaUprawnien.Add("fk_read_przekaz");
            //listaUprawnien.Add("fk_create_przekaz");
            //listaUprawnien.Add("fk_update_przekaz");
            //listaUprawnien.Add("fk_delete_przekaz");

            string stare = doAQuery("SELECT " + tablica + " FROM uprawnienia WHERE fk_uzytkownika='" + id + "';", true);
    
            int index = listaUprawnien.IndexOf(uprawnienieDoZabrania);
          //  stare[index] = 0;
            StringBuilder stareUpr = new StringBuilder(stare);
            stareUpr[index] = '0';
            doAQuery("UPDATE uprawnienia SET " + tablica + "='" + stareUpr + "' WHERE fk_uzytkownika='" + id + "';", false);
            doAQuery("UPDATE drzewo SET " + uprawnienieDoZabrania + "=null WHERE tablica='" + tablica + "' AND fk_uzytkownika='" + id + "';",false);



        } 

        private void populateComboBoxUsers()
        {
            comboBoxUsers.Items.Clear();
            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT login FROM uzytkownicy;";

            List<string> loginy = new List<string>();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                loginy.Add(reader["login"].ToString());
            }
            foreach (string n in loginy)
            {
                // Console.WriteLine(n);
                comboBoxUsers.Items.Add(n);
            }
            reader.Close();
            connection.Close();

        }
        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        private void wyzeruj_textboxy()
        {
            imie.Text = "";
            mail.Text = "";
            login.Text = "";
            nazwisko.Text = "";
            przejmij.IsChecked = false;
            //przekaz.IsChecked = false;
            haslo.Password = "";
        }
        private void Zatwierdz_Click(object sender, RoutedEventArgs e) //dodawanie uzytkownika
        {
            string imieStr, hasloStr, mailStr, loginStr, nazwiskoStr, przejmijStr, przekazStr;
            imieStr = imie.Text;
            mailStr = mail.Text;
            loginStr = login.Text;

            string czyIstniejeTakiLoginWBazie=doAQuery("SELECT IF(   EXISTS(SELECT * FROM uzytkownicy WHERE login='" + loginStr + "') , 1, 0);",true);
            if (czyIstniejeTakiLoginWBazie == "1")
            {
                MessageBox.Show("Nie mozna dodać użytkownika o takim loginie, bo jest już taki w bazie");

            }
            else {
                nazwiskoStr = nazwisko.Text;
                if (przejmij.IsChecked ?? true)
                    przejmijStr = "1";
                else
                    przejmijStr = "0";

               
                    przekazStr = "1";


                DateTime today = DateTime.Now;
                string data = today.ToString("yyyy-MM-dd");

                hasloStr = GenerateSHA512String(haslo.Password);

                string insertQuery = "INSERT INTO uzytkownicy (`login`, `pass_hash`,`adres_e-mail`,`imie` ,`nazwisko` ,`data_utworzenia` , `przejmij` ) ";
                insertQuery += "VALUES ('" + loginStr + "','" + hasloStr + "','" + mailStr + "','" + imieStr + "','" + nazwiskoStr + "','" + data + "'," + przejmijStr + ");";
                doAQuery(insertQuery, false);
                MessageBox.Show("Nowy użytkownik został poprawnie dodany do bazy");
                string idNowego = doAQuery("SELECT id_uzytkownika FROM uzytkownicy WHERE login='" + loginStr + "';", true);
                string insertUprawnieniaQuery = "INSERT INTO uprawnienia (`fk_uzytkownika`, `zwierze` ,`klient` ,`usluga` ,`wizyta` ,`lecznica` ,`pracownik` ,`weterynarz` )";
                insertUprawnieniaQuery += "VALUES('" + idNowego + "', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000', '00000000'); ";
                doAQuery(insertUprawnieniaQuery, false);

                //dodawanie drzew

                List<string> tablice = new List<string>();
                tablice.Add("zwierze");
                tablice.Add("klient");
                tablice.Add("usluga");
                tablice.Add("wizyta");
                tablice.Add("lecznica");
                tablice.Add("pracownik");
                tablice.Add("weterynarz");

                string qry = "";
                foreach (string a in tablice)
                {
                    qry = "INSERT INTO `drzewo` (`fk_uzytkownika`,`tablica`,`fk_create`,`fk_create_przekaz` ,`fk_read` ,`fk_read_przekaz`,`fk_update` ,`fk_update_przekaz` ,`fk_delete` ,`fk_delete_przekaz` )";
                    qry += "VALUES(" + idNowego + ",'" + a + "',null,null,null,null,null,null,null,null);";
                    doAQuery(qry, false);
                }

                populateComboBoxUsers();
            }
        }

        private void doMenu_Click(object sender, RoutedEventArgs e)
        {
            Window1 win2 = new Window1(idUzytkownika, connString);
            win2.Show();
            this.Close();

        }
        private string doAQuery(string query,bool czySpodziewacSieWyniku)
        {
            MySqlConnection connection = new MySqlConnection(connString);
            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            string result = "";
            try
            {if (czySpodziewacSieWyniku == true)
                    result = cmd.ExecuteScalar().ToString();
                else
                    cmd.ExecuteScalar();
            }
            catch (MySqlException er)
            {
                MessageBox.Show("Wystąpił błąd z bazą danych");
            }
            return result;
        }
        private void usun_Click(object sender, RoutedEventArgs e)
        {
            string loginDoUsuniecia = comboBoxUsers.SelectedValue.ToString();
            string id = doAQuery("SELECT id_uzytkownika FROM uzytkownicy WHERE login='"+loginDoUsuniecia+"';",true);
            doAQuery("DELETE FROM uprawnienia WHERE fk_uzytkownika='" + id + "';",false);
          
            foreach (string a in tablice)
            {
                usunRazemZDziecmi(a, id);
            }
            doAQuery("DELETE FROM drzewo WHERE fk_uzytkownika='" + id + "';", false);

            doAQuery("DELETE FROM uzytkownicy WHERE login='" + loginDoUsuniecia + "';",false);

            MessageBox.Show("Uzytkownik zostal usuniety");
            populateComboBoxUsers();
        }

        private void nadajPrawoPrzejmij(object sender, RoutedEventArgs e)
        {
            string loginDoUzycia = comboBoxUsers.SelectedValue.ToString();
           if(prawoPrzejmij.IsChecked==true)
            doAQuery("UPDATE uzytkownicy SET przejmij='1' WHERE login='" + loginDoUzycia + "';", false);
           else
                doAQuery("UPDATE uzytkownicy SET przejmij='0' WHERE login='" + loginDoUzycia + "';", false);
            MessageBox.Show("Nadano prawo");
        }
        private void nowyUzytkownikWybrany(object sender, EventArgs e)
        {
            if (comboBoxUsers.SelectedValue != null)
            {
                string prawo = doAQuery("SELECT przejmij FROM uzytkownicy WHERE login='" + comboBoxUsers.SelectedValue.ToString() + "';", true);

                if (prawo == "True")
                    prawoPrzejmij.IsChecked = true;
                else
                    prawoPrzejmij.IsChecked = false;
            }
        }
        
    }
}
