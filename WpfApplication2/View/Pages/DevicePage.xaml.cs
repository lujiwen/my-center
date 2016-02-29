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

namespace WpfApplication2.View.Pages
{
    /// <summary>
    /// DevicePage.xaml 的交互逻辑
    /// </summary>
    public partial class DevicePage : Page
    {
        private Frame systemFrame;
        private Cab cab;
        private List<Device> devices;
        private Dictionary<String, List<DeviceUI>> subSystem;
        public DevicePage(Frame fm,Cab c)
        {
            InitializeComponent();
            this.systemFrame = fm;
            this.cab = c;
            init();

        }

        public DevicePage(Frame fm)
        {
            InitializeComponent();
            this.systemFrame = fm;
            init();

        }
        private void init()
        {
            this.Unloaded += new RoutedEventHandler(DevicePage_Unloaded);
            subSystem = new Dictionary<String, List<DeviceUI>>();
            devices = new List<Device>();
            if(cab!=null&&cab.Devices.Count>0)
            {
                for (int i = 0; i <cab.Devices.Count; i++)
                {
                    Device d = cab.Devices[i];
                    DeviceUI deviceUI =null;
                     switch(d.HandleTypeInSystem)
                     {
                         case "6517AB":
                             deviceUI = new DeviceUI6517AB(cab.Devices[i], systemFrame);
                             break; 
                         case "Presure":
                             deviceUI = new DeviceUIPresure(cab.Devices[i], systemFrame);
                             break ;
                        case "Pump":
                             deviceUI = new DeviceUIPump(cab.Devices[i], systemFrame);
                             break ;
                        case "XH31253127":
                             deviceUI = new DeviceUIXH31253127(cab.Devices[i], systemFrame);
                             break ;
                         case "Quality":
                             deviceUI = new DeviceUIQuality(cab.Devices[i], systemFrame);
                             break;
                         default:
                             deviceUI = new DeviceUI(cab.Devices[i], systemFrame);
                             break;
                     }
                     if (subSystem.ContainsKey(d.SubSystemName))
                     {
                         subSystem[d.SubSystemName].Add(deviceUI);
                     }
                     else
                     {
                         subSystem[d.SubSystemName] = new List<DeviceUI>();
                         subSystem[d.SubSystemName].Add(deviceUI);
                     }
                  
                //     DeviceList.Items.Add(deviceUI);
                     devices.Add(d);
                }
            }
            showDeviceBySubSystem();
        }

        void showDeviceBySubSystem()
        {
            foreach (var dic in subSystem)
            {
                String subname = dic.Key ;
                List<DeviceUI> devices = dic.Value;
                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;
                Label subLab = new Label();
                subLab.Content = subname;
                subLab.Foreground = new SolidColorBrush(Colors.White);
                subLab.Background = new SolidColorBrush(Colors.Blue);
                panel.Children.Add(subLab);

                StackPanel devicepanel = new StackPanel();
                devicepanel.Orientation = Orientation.Horizontal;
                Thickness t = new Thickness(7);
                devicepanel.Margin = t;
                foreach (DeviceUI d in devices)
                {
                    devicepanel.Children.Add(d);
                }
                panel.Children.Add(devicepanel);
                DeviceList.Items.Add(panel);
            }
        }

        void DevicePage_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("DevicePage_Unloaded");
             
        }
        
    }
}
