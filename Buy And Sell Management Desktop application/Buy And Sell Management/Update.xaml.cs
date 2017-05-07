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
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Window
    {

        Products product = new Products();
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        string source = "";
        string temp = "";
        public Update()
        {
            InitializeComponent();
            
        }

      

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        

        //initialize product and user inferface
        public void set(string name)
        {
            temp = name;
            //get the image from database because we can not convert the imagesource to byte from Ediitem.xaml datagrid
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlDataAdapter da,da1;
            DataTable dt,dt1;
            SqlCommand cmd1 = new SqlCommand("SELECT BuyPrice,SellPrice,Quantity,ProductImage FROM TProducts WHERE ProductName = '" + name + "'", con);
            SqlCommand cmd2 = new SqlCommand("SELECT Stock FROM Stock WHERE ProductName = '" + name + "'", con);
            da = new SqlDataAdapter(cmd1);
            da1 = new SqlDataAdapter(cmd2);
            dt = new DataTable();
            dt1 = new DataTable();
            da.Fill(dt);
            da1.Fill(dt1);
            //fill the user inteface
            product.DProductName = name;
            ProductName.Text = name;
            Buy.Text = dt.Rows[0][0].ToString();
            Sell.Text = dt.Rows[0][1].ToString();
            Quantity.Text = dt1.Rows[0][0].ToString();

            //if image is avilable set the image for viewing and set the product image = bytestream
            if (dt.Rows[0][3] != DBNull.Value)
            {
                byte[] img = (byte[])dt.Rows[0][3];
                product.DProductImage = img;
                MemoryStream ms = new MemoryStream(img);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = null;
                bi.StreamSource = ms;
                bi.EndInit();

                image1.Source = bi;
            }
            con.Close();
       }

        //setting up the image1 in update.xaml and assign the image to product to update database
        private void Picture_Button_Click(object sender, RoutedEventArgs e)
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

        private void update_Button_Click(object sender, RoutedEventArgs e)
        {
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            //Delete The Edit product. The data will be will be added again after edited
            SqlCommand cmd1 = new SqlCommand("DELETE FROM TProducts WHERE ProductName = '" + temp + "'", con);
            MessageBox.Show(name.ToString());
            SqlCommand cmd2 = new SqlCommand("DELETE FROM Stock WHERE ProductName = '" + temp + "'", con);
            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            con.Close();

            int num1;
            if (ProductName.Text != "" && Buy.Text != "" && Sell.Text != "" && int.TryParse(Buy.Text, out num1) && int.TryParse(Sell.Text, out num1) && (int.TryParse(Quantity.Text, out num1) || Quantity.Text == ""))
            {
                product.DProductName = ProductName.Text;
                product.DProductBuyPrice = int.Parse(Buy.Text);
                product.DProductSellPrice = int.Parse(Sell.Text);
                product.DProductQuantity = int.Parse(Quantity.Text);

                product.EditProduct(product); //add product to database

                Stock stock = new Stock();
                stock.DName = ProductName.Text;
                stock.DQuantity = int.Parse(Quantity.Text);
                stock.AddStock();

                ProductName.Clear();
                Buy.Clear();
                Sell.Clear();
                Quantity.Clear();
                image1.Source = null;
            }

            else MessageBox.Show("Name , Buy Price And Sell Price Can Not Be Empty And Number Can Not Be Characters");
        }
        private void close_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
