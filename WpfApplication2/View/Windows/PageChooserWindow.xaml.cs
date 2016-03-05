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
    /// PageChooserWindow.xaml 的交互逻辑
    /// </summary>
   public  delegate void PageBtnClick(int choose);
   public partial class PageChooserWindow : Window
    {
        public event PageBtnClick pageClick; 
        private bool selected ;
        public PageChooserWindow()
        {
            InitializeComponent();
            Topmost = true;
        }

        public void mapPageBtn_Click(object sender, RoutedEventArgs e)
        {
            selected = true;
            pageClick(1);
            this.Close();
        }

        public void cabPageBtn_Click(object sender, RoutedEventArgs e)
        {
             selected = true;
             pageClick(2);
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!selected)
            {
                MessageBoxResult r = MessageBox.Show(this,"未选择任何界面，将默认显示地图界面！", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    pageClick(1);
                    e.Cancel = false; 
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
