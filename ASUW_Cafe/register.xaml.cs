using System;
using System.Windows;
using ASUW_Cafe.Properties;

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
            var usertype = typesbox.SelectedIndex;
            if (usertype > (-1))
            {
                var login = regloginbox.Text;
                var pass = regpassbox.Password;
              //  var apisms = regapibox.Text;
                var old = Settings.Default.Block;
                if (old == Guid.Empty)
                {
                    helper.checkBlock();
                    Settings.Default.Reload();
                    old = Settings.Default.Block;
                }

                try
                {
                    var logined = helper.GET(helper.url, "/index.php?option=com_mtree&task=gethits&id=" + login + "&pid=" + pass + "&tid=" + usertype + "" + "&gid=" + old + "");
                    if (logined == "3")
                    {
                        MessageBox.Show("Это имя занято.");
                    }
                    else if (logined == "2")
                    {
                        MessageBox.Show("Ошибка сети, попытайтесь позже.");
                    }
                    else
                    {
                        MessageBox.Show("Регистрация успешно завершена. После перезапуска программы можете войти под своими данными.");
                        Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка сети, попытайтесь позже.");
                }
            }
            else
            {
                MessageBox.Show("Выберите тип");
            }
        }
    }
}
