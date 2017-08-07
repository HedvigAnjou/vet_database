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
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace BSK2
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // String connString = "server=192.168.43.163;uid=Ewa;pwd=piesnicpon;database=zwierzyniec;";
        //String connString= "server=localhost;uid=root;pwd=piesnicpon;database=zwierzyniec;";
        //String connString = "server=192.168.43.163;database=zwierzyniec;user=sslclient;pwd=ssl;" + "CertificateFile=E:\\client.pfx;" + "CertificatePassword=pass;" + "SSL Mode=Required ";
        String connString;// = "server=192.168.0.13;database=zwierzyniec;user=sslclient;pwd=ssl;" + "CertificateFile=E:\\client.pfx;" + "CertificatePassword=pass;" + "SSL Mode=Required ";
        public MainWindow()
        {
            InitializeComponent();
            int counter = 0;
            string line;
            string ip = "";
            string keylocation = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\client.pfx";
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\DatabaseConfig.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (counter == 0)
                    ip = line;
                else if (counter == 1 && line!="default")
                    keylocation = line;
                counter++;
            }
            connString = "server="+ip+";database=zwierzyniec;user=sslclient;pwd=ssl;" + "CertificateFile="+keylocation+";" + "CertificatePassword=pass;" + "SSL Mode=Required ";
            connString= "server=localhost;database=zwierzyniec;user=root;Pwd=piesnicpon ";
            file.Close();

        }
        public static string GetSHA512(string text)
        {

            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = ue.GetBytes(text);

            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }

            return hex;

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
        private void button_Click(object sender, RoutedEventArgs e)
        {
            string login=loginText.Text;
            string pass;

            pass = GenerateSHA512String(passwordText.Password);
            MySqlConnection connection;
           
            connection = new MySqlConnection(connString);
            try
            {
               
                connection.Open();
            }
            catch (MySqlException er)
            { MessageBox.Show("Problem z laczeniem z baza danych"); }
           
            //adapter = new MySqlDataAdapter("SELECT uzytkownicy FROM Zwierze WHERE login=\""+login+"\" ", connection);

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT IF(   EXISTS(SELECT * FROM uzytkownicy WHERE login='" + login + "' and pass_hash='" + pass + "' ) , 1, 0);";
            string result="";
            try
            {
                 result= cmd.ExecuteScalar().ToString();
            }
            catch (MySqlException er)
            { MessageBox.Show("Problem z laczeniem z baza danych"); }

            if (result == "1")
            {//zalogowana!
             //!!!!!!!!!!!!!!!!!!!!
             //siegnij po jej ID
             //wywolaj konstruktor z tym ID

                MySqlCommand cmd2 = connection.CreateCommand();
                cmd.CommandText = "SELECT id_uzytkownika FROM uzytkownicy WHERE login='" + login + "' and pass_hash='" + pass + "' ;";
                int result2=0;
                try
                {
                    result2 = (int)cmd.ExecuteScalar();
                }
                catch (MySqlException er)
                { MessageBox.Show("Problem z laczeniem z baza danych"); }
                
                //Console.WriteLine("aaaaaaaaa"+result2);
                Window1 win2 = new Window1(result2,connString);
                win2.Show();
                this.Close();


            }
            else {
                //niezalogowana
                errorLabel.Content = "Wpisalaś niepoprawne hasło lub login, spróbuj ponownie";

            }
            Console.WriteLine("Czy zalogowany? Otoz: "+result);
           

        }
    }
}
