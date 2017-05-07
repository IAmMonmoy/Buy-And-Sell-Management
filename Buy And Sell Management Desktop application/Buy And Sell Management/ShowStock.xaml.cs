using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
using System.ComponentModel;

namespace Buy_And_Sell_Management
{
    /// <summary>
    /// Interaction logic for ShowStock.xaml
    /// </summary>
    public partial class ShowStock : Window
    {
        public ShowStock()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Stock stock = new Stock();
            DataTable dt = stock.showstock();
            List<stk> xy = new List<stk>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                xy.Add(new stk() { Name = dt.Rows[i][1].ToString(), Stock = int.Parse(dt.Rows[i][2].ToString())});
            }
            showdata.ItemsSource = null;
            showdata.ItemsSource = xy;
        }
        public class stk
        {
            public string Name { get; set; }
            public int Stock { get; set; }
        }
        private void Form_Closing(object sender, CancelEventArgs e)
        {
            showdata.ItemsSource = null;

        }
    }
}
