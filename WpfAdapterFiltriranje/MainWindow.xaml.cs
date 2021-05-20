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
using System.Data;
using System.Data.SqlClient;

namespace WpfAdapterFiltriranje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView1.SelectedIndex > -1)
            {
                DataRowView dr = ListView1.SelectedItem as DataRowView;
                decimal cena = (decimal)dr.Row[2];

                TextBlock1.Text ="Cena" + cena;
            }

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            TextBlock1.Text = "";
            if (!decimal.TryParse(TextBoxMin.Text,out decimal min))
            {
                MessageBox.Show("Unesite broj");
                TextBoxMin.Clear();
                TextBoxMin.Focus();
                return;
            }

            if (!decimal.TryParse(TextBoxMax.Text, out decimal max))
            {
                MessageBox.Show("Unesite broj");
                TextBoxMax.Clear();
                TextBoxMax.Focus();
                return;
            }

            if (min >= max)
            {
                MessageBox.Show("Max mora biti veci");
                TextBoxMax.Focus();
                return;
            }

            SqlDataAdapter da = new SqlDataAdapter("SELECT ProizvodId,NazivProizvoda, JedinicnaCena FROM Proizvodnja.Proizvodi", Konekcija.cnnTSQL2018);

            DataSet ds = new DataSet();

            DataTable tbl = null;

            try
            {
                da.Fill(ds, "Proizvodi");
                tbl = ds.Tables["Proizvodi"];
            }
            catch (Exception xcp)
            {
                MessageBox.Show(xcp.Message);
                return;
            }

            DataView dv = new DataView(tbl);
            string filter = $"JedinicnaCena >={min} AND JedinicnaCena<={max}";
            dv.RowFilter = filter;
            dv.Sort = "JedinicnaCena";

            ListView1.ItemsSource = dv;
        }
    }
}
