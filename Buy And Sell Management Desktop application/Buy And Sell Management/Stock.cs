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
    class Stock
    {
        private int Quantity;
        private string Name;
        public int DQuantity
        {
            get
            {
                return Quantity;
            }
            set
            {
                Quantity = value;
            }
        }
        public string DName
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }
        public bool CheckStock(string name)
        {
            return true;
        }
        public void UpdateStock()
        {
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd1 = new SqlCommand("UPDATE Stock SET Stock = " + Quantity + " Where ProductName = '" + Name + "'", con);
            try
            {
                cmd1.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Can Not update");
            }
            con.Close();
        }
        public void AddStock()
        {
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand("sp_insert_stock", con); //stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProductName", SqlDbType.NChar).Value = Name;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;


                try
                {
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    ex.ToString(); //dummy for removing warning
                   
                        SqlCommand cmd1 = new SqlCommand("DELETE FROM Stock WHERE ProductName = '" + Name + "'", con);
                        try
                        {
                            cmd1.ExecuteNonQuery();
                            //first delete the data then recursive call to insert updated one
                            con.Close();
                            AddStock();
                            con.Close();
                        }
                        catch (SqlException x)
                        {
                            MessageBox.Show(x.ToString());
                        }
                    }
 
                }
                else MessageBox.Show("DataBase Connection Problem");
        }

        public DataTable showstock()
        {
            SqlConnection con;
            SqlDataAdapter da;
            SqlCommand cmd;
            DataTable dt;
            con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true");
            con.Open();
            cmd = new SqlCommand("select * from Stock", con);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public int returnStock(string s)
        {
            SqlConnection con;
            con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true");
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT Stock FROM Stock WHERE ProductName = '" + s + "'", con);
            SqlDataAdapter da;
            DataTable dt;
            da = new SqlDataAdapter(cmd1);
            dt = new DataTable();
            da.Fill(dt);
            con.Close();
            string tmp = dt.Rows[0][0].ToString();
            int price = int.Parse(tmp);
            return price;
        }
    }
}