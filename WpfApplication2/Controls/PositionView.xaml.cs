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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication2.Util;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.Controls
{
    /// <summary>
    /// PositionView.xaml 的交互逻辑
    /// </summary>
    public partial class PositionView : UserControl
    {
        private Building build;
        public Building Building {
            get { 
                return build;
            } 
            set {
                build = value;
            }
        }
        public PositionView(Building build)
        {
           InitializeComponent();
           init(build);
        }

        private void init(Building b)
        {
            build = b;
            position_name.Content = build.Name;
            position_lat.Text = build.Lat.ToString();
            position_lon.Text = build.Lng.ToString();
        }
    }
}
