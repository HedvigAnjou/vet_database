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
    /// Interaction logic for Uprawnienia.xaml
    /// </summary>
    public partial class Uprawnienia : Window
    {
        String connString;
        MySqlConnection connection;
        MySqlDataAdapter adapter;
        private string wybranaTablica;
        private string wybranyUser;
        List<string> listaUprawnien = new List<string>();
        List<string> tablice = new List<string>();

        List<string> listaID = new List<string>(); //lista id wszystkich uzytkownikow
      //  int ID_ROOTA=12;

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
        public int idUzytkownika;
        public Uprawnienia()
        {
            InitializeComponent();
        }
        public Uprawnienia(int id,string connStr)
        {
            connString = connStr;
            InitializeComponent();
            idUzytkownika=id;

          connection = new MySqlConnection(connString);
            connection.Open();
            populateComboBox();
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

            connection.Open();

            string query = "SELECT id_uzytkownika FROM uzytkownicy;";
                //using (MySqlCommand command = new MySqlCommand(query, connection))
                //{
                //    using (MySqlDataReader reader = command.ExecuteReader())
                //    {
                    //MySqlCommand command = new MySqlCommand(query, connection);
                    //MySqlDataReader reader = command.ExecuteReader();

                    //while (reader.Read())
                    //    {
                    //        listaID.Add(reader.GetString(0));
                    //    }
            // }
            //}
            //MySqlConnection connection = new MySqlConnection(connString);
            //connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id_uzytkownika FROM uzytkownicy;";

           // List<string> loginy = new List<string>();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                listaID.Add(reader["id_uzytkownika"].ToString());
            }
            reader.Close();
           // connection.Close();
        }

        public Uprawnienia(int id, string connStr, string przekazanaTablica, string przekazanyUzytkownik) //konstruktor wywolywany przez przeglad uprawnien
        {
            connString = connStr;
            InitializeComponent();
            idUzytkownika = id;

            //if (idUzytkownika == ID_ROOTA)
            //    pokazPrzeglad.Visibility = System.Windows.Visibility.Visible;
            if(doAQuery("SELECT admin FROM uzytkownicy WHERE id_uzytkownika='"+idUzytkownika+"';",true)=="True")
                pokazPrzeglad.Visibility = System.Windows.Visibility.Visible;


            connection = new MySqlConnection(connString);
            connection.Open();
            populateComboBox();
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

            connection.Open();

            string query = "SELECT id_uzytkownika FROM uzytkownicy;";
            //using (MySqlCommand command = new MySqlCommand(query, connection))
            //{
            //    using (MySqlDataReader reader = command.ExecuteReader())
            //    {
            //MySqlCommand command = new MySqlCommand(query, connection);
            //MySqlDataReader reader = command.ExecuteReader();

            //while (reader.Read())
            //    {
            //        listaID.Add(reader.GetString(0));
            //    }
            // }
            //}
            //MySqlConnection connection = new MySqlConnection(connString);
            //connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id_uzytkownika FROM uzytkownicy;";

            // List<string> loginy = new List<string>();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                listaID.Add(reader["id_uzytkownika"].ToString());
            }
            reader.Close();

            comboBoxUzytkownik.SelectedItem = przekazanyUzytkownik;

            switch (przekazanaTablica)
            {
                case "zwierze":
                    comboBoxObiekt.SelectedItem = Tables.zwierze;
                    break;
                case "klient":
                    comboBoxObiekt.SelectedItem = Tables.klient;
                    break;
                case "weterynarz":
                    comboBoxObiekt.SelectedItem = Tables.weterynarz;
                    break;
                case "usluga":
                    comboBoxObiekt.SelectedItem = Tables.usluga;
                    break;
                case "lecznica":
                    comboBoxObiekt.SelectedItem = Tables.lecznica;
                    break;
                case "pracownik":
                    comboBoxObiekt.SelectedItem = Tables.pracownik;
                    break;
                case "wizyta":
                    comboBoxObiekt.SelectedItem = Tables.wizyta;
                    break;

            }
            pokazUprawnienia();


            //comboBoxObiekt.SelectedItem = "zwierze";
            // connection.Close();
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
        private string wczytajUprawnienia(string idUsera, string tablica)
        {
          
            //zalezy co wstawiamy
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT "+tablica+" FROM uprawnienia WHERE fk_uzytkownika='" + idUsera+ "';";
            string am = "";
            try
            {
                am = cmd.ExecuteScalar().ToString();

            }
            catch (MySqlException er)
            {
                MessageBox.Show("Wystąpił błąd z bazą danych");
            }
            return am;

        }
        private void populateComboBox()
        {
            foreach (Tables table in Enum.GetValues(typeof(Tables)))
            {
                comboBoxObiekt.Items.Add(table);
                //comboBox.SelectedIndex = 0; //domyslnie Zwierze
            }
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT login  FROM uzytkownicy;";

            List<string> loginy = new List<string>();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                loginy.Add(reader["login"].ToString());
            }
            foreach (string n in loginy){
                // Console.WriteLine(n);
                comboBoxUzytkownik.Items.Add(n);
            }
            reader.Close();
            connection.Close();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {

            Window1 win2 = new Window1(idUzytkownika,connString);
            win2.Show();
            this.Close();

        }
        private string uprawnieniaWybranegoWInterfejsieUzytkownika()
        {
            wybranaTablica = comboBoxObiekt.SelectedValue.ToString();

            connection = new MySqlConnection(connString);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            string a = comboBoxUzytkownik.SelectedValue.ToString();
            cmd.CommandText = "SELECT id_uzytkownika FROM uzytkownicy WHERE login='" + a + "';";

            string id = cmd.ExecuteScalar().ToString();
            wybranyUser = id;

           return wczytajUprawnienia(id, comboBoxObiekt.SelectedValue.ToString());


        }
        private void pokazUprawnienia() {
            ////bierzemy z bazy uprawnienia wybranego uzytkownika
            string nasze = wczytajUprawnienia(idUzytkownika.ToString(),comboBoxObiekt.SelectedValue.ToString());
            string uprawnienia = uprawnieniaWybranegoWInterfejsieUzytkownika();
            foreach (char n in nasze)
            {
                if (n == '1')
                {
                    //wyswietla zaznaczone checkboxy jesli wybrany z combobox uzytkownik ma do tego uprawnienie
                    if (uprawnienia[0] == '1')
                    { czytaj.IsChecked = true; }
                    else
                        czytaj.IsChecked = false;

                    if (uprawnienia[1] == '1')
                    { dopisz.IsChecked = true; }
                    else
                        dopisz.IsChecked = false;

                    if (uprawnienia[2] == '1')
                    { modyfikuj.IsChecked = true; }
                    else
                        modyfikuj.IsChecked = false;

                    if (uprawnienia[3] == '1')
                    { usun.IsChecked = true; }
                    else
                        usun.IsChecked = false;

                    if (uprawnienia[4] == '1')
                    {
                        czytajPrzekaz.IsChecked = true;
                        czytajP.IsEnabled = true;
                    }
                    else {
                        czytajPrzekaz.IsChecked = false;
                        czytajP.IsChecked = false;
                        //  czytajP.Visibility = false;
                    }
                    if (uprawnienia[5] == '1')
                    {
                        dopiszPrzekaz.IsChecked = true;
                        dodajP.IsEnabled = true;
                    }
                    else {
                        dopiszPrzekaz.IsChecked = false;
                        dodajP.IsChecked = false;
                    }
                    if (uprawnienia[6] == '1')
                    {
                        modyfikujPrzekaz.IsChecked = true;
                        modyfikujP.IsEnabled = true;
                    }
                    else {
                        modyfikujPrzekaz.IsChecked = false;
                        modyfikujP.IsChecked = false;
                    }
                    if (uprawnienia[7] == '1')
                    {
                        usunPrzekaz.IsChecked = true;
                        usunP.IsEnabled = true;
                    }
                    else {
                        usunPrzekaz.IsChecked = false;
                        usunP.IsChecked = false;
                    }

                    uprawnienia = wczytajUprawnienia(idUzytkownika.ToString(), comboBoxObiekt.SelectedValue.ToString());
                    //jesli uzytkownik zalogowany ma uprawnienia do zmiany to bedzie mial mozliwosc zaznaczania checkboxow

                    if (uprawnienia[4] == '1')
                    {
                        czytajPrzekaz.IsEnabled = true;
                        czytaj.IsEnabled = true;
                        czytajP.IsEnabled = true;
                    }
                    else {
                        czytaj.IsEnabled = false;
                        czytajPrzekaz.IsEnabled = false;
                        czytajP.IsEnabled = false;
                    }

                    if (uprawnienia[5] == '1')
                    {
                        dopiszPrzekaz.IsEnabled = true;
                        dopisz.IsEnabled = true;
                        dodajP.IsEnabled = true;
                    }
                    else
                    {
                        dopiszPrzekaz.IsEnabled = false;
                        dopisz.IsEnabled = false;
                        dodajP.IsEnabled = false;
                    }
                    if (uprawnienia[6] == '1')
                    {
                        modyfikujPrzekaz.IsEnabled = true;
                        modyfikuj.IsEnabled = true;
                        modyfikujP.IsEnabled = true;
                    }
                    else
                    {
                        modyfikujPrzekaz.IsEnabled = false;
                        modyfikuj.IsEnabled = false;
                        modyfikujP.IsEnabled = false;
                    }
                    if (uprawnienia[7] == '1')
                    {
                        usunPrzekaz.IsEnabled = true;
                        usun.IsEnabled = true;
                        usunP.IsEnabled = true;
                    }
                    else
                    {
                        usunPrzekaz.IsEnabled = false;
                        usun.IsEnabled = false;
                        usunP.IsEnabled = false;
                    }
                    if (wybranyUser == idUzytkownika.ToString())
                    {
                        modyfikuj.IsEnabled = false;
                        modyfikujP.IsEnabled = false;

                        usun.IsEnabled = false;
                        usunP.IsEnabled = false;

                        dopisz.IsEnabled = false;
                        dodajP.IsEnabled = false;

                        czytaj.IsEnabled = false;
                        czytajP.IsEnabled = false;
                    }
                    return;

                }
            }
            MessageBox.Show("Nie masz prawa do oglądania tych uprawnien");
            


        }
        private void PokazUprawnieniaButton_Click(object sender, RoutedEventArgs e)
        {
            pokazUprawnienia();
        }
        //private void usunRazemZDziecmi(string tablica, string id)
        //{

        //    doAQuery("UPDATE uprawnienia SET " + tablica + "='" + "00000000" + "' WHERE fk_uzytkownika='" + id + "';", false); //sobie usuwam
        //    foreach (string a in listaUprawnien)
        //    {
        //        doAQuery("UPDATE drzewo SET " + a + "=null WHERE fk_uzytkownika='" + id+"' AND tablica='" + tablica + "';", false);


        //    }



        //    List<string> listaDoUsuniecia = new List<string>();
        //        foreach (string a in listaUprawnien)
        //        {
        //           // string b = doAQuery("SELECT fk_uzytkownika FROM drzewo WHERE " + a + "='" + id + "' AND tablica='" + tablica + "';", true);
        //        MySqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandText = "SELECT fk_uzytkownika FROM drzewo WHERE " + a + "='" + id + "' AND tablica='" + tablica + "';";

        //        // List<string> loginy = new List<string>();

        //        MySqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            listaDoUsuniecia.Add(reader["fk_uzytkownika"].ToString());
        //        }
        //        reader.Close();
        //        foreach (string m in listaDoUsuniecia)
        //        { usunRazemZDziecmi(tablica,m); }
        //    }

        //}
        private bool czyPrzejmij() {
            if (czyMaUprawnienie(wybranyUser, "przejmij") == "True")
            {
                if (czytajPrzekaz.IsChecked == true || dopiszPrzekaz.IsChecked == true || modyfikujPrzekaz.IsChecked == true || usunPrzekaz.IsChecked == true || czytaj.IsChecked == true || dopisz.IsChecked == true || modyfikuj.IsChecked == true || usun.IsChecked == true)
                {
                    //nowe

                    //foreach (string a in tablice)
                    //{
                    //    usunRazemZDziecmi(a, idUzytkownika.ToString());
                    //}
                    List<string> mojeUprawnienia = new List<string>();
                    string uprawnienie;
                    int i = 0;
                    if (doAQuery("SELECT admin FROM uzytkownicy WHERE id_uzytkownika='" + idUzytkownika + "';", true) == "True")
                    {
                        doAQuery("UPDATE uzytkownicy SET admin=1 WHERE id_uzytkownika='" + wybranyUser + "';", false);
                        doAQuery("UPDATE uzytkownicy SET admin=0 WHERE id_uzytkownika='" + idUzytkownika + "';", false);

                    }

                        foreach (string u in tablice)
                    {


                        uprawnienie = doAQuery("SELECT " + u + " FROM uprawnienia WHERE fk_uzytkownika='" + idUzytkownika + "';", true); //uprawnienia do przekazania

                        //spr czy male s czy S


                        StringBuilder uprasy = new StringBuilder(uprawnienie);
                        for (int p = 0; p < 4; p++)
                        {
                            if (uprasy[p] == '1' && uprasy[p + 4] == '0')
                            {
                                uprasy[p] = '0';
                            }
                        }
                        uprawnienie = uprasy.ToString();

                        usunRazemZDziecmi(u, wybranyUser);

                        usunRazemZDziecmi(u, idUzytkownika.ToString());


                        foreach (string upr in listaUprawnien)
                        {
                            zabierzUprawnienie(wybranyUser, upr, u);
                            zabierzUprawnienie(idUzytkownika.ToString(), upr, u);
                        }

                        doAQuery("UPDATE uprawnienia SET " + u + "='" + uprawnienie + "' WHERE fk_uzytkownika='" + wybranyUser + "';", false); //wstawiam je temu ktoremu je oddaje

                        i++;
                    }
                    doAQuery("UPDATE uzytkownicy SET przejmij='0' WHERE id_uzytkownika='" + wybranyUser + "';", false);




                }
                return true;
            }
            return false;
        }
        private void button1_Click(object sender, RoutedEventArgs e) //zatwierdzenie update uprawnien
        {
            if(czyPrzejmij())
                return;
            
            string stareUprawnienia = uprawnieniaWybranegoWInterfejsieUzytkownika(); ;

                string uprawnieniaDoPrzekazania = "";
                
            if (czytaj.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';

                if (dopisz.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';

                if (modyfikuj.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';

                if (usun.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';

                if (czytajPrzekaz.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';

                if (dopiszPrzekaz.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';

                if (modyfikujPrzekaz.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';

                if (usunPrzekaz.IsChecked == true)
                    uprawnieniaDoPrzekazania += '1';
                else
                    uprawnieniaDoPrzekazania += '0';
                bool czyByloSprawdzane= false;

            for (int i = 0; i < 8; i++)
            {
                if (uprawnieniaDoPrzekazania[i] == '1' && stareUprawnienia[i] == '0') //jesli nadajemy uprawnienie
                {
                    //if (czyByloSprawdzane == true)
                    //{
                    //    doAQuery("UPDATE drzewo SET " + listaUprawnien[i] + "=" + idUzytkownika.ToString() + " WHERE fk_uzytkownika='" + wybranyUser + "' AND tablica='" + wybranaTablica + "';", false);
                    //    //  doAQuery("UPDATE uprawnienia SET " + wybranaTablica + " ='" + uprawnieniaDoPrzekazania + "' WHERE fk_uzytkownika='" + wybranyUser + "';", false);


                    //}
                    //else {
                        if (czyMogePrzyznacUprawniania(idUzytkownika.ToString()))
                        {
                            if (i > 3)
                            {
                                StringBuilder upr = new StringBuilder(uprawnieniaDoPrzekazania);
                                upr[i - 4] = '1';
                                uprawnieniaDoPrzekazania = upr.ToString();
                            doAQuery("UPDATE drzewo SET " + listaUprawnien[i-4] + "=" + idUzytkownika.ToString() + " WHERE fk_uzytkownika='" + wybranyUser + "' AND tablica='" + wybranaTablica + "';", false);
                        }

                            doAQuery("UPDATE drzewo SET " + listaUprawnien[i] + "=" + idUzytkownika.ToString() + " WHERE fk_uzytkownika='" + wybranyUser + "' AND tablica='" + wybranaTablica + "';", false);

                            doAQuery("UPDATE uprawnienia SET " + wybranaTablica + " ='" + uprawnieniaDoPrzekazania + "' WHERE fk_uzytkownika='" + wybranyUser + "';", false);

                           

                        //    czyByloSprawdzane = true;
                        //}


                    }

                    pokazUprawnienia();
                }
                else if (uprawnieniaDoPrzekazania[i] == '0' && stareUprawnienia[i] == '1') //jesli zabieramy uprawnienie

                {
                    if (czyMogePrzyznacUprawniania(idUzytkownika.ToString()))
                    {
                        string s = listaUprawnien[i];
                        //wywalamy razem z dziecmi to uprawnienie z drzewa
                        zabierzUprawnienie(wybranyUser, s, wybranaTablica);
                        usunDzieciomUprawnienie(wybranaTablica, wybranyUser, s);
                        if (i <= 3)
                        {
                            s = listaUprawnien[i + 4];
                            zabierzUprawnienie(wybranyUser, s, wybranaTablica);
                            usunDzieciomUprawnienie(wybranaTablica, wybranyUser, s);

                        }
                        if (i > 3)
                        {
                            s = listaUprawnien[i - 4];
                          //  zabierzUprawnienie(wybranyUser, s, wybranaTablica);
                            usunDzieciomUprawnienie(wybranaTablica, wybranyUser, s);
                        }
                    }
                    pokazUprawnienia();
                    //wybranyUser,s,wybranaTablica);
                }
              
            }

        }

        private bool czyMogePrzyznacUprawniania(string id) {

            string pusty;
            foreach (string a in listaUprawnien)
            {
               
                    pusty = doAQuery("SELECT " + a + " FROM drzewo WHERE fk_uzytkownika='" + id + "' AND tablica='" + wybranaTablica + "';", true);
                if (pusty != null)
                {
                    if (pusty == wybranyUser)
                    {
                        MessageBox.Show("Nie możesz przydzielić tej osobie uprawnienia do tej tablicy");
                        return false;
                    }
                    //if (pusty == "12") //jesli doszlismy do roota
                    //    return true;
                    if (czyMogePrzyznacUprawniania(a) == false)
                        return false;

                }
            }
            return true;
        }
        private string czyMaUprawnienie(string user, string uprawnienie)
        {

                    MySqlCommand cmd = connection.CreateCommand();

                    cmd.CommandText = "SELECT " + uprawnienie + " FROM uzytkownicy WHERE  id_uzytkownika='" + user + "';";

            return cmd.ExecuteScalar().ToString();


        }

        private string jakieMaUprawnienia(string user, string tablica) {

            MySqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT "+tablica +" FROM uprawnienia WHERE  fk_uzytkownika='" + user + "';";

            return cmd.ExecuteScalar().ToString();

        }
        private void usunRazemZDziecmi(string tablica, string id)// zabiera uprawnienia wszystkim dzieciom tego id , te ktore on im przekazal
        {


            if (tablica == "wizyta")
            { int a=0;
                a += 1;

            }
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
                    zabierzUprawnienie(temp, a, tablica);
                }
                reader.Close();
                foreach (string m in listaDoUsuniecia)
                { usunDzieciomUprawnienie(tablica, m, a); }
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

            zabierzUprawnienie(id, uprawnienie, tablica);
            while (reader.Read())
            {
                listaDoUsuniecia.Add(reader["fk_uzytkownika"].ToString());
               
            }
            reader.Close();
            foreach (string m in listaDoUsuniecia)
            { usunDzieciomUprawnienie(tablica, m, uprawnienie); }


        }
        private void zabierzUprawnienie(string id, string uprawnienieDoZabrania, string tablica)// zabiera uprawnienie i info on kogo je dostal w tab drzewo
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
            doAQuery("UPDATE drzewo SET " + uprawnienieDoZabrania + "=null WHERE tablica='" + tablica + "' AND fk_uzytkownika='" + id + "';", false);



        }
        private void zatwierdzPrzekaz_Click(object sender, RoutedEventArgs e)
        {


            if (czyMaUprawnienie(wybranyUser, "przejmij") == "True")
            {
                if (czytajP.IsChecked == true || dodajP.IsChecked == true || modyfikujP.IsChecked == true || usunP.IsChecked == true)
                {
                    //nowe

                    //foreach (string a in tablice)
                    //{
                    //    usunRazemZDziecmi(a, idUzytkownika.ToString());
                    //}
                    List<string> mojeUprawnienia = new List<string>();
                    string uprawnienie;
                    int i = 0;
                    foreach (string u in tablice)
                    {


                        uprawnienie = doAQuery("SELECT " + u + " FROM uprawnienia WHERE fk_uzytkownika='" + idUzytkownika + "';", true); //uprawnienia do przekazania

                        usunRazemZDziecmi(u, wybranyUser);
                        
                        usunRazemZDziecmi(u, idUzytkownika.ToString());

                 
                            foreach (string upr in listaUprawnien)
                            {
                                zabierzUprawnienie(wybranyUser, upr, u);
                                zabierzUprawnienie(idUzytkownika.ToString(), upr, u);
                            }

                        doAQuery("UPDATE uprawnienia SET " + u + "='" + uprawnienie + "' WHERE fk_uzytkownika='" + wybranyUser + "';", false); //wstawiam je temu ktoremu je oddaje

                        i++;
                    }
                    doAQuery("UPDATE uzytkownicy SET przejmij='0' WHERE id_uzytkownika='"+wybranyUser+"';", false);




                }
            }
            else
                MessageBox.Show("Nie mozna, ten uzytkownik nie moze przejmowac");

            MessageBox.Show("udalo sie");
            //        if (czytajP.IsChecked == true)
            //    {

            //        string uprawnieniaObecnieTypa = jakieMaUprawnienia(wybranyUser, wybranaTablica);
            //        string uprawnieniaObecnieMoje = jakieMaUprawnienia(idUzytkownika.ToString(), wybranaTablica);
            //        StringBuilder sObce = new StringBuilder(uprawnieniaObecnieTypa);
            //        StringBuilder sMoje = new StringBuilder(uprawnieniaObecnieMoje);
            //        sObce[0] = '1';
            //        sObce[4] = '1';
            //        sMoje[0] = '0';
            //        sMoje[4] = '0';

            //        //   uprawnieniaObecnieTypa[0] = "1";
            //        MySqlCommand cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sObce + "' WHERE fk_uzytkownika='" + wybranyUser + "';";

            //        cmd.ExecuteScalar();
            //        cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sMoje + "' WHERE fk_uzytkownika='" + idUzytkownika + "';";
            //        try
            //        {
            //            cmd.ExecuteScalar();
            //        }
            //        catch (MySqlException er)
            //        {
            //            MessageBox.Show("Błąd z połączeniem z bazą danych");
            //        }
            //        czytajP.IsChecked = false;
            //    }

            //    if (dodajP.IsChecked == true)
            //    {
            //        string uprawnieniaObecnieTypa = jakieMaUprawnienia(wybranyUser, wybranaTablica);
            //        string uprawnieniaObecnieMoje = jakieMaUprawnienia(idUzytkownika.ToString(), wybranaTablica);
            //        StringBuilder sObce = new StringBuilder(uprawnieniaObecnieTypa);
            //        StringBuilder sMoje = new StringBuilder(uprawnieniaObecnieMoje);

            //        sObce[1] = '1';
            //        sObce[5] = '1';
            //        sMoje[1] = '0';
            //        sMoje[5] = '0';

            //        //   uprawnieniaObecnieTypa[0] = "1";
            //        MySqlCommand cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sObce + "' WHERE fk_uzytkownika='" + wybranyUser + "';";

            //        cmd.ExecuteScalar();
            //        cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sMoje + "' WHERE fk_uzytkownika='" + idUzytkownika + "';";

            //        try
            //        {
            //            cmd.ExecuteScalar();
            //        }
            //        catch (MySqlException er)
            //        {
            //            MessageBox.Show("Błąd z połączeniem z bazą danych");
            //        }

            //        dodajP.IsChecked = false;
            //    }

            //    if (modyfikujP.IsChecked == true)
            //    {
            //        string uprawnieniaObecnieTypa = jakieMaUprawnienia(wybranyUser, wybranaTablica);
            //        string uprawnieniaObecnieMoje = jakieMaUprawnienia(idUzytkownika.ToString(), wybranaTablica);
            //        StringBuilder sObce = new StringBuilder(uprawnieniaObecnieTypa);
            //        StringBuilder sMoje = new StringBuilder(uprawnieniaObecnieMoje);

            //        sObce[2] = '1';
            //        sObce[6] = '1';
            //        sMoje[2] = '0';
            //        sMoje[6] = '0';

            //        //   uprawnieniaObecnieTypa[0] = "1";
            //        MySqlCommand cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sObce + "' WHERE fk_uzytkownika='" + wybranyUser + "';";

            //        cmd.ExecuteScalar();
            //        cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sMoje + "' WHERE fk_uzytkownika='" + idUzytkownika + "';";
            //        try
            //        {
            //            cmd.ExecuteScalar();
            //        }
            //        catch (MySqlException er)
            //        {
            //            MessageBox.Show("Błąd z połączeniem z bazą danych");
            //        }
            //        modyfikujP.IsChecked = false;
            //    }

            //    if (usunP.IsChecked == true)
            //    {
            //        string uprawnieniaObecnieTypa = jakieMaUprawnienia(wybranyUser, wybranaTablica);
            //        string uprawnieniaObecnieMoje = jakieMaUprawnienia(idUzytkownika.ToString(), wybranaTablica);
            //        StringBuilder sObce = new StringBuilder(uprawnieniaObecnieTypa);
            //        StringBuilder sMoje = new StringBuilder(uprawnieniaObecnieMoje);

            //        sObce[3] = '1';
            //        sObce[7] = '1';
            //        sMoje[3] = '0';
            //        sMoje[7] = '0';

            //        //   uprawnieniaObecnieTypa[0] = "1";
            //        MySqlCommand cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sObce + "' WHERE fk_uzytkownika='" + wybranyUser + "';";

            //        cmd.ExecuteScalar();
            //        cmd = connection.CreateCommand();

            //        cmd.CommandText = "UPDATE uprawnienia SET " + wybranaTablica + " ='" + sMoje + "' WHERE fk_uzytkownika='" + idUzytkownika + "';";

            //        try
            //        {
            //            cmd.ExecuteScalar();
            //        }
            //        catch (MySqlException er)
            //        {
            //            MessageBox.Show("Błąd z połączeniem z bazą danych");
            //        }

            //        usunP.IsChecked = false;
            //    }
            //    pokazUprawnienia();
            //}

            //else
            //    MessageBox.Show("Ten uzytkownik nie ma uprawnienia do przejmowania uprawnien");
  }

        private void pokazPrzeglad_Click(object sender, RoutedEventArgs e)
        {
            PrzegladUprawnien pu = new PrzegladUprawnien(idUzytkownika, connString);
            pu.Show();
            this.Close();
        }
    }
}
