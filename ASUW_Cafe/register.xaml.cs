using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ASUW_Cafe
{
    /// <summary>
    /// Логика взаимодействия для register.xaml
    /// </summary>
    public partial class register : Window
    {
        public register()
        {
            InitializeComponent();
        }

        private void Regbutton_Click(object sender, RoutedEventArgs e)
        {
            int usertype;
            string login = "";
            string pass = "";
            string apisms = "";
            usertype = typesbox.SelectedIndex;
            if (usertype > (-1))
            {
                login = regloginbox.Text.ToString();
                pass = regpassbox.Password.ToString();
                apisms = regapibox.Text.ToString();
                Guid old = ASUW_Cafe.Properties.Settings.Default.Block;
                if (old == Guid.Empty)
                {
                    helper.checkBlock();
                    ASUW_Cafe.Properties.Settings.Default.Reload();
                    old = ASUW_Cafe.Properties.Settings.Default.Block;
                }

                try
                {
                    string logined = helper.GET(helper.url, "/index.php?option=com_mtree&task=gethits&id=" + login + "&pid=" + pass + "&tid=" + usertype + "" + "&gid=" + old + "&aid = " + apisms + "").ToString();
                    if (logined == "3")
                    {
                        MessageBox.Show("Это имя занято.");
                        return;
                    }
                    else if (logined == "2")
                    {
                        MessageBox.Show("Ошибка сети, попытайтесь позже.");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Регистрация успешно завершена. После перезапуска программы можете войти под своими данными.");
                        this.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка сети, попытайтесь позже.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Выберите тип");
            }
        }
    }
}
