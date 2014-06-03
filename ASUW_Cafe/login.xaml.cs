using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Window
    {
        BackgroundWorker _worker = new BackgroundWorker();
        Dictionary<string, string> users = new Dictionary<string, string>();
        Guid block = Guid.Empty;
        public login()
        {
            InitializeComponent();
            progress.Maximum = 100;
            block = helper.checkBlock();
            try
            {
                getUsers();
            }
            catch
            {
                MessageBox.Show("Ошибка сети, попытайтесь позже.");
                this.Close();
            }
        }

        private void getUsersList(object sender, DoWorkEventArgs e)
        {
            users = helper.ParseJson(helper.GET(helper.url, "/index.php?option=com_mtree&task=getusers&id=" + block.ToString() + "").ToString());
            for (int i = 0; i < 101; i++)
            {
                _worker.ReportProgress(i);
                System.Threading.Thread.Sleep(8);
            }
        }


        public void getUsers()
        {
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += new DoWorkEventHandler(getUsersList);
            _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);
            _worker.ProgressChanged += new ProgressChangedEventHandler(_worker_ProgressChanged);
            _worker.RunWorkerAsync();
            
        }

        void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value =e.ProgressPercentage;
        }

        void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            usersbox.ItemsSource = users;
            usersbox.DisplayMemberPath = "Value";
        }

        private void logbut_Click(object sender, RoutedEventArgs e)
        {
            string userid = "";
            string username = "";
            string pass = "";
            string logined = "";
            System.Collections.Generic.KeyValuePair<string, string> user = new System.Collections.Generic.KeyValuePair<string, string>();
            try
            {
                user = (System.Collections.Generic.KeyValuePair<string, string>)usersbox.SelectedValue;
                userid = user.Key;
                username = user.Value;
                pass = passbox.Password.ToString();
                logined = helper.GET(helper.url, "/index.php?option=com_mtree&task=getstats&id=" + userid + "&pid=" + pass + "&lid=" + username + "").ToString();
                if (logined == "1")
                {
                    try
                    {
                        MainWindow mwin = new MainWindow();
                        helper.username = username;
                        mwin.Show();
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка запуска программы");
                    }
                }
                else if (logined == "2")
                {
                    try
                    {
                        MainWindow mwin = new MainWindow();
                        helper.username = username;
                        helper.userisDemo = true;
                        this.Close();
                        mwin.Show();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка запуска программы");
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка авторизации");
                }
            }
            catch
            {
                MessageBox.Show("Выберите пользователя.");
                return;
            }

        }

        private void regbut_Click(object sender, RoutedEventArgs e)
        {
            register reg = new register();
            reg.Show();
        }
    }
}
