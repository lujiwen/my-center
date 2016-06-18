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

namespace WpfApplication2.View.Pages
{
    /// <summary>
    /// Page202.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleDevicePage : Page
    {
        public SimpleDevicePage()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    simpleDeviceList.Items.Add(new SimpleDeviceUI());
            //}
            showDeviceBySubSystem();
        }


         /// <summary>
        /// 如果一个柜子当中存在多个子系统（通道），按照不同的通道（不同的行 不同的通道）来进行显示
        /// </summary>
        void showDeviceBySubSystem()
        {
            for(int i=0;i<6;i++)
            {
                String subname = "系列" + (i + 1);
                List<UserControl> devices = new List<UserControl>();
                for (int j= 0; j < 7; j++)
                {
                    SimpleDeviceUI simpleUI = new SimpleDeviceUI();
                    Thickness t = new Thickness(5);
                    simpleUI.Margin = t;
                    devices.Add(simpleUI);
                }
            
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
              
                Label subLab = new Label();
                subLab.Content = subname;
                subLab.Foreground = new SolidColorBrush(Colors.White);
                subLab.Background = new SolidColorBrush(Colors.Blue);
                panel.Children.Add(subLab);

                StackPanel devicepanel = new StackPanel();
                devicepanel.Orientation = Orientation.Horizontal;
               
                foreach (UserControl d in devices)
                {
                    devicepanel.Children.Add(d);
                }
                panel.Children.Add(devicepanel);
                simpleDeviceList.Items.Add(panel);
            }
        }
        
    }
}
