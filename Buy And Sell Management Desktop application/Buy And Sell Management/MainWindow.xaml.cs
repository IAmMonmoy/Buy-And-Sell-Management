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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
namespace Buy_And_Sell_Management
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbConnection db;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection con;
            con = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true");
            try
            {
                con.Open();
                con.Close();
                LoadImageAndNameButton();
            }
            catch
            {
                create_database_from_script();
                MessageBox.Show("This is first run of The application. CREATING DataBase. Reopen Application after closing");
                Environment.Exit(0);
            }
            
        }

        public class data1 //for inserting into datagrid
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int Quantity { get; set; }
            public int Total { get; set; }
        }
        public class data2 // for inserting into transaction database
        {
            public string ProductName { get; set; }
            public int BuyPrice { get; set; }
            public int SellPrice { get; set; }
            public int Quantity { get; set; }
            public int Total { get; set; }
            public int Revenue { get; set; }
            public string Date { get; set; }
        }
        List<data1> xy = new List<data1>();
        List<data2> xyz = new List<data2>();
        int sum = 0,TranSactionId;

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            wrappanel.Children.Clear();
            LoadImageAndNameButton();
        }

        private void create_database_from_script()
        {
            List<string> cmds = new List<string>();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\script.sql"))
            {
                //Get commands from script
                TextReader tr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\script.sql");
                string line = "";
                string cmd = "";
                while ((line = tr.ReadLine()) != null)
                {
                    if (line.Trim().ToUpper() == "GO")
                    {
                        cmds.Add(cmd);
                        cmd = "";
                    }
                    else cmd += line + "\r\n";
                }
                if (cmd.Length > 0)
                    cmds.Add(cmd);
                tr.Close();
            }

            if (cmds.Count > 0)
            {
                //run commands
                SqlCommand command = new SqlCommand();
                command.Connection = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog = Master; Integrated Security = true");
                command.CommandType = System.Data.CommandType.Text;
                command.Connection.Open();
                for (int i = 0; i < cmds.Count; i++)
                {
                    command.CommandText = cmds[i];
                    command.ExecuteNonQuery();
                }

            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Product_Data pd = new Product_Data();
            pd.ShowDialog();
        }

        private void LoadImageAndNameButton()
        {
            TProductsDB GetData = new TProductsDB(); //Methods for filling datatable and get all the data from Tproducts Table in database
            DataTable dt = GetData.Connect();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][5] != DBNull.Value) //Button with image and button must have name
                    {
                        Button newbtn = new Button();
                        StackPanel stk = new StackPanel();
                        Image img1 = new Image();

                        img1.Source = GetData.Get_image(i);
                        img1.Stretch = Stretch.Fill;

                        Label labelx = new Label();

                        labelx.FontSize = 14;
                        labelx.Content = GetData.Get_ProductName(i);

                        stk.Children.Add(labelx);
                        stk.Children.Add(img1);


                        newbtn.Content = stk;

                        newbtn.Height = 100;
                        newbtn.Width = 100;
                        newbtn.Click += (sender, EventArgs) => { newbtn_Click(sender, EventArgs, labelx.Content.ToString()); };

                        wrappanel.Children.Add(newbtn);
                    }
                    else //Button without Image
                    {
                        Button newbtn = new Button();
                        newbtn.Height = 100;
                        newbtn.Width = 100;

                        Label labelx = new Label();
                        labelx.FontSize = 22;
                        labelx.Content = GetData.Get_ProductName(i);

                        newbtn.Content = labelx;


                        newbtn.Click += (sender, EventArgs) => { newbtn_Click(sender, EventArgs, labelx.Content.ToString()); };
                        wrappanel.Children.Add(newbtn);
                    }
                }
            }
           

            GetData.CloseConnection();

            //get the transactionid from database
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM TransactionId", con);
            SqlDataAdapter da;
            DataTable dt1;
            da = new SqlDataAdapter(cmd1);
            dt1 = new DataTable();
            da.Fill(dt1);
            con.Close();
            TranSactionId = int.Parse(dt1.Rows[0][0].ToString());
            sale_textbox.Text = TranSactionId.ToString();
            Date_textbox.Text = DateTime.Now.ToString();
        }

        /*private void LoadIndividualImageAndNameButton(object sender,string s)
        {
            TProductsDB GetData = new TProductsDB(); //Methods for filling datatable and get all the data from Tproducts Table in database
            DataTable dt = GetData.ReturnSingleContentConnect(s);
            Button button = new Button();
            button = (Button)sender;
         
            if (dt.Rows[0][5] != DBNull.Value) //Button with image and button must have name
            {
                StackPanel stk = new StackPanel();
                Image img1 = new Image();

                //img1.Source = GetData.Get_image(0);
                //img1.Stretch = Stretch.Fill;

                Label labelx = new Label();

                labelx.FontSize = 14;
                labelx.Content = GetData.Get_ProductName(0);

                stk.Children.Add(labelx);
                stk.Children.Add(img1);


                button.Content = stk;

            }
            else //Button without Image
            {
            
                Label labelx = new Label();
                labelx.FontSize = 22;
                labelx.Content = GetData.Get_ProductName(0);

                button.Content = labelx;

            }

            GetData.CloseConnection();
        }*/
       
        //change the button style for taking quantity
        private void newbtn_Click(object sender, EventArgs e,string s)
        {
            Button button = new Button();
            button = (Button)(sender);
            StackPanel stk = new StackPanel();
            TextBox txt = new TextBox();
            txt.Height = 30;
            txt.Width = 80;
            
            Button btn = new Button();
            btn.Content = "Enter";
            btn.Height = 20;
            btn.Width = 80;

            Button btn1 = new Button();
            btn1.Content = "Cancel";
            btn1.Height = 20;
            btn1.Width = 80;

            stk.Children.Add(txt);
            stk.Children.Add(btn);
            stk.Children.Add(btn1);
            button.Content = stk;

            btn.Click += (sender1, EventArgs) => { btn_Click(sender1, EventArgs, s, txt.Text,ref sender); };
            btn1.Click += (sender1, EventArgs) => { btn1_Click(sender1, EventArgs); };
          
        }

        //Add to the datagrid and check stock and minus from stock
        private void btn_Click(object sender1, EventArgs e, string s, string q,ref object sender)
        {
           // LoadIndividualImageAndNameButton(sender,name);
             wrappanel.Children.Clear();
             LoadImageAndNameButton();

              string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
              SqlConnection con = new SqlConnection(conn);
              con.Open();
              SqlCommand cmd1 = new SqlCommand("SELECT * FROM TProducts WHERE ProductName = '" + s + "'", con);
              SqlDataAdapter da;
              DataTable dt;
              da = new SqlDataAdapter(cmd1);
              dt = new DataTable();
              da.Fill(dt);
              con.Close();
           
              //add -quantity stock
              if (q == string.Empty)
                  q = "1";
              int quantity = int.Parse(q);
              Stock stock = new Stock();
              if (stock.returnStock(s) < quantity)
              {
                  MessageBox.Show("Not Enough Product Is In Stock");
                  return;
              }
              string tmp = dt.Rows[0][3].ToString();
              int price = int.Parse(tmp);
              sum += (price * quantity);

              xy.Add(new data1() { Name = s, Price = price,Quantity=quantity,Total=price*quantity });
              xyz.Add(new data2() { ProductName = s, BuyPrice = int.Parse(dt.Rows[0][2].ToString()), SellPrice = int.Parse(dt.Rows[0][3].ToString()), Quantity = quantity, Total = price * quantity, Revenue = (price * quantity) - (quantity * int.Parse(dt.Rows[0][2].ToString())), Date = DateTime.Now.ToString("MM/dd/yyyy") });

              showdata.ItemsSource = null;
              showdata.ItemsSource = xy;
              stock.DName = s;
              stock.DQuantity = stock.returnStock(s) - quantity;
              stock.UpdateStock();

        }

        private void btn1_Click(object sender1, EventArgs e)
        {
            wrappanel.Children.Clear();
            LoadImageAndNameButton();
        }


        
        private void EditButtonClick(object sender, EventArgs e)
        {
            EditItem edit = new EditItem();
            edit.ShowDialog();
        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            EditItem edit = new EditItem();
            edit.ShowDialog();
        }
        private void AddStockButtonClick(object sender, EventArgs e)
        {
            UpdateStock update = new UpdateStock();
            update.ShowDialog();
        }
        private void ShowStockButtonClick(object sender, EventArgs e)
        {
            ShowStock show = new ShowStock();
            show.ShowDialog();
        }
       

        private void total_button_Click_1(object sender, RoutedEventArgs e)
        {
            total_label.Content = sum.ToString();

            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                for (int i = 0; i < xyz.Count; i++)
                {

                    SqlCommand cmd = new SqlCommand("sp_insert_transaction", con); //stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TransactionId", SqlDbType.Int).Value = TranSactionId;
                    cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = xyz[i].ProductName;
                    cmd.Parameters.Add("@BuyPrice", SqlDbType.Int).Value = xyz[i].BuyPrice;
                    cmd.Parameters.Add("@SellPrice", SqlDbType.Int).Value = xyz[i].SellPrice;
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = xyz[i].Quantity;
                    cmd.Parameters.Add("@Total", SqlDbType.Int).Value = xyz[i].Total;
                    cmd.Parameters.Add("@Revenue", SqlDbType.Int).Value = xyz[i].Revenue;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar).Value = xyz[i].Date.ToString();
                    cmd.ExecuteNonQuery();
                }
                xyz.Clear();
                con.Close();
            }


            TranSactionId++;
            sale_textbox.Text = TranSactionId.ToString();
            Date_textbox.Text = DateTime.Now.ToString();
            sum = 0;
            xy.Clear();
            showdata.ItemsSource = null;

            con.Open();
            SqlCommand cmd1 = new SqlCommand("DELETE FROM TransactionId", con);
            SqlCommand cmd2 = new SqlCommand("sp_insert_transaction_id", con); //stored procedure
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.Add("@TransactionId", SqlDbType.Int).Value = TranSactionId;
            cmd1.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            con.Close();
        }

        

        private void remove_button_Click(object sender, RoutedEventArgs e)
        {
            if (showdata.SelectedItems.Count > 0)
            {
                string name = ((data1)showdata.SelectedItem).Name;
                int price = ((data1)showdata.SelectedItem).Price;
                var item = xy.FindIndex(x => x.Name == name);
                var item1 = xyz.FindIndex(x => x.ProductName == name);
                xy.RemoveAt(item);
                xyz.RemoveAt(item1);

                showdata.ItemsSource = null;
                showdata.ItemsSource = xy;
                sum -= price;
                total_label.Content = sum.ToString();
                //add +1 to stock
                Stock stock = new Stock();
                stock.DName = name;
                stock.DQuantity = stock.returnStock(name)+1;
                stock.UpdateStock();
            }
            
        }

        private void recipt_button_Click(object sender, RoutedEventArgs e)
        {
            total_label.Content = "0";
            Date_textbox.Text = DateTime.Now.ToString();
            sum = 0;
            xy.Clear();
            showdata.ItemsSource = null;
        }

        //save the latest transactionid to the database
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd1 = new SqlCommand("DELETE FROM TransactionId", con);
            SqlCommand cmd = new SqlCommand("sp_insert_transaction_id", con); //stored procedure
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TransactionId", SqlDbType.Int).Value = TranSactionId;
            cmd1.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TotalSoldToday(object sender, EventArgs e)
        {
            string conn = @"Data Source=.\SQLEXPRESS; Initial Catalog = BuyAndSell; Integrated Security = true";
            SqlConnection con = new SqlConnection(conn);
            con.Open();
            SqlCommand cmd1 = new SqlCommand("SELECT Total FROM Transactions WHERE Date = '" + DateTime.Now.ToShortDateString() + "'", con);
            SqlDataAdapter da;
            DataTable dt1;
            da = new SqlDataAdapter(cmd1);
            dt1 = new DataTable();
            da.Fill(dt1);
            con.Close();

            int sum = 0;

            for (int i = 0; i < dt1.Rows.Count; i++)
                sum += int.Parse(dt1.Rows[i][0].ToString());

            MessageBox.Show("TOTAL SOLD " + sum + " Today");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
