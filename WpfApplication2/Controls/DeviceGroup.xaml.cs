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
using WpfApplication2.Model.Vo;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;

namespace WpfApplication2.CustomMarkers.Controls
{
    /// <summary>
    /// DeviceGroup.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceGroup : UserControl
    {
        private int _cabId;

        public int CabID { get { return _cabId; } set { _cabId = value; } }
        private Building building;
        private Cab cab;
        public DeviceGroup()
        {
            InitializeComponent();
            init();
        }
        public DeviceGroup(int cabId)
        {
            InitializeComponent();
            _cabId = cabId;
            init();
        }
        public DeviceGroup(Cab c)
        {
            InitializeComponent();
            cab = c;
            init();
        }

        private void init()
        {
            device_group.Header = "柜子：" + cab.Name;
            info_panel.Children.Add(new LabelAndText("状态 : ", cab.State.Equals("Normal") ? "正常" : "异常", Colors.White));
            cab.PropertyChanged += DeviceGroupStatusChage;
            //info_panel.Children.Add(new LabelAndText("状态：", "正常", Colors.White));
        }
        private void DeviceGroupStatusChage(object sender, PropertyChangedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                Cab c = (Cab)sender;
                info_panel.Children.RemoveAt(0);
                info_panel.Children.Add(new LabelAndText("状态 : ", cab.State.Equals("Normal") ? "正常" : "异常", Colors.White));
            });
        }
    }
}
