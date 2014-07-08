using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

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
           // RunTest();
            block = helper.checkBlock();
            try
            {
                getUsers();
            }
            catch
            {
                MessageBox.Show("Ошибка сети, попытайтесь позже.");
                Close();
            }
        }

        private void getUsersList(object sender, DoWorkEventArgs e)
        {
            users = helper.ParseJson(helper.GET(helper.url, "/index.php?option=com_mtree&task=getusers&id=" + block + ""));
            for (var i = 0; i < 101; i++)
            {
                _worker.ReportProgress(i);
                Thread.Sleep(8);
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
           // helper.PercentageCompletion = e.ProgressPercentage;
            //helper.Description = String.Format("Завершено {0}%", e.ProgressPercentage);
            progress.Value =e.ProgressPercentage;
        }

        void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
                 usersbox.ItemsSource = users;
                usersbox.DisplayMemberPath = "Value";
           //helper.PercentageCompletion = 100;
           // helper.Description = String.Format("Завершено {0}%", 100);
        }

        private void logbut_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<string, string> user;
            try
            {
                user = (KeyValuePair<string, string>)usersbox.SelectedValue;
                var userid = user.Key;
                var username = user.Value;
                var pass = passbox.Password;
                var logined = helper.GET(helper.url, "/index.php?option=com_mtree&task=getstats&id=" + userid + "&pid=" + pass + "");
                if (logined == "1")
                {
                    try
                    {
                        var mwin = new MainWindow("user");
                        helper.username = username;
                        Close();
                        mwin.Show();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка запуска программы");
                    }
                } 
                else if (logined == "2")
                {
                    helper.username = username;
                    helper.userisManager = true;
                    var mwin = new MainWindow("manager");
                    Close();
                    mwin.Show();
                }
                else if (logined == "3")
                {
                    helper.username = username;
                    helper.userisAdmin = true;
                    var mwin = new MainWindow("admin");
                    Close();
                    mwin.Show();
                }
                else
                {
                    MessageBox.Show("Ошибка авторизации");
                }
            }
            catch
            {
                MessageBox.Show("Выберите пользователя.");
            }

        }

        private void regbut_Click(object sender, RoutedEventArgs e)
        {
            var reg = new register();
            reg.Show();
        }
    }
}
