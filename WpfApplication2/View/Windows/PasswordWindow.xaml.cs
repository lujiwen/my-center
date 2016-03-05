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

namespace WpfApplication2.CustomMarkers.Controls
{
    /// <summary>
    /// PasswordWindow.xaml 的交互逻辑
    /// </summary>
    public  delegate void isPasswordCorrect(bool isCorrect);
    public partial class PasswordWindow : Window
    {
        private String passwordTip ;
        private string correctPassword ;
        //public PasswordWindow()
        //{
        //    InitializeComponent();
        //    passwordTip = "";
        //    Topmost = true;
        //}
        public event  isPasswordCorrect PasswordCorrect;
        public PasswordWindow(string pwd)
        {
            InitializeComponent();
            passwordTip = "";
            Topmost = true;
            this.correctPassword = pwd;
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password.Equals(correctPassword))
            {
                Console.WriteLine("正确!");
               
                PasswordCorrect(true);
                Close();
            }
            else
            {
                Console.WriteLine("错误！");
                passwordtip.Content = "密码不正确,请重新输入！";
                passwordBox.Clear();
                PasswordCorrect(false);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void setPasswordTip(String pwdTip)
        {
            passwordtip.Content = pwdTip;
        }

        public void clearPassword()
        {
            passwordBox.Clear() ;
        }
       

       
    }
}
