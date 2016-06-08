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
using WpfApplication2.Controls;
using WpfApplication2.Model.Vo;
using WpfApplication2.Util;
using WpfApplication2.CustomMarkers.Controls.DeviceUIs;
using Project208Home.Model;
using WpfApplication2.CustomMarkers.Controls.DeviceUIs.PavilionUI;
using PavilionMonitor;
using Yancong;
using Project2115Home.Model;

namespace WpfApplication2.View.Pages
{
    /// <summary>
    /// DevicePage.xaml 的交互逻辑
    /// 点击一个柜子，进入到这个柜子当中的所有设别页面的展示，如果一个监测点只有一个柜子则直接进入设备页面
    /// </summary>
    public partial class FactoryPlantPage : Page
    {
        private Frame systemFrame;
        private Cab cab;
        private Building building;
        private List<Device> devices;//用来展示的所有的设备对象
        private Dictionary<String, List<UserControl>> subSystem;
        public FactoryPlantPage(Frame fm,Cab c)
        {
            InitializeComponent();
            this.systemFrame = fm;
            this.cab = c;
            init();
        }

        /// <summary>
        /// 针对一个监测点是多个设备 没有柜子概念
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="b"></param>
        public FactoryPlantPage(Frame fm,Building b)
        {
            InitializeComponent();
            this.systemFrame = fm;
            this.building = b ; //如果没有柜子可以把所有需要显示的设备放在一个柜子里面进行显示
            this.cab = b.Cabs[0];
            init();
        }

        public FactoryPlantPage(Frame fm)
        {
            InitializeComponent();
            this.systemFrame = fm;
            init();
        }

        private void init()
        {
            
        }
    }
}
