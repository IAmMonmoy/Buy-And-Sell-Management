using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;
namespace Buy_And_Sell_Management
{
    class TProductsDB
    {
        private SqlConnection con;
        private SqlDataAdapter da;
        private SqlCommand cmd;
        private DataTable dt;
        public DataTable Connect()
        {
            con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true");
            try
            {

                con.Open();
                cmd = new SqlCommand("select * from TProducts", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {
                
                return dt;
            }
        }

        public DataTable ReturnSingleContentConnect(string s)
        {
            con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true");
            try
            {

                con.Open();
                cmd = new SqlCommand("select * from TProducts" + " Where ProductName = '" + s + "'", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch
            {

                return dt;
            }
        }
      
        public string Get_ProductID(int i)
        {
            return (dt.Rows[i][0].ToString());
        }
        public string Get_ProductName(int i)
        {
            return (dt.Rows[i][1].ToString());
        }
        public string Get_BuyPrice(int i)
        {
            return (dt.Rows[i][2].ToString());
        }
        public string Get_SellPrice(int i)
        {
            return (dt.Rows[i][3].ToString());
        }
        public string Get_Quantity(int i)
        {
            return (dt.Rows[i][4].ToString());
        }
        public BitmapImage Get_image(int i)
        {
            string name = (string)dt.Rows[i][1];
            byte[] img = (byte[])dt.Rows[i][5];
            MemoryStream ms = new MemoryStream(img);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = null;
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
        public void CloseConnection()
        {
            con.Close();
        }
    }
}
