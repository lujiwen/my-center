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

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// ConnectionProcessWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectionProcessWindow : Window
    {
        public delegate void PageBtnClick();
        public event PageBtnClick pageClick; 
        public ConnectionProcessWindow()
        {
            InitializeComponent();
        }

        private void Comfirm_Click(object sender, RoutedEventArgs e)
        {
            pageClick();
        }
    }
}
