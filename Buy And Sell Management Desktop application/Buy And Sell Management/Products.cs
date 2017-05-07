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
    class Products
    {
        private string ProductName;
        private int ProductBuyPrice;
        private int ProductSellPrice;
        private int ProductQuantity;
        private byte[] ProductImage = new byte[0];

        public string DProductName
        {
            get
            {
                return ProductName;
            }
            set
            {
                ProductName = value;
            }
        }

        public int DProductBuyPrice
        {
            get
            {
                return ProductBuyPrice;
            }
            set
            {
                ProductBuyPrice = value;
            }
        }

        public int DProductSellPrice
        {
            get
            {
                return ProductSellPrice;
            }
            set
            {
                ProductSellPrice = value;
            }
        }

        public int DProductQuantity
        {
            get
            {
                return ProductQuantity;
            }
            set
            {
                ProductQuantity = value;
            }
        }

        public byte[] DProductImage
        {
            get
            {
                return ProductImage;
            }
            set
            {
         
                ProductImage = new byte[value.Length];
                ProductImage = value;
            }
        }

        public int AddProduct()
        {
            int value = 1; //check if there are duplicates and user wants to change then returns 1 else returns 0. for checking whether insert to stock or not
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand("sp_insert_data", con); //stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProductName", SqlDbType.NChar).Value = ProductName;
                cmd.Parameters.Add("@BuyPrice", SqlDbType.BigInt).Value = ProductBuyPrice;
                cmd.Parameters.Add("@SellPrice", SqlDbType.BigInt).Value = ProductSellPrice;

                if (ProductQuantity.ToString() != "")
                    cmd.Parameters.Add("@Quantity", SqlDbType.NChar).Value = ProductQuantity;
                else cmd.Parameters.Add("@Quantity", SqlDbType.NChar).Value = DBNull.Value;

                if (ProductImage.Length > 0)
                    cmd.Parameters.Add("@ProductImage", SqlDbType.Image).Value = ProductImage;
                else
                    cmd.Parameters.Add("@ProductImage", SqlDbType.Image).Value = DBNull.Value;


                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    ex.ToString(); //dummy for removing warning
                    MessageBoxResult result = MessageBox.Show("Already contains data do u want to replace", "Important", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                    if (result == MessageBoxResult.Yes)
                    {
                        SqlCommand cmd1 = new SqlCommand("DELETE FROM TProducts WHERE ProductName = '" + ProductName + "'", con);
                        try
                        {
                            cmd1.ExecuteNonQuery();
                            //first delete the data then recursive call to insert updated one
                            con.Close();
                            AddProduct();
                            con.Close();
                        }
                        catch (SqlException x)
                        {
                            MessageBox.Show(x.ToString());
                        }
                    }
                    else value = 0;
                }
            }
            else MessageBox.Show("DataBase Connection Problem");
            return value;
       }
        

        public void EditProduct(Products product)
        {
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            //first delete the product and add newly.
            SqlCommand cmd1 = new SqlCommand("DELETE FROM TProducts WHERE ProductName = '" + product.DProductName + "'", con);
            try
            {
                cmd1.ExecuteNonQuery();
            }
            catch (SqlException x)
            {
                x.ToString();
                MessageBox.Show("Can Not Edit Product.restart application");
                return;
            }
            
            
            if (con.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand("sp_insert_data", con); //stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProductName", SqlDbType.NChar).Value = product.DProductName;
                cmd.Parameters.Add("@BuyPrice", SqlDbType.BigInt).Value = product.DProductBuyPrice;
                cmd.Parameters.Add("@SellPrice", SqlDbType.BigInt).Value = product.DProductSellPrice;

                if (ProductQuantity.ToString() != "")
                    cmd.Parameters.Add("@Quantity", SqlDbType.NChar).Value = product.DProductQuantity;
                else cmd.Parameters.Add("@Quantity", SqlDbType.NChar).Value = DBNull.Value;

                if (ProductImage.Length > 0)
                    cmd.Parameters.Add("@ProductImage", SqlDbType.Image).Value = product.DProductImage;
                else
                    cmd.Parameters.Add("@ProductImage", SqlDbType.Image).Value = DBNull.Value;


                try
                {
                    cmd.ExecuteNonQuery();
                    Stock stock = new Stock();
                    stock.DName = product.DProductName;
                    stock.DQuantity = product.DProductQuantity;
                    stock.UpdateStock();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    ex.ToString(); //dummy for removing warning
                    MessageBox.Show("Can Not Add Product Restart Application Or Contract Application Provider");
                }
            }
            else MessageBox.Show("DataBase Connection Problem");

        }

        public void DeleteProduct()
        {
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd1 = new SqlCommand("DELETE FROM TProducts WHERE ProductName = '" + ProductName + "'", con);
            SqlCommand cmd2 = new SqlCommand("DELETE FROM Stock WHERE ProductName = '" + ProductName + "'", con);
            try
            {
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Data is deleted");
            }
            catch (SqlException x)
            {
                x.ToString();
                MessageBox.Show("Can Not Delete Product.restart application");
                return;
            }
        }


    }
}
