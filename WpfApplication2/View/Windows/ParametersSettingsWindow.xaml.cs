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
using WpfApplication2.Controller;

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// ParametersSettings.xaml 的交互逻辑
    /// </summary>
    public partial class ParametersSettingsWindow : Window
    {
        private enum paraSettingsWindowType { OnMainMenu, onCab, onDevice };
        private paraSettingsWindowType type;
        private Building building;
        private Cab cab;
        private Device device;
        private List<Building> buildings;
        private List<Cab> cabs;
        private List<Device> devices;

        private List<string> deviceName;
        private Dictionary<string, Device> deviceDictionary;
        public ParametersSettingsWindow()
        {
            InitializeComponent();
            type = paraSettingsWindowType.OnMainMenu;
            init();
        }

        public ParametersSettingsWindow(Cab c)
        {
            InitializeComponent();
            type = paraSettingsWindowType.onCab;
            this.cab = c;
            init();
        }
        public ParametersSettingsWindow(Device d)
        {
            InitializeComponent();
            this.device = d;
            type = paraSettingsWindowType.onDevice;
            init();
        }
        
        private void init()
        {
            //最上层
            Topmost = true;

            //初始化监测点
            buildings = new List<Building>();
            List<string> buildingName = new List<string>();
            Dictionary<string, Building> build = GlobalMapForShow.globalMapForBuiding;
            foreach (KeyValuePair<string, Building> var in build)
            {
                buildings.Add(var.Value);
                buildingName.Add(var.Value.Name);
            }
            Building_Combox.ItemsSource = buildingName;
        //    Building_Combox.SelectedIndex = 0;
            //初始化柜子
              cabs = new List<Cab>();
            List<string> cabName = new List<string>();
            Dictionary<string, Cab> cab = GlobalMapForShow.globalMapForCab;
            foreach (KeyValuePair<string, Cab> var in cab)
            {
                cabs.Add(var.Value);
                cabName.Add(var.Value.Name);
            }

             Cab_Combox.ItemsSource = cabName;
         //   Cab_Combox.SelectedIndex = 0;
            //初始化设备
             devices = new List<Device>();
             deviceName = new List<string>();
             deviceDictionary = GlobalMapForShow.globalMapForDevice;
             foreach (KeyValuePair<string, Device> var in deviceDictionary)
            {
                devices.Add(var.Value);
                deviceName.Add(var.Value.SubSystemName);
            }

            Device_Combox.ItemsSource = deviceName;
         //   Device_Combox.SelectedIndex = 0;

            switch (type)
            {
                case paraSettingsWindowType.OnMainMenu:

                    break;
                case paraSettingsWindowType.onCab:
                    Cab c = this.cab;

                    this.building = build[c.BuildingId + ""];
                    string buidingname = building.Name;
                    Building_Combox.Text = buidingname;
                    Building_Combox.IsEnabled = false;

                    
                    Cab_Combox.Text = c.Name;
                    Cab_Combox.IsEnabled = false;
                    
                    List<string> devicename = new List<string>();
                    devices.Clear();
                    for (int i = 0; i < c.Devices.Count; i++)
                    {
                        devicename.Add(c.Devices[i].SubSystemName);
                        devices.Add(c.Devices[i]);
                    }

                    Device_Combox.ItemsSource = devicename;
                    break;
                case paraSettingsWindowType.onDevice:
                    this.building = build[this.device.BuildingId + ""];
                    string buid = this.building.Name;
                    Building_Combox.Text = buid;
                    Building_Combox.IsEnabled = false;

                    this.cab = cab[this.device.BuildingId + "_" + this.device.CabId];
                    string cabname = this.cab.Name;
                    Cab_Combox.Text = cabname;
                    Cab_Combox.IsEnabled = false;
                    
                    Device_Combox.Text = this.device.SubSystemName;
                    Device_Combox.IsEnabled = false;
                    break;
            }
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            string high_threshold = High_Threshold.Text;
            string low_threshold =  Low_Threshold.Text;
            string param1 = Parameters1.Text;
            string factor = Correct_Factor.Text;
            int selctDevice = Device_Combox.SelectedIndex;
            Device d ;
            if (type.Equals(paraSettingsWindowType.onDevice))
            {
                d = device;
            }
            else
            {
                d = devices[selctDevice];
            }
           
            resetParams(d,high_threshold, low_threshold, param1, factor);

        }

        private void resetParams(Device d, string high_threshold, string low_threshold, string param1, string factor)
        {

            /**
              * 发送控制命令到二级的调用方法
            * */
            DeviceCommandBox dcb = new DeviceCommandBox();
            dcb.load(d.DeviceId, d.CabId, high_threshold, low_threshold, param1, factor);
            List<Box> lb = new List<Box>();
            lb.Add(dcb);
            String data = PackageWorker.pack(lb);
            MainController.sendCommand("192.168.0.105", "6003", data);

            //SocketConnection con = new SocketConnection("127.0.0.1", "58888");
            //con.Connect();
            //DeviceCommandBox box = new DeviceCommandBox();
            //box.load(d.DeviceId, d.CabId, high_threshold, low_threshold, param1, factor);
            //List<Box> boxes = new List<Box>();
            //boxes.Add(box);
            //string package = PackageWorker.pack(boxes);
            //con.Send(package);
            //con.exit();
            //Console.WriteLine("发送参数！");



          
            
        }

        
        private void Building_Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cab_Combox.ItemsSource = null;
            Device_Combox.ItemsSource = null;
            ComboBox c = (ComboBox)sender ;
            int select = c.SelectedIndex;
            if (select < 0)
            {
                return;
            }
            Building b = buildings[select];
            List<string> cabsname = new List<string>();
            cabs.Clear();
            devices.Clear();
            for (int i = 0; i < b.Cabs.Count;i++ )
            {
                cabsname.Add(b.Cabs[i].Name);
                cabs.Add(b.Cabs[i]);
            }

            Cab_Combox.ItemsSource = cabsname;
        }

        private void Cab_Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          //  Cab_Combox.ItemsSource = null;
            Device_Combox.ItemsSource = null;
            ComboBox c = (ComboBox)sender;
            int select = c.SelectedIndex;
            if(select<0)
            {
                return;
            }
            Cab cab = cabs[select];
            List<string> devicename = new List<string>();
            devices.Clear();
            for (int i = 0; i < cab.Devices.Count; i++)
            {
                devicename.Add(cab.Devices[i].SubSystemName);
                devices.Add( cab.Devices[i]);
            }

            Device_Combox.ItemsSource = devicename;

       //     Console.WriteLine("Cab_Combox_SelectionChanged");
        }

        private void Device_Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Console.WriteLine("");
        }

       

        
    }
}
