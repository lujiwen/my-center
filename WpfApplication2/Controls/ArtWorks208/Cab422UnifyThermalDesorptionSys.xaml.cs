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
using Project208Home.Model;
using System.Windows.Threading;
using WpfApplication2.Model.Vo;

namespace Project208Home.Views.ArtWorks208
{
    /// <summary>
    /// Cab422ThermalDesorptionSys.xaml 的交互逻辑
    /// </summary>        
    public partial class Cab422UnifyThermalDesorptionSys : UserControl
    {
        //Dictionary<UInt32, DeviceGroup> subSystemNum_DeviceGroups;
        Dictionary<UInt32, DevicePump> subSystemNum_DevicePump;
        //防止用户连续点击，无效操作
        Boolean canClick; //能够再次点击
        DateTime lastClicktime;//上次点击时间
        Cab cabInArtwork;
        public Cab422UnifyThermalDesorptionSys(Cab cab)
        {
            InitializeComponent();
            cabInArtwork = cab;
             InitCab();
        }
        void initBindings()
        {
            //解体氚测量仪
            Binding nowding1 = new Binding();
            nowding1.Source = cabInArtwork.Devices[0];
            nowding1.Path = new PropertyPath("NowValue");
            Text1.SetBinding(TextBlock.TextProperty, nowding1);
            
            //1 3 号设备是pump

            ////房间氚测量仪
            Binding nowding2 = new Binding();
            nowding2.Source = cabInArtwork.Devices[2];
            nowding2.Path = new PropertyPath("NowValue");
            Text2.SetBinding(TextBlock.TextProperty, nowding2);
        }
       /// <summary>
       /// 初始化柜子工艺图
       /// </summary>
        private void InitCab()
        {
             initBindings();
        }
        //刷新泵的状态
        public void RefreshPumpState(UInt32 sysnum,Boolean values)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new FreshPumpFunc(setSwithchToWindow), sysnum, values);
        }
        public void setSwithchToWindow(UInt32 sysnum,Boolean openstate)
        {
            String devpumpepname = "subSys" + sysnum + "Pumpep";
            Ellipse devpumpep = WindowsUtils.GetChildObject<Ellipse>(artWorkCn, devpumpepname);
            if (devpumpep != null)
            {
                if (openstate)
                {
                    devpumpep.Fill = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    devpumpep.Fill = new SolidColorBrush(Colors.Blue);
                }
            }
        }
        private void pumpSwitch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ellipse ep = sender as Ellipse;
            if (canClick)//能够再次点击
            {
                //InputKey inputkey = new InputKey();
                //inputkey.Uid = ep.Uid;
                //inputkey.Show();
                //inputkey.ReturnKey += setSwitch;//注册事件
            }
        }
    
     }
}
