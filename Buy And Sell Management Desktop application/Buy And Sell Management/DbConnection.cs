using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Buy_And_Sell_Management
{
    class DbConnection
    {
        private SqlConnection conn;
        public SqlCommand _cmd;
        private SqlDataAdapter da;
        private DataTable dt;

        public int connect_database(string str)
        {
            conn = new SqlConnection(str);
            try
            {
                conn.Open();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public void sql_query(string query_text)
        {
            _cmd = new SqlCommand(query_text, conn);
        }
        public DataTable QueryEx()
        {
            da = new SqlDataAdapter(_cmd);
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public void NonQueryEx()
        {
            _cmd.ExecuteNonQuery();
        }
        public void close()
        {
            conn.Close();
        }
    }
}
