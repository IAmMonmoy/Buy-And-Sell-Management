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
    /// Interaction logic for EditItem.xaml
    /// </summary>
    public partial class EditItem : Window
    {
        public EditItem()
        {
            InitializeComponent();
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //load all the contents at start
            UpadateDataGrid();
        }

        private void Form_Closing(object sender, CancelEventArgs e)
        {
            ShowData.ItemsSource = null;
           
        }

        private void UpadateDataGrid()
        {
            TProductsDB GetData = new TProductsDB();
            DataTable dt = GetData.Connect();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ShowData.Items.Add(GetData.Get_ProductName(i));
            }
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowData.SelectedItem != null)
            {
                string drv = (string)ShowData.SelectedItem;
                Update edit = new Update();
                edit.set(drv);
                edit.ShowDialog();
          
            }
            else
            {
                MessageBox.Show("No Data is selected");
            }
        }


        private void DeleteButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (ShowData.SelectedItem != null)
            {
                string drv = (string)ShowData.SelectedItem;
                Products product = new Products();
                product.DProductName = drv;
                product.DeleteProduct();
                ShowData.Items.Remove(drv);
            }
            else
            {
                MessageBox.Show("No Data is selected");
            }
        }

       

    }

 }

