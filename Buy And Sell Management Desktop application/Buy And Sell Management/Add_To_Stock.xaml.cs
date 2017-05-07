using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Buy_And_Sell_Management
{
    /// <summary>
    /// Interaction logic for Add_To_Stock.xaml
    /// </summary>
    public partial class Add_To_Stock : Window
    {
        public Add_To_Stock()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void inti(string nam, int quantity)
        {
            name.Text = nam;   
            PStock.Text = quantity.ToString();
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            Stock stock = new Stock();
            if (NStock.Text.ToString() != "")
            {
                stock.DQuantity = int.Parse(NStock.Text) + int.Parse(PStock.Text);
                stock.DName = name.Text.ToString();
                stock.UpdateStock();
                //reinitialize
                name.Text = "";
                PStock.Text = "";
                NStock.Text = "";
            }
            else MessageBox.Show("Enter Added Stock");
        }
    }
}
