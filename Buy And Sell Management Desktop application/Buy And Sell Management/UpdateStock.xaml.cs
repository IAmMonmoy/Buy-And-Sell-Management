using System;
using System.Collections.Generic;
using System.Windows;

using System.IO;
using System.Data;
using System.Data.SqlClient;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Drawing;
using System.ComponentModel;

namespace Buy_And_Sell_Management
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class UpdateStock : Window
    {
        public UpdateStock()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadinterface();
        }
        private void loadinterface()
        {
            Stock stock = new Stock();
            DataTable dt = stock.showstock();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ShowData.Items.Add(dt.Rows[i][1]);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowData.SelectedItem != null)
            {
                string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd1 = new SqlCommand("SELECT Stock FROM Stock WHERE ProductName = '" + ShowData.SelectedItem + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                DataTable dt = new DataTable();
                da.Fill(dt);

                Add_To_Stock add = new Add_To_Stock();
                int temp = int.Parse(dt.Rows[0][0].ToString());
                add.inti(ShowData.SelectedItem.ToString(), temp);
                add.ShowDialog();
            }
            else MessageBox.Show("Select Some item");
        }
    }
}
