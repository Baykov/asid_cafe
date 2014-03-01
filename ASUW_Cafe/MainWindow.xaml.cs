using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ASUW_Cafe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string idCafe = "";

        public static MySqlConnection remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
        static public MySqlDataAdapter listdataAdapter = new MySqlDataAdapter();
        static public DataSet listdataSet = new DataSet();
        public MainWindow()
        {
            InitializeComponent();
            loadobjs();
        }

        public void loadobjs()
        {
            InitializeComponent();
            Guid block = helper.checkBlock();
            try
            {
                if (remMysqlConn.State == System.Data.ConnectionState.Closed)
                {
                    remMysqlConn.Open();
                }
                listdataAdapter.SelectCommand = new MySqlCommand(@"SELECT  link_id, link_name FROM qzgoj_mt_links ", remMysqlConn);
                listdataAdapter.Fill(listdataSet);
                gridobjects.ItemsSource = listdataSet.Tables[0].DefaultView;
                if (remMysqlConn.State == System.Data.ConnectionState.Open)
                {
                    remMysqlConn.Close();
                }
                //gridobjects.Columns[0].Header = "Id";WHERE block='" + block + "'
                //  gridobjects.Columns[1].Header = "Название";
            }
            catch
            {
                MessageBox.Show("Ошибка сети, попытайтесь позже.");
                this.Close();
            }
           // loadStats();
        }

        private void gridobjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView rowView = gridobjects.SelectedValue as DataRowView;
            try 
            {
                idCafe = rowView[0].ToString();
            }
            catch
            {
                MessageBox.Show("Ошибка программы, попытайтесь позже.");
            }
           // MessageBox.Show(rowView[0].ToString());            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cafe cafe = new cafe("");
            cafe.Show();
            cafe.Activate();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            cafe cafe = new cafe(idCafe);
            cafe.Show();
            cafe.Activate();

        }

        private void serch_btn_Click(object sender, RoutedEventArgs e)
        {
            string serchstr = "";
            DataRow[] serchrows = new DataRow[] { };
            serchstr = serch_input.Text;
            try
            {
                DataView dv = (DataView)gridobjects.ItemsSource;
                dv.RowFilter = "link_name like '%" + serchstr + "%'";
                gridobjects.ItemsSource = dv;
            }
            catch
            {
                MessageBox.Show("Ошибка программы, попытайтесь позже.");
            }
        }
        private void loadStats()
        {
            try
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(@"http://kafe-taganrog.ru/asuw_st/asidupd.xml"); 
                stat_grid.ItemsSource = dataSet.Tables[0].DefaultView; 
            }
            catch
            {
                MessageBox.Show("Ошибка сети, попытайтесь позже.");
            }
        }

        private void savestatbut_Click(object sender, RoutedEventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable tabnew = new DataTable();
            try
            {
                var ftpClient = new FtpClient(helper.ftpaddr, helper.ftplogin, helper.ftppass);
                DataView dv = (DataView)stat_grid.ItemsSource;
                tabnew = dv.Table.Copy();
                ds.Tables.Add(tabnew);
                XmlTextWriter newXml = new XmlTextWriter("asidupd.xml", Encoding.UTF8);
                ds.WriteXml(newXml);
                newXml.Close();

                ftpClient.Upload("/asuw_st/asidupd.xml", File.ReadAllBytes("asidupd.xml"));
                Console.WriteLine();
                loadStats();
            }
            catch
            {
                MessageBox.Show("Ошибка сети, попытайтесь позже.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridobjects.Columns[0].Header = "Id";
            gridobjects.Columns[1].Header = "Название";

        }


    }
}
