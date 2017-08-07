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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;

namespace BSK2
{
    /// <summary>
    /// Interaction logic for InsertWindow.xaml
    /// </summary>
    /// 
    public class klasa
    {
        public string jeden;
        public string dwa;
        public klasa(string g, string s) { jeden = g; dwa = s; }
    }
    public partial class InsertWindow : Window
    {

        private String connString;
        public int idUzytkownika;
        // private List<string> wynikowa = new List<string>();
         private List<string> wynikowa2 = new List<string>();
        private List<string> mainList;
        private Dane okno;
        public string rowsBind { get; set; }
        public ObservableCollection<string> wynikowa { get; set; }
        public ObservableCollection<string> bla { get; set; }
        public InsertWindow(List<string> listOfAttr, Dane window,int id,string connStr)
        {
            connString = connStr;
            idUzytkownika = id;
            okno = window;
            InitializeComponent();
            mainList = listOfAttr;
            rowsBind = "kolumna";
            wynikowa = new ObservableCollection<string>();
            bla = new ObservableCollection<string>();
            for (int i = 0; i < listOfAttr.Count(); i++)
            {
                var column = new DataGridTextColumn();
                column.Header = listOfAttr[i];
               column.Binding = new Binding(rowsBind);
                dataGrid1.Columns.Add(column);
                wynikowa.Add("");
                bla.Add("");
            }
      
          //  bla.Add("");
            //  dataGrid1.ItemsSource = bla;
            //   pusta.CollectionChanged += HandleChange;

            // var kk = new DataGridRow();
            //  dataGrid1.Items.Add(wynikowa);
            //ataGrid1.IsReadOnly = false;
            // dataGrid1.Items.
            // kk.Binding = new Binding("ROW");
            dataGrid1.ItemsSource = bla;
        }


        private void Datagrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //int i=dataGrid1.Items.Count;
            var currentRowIndex = dataGrid1.Items.IndexOf(dataGrid1.SelectedItem);

            TextBox t = e.EditingElement as TextBox;
        
            string columnName = e.Column.Header.ToString();
            string newValue = t.Text;

            for (int i = 0; i < mainList.Count(); i++)
            {
                if (mainList[i] == columnName)
                {
                    wynikowa[i] = newValue;
                    break;
                }

            }
           // t.Text = newValue;
         
        }

        private void Buttonix_Click(object sender, RoutedEventArgs e)
        {
            foreach (string a in wynikowa)
                wynikowa2.Add(a);
            okno.getValues(wynikowa2);
            this.Close();
        }
    }
}
