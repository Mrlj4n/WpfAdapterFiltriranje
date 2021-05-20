using System;
using System.Data;
using System.Data.SqlClient;
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
using Diacritics.Extensions; //EKSTENZIJA

namespace WpfAdapterFiltriranje
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private DataView Filtriraj(string deoNaziva)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT KupacId as Id, ImeKompanije as Firma, KontaktOsoba as Kontakt FROM Prodaja.Kupci", Konekcija.cnnTSQL2018);

            DataSet ds = new DataSet();
            DataTable tbl = null;
            try
            {
                da.Fill(ds, "Kupci");
                tbl = ds.Tables["Kupci"];
                tbl.Columns.Add("FirmaAI",typeof(string));

                foreach (DataRow row in tbl.Rows)
                {
                    row["FirmaAI"] = row["Firma"].ToString().RemoveDiacritics();
                }
            }
            catch (Exception xcp)
            {
                MessageBox.Show(xcp.Message);
                return null;
            }


            DataView dv = new DataView(tbl);
            string filter = $"FirmaAI LIKE '%{deoNaziva}%'";
            dv.RowFilter = filter;
            dv.Sort = "Firma";
            return dv;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid1.ItemsSource = Filtriraj("");
        }

        private void TextBoxPretraga_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataGrid1.ItemsSource = Filtriraj(TextBoxPretraga.Text);
            }
        }
    }
}
