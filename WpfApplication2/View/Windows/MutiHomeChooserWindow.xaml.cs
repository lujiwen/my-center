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
using WpfApplication2.CustomMarkers.Controls;

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// MutiHomeChooser.xaml 的交互逻辑
    /// </summary>
    public delegate void ChooseRoom(List<Building> builds);
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
                    if (buidings.ContainsKey((i + 1) + ""))
                    {
                        MyCheckBox c = new MyCheckBox();
                        c.Margin = new Thickness(10, 10, 10, 10);
                        c.Name = "building" + buidings[(i + 1) + ""].SystemId;
                        c.Content = buidings[(i + 1) + ""].Name;
                        c.FontSize = 15;
                        c.NodeObject = buidings[(i + 1) + ""];
                        c.Foreground = new SolidColorBrush(Colors.White);
                        roomsPanel.Children.Add(c);
                    }
                }
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            bool [] arr = new bool[GlobalMapForShow.globalMapForBuiding.Count];
            List<Building> buildings = new List<Building>();
            int i = 0;
            foreach( MyCheckBox b in roomsPanel.Children)
            {
                if (b.IsChecked ?? false)
                 {
                     buildings.Add((Building)b.NodeObject);
                     arr[i] = true;
                 }
                i++;
            }
            roomChose(buildings);
            Close();
        }
    }
}
