﻿using System;
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
    public partial class DevicePage : Page
    {
        private Frame systemFrame;
        private Cab cab;
        private Building building;
        private List<Device> devices;//用来展示的所有的设备对象
        private Dictionary<String, List<UserControl>> subSystem;
        public DevicePage(Frame fm,Cab c)
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
        public DevicePage(Frame fm,Building b)
        {
            InitializeComponent();
            this.systemFrame = fm;
            this.building = b ; //如果没有柜子可以把所有需要显示的设备放在一个柜子里面进行显示
            this.cab = b.Cabs[0];
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
            subSystem = new Dictionary<String, List<UserControl>>();
            devices = new List<Device>();
            if(cab!=null&&cab.Devices.Count>0)
            {
                for (int i = 0; i <cab.Devices.Count; i++)
                {
                    Device d = cab.Devices[i];
                    UserControl deviceUI =null;

                    //根据数据库中设备的handletype来初始化不同种类的设备
                     switch(d.HandleTypeInSystem)
                     {
                         case "6517AB":
                             deviceUI = new DeviceUI6517AB((Device6517AB)d, systemFrame);
                             break; 
                         case "Presure":
                             deviceUI = new DeviceUIPresure(d, systemFrame);
                             break ;
                        case "Pump":
                             deviceUI = new DeviceUIPump(d, systemFrame);
                             break ;
                        case "XH3125":
                             deviceUI = new DeviceUIXH31253127((DeviceXH31253127)d, systemFrame);
                             break ;
                         case "Quality":
                             deviceUI = new DeviceUIQuality((DeviceQuality)d, systemFrame);
                             break;
                         case "Asm02":
                             deviceUI = new DeviceUIASM02((DeviceASM02)d, systemFrame);
                             break;
                         case "Jl900":
                             deviceUI = new DeviceUIJL900((DeviceJL900)d, systemFrame);
                             break;
                         case "rss131":
                             deviceUI = new DeviceUIRSS131(d, systemFrame);
                             break;
                         case "h3r7000":
                             deviceUI = new DeviceUIH3R7000(d, systemFrame);
                             break;
                         case "DryWet":
                             deviceUI = new DeviceUIDryWet((DeviceDryWet)d, systemFrame);
                             break;
                         case "gamma":
                             deviceUI = new DeviceUIGamma((DeviceGamma)d, systemFrame);
                             break;
                         case "neutron":
                             deviceUI = new DeviceUINeutron((DeviceNeutron)d, systemFrame);
                             break;
                         case "KSJ":
                             deviceUI = new DeviceUIKSJKSD((DeviceKSJKSD)d, systemFrame);
                             break;
                         case "XB2401":
                             deviceUI = new DeviceUIXB2401((DeviceXb2401)d, systemFrame);
                             break;
                         case "2115":
                             deviceUI = new DeviceUI2115((Device2115)d, systemFrame);
                             break;
                         default:
                             deviceUI = new DeviceUI(d, systemFrame);
                             break;
                     }
                     if (subSystem.ContainsKey(d.SubSystemName))
                     {
                         subSystem[d.SubSystemName].Add(deviceUI);
                     }
                     else
                     {
                         subSystem[d.SubSystemName] = new List<UserControl>();
                         subSystem[d.SubSystemName].Add(deviceUI);
                     }
                     devices.Add(d);
                }
            }
            showDeviceBySubSystem();
        }
 
        /// <summary>
        /// 如果一个柜子当中存在多个子系统（通道），按照不同的通道（不同的行 不同的通道）来进行显示
        /// </summary>
        void showDeviceBySubSystem()
        {
            foreach (var dic in subSystem)
            {
                String subname = dic.Key ;
                List<UserControl> devices = dic.Value;
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
                foreach (UserControl d in devices)
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
