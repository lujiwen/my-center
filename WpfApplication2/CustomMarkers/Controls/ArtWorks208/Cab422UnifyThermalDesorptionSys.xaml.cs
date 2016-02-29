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
            //subSystemNum_DeviceGroups = cab.SubSystemNum_DeviceGroups;
            //subSystemNum_DevicePump = cab.SubSystemNum_DevicePump;
             InitCab();
        }
        void initBindings()
        {
            //解体氚测量仪
            Binding nowding1 = new Binding();
            nowding1.Source = cabInArtwork.Devices[0];
            nowding1.Path = new PropertyPath("NowValue");
            subSys1Qualitytb.SetBinding(TextBlock.TextProperty, nowding1);
            //房间氚测量仪
            Binding nowding2 = new Binding();
            nowding2.Source = cabInArtwork.Devices[0];
            nowding2.Path = new PropertyPath("NowValue");
            subSys2Qualitytb.SetBinding(TextBlock.TextProperty, nowding2);

        }
       /// <summary>
       /// 初始化柜子工艺图
       /// </summary>
        private void InitCab()
        {

            initBindings();
            ////绑定对应子系统的泵设备状态
            //foreach (UInt32 subSysNum in subSystemNum_DevicePump.Keys)
            //{
            //    //初始化操作
            //    String devpumpepname = "subSys" + subSysNum + "Pumpep";
            //    Ellipse devpumpep = WindowsUtils.GetChildObject<Ellipse>(artWorkCn, devpumpepname);
            //    if (devpumpep != null)
            //    {
            //        devpumpep.Uid = subSysNum.ToString();//根据Uid设备相应的泵状态，以便点击后唯一定位某个泵按钮
            //    }
            //    //初始化显示和更新
            //    DevicePump dp = subSystemNum_DevicePump[subSysNum];
            //    if (dp.PumpPropertyChanged == null)
            //    {
            //        //dp.PumpPropertyChanged += RefreshPumpState;
            //    }
            //    //从其他页面跳转来后需刷新界面
            //    RefreshPumpState(subSysNum,dp.OpenState);
                
            //}
            //绑定对应子系统 对应设备的数据显示
            //foreach (UInt32 subSysNum in subSystemNum_DeviceGroups.Keys)
            //{
            //    Dictionary<String,Device> tempDGTD = subSystemNum_DeviceGroups[subSysNum].Types_Devices;
            //    foreach (String handleTypeInSystem in tempDGTD.Keys)
            //    {
            //        if (handleTypeInSystem != Constants.Dev_Pump)//如果设备不为泵，则应添加数据更新
            //        {
            //            //查找设备类型的数据显示，并绑定
            //            String devdatatbname = "subSys" + subSysNum + handleTypeInSystem + "tb";
            //            TextBlock devdatatb = WindowsUtils.GetChildObject<TextBlock>(artWorkCn, devdatatbname);
            //            if (devdatatb != null)
            //            {
            //                switch (handleTypeInSystem)
            //                {
            //                    case Constants.Dev_Quality://该柜子只有质量流量计
            //                        DeviceQuality tempDQ = (DeviceQuality)tempDGTD[handleTypeInSystem];
            //                        //绑定数据源
            //                        //实时值绑定
            //                        Binding nowding = new Binding();
            //                        nowding.Source = tempDQ;
            //                        nowding.Path = new PropertyPath("DoseNow");
            //                        devdatatb.SetBinding(TextBlock.TextProperty, nowding);
            //                        break;
            //                    default:
            //                        break;
            //                }

            //            }
            //            else
            //            {
            //                MessageBox.Show("请检查324PurificationSys的设备输入是否正常？");
            //            }
            //        }


            //    }

            //}
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
