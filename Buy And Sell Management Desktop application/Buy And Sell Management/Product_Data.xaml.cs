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
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Buy_And_Sell_Management
{
    /// <summary>
    /// Interaction logic for Product_Data.xaml
    /// </summary>
    public partial class Product_Data : Window
    {
        
        public Product_Data()
        {
            InitializeComponent();
        }

        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        string source = "";
        Products product = new Products();

        

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            int num1;
            if (name.Text != "" && Buy.Text != "" && Sell.Text != "" && int.TryParse(Buy.Text, out num1) && int.TryParse(Sell.Text, out num1) && (int.TryParse(Quantity.Text,out num1) || Quantity.Text == ""))
            {
                product.DProductName = name.Text;
                product.DProductBuyPrice = int.Parse(Buy.Text);
                product.DProductSellPrice = int.Parse(Sell.Text);
                product.DProductQuantity = int.Parse(Quantity.Text);

                int x = product.AddProduct(); //add product to database
                if (x == 1) //if there are existing elements and user does not want to change then this does not work
                {
                    //add to stock
                    Stock stock = new Stock();
                    stock.DName = name.Text.ToString();
                    if (Quantity.Text.ToString() != "")
                        stock.DQuantity = int.Parse(Quantity.Text.ToString());
                    else stock.DQuantity = 0;
                    stock.AddStock();
                }

                name.Clear();
                Buy.Clear();
                Sell.Clear();
                Quantity.Clear();
                image1.Source = null;
            }
   
            else MessageBox.Show("Name , Buy Price And Sell Price Can Not Be Empty And Number Can Not Be Characters");

        }

        

        private void Picture_Butoon_Click(object sender, RoutedEventArgs e)
        {
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"; 
            Nullable<bool> T = dlg.ShowDialog();
            if (T == true)
            {
                source = dlg.FileName;
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(source);
                img.EndInit();
                image1.Source = img;

                
                FileStream fs;
                byte[] img1;
                fs = new FileStream(source, FileMode.Open, FileAccess.Read);
                img1 = new byte[fs.Length];
                product.DProductImage = img1; //setting the size of DproductImage According to image size
                fs.Read(product.DProductImage, 0, Convert.ToInt32(fs.Length));
               
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
