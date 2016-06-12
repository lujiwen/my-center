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
using System.Windows.Shapes;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.CustomMarkers.Controls
{
    /// <summary>
    /// PasswordWindow.xaml 的交互逻辑
    /// </summary>
    public  delegate void UpdatePassword(User user);
    public partial class UpdatePasswordWindow : Window
    {
        public event UpdatePassword updatePassword;
        private string correctPassword ;
        public UpdatePasswordWindow()
        {
            InitializeComponent();
          
            Topmost = true;
        }
        public UpdatePasswordWindow(string pwd)
        {
            InitializeComponent();
            Topmost = true;
            this.correctPassword = pwd;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
           if(username.Text.Equals("")||oldPasswordBox.Password.Equals("")||newPasswordBox.Equals("")||comfirmPasswordBox.Equals(""))
           {
               MessageBox.Show("任意一项不得为空！");
               return ;
           }
           if(newPasswordBox.Password.Equals(comfirmPasswordBox.Password))
           {
               updatePassword(new User(username.Text,newPasswordBox.Password));
           }
            Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

 

       
    }
}
