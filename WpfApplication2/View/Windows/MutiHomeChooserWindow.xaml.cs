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
using WpfApplication2.Util;

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// MutiHomeChooser.xaml 的交互逻辑
    /// </summary>
   public  delegate void ChooseRoom(bool[] chooseArr);
    public partial class MutiHomeChooserWindow : Window
    {
        public event ChooseRoom roomChose;
        private Dictionary<string, Building> buidings;
        public MutiHomeChooserWindow()
        {
            InitializeComponent();
            Topmost = true;
            init();
        }

        private void init()
        {
             buidings = GlobalMapForShow.globalMapForBuiding;
            if (buidings != null)
            {
                for (int i = 0; i <buidings.Count; i++)
                {
                    CheckBox c = new CheckBox();
                    c.Margin = new Thickness(10,10,10,10);
                    c.Name = "building" + buidings[(i + 1) + ""].SystemId;
                    c.Content = buidings[(i + 1) + ""].Name;
                    c.FontSize = 15;
                    c.Foreground = new SolidColorBrush(Colors.White);
                    roomsPanel.Children.Add(c);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            bool [] arr = new bool[GlobalMapForShow.globalMapForBuiding.Count];
            int i = 0;
            foreach( CheckBox b in roomsPanel.Children)
            {
                if (b.IsChecked ?? false)
                 {
                     arr[i] = true;
                 }
                i++;
            }
            roomChose(arr);
            Close();
        }
    }
}
