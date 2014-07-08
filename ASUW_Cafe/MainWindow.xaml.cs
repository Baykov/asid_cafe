using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ASUW_Cafe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string idCafe = "";
        static public MySqlDataAdapter listdataAdapter = new MySqlDataAdapter();
        static public DataSet listdataSet = new DataSet();

        public static string idFotoset = "";
        static public MySqlDataAdapter fotosetsdataAdapter = new MySqlDataAdapter();
        static public DataSet fotosetsdataSet = new DataSet();

        public static string idNews = "";
        static public MySqlDataAdapter newsdataAdapter = new MySqlDataAdapter();
        static public DataSet newsdataSet = new DataSet();
        public MainWindow(string user)
        {
            InitializeComponent();
            loadStats();
            loadobjs();
            loadfotosets();
            loadnews();
        }

        public void loadobjs()
        {
            try
            {
                var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                remMysqlConn.Open();

                listdataAdapter.SelectCommand = new MySqlCommand(@"SELECT link_id, link_name FROM  qzgoj_mt_links", remMysqlConn);
                listdataAdapter.Fill(listdataSet);
                gridobjects.ItemsSource = listdataSet.Tables[0].DefaultView;
                remMysqlConn.Close();
                helper.listObjects = listdataSet.Copy();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+"Ошибка сети, попытайтесь позже.");
                Close();
            }
        }

        public void loadfotosets()
        {
            try
            {
                fotosetsdataSet.Clear();
                var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                remMysqlConn.Open();
                fotosetsdataAdapter.SelectCommand = new MySqlCommand(
                    @"SELECT *  FROM asuw_fotosets ", remMysqlConn);
                fotosetsdataAdapter.Fill(fotosetsdataSet);
                gridofotosets.ItemsSource = fotosetsdataSet.Tables[0].DefaultView;
                remMysqlConn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Close();
            }
        }

        private void loadnews()
        {
            try
            {
                var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                remMysqlConn.Open();
                newsdataAdapter.SelectCommand = new MySqlCommand(
                    @"SELECT id, title  FROM qzgoj_content WHERE catid = 10 ", remMysqlConn);
                newsdataAdapter.Fill(newsdataSet);
                gridnews.ItemsSource = newsdataSet.Tables[0].DefaultView;
                remMysqlConn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message+"Ошибка сети, попытайтесь позже.");
                Close();
            }
        }

        private void gridobjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var rowView = gridobjects.SelectedValue as DataRowView;
            try 
            {
                idCafe = rowView[0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Ошибка программы, попытайтесь позже.");
            }
           // MessageBox.Show(rowView[0].ToString());            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cafe = new cafe("");
            cafe.Show();
            cafe.Activate();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var cafe = new cafe(idCafe);
            cafe.Show();
            cafe.Activate();

        }

        private void serch_btn_Click(object sender, RoutedEventArgs e)
        {
            var serchstr = "";
            var serchrows = new DataRow[] { };
            serchstr = serch_input.Text;
            try
            {
                var dv = (DataView)gridobjects.ItemsSource;
                dv.RowFilter = "link_name like '%" + serchstr + "%'";
                gridobjects.ItemsSource = dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Ошибка программы, попытайтесь позже.");
            }
        }
        private void loadStats()
        {
            try
            {
                var dataSet = new DataSet();
                dataSet.ReadXml(@"http://kafe-taganrog.ru/asuw_st/asidupd.xml"); 
                stat_grid.ItemsSource = dataSet.Tables[0].DefaultView; 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "Ошибка сети, попытайтесь позже.");
            }
        }

        private void savestatbut_Click(object sender, RoutedEventArgs e)
        {
            var ds = new DataSet();
            var tabnew = new DataTable();
            try
            {
                var ftpClient = new FtpClient(helper.ftpaddr, helper.ftplogin, helper.ftppass);
                var dv = (DataView)stat_grid.ItemsSource;
                tabnew = dv.Table.Copy();
                ds.Tables.Add(tabnew);
                var newXml = new XmlTextWriter("asidupd.xml", Encoding.UTF8);
                ds.WriteXml(newXml);
                newXml.Close();

                ftpClient.Upload("/asuw_st/asidupd.xml", File.ReadAllBytes("asidupd.xml"));
                Console.WriteLine();
                loadStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Ошибка сети, попытайтесь позже.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridobjects.Columns[0].Header = "Id";
            gridobjects.Columns[1].Header = "Название";
            gridofotosets.Columns[0].Header = "Id";
            gridofotosets.Columns[1].Header = "Название";
            gridnews.Columns[0].Header = "Id";
            gridnews.Columns[1].Header = "Название";

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var fotoset = new fotoset("", this);
            fotoset.Show();
            fotoset.Activate();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var fotoset = new fotoset(idFotoset, this);
            fotoset.Show();
            fotoset.Activate();

        }

        private void serch_btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            var serchstr = "";
            serchstr = serch_input_Copy.Text;
            try
            {
                var dv = (DataView)gridofotosets.ItemsSource;
                dv.RowFilter = "title like '%" + serchstr + "%'";
                gridofotosets.ItemsSource = dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Ошибка программы, попытайтесь позже.");
            }

        }

        private void gridofotosets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridofotosets.SelectedValue != null)
            {
                var rowView = gridofotosets.SelectedValue as DataRowView;
                try
                {
                    idFotoset = rowView[0].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Ошибка программы, попытайтесь позже.");
                }
            }
        }

        private void addnews_Click(object sender, RoutedEventArgs e)
        {
            var news = new news("");
            news.Show();
            news.Activate();

        }

        private void editnews_Click(object sender, RoutedEventArgs e)
        {

            var news = new news(idNews);
            news.Show();
            news.Activate();
        }

        private void serchnews_btn_Click(object sender, RoutedEventArgs e)
        {
            var serchstr = "";
            var serchrows = new DataRow[] { };
            serchstr = serch_news.Text;
            try
            {
                var dv = (DataView)gridnews.ItemsSource;
                dv.RowFilter = "title like '%" + serchstr + "%'";
                gridnews.ItemsSource = dv;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Ошибка программы, попытайтесь позже.");
            }
        }

        private void gridnews_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var rowView = gridnews.SelectedValue as DataRowView;
            try
            {
                idNews = rowView[0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Ошибка программы, попытайтесь позже.");
            }
        }
    }
}
