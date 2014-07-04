using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Path = System.IO.Path;

namespace ASUW_Cafe
{
    /// <summary>
    /// Логика взаимодействия для fotoset.xaml
    /// </summary>
    public partial class fotoset : Window
    {
        public static string idFotoset = "";
        public fotoset(string idFoto)
        {
            InitializeComponent();
            idFotoset = idFoto;
        //   Data helper.listObjects;
        //    initImages();
            loadImages(idFotoset);
            loadObjs();
        }

        private void loadObjs()
        {
            listobjs.ItemsSource = helper.listObjects.Tables[0].DefaultView;
            listobjs.DisplayMemberPath = helper.listObjects.Tables[0].Columns[1].Caption;
            listobjs.SelectedValuePath = helper.listObjects.Tables[0].Columns[0].Caption;
         //   listobjs.ItemsSource = helper.listObjects.Tables[0].DefaultView;
           // listobjs.
        }

        private void loadImages(string idFotoset = "")
        {
            if (idFotoset.Length > 0)
            {
                try
                {
                    var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                    remMysqlConn.Open();
                    MySqlCommand SelectCommand =
                        new MySqlCommand(@"SELECT * FROM asuw_fotoset_imgs WHERE fotoset_id=" + idFotoset + "",
                            remMysqlConn); //AND link_published=1
                    MySqlDataReader imgReader = SelectCommand.ExecuteReader();
                    while (imgReader.Read())
                    {
                        Image img = new Image();
                        img.BeginInit();
                        var marg = new Thickness(15);
                        img.Height = 100;
                        img.Width = 117;
                        img.Margin = marg;
                        img.Source =
                            (ImageSource)
                                new ImageSourceConverter().ConvertFrom(
                                    new Uri(@"http://kafe-taganrog.ru/images/fotosets/" + imgReader[3] + ""));
                        panelimgs.Children.Add(img);
                    }
                    imgReader.Close();
                    remMysqlConn.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка сети, попытайтесь позже.");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog newimage = helper.loadedImage();
            if (newimage != null)
            {
                Image img = new Image();
                img.BeginInit();
                var marg = new Thickness(15);
                img.Height = 100;
                img.Width = 117;
                img.Margin = marg;
               // img.Name = Path.GetFileNameWithoutExtension(newimage.FileName);
                var newimagename = newimage.FileName;
                img.Source = (ImageSource) new ImageSourceConverter().ConvertFrom(new Uri(newimagename));
                img.EndInit();
                panelimgs.Children.Add(img);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var titlestr = title.Text;
            if (titlestr == null) {MessageBox.Show("Введите название","Ошибка");}
            saveFotoSet(titlestr, idFotoset);
        }

        private void saveFotoSet(string titlestr, string idFotoset)
        {
            try
            {
                var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                remMysqlConn.Open();
                if (idFotoset != "")
                {
                    var selectCommand =
                        new MySqlCommand(
                            @"UPDATE asuw_fotosets SET title='" + titlestr + "' WHERE id='" + idFotoset + "'",
                            remMysqlConn); //AND link_published=1

                    selectCommand.ExecuteNonQuery();
                }
                else
                {
                    var selectCommand =
                        new MySqlCommand(
                            @"INSERT INTO asuw_fotosets (title, link_id) VALUES ('" + titlestr + "', ) ",
                            remMysqlConn); //AND link_published=1
                    

                }
            } catch(MySqlException exception){Console.WriteLine(exception.Message);}

        }
    }
}
