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
using WpfApplication2.Util;
using WpfApplication2.Controls;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// UpdatePosition.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePositionWindow : Window
    {
        public delegate void UpdatePosition(List<Building> buildings);
        public event UpdatePosition updatePosition;
        public UpdatePositionWindow()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            foreach(var v in GlobalMapForShow.globalMapForBuiding)
            {
                PositionView view = new PositionView((Building)v.Value);
                building_position_panel.Children.Add(view);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            //入库
            List<Building> buildings = new List<Building>();
            foreach (PositionView view in building_position_panel.Children)
            {
                Building b = view.Building;
                b.Lat = double.Parse(view.position_lat.Text);
                b.Lng = double.Parse(view.position_lon.Text);
                buildings.Add(b);
            }
            updatePosition(buildings);
            Close();
        }
    }
}
