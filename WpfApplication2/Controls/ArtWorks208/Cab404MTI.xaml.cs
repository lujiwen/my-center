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
using System.Windows.Threading;
using Project208Home.Model;
using WpfApplication2.Model.Vo;

namespace Project208Home.Views.ArtWorks208
{
    /// <summary>
    /// Cab404MTI.xaml 的交互逻辑
    /// </summary>
    public partial class Cab404MTI : UserControl
    {
        //Dictionary<UInt32, DeviceGroup> subSystemNum_DeviceGroups;
        Dictionary<UInt32, DevicePump> subSystemNum_DevicePump;
        //防止用户连续点击，无效操作
        Boolean canClick; //能够再次点击
        DateTime lastClicktime;//上次点击时间
        Cab cabInArtwork;
        public Cab404MTI(Cab cab)
        {
            InitializeComponent();
            //subSystemNum_DeviceGroups = cab.SubSystemNum_DeviceGroups;
            //subSystemNum_DevicePump = cab.SubSystemNum_DevicePump;
            cabInArtwork = cab;
            InitCab();
        }
        void initBindings()
        {
            //压力计
            //Binding nowding1 = new Binding();
            //nowding1.Source = cabInArtwork.Devices[0];
            //nowding1.Path = new PropertyPath("NowValue");
            //subSys1Presuretb.SetBinding(TextBlock.TextProperty, nowding1);
            //压力计
            //Binding nowding2 = new Binding();
            //nowding2.Source = cabInArtwork.Devices[1];
            //nowding2.Path = new PropertyPath("NowValue");
            //subSys2Presuretb.SetBinding(TextBlock.TextProperty, nowding2);
            //压力计
            //Binding nowding3 = new Binding();
            //nowding3.Source = cabInArtwork.Devices[2];
            //nowding3.Path = new PropertyPath("NowValue");
            //subSys3Presuretb.SetBinding(TextBlock.TextProperty, nowding3);

            //特排氚测量仪
            Binding nowding4 = new Binding();
            nowding4.Source = cabInArtwork.Devices[0];
            nowding4.Path = new PropertyPath("NowValue");
            subSys1Qualitytb.SetBinding(TextBlock.TextProperty, nowding4);

            //房间氚测量仪
            Binding nowding5 = new Binding();
            nowding5.Source = cabInArtwork.Devices[2];
            nowding5.Path = new PropertyPath("NowValue");
            subSys2Qualitytb.SetBinding(TextBlock.TextProperty, nowding5);

            //房间氚测量仪
            Binding nowding6 = new Binding();
            nowding6.Source = cabInArtwork.Devices[4];
            nowding6.Path = new PropertyPath("NowValue");
            subSys3Qualitytb.SetBinding(TextBlock.TextProperty, nowding6);

            //特排氚测量仪
            Binding nowding7 = new Binding();
            nowding7.Source = cabInArtwork.Devices[1];
            nowding7.Path = new PropertyPath("NowValue");
            subSys16517ABtb.SetBinding(TextBlock.TextProperty, nowding7);

            //特排氚测量仪
            Binding nowding8 = new Binding();
            nowding8.Source = cabInArtwork.Devices[3];
            nowding8.Path = new PropertyPath("NowValue");
            subSys26517ABtb.SetBinding(TextBlock.TextProperty, nowding8);

            //特排氚测量仪
            Binding nowding9 = new Binding();
            nowding9.Source = cabInArtwork.Devices[5];
            nowding9.Path = new PropertyPath("NowValue");
            subSys36517ABtb.SetBinding(TextBlock.TextProperty, nowding9);

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
            //if (canClick)//能够再次点击
            //{
            //    InputKey inputkey = new InputKey();
            //    inputkey.Uid = ep.Uid;
            //    inputkey.Show();
            //    inputkey.ReturnKey += setSwitch;//注册事件
            //}
        }
        /// <summary>
        /// 设置开关
        /// </summary>
        /// <param name="inputkey"></param>
    //    private void setSwitch(InputKey inputkey)
    //    {
    //        if (inputkey.Permission)
    //        {
    //            //找到对应的泵设备
    //            UInt32 subsysnum = Convert.ToUInt32(inputkey.Uid);
    //            subSystemNum_DevicePump[subsysnum].pumpOperation = true;
    //            subSystemNum_DevicePump[subsysnum].PumpNoUpdated = true;
    //            if (subSystemNum_DevicePump[subsysnum].OpenState)
    //            {
    //                PutInfoObjToQueue.SaveInfors(new InforBasic(DateTime.Now, "正在关闭泵....."));
    //            }
    //            else
    //            {
    //                PutInfoObjToQueue.SaveInfors(new InforBasic(DateTime.Now, "正在打开泵....."));
    //            }
    //            //设置该泵是否可点击
    //            if (WindowsUtils.setClick(DateTime.Now, lastClicktime))//可点击
    //            {
    //                canClick = true;
    //                lastClicktime = DateTime.Now;
    //            }
    //            else
    //            {
    //                canClick = false;
    //            }
    //        }
    //        inputkey.Close();
    //        inputkey.ReturnKey -= setSwitch;//注册事件
    //    }
     }
}
