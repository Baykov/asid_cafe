using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace ASUW_Cafe
{
    class helper
    {
        public static string url = ASUW_Cafe.Properties.Settings.Default.site;
        public static string loginbd = Properties.Settings.Default.loginbds;//"bladge_kafeyaro";
        public static string passbd = Properties.Settings.Default.passbds;//"s98Z2Gpq";
        public static string addrbd = Properties.Settings.Default.addrbds;//91.106.201.126"91.106.201.126";
        public static string namebd = Properties.Settings.Default.namebds;//"bladge_kafeyaro";
        public static string ASIDConnectionString = @"Server=" + addrbd + ";Database=" + namebd + ";Uid=" + loginbd + ";Pwd=" + passbd + ";CharSet=utf8;Allow Zero Datetime=true";
        public static MySqlConnection remMysqlConn = new MySqlConnection(ASIDConnectionString);

        public static string[] coords = new string[]{ "", "" };
       // public static string url = ASUW_Cafe.Properties.Settings.Default.site;
        public static OpenFileDialog loadImage = new  Microsoft.Win32.OpenFileDialog();
        public static Dictionary<string, string> words = new Dictionary<string, string>();

        public static string ftpaddr = ASUW_Cafe.Properties.Settings.Default.ftpaddr;
        public static string ftplogin = ASUW_Cafe.Properties.Settings.Default.ftplogin;
        public static string ftppass = ASUW_Cafe.Properties.Settings.Default.ftppass;
        public static string ftpdir = ASUW_Cafe.Properties.Settings.Default.ftpdir;

       // public static Image MemForImage;
        
        public static string username = "Гость";
        public static string GET(string Url, string Data)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url + Data);
            // req.Proxy = new WebProxy("195.200.245.49");
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        public static Guid checkBlock()
        {
            Guid old = ASUW_Cafe.Properties.Settings.Default.Block;
            Console.WriteLine(old);
            Guid Block = Guid.NewGuid();
            // bool emptyGuid = (,);
            if (old == Guid.Empty)
            {
                try
                {
                    ASUW_Cafe.Properties.Settings.Default.Block = Block;
                    ASUW_Cafe.Properties.Settings.Default.Save();
                    ASUW_Cafe.Properties.Settings.Default.Reload();
                    return Block;
                }
                catch
                {
                    return Block;
                }
            }
            else
            {
                return old;
            }

        }

        public static Dictionary<string, string> ParseJson(string res)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Newtonsoft.Json.Linq.JArray result = new Newtonsoft.Json.Linq.JArray();
            try
            {
                result = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(res);

            }
            catch
            {
                MessageBox.Show("Ошибка сети, попытайтесь позже.");
                return null;
            }

            foreach (Newtonsoft.Json.Linq.JObject value in result)
            {
                dict.Add(value.Value<string>("userid").ToString().Trim(), value.Value<string>("login").ToString().Trim());
            }
            return dict;
        }

        public static OpenFileDialog loadedImage()
        {
            loadImage.Filter = "Графические файлы|*.jpg;*.jpeg;*.gif;*.png;*.bmp|Все файлы|*.*";
            Nullable<bool> result = loadImage.ShowDialog();

            if (result == true)
            {
                return loadImage;
            }
            else
            {
                return null;
            }
        }
        public static void InitDictionary()
        {
            words.Add("а", "a");
            words.Add("б", "b");
            words.Add("в", "v");
            words.Add("г", "g");
            words.Add("д", "d");
            words.Add("е", "e");
            words.Add("ё", "yo");
            words.Add("ж", "zh");
            words.Add("з", "z");
            words.Add("и", "i");
            words.Add("й", "j");
            words.Add("к", "k");
            words.Add("л", "l");
            words.Add("м", "m");
            words.Add("н", "n");
            words.Add("о", "o");
            words.Add("п", "p");
            words.Add("р", "r");
            words.Add("с", "s");
            words.Add("т", "t");
            words.Add("у", "u");
            words.Add("ф", "f");
            words.Add("х", "h");
            words.Add("ц", "c");
            words.Add("ч", "ch");
            words.Add("ш", "sh");
            words.Add("щ", "sch");
            words.Add("ъ", "j");
            words.Add("ы", "i");
            words.Add("ь", "j");
            words.Add("э", "e");
            words.Add("ю", "yu");
            words.Add("я", "ya");
            words.Add("А", "a");
            words.Add("Б", "b");
            words.Add("В", "v");
            words.Add("Г", "g");
            words.Add("Д", "d");
            words.Add("Е", "e");
            words.Add("Ё", "yo");
            words.Add("Ж", "zh");
            words.Add("З", "z");
            words.Add("И", "i");
            words.Add("Й", "j");
            words.Add("К", "k");
            words.Add("Л", "l");
            words.Add("М", "m");
            words.Add("Н", "n");
            words.Add("О", "o");
            words.Add("П", "p");
            words.Add("Р", "r");
            words.Add("С", "s");
            words.Add("Т", "t");
            words.Add("У", "u");
            words.Add("Ф", "f");
            words.Add("Х", "h");
            words.Add("Ц", "c");
            words.Add("Ч", "ch");
            words.Add("Ш", "sh");
            words.Add("Щ", "sch");
            words.Add("Ъ", "j");
            words.Add("Ы", "i");
            words.Add("Ь", "j");
            words.Add("Э", "e");
            words.Add("Ю", "yu");
            words.Add("Я", "ya");
            words.Add(" ", "-");
            words.Add("_", "-");
            words.Add(",", "-");
            words.Add(".", "-");
            words.Add(";", "-");
            words.Add("#", "-");
            words.Add("»", "-");
            words.Add("«", "-");
            words.Add("%", "-");
            words.Add("@", "-");
            words.Add("!", "-");
            words.Add("\"", "-");
            words.Add("=", "-");

        }

        public static string ToTranslit(string str)
        {
            string result = str.Trim();
            foreach (KeyValuePair<string, string> pair in words)
            {
                result = result.Replace(pair.Key, pair.Value);
            }
            return result.Trim();
        }


        internal static void loggingStart(string idCafe)
        {
            Guid block = checkBlock();
            string res = "";
            try
            {
               res = GET(url, "/index.php?option=com_mtree&task=start&user=" + block.ToString() + "&name=" + username + "&id=" + idCafe + "");
               if (res != "1")
               {
                  WriteToFile("Ошибка при логе старта юзера " + block.ToString() + " и объекта " + idCafe + "", "log.txt");
               }
            }
            catch
            {
                WriteToFile("Ошибка при логе старта юзера " + block.ToString() + " и объекта " + idCafe + "", "log.txt");
            }
         //   throw new NotImplementedException();
        }

        internal static void loggingFinish(string idCafe)
        {
            Guid block = checkBlock();
            string res = "";
            try
            {
               res =  GET(url, "/index.php?option=com_mtree&task=finish&user=" + block.ToString() + "&name=" + username + "&id=" + idCafe + "");
               if (res != "1")
               {
                   WriteToFile("Ошибка при логе финиша юзера " + block.ToString() + " и объекта " + idCafe + "", "log.txt");
               }
            }
            catch
            {
                WriteToFile("Ошибка при логе финиша юзера "+block.ToString()+" и объекта " + idCafe + "" , "log.txt" );
            }
            //   throw new NotImplementedException();
        }
        public static void WriteToFile(string sNote, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString() + " " + sNote);
                }
            }
            catch (Exception) { }
        }
        public static string[] ParseGeoCoords(string address)
        {
            try
            {
                string ResultSearchObject = helper.GET("http://geocode-maps.yandex.ru/1.x/?geocode=", address);
                XDocument doc = XDocument.Parse(ResultSearchObject);
                foreach (XElement el in doc.Root.Elements())
                {
                    if (coords[1].Length == 0)
                    {
                        if (el.Name != "pos")
                        {
                            searchEls(el);
                           // Console.WriteLine(el.Name + "===" + el.Value);
                        }
                    }
                    else
                    {
                        return coords;
                    }
                } 
                return coords;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return new[] { "", "" };
            }
        }

        public static void searchEls(XElement el)
        {
            if (el.HasElements)
            {
                foreach (XElement element in el.Elements())
                {
                    if (coords[1].Length == 0)
                    {
                        if (element.Name != "{http://www.opengis.net/gml}pos")
                        {
                            searchEls(element);
                            //Console.WriteLine(element.Name + "===" + element.Value);
                        }
                        else
                        {
                            coords = element.Value.Split(' ');
                        }
                    }
                }
            }
        }

        private static string _description = string.Empty;
        private static int _percentagecompletion = 0;
        public static String Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
               // NotifyPropertyChanged("Description");
            }
        }
        public static int PercentageCompletion
        {
            get { return _percentagecompletion; }
            set
            {
                if (value == _percentagecompletion) return;
                _percentagecompletion = value;
               // NotifyPropertyChanged("PercentageCompletion");
            }
        }

        public static void ScaleByWidthAndHeight(System.Drawing.Image oImg, int maxWidth, int maxHeight, int resolutionDPI, string text)
        {
            var originalBitmap = new Bitmap(oImg);
            double ratioWidthToHeight = originalBitmap.Width / (double)originalBitmap.Height;
            int newHeight;
            int newWidth;
            if (ratioWidthToHeight > 1)
            {
                newWidth = maxWidth;
                newHeight = (int)(maxHeight / ratioWidthToHeight);
            }
            else
            {
                newHeight = maxHeight;
                newWidth = (int)(maxWidth * ratioWidthToHeight);
            }
            Bitmap newBitmap = new Bitmap(newWidth, newHeight);
            Graphics graphic = Graphics.FromImage(newBitmap);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(originalBitmap, 0, 0, newBitmap.Width, newBitmap.Height);

            newBitmap.SetResolution(resolutionDPI, resolutionDPI);
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
          //  return newBitmap;
            newBitmap.Save(text, format);
        }
    
        public  static System.Drawing.Image DrawImageFromContrImage(System.Windows.Controls.Image img){
           MemoryStream ms = new MemoryStream();
           System.Windows.Media.Imaging.BmpBitmapEncoder bbe = new BmpBitmapEncoder();
           bbe.Frames.Add(BitmapFrame.Create(new Uri(img.Source.ToString(), UriKind.RelativeOrAbsolute)));

           bbe.Save(ms);
           System.Drawing.Image img2 = System.Drawing.Image.FromStream(ms);
           return img2;
       }
    }

    public class Rewiev
    {
        public int rev_id { get; set; }
        public int link_id { get; set; }
        public string rev_title { get; set; }
        public string guest_name { get; set; }
        public string rev_text { get; set; }
        public string rev_date { get; set; }
        public int rev_approved { get; set; }
        public int level { get; set; }
        public int rev_parent { get; set; }
        public Types types { get; set; }
       // public Instances instances { get; set; }

    }
 
    public class Project
    {
        public string rev_title { get; set; }
        public string rev_id { get; set; }
        public string guest_name { get; set; }
        public string rev_text { get; set; }
        public string rev_date { get; set; }

        public Types types { get; set; }
        //public Instances instances { get; set; }
    }

    public class Wrapper
    {
        public object Item { get; set; }
        public string rev_title
        {
            get
            {
                Type t = Item.GetType();
                PropertyInfo pi = t.GetProperty("rev_title");
                return pi.GetValue(Item, null).ToString();
            }
        }
        public string rev_id
        {
            get
            {
                Type t = Item.GetType();
                PropertyInfo pi = t.GetProperty("rev_id");
                return pi.GetValue(Item, null).ToString();
            }
        }
        public string guest_name
        {
            get
            {
                Type t = Item.GetType();
                PropertyInfo pi = t.GetProperty("guest_name");
                return pi.GetValue(Item, null).ToString();
            }
        }
        public string rev_text
        {
            get
            {
                Type t = Item.GetType();
                PropertyInfo pi = t.GetProperty("rev_text");
                return pi.GetValue(Item, null).ToString();
            }
        }
        public string rev_date
        {
            get
            {
                Type t = Item.GetType();
                PropertyInfo pi = t.GetProperty("rev_date");
                return pi.GetValue(Item, null).ToString();
            }
        }
        public List<Wrapper> Children
        {
            get
            {
                List<Wrapper> list = new List<Wrapper>();
                if (Item is Project)
                {
                    list.Add(
                        new Wrapper() { Item = (Item as Project).types }
                        );
                    //list.Add(
                    //    new Wrapper() { Item = (Item as Project).instances }
                    //    );
                }

                if (Item is Types)
                {
                    foreach (var item in (Item as Types).projectType)
                    {
                        list.Add(
                        new Wrapper() { Item = item }
                        );
                    }
                }
                //if (Item is Instances)
                //{
                //    foreach (var item in (Item as Instances).projectInstance)
                //    {
                //        list.Add(
                //        new Wrapper() { Item = item }
                //        );
                //    }
                //}
                return list;
            }
        }
    }

    public class Types
    {
        public string rev_title { get; set; }
        public string rev_id { get; set; }
        public string guest_name { get; set; }
        public string rev_text { get; set; }
        public string rev_date { get; set; }

        public List<ProjectType> projectType { get; set; }
    }

    public class ProjectType
    {
        public string rev_title { get; set; }
        public string rev_id { get; set; }
        public string guest_name { get; set; }
        public string rev_text { get; set; }
        public string rev_date { get; set; }
    }
    public class LinkImage
    {
        public int img_id { get; set; }
        public string link_id { get; set; }
        public string img_name { get; set; }
        public string img_addr { get; set; }
        public string img_order { get; set; }
    }
    public class FilterType
    {
        public int id { get; set; }
        public string title { get; set; }
    }
    public class FilterValue
    {
        public int id { get; set; }
        public int link_id { get; set; }
        public string title { get; set; }
        public int type { get; set; }
    }
    public class ChartPoint
    {
        public int Value { get; set; }
        public DateTime Time { get; set; }
    }

}
