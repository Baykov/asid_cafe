using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ASUW_Cafe.Properties;
using MySql.Data.MySqlClient;

namespace ASUW_Cafe
{
    /// <summary>
    /// Логика взаимодействия для fotoset.xaml
    /// </summary>
    public partial class fotoset : Window
    {
        public static string idFotoset = "";
        private MainWindow mainWin;
        List<Image> imgsFromsite = new List<Image>();
        List<Image> imgsFromlocal = new List<Image>();
        public static Image img_from = new Image();
        public static Image img_to = new Image();
        public static int int_from;
        public static int int_to;
        public fotoset(string idFoto, MainWindow mainWindow)
        {
            InitializeComponent();
            idFotoset = idFoto;
            mainWin = mainWindow;

            //   Data helper.listObjects;
        //    initImages();
            pbfotosets.Visibility = Visibility.Hidden;
           // pbfotosets.Maximum = 100;
            loadImages(idFotoset);
            loadObjs();
        }

        private void loadObjs()
        {
            listobjs.ItemsSource = helper.listObjects.Tables[0].DefaultView;
            listobjs.DisplayMemberPath = helper.listObjects.Tables[0].Columns[1].Caption;
            listobjs.SelectedValuePath = helper.listObjects.Tables[0].Columns[0].Caption;
        }

        private void loadImages(string idFotoset = "")
        {
            if (idFotoset.Length > 0)
            {
                try
                {
                    var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                    remMysqlConn.Open();
                    var SelectCommand =
                        new MySqlCommand(@"SELECT * FROM asuw_fotoset_imgs WHERE fotoset_id=" + idFotoset + " ORDER BY pos",
                            remMysqlConn); //AND link_published=1
                    var imgReader = SelectCommand.ExecuteReader();
                    while (imgReader.Read())
                    {

                        //string imgpath = @"images/fotosets/" + imgReader[1];
                        //bool imgisprsent = File.Exists(@"http://kafe-taganrog.ru/" + imgpath);
                        //if (!imgisprsent)
                        //{
                        //    return;
                        //}
                        var img = new Image();
                        img.MouseUp += img_MouseUp;
                        img.MouseDown += img_MouseDown;
                        img.QueryContinueDrag += img_QueryContinueDrag;
                        img.Drop += img_Drop;
                        img.BeginInit();
                        img.AllowDrop = true;
                        var marg = new Thickness(5);
                        img.Height = 100;
                        img.Width = 117;
                        img.Margin = marg;
                        img.DataContext = imgReader[1]; // Имя фото
                       // img.Tag = "old";
                        img.Source =
                            (ImageSource)
                                new ImageSourceConverter().ConvertFrom(
                                    new Uri(@"http://kafe-taganrog.ru/images/fotosets/" + imgReader[1] + ""));
                        img.EndInit();
                        imgsFromsite.Add(img);
                        panelimgs.Children.Add(img);
                    }
                    imgReader.Close();

                    var SelectCommand1 =
                        new MySqlCommand(@"SELECT * FROM asuw_fotosets WHERE id=" + idFotoset + "",
                            remMysqlConn); //AND link_published=1
                    var imgReader1 = SelectCommand1.ExecuteReader();
                    while (imgReader1.Read())
                    {
                        title.Text = imgReader1[1].ToString();
                        int id_link = Convert.ToInt32(imgReader1[2]);
                        if (id_link > 0)
                        {
                            listobjs.SelectedValue = id_link;
                        }
                    }
                    imgReader1.Close();
                    remMysqlConn.Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка сети, попытайтесь позже.");
                }
            }
        }

        void img_Drop(object sender, DragEventArgs e)
        {
            img_to = e.Source as Image;
            img_to.DataContext = img_from.DataContext;
            img_to.Tag = "dr";

            int_to = panelimgs.Children.IndexOf(img_to);
          //  string draggedText = (string)e.Data.GetType().GetData(DataFormats.StringFormat);
            DataObject draggedText = (DataObject)e.Data;
            var bms = draggedText.GetData("System.Windows.Media.Imaging.BitmapFrameDecode");
          //  Console.WriteLine(bms);
           //  Image toLabel = e.Source as Image;

            img_from.Source = img_to.Source;
            img_from.DataContext = img_to.DataContext;
            img_from.Tag = "dr";

            //img_to.Source = img_to.Source;
            // panelimgs.Children.Remove(img_from);
            // panelimgs.Children.Insert(int_from,img_to.);
            //panelimgs.Children.Remove(img_to);
            // panelimgs.Children.Insert(int_to,img_from);
            // = from;
            img_to.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(new Uri(bms.ToString()));
            // toLabel.Source = bms;
        }

        void img_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
          //  Image lblFrom = e.Source as Image;
            img_from = e.Source as Image;
            int_from = panelimgs.Children.IndexOf(img_from);
            
            if (!e.KeyStates.HasFlag(DragDropKeyStates.LeftMouseButton))
            {
              //  panelimgs.Children.Remove(lblFrom);
               // lblFrom.Source = null;
            }
        }

        void img_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image lblFrom = e.Source as Image;
            //imgsForDragDrop.Add(lblFrom);
            if (e.LeftButton == MouseButtonState.Pressed)
                DragDrop.DoDragDrop(lblFrom, lblFrom.Source, DragDropEffects.Copy);
        } 

        void img_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            img.BeginInit();
            if (img.Opacity < 1)
            {
                img.Opacity = 1;
                img.Tag = "";
            }
            else
            {
                img.Opacity = 0.5;
                img.Tag = "del";
            }
            img.EndInit();
        }

        private void addFoto_Click(object sender, RoutedEventArgs e)
        {
            var newimage = helper.loadedImage();
            if (newimage != null)
            {
                var img = new Image();
                img.BeginInit();
                var marg = new Thickness(5);
                img.MouseDown += img_MouseDown;
                img.QueryContinueDrag += img_QueryContinueDrag;
                img.AllowDrop = true;
                img.Drop += img_Drop;
                img.Height = 100;
                img.Width = 117;
                img.Margin = marg;
              //  img.Name = (string)(Path.GetFileNameWithoutExtension(newimage.FileName) +"_"+ DateTime.Now.Ticks);
                var newimagename = newimage.FileName;
                img.Tag = "new";
                img.DataContext = Path.GetFileName(newimage.FileName);  // Имя фото
                img.Source = (ImageSource) new ImageSourceConverter().ConvertFrom(new Uri(newimagename));
                img.EndInit();
                imgsFromlocal.Add(img);
                panelimgs.Children.Add(img);
            }
        }

        private void savefotos_Click(object sender, RoutedEventArgs e)
        {
            var titlestr = title.Text;
            string link_id = "";
            if (listobjs.SelectedValue != null)
            {
                link_id = listobjs.SelectedValue.ToString();
            }
            if (titlestr.Length == 0)
            {
                MessageBox.Show("Введите название", "Ошибка");
                return;
            }
            saveFotoSet(titlestr, idFotoset, link_id);
        }

        private void saveFotoSet(string titlestr, string idFotoset, string link_id)
        {
            try
            {
                var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                remMysqlConn.Open();
                if (idFotoset != "")
                {
                    var selectCommand =
                        new MySqlCommand(
                            @"UPDATE asuw_fotosets SET title='" + titlestr + "', link_id = '" + link_id +
                            "'  WHERE id='" + idFotoset +
                            "';  ",
                            remMysqlConn);
                        //AND link_published=1  DELETE FROM asuw_fotoset_imgs WHERE fotoset_id = '" + idFotoset + "';

                    selectCommand.ExecuteNonQuery();
                }
                else
                {
                    var selectCommand =
                        new MySqlCommand(
                            @"INSERT INTO asuw_fotosets (title, link_id) VALUES ('" + titlestr + "', '" + link_id +
                            "' ); SELECT LAST_INSERT_ID() ",
                            remMysqlConn); //AND link_published=1

                    idFotoset = Convert.ToInt32(selectCommand.ExecuteScalar()).ToString();
                }
                remMysqlConn.Close();

                saveFotosOfFotoset(idFotoset);

                Close();

                mainWin.loadfotosets();

            }
            catch (MySqlException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void saveFotosOfFotoset(string idFotoset)
        {
            if (idFotoset != "")
            {
                var ftpClient = new FtpClient(helper.ftpaddr, helper.ftplogin, helper.ftppass);
                var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
                //pbfotosets.Value = 0;
                //pbfotosets.Visibility = Visibility.Visible;
                //pbfotosets.Maximum = panelimgs.Children.Count;
                for (int i = 0; i < panelimgs.Children.Count; i++)
                {
                    var img = panelimgs.Children[i] as  Image;
                    if (imgsFromlocal.Exists(
                        delegate(Image bk)
                        {
                            return bk.Source.Equals(img.Source);
                        }
                        )
                        )
                    {
                        insertImg(img, i);
                    }
                    if (imgsFromsite.Exists(
                        delegate(Image bk)
                        {
                            return bk.Source.Equals(img.Source);
                        }
                        )
                        )
                    {
                        updateImg(img, i);
                    }
                    //if (img.Tag != null)
                    //{
                    //    if ((string) img.Tag == "new")
                    //    {
                    //        string nameofImage = idFotoset + "_" + img.DataContext;
                    //        string imgpath = @"/images/fotosets/" + nameofImage;
                    //        try
                    //        {
                    //            remMysqlConn.Open();
                    //            var selectCommand =
                    //                new MySqlCommand(
                    //                    @" INSERT INTO asuw_fotoset_imgs (title, pos, imgaddr, fotoset_id) VALUES ('"
                    //                    + nameofImage + "', " + i + ", '" + imgpath + "', '" + idFotoset +
                    //                    "') ",
                    //                    remMysqlConn);
                    //            selectCommand.ExecuteNonQuery();
                    //            remMysqlConn.Close();
                    //            bool imgisprsent = File.Exists(Settings.Default.site + imgpath);
                    //            if (!imgisprsent)
                    //            {
                    //                var imgaddr = Regex.Split(img.Source.ToString(), @"///");
                    //                var imgpathnew = imgaddr[1];
                    //                ftpClient.Upload(string.Format("images/fotosets/{0}", nameofImage),
                    //                    File.ReadAllBytes(imgpathnew));
                    //                Console.WriteLine(imgpathnew);
                    //            }
                    //        }
                    //        catch (MySqlException exception)
                    //        {
                    //            MessageBox.Show(exception.Message, "Ошибка сохранения отчета.");
                    //        }


                    //    }
                    //    else if ((string) img.Tag == "dr")
                    //    {
                    //        string nameofImage = idFotoset + "_" + img.DataContext;
                    //        string imgpath = @"/images/fotosets/" + nameofImage;
                    //        try
                    //        {
                    //            remMysqlConn.Open();
                    //            var selectCommand =
                    //                new MySqlCommand(
                    //                    @" REPLACE INTO asuw_fotoset_imgs (title, pos, imgaddr, fotoset_id) VALUES ('"
                    //                    + nameofImage + "', " + i + ", '" + imgpath + "', '" + idFotoset +
                    //                    "')",
                    //                    remMysqlConn);
                    //            selectCommand.ExecuteNonQuery();
                    //            remMysqlConn.Close();
                    //            bool imgisprsent = File.Exists(Settings.Default.site + imgpath);
                    //            if (!imgisprsent)
                    //            {
                    //                var imgaddr = Regex.Split(img.Source.ToString(), @"///");
                    //                var imgpathnew = imgaddr[1];
                    //                ftpClient.Upload(string.Format("images/fotosets/{0}", nameofImage),
                    //                    File.ReadAllBytes(imgpathnew));
                    //                Console.WriteLine(imgpathnew);
                    //            }
                    //        }
                    //        catch (MySqlException exception)
                    //        {
                    //            MessageBox.Show(exception.Message, "Ошибка сохранения отчета.");
                    //        }

                    //    }
                    //}
                    //pbfotosets.Value = i;
                }
                //pbfotosets.Visibility = Visibility.Hidden;

            }
        }

        private void updateImg(Image img, int i)
        {
            var ftpClient = new FtpClient(helper.ftpaddr, helper.ftplogin, helper.ftppass);
            var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
            string nameofImage = img.DataContext.ToString();
            string imgpath = @"/images/fotosets/" + nameofImage;
            try
            {
                remMysqlConn.Open();
                var selectCommand =
                    new MySqlCommand(
                        @" REPLACE INTO asuw_fotoset_imgs (title, pos, imgaddr, fotoset_id) VALUES ('"
                        + nameofImage + "', " + i + ", '" + imgpath + "', '" + idFotoset +
                        "')",
                        remMysqlConn);
                selectCommand.ExecuteNonQuery();
                remMysqlConn.Close();
                bool imgisprsent = File.Exists(Settings.Default.site + imgpath);
                if (!imgisprsent)
                {
                   // var imgaddr = Regex.Split(img.Source.ToString(), @"///");
                    //var imgpathnew = imgaddr[1];
                    ftpClient.Upload(string.Format("images/fotosets/{0}", nameofImage),
                        File.ReadAllBytes(imgpath));
                    Console.WriteLine(imgpath);
                }
            }
            catch (MySqlException exception)
            {
                MessageBox.Show(exception.Message, "Ошибка сохранения отчета.");
            }
        }

        private void insertImg(Image img, int i)
        {
            var ftpClient = new FtpClient(helper.ftpaddr, helper.ftplogin, helper.ftppass);
            var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
            string nameofImage = idFotoset + "_" + img.DataContext;
            string imgpath = @"/images/fotosets/" + nameofImage;
            try
            {
                remMysqlConn.Open();
                var selectCommand =
                    new MySqlCommand(
                        @" INSERT INTO asuw_fotoset_imgs (title, pos, imgaddr, fotoset_id) VALUES ('"
                        + nameofImage + "', " + i + ", '" + imgpath + "', '" + idFotoset +
                        "') ",
                        remMysqlConn);
                selectCommand.ExecuteNonQuery();
                remMysqlConn.Close();
                bool imgisprsent = File.Exists(Settings.Default.site + imgpath);
                if (!imgisprsent)
                {
                    var imgaddr = Regex.Split(img.Source.ToString(), @"///");
                    var imgpathnew = imgaddr[1];
                    ftpClient.Upload(string.Format("images/fotosets/{0}", nameofImage),
                        File.ReadAllBytes(imgpathnew));
                    Console.WriteLine(imgpathnew);
                }
            }
            catch (MySqlException exception)
            {
                MessageBox.Show(exception.Message, "Ошибка сохранения отчета.");
            }
        }

        private void delbtn_Click(object sender, RoutedEventArgs e)
        {
            var ftpClient = new FtpClient(helper.ftpaddr, helper.ftplogin, helper.ftppass);
            var remMysqlConn = new MySqlConnection(helper.ASIDConnectionString);
            remMysqlConn.Open();
            //pbfotosets.Value = 0;
            //pbfotosets.Visibility = Visibility.Visible;
            //pbfotosets.Maximum = panelimgs.Children.Count;
            List<Image> listToDel = new List<Image>();
            for (int i = 0; i < panelimgs.Children.Count; i++)
            {
                var img = panelimgs.Children[i] as Image;
                string nameofImage = img.DataContext.ToString();
                if (img.Opacity < 1)
                {
                    var selectCommand =
                        new MySqlCommand(
                            @" DELETE FROM asuw_fotoset_imgs  WHERE title = '" + nameofImage + "' ",
                            remMysqlConn);
                    try
                    {
                        selectCommand.ExecuteNonQuery();
                        ftpClient.Delete(string.Format("images/fotosets/{0}", nameofImage));
                    }
                    catch (MySqlException exception)
                    {
                        MessageBox.Show(exception.Message, "Ошибка удаления фото.");
                    }
                    listToDel.Add(img);
                }
                //pbfotosets.Value = i;
            }
            foreach (Image image in listToDel)
            {
                panelimgs.Children.Remove(image);
            }
           
            remMysqlConn.Close();
            //pbfotosets.Visibility = Visibility.Hidden;
            MessageBox.Show("Dct выделенные изображения удалены успешно.", "Все замечательно!");
        }
    }
}
