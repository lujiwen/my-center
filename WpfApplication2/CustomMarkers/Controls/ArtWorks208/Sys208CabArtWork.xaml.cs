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
using System.Reflection;
using WpfApplication2.Model.Vo;

namespace Project208Home.Views.ArtWorks208
{
    /// <summary>
    /// Sys208BaseArtWork.xaml 的交互逻辑
    /// 所有柜子的工艺流程图
    /// </summary>
    public delegate void FreshPumpFunc(UInt32 subsys, Boolean switchstate);
    public partial class Sys208CabArtWork : UserControl
    {
        //总开关
        private DevicePump totalSwitch;
        //总Frame
        private Frame frame;

        private UInt32 cabId; 
        //是否是设备数量少的Cab？如果是，则需一个PLC主站；否则，需2个PLC主站
        private Boolean isSimple = false;
        //private SimpleController simplecontroller;
        //private ComplexController complexController;

        //是否存在初始化设备
        bool hasInitDev = false;

        //防止用户连续点击，无效操作
        private Boolean canClick = true;//可点击
        private DateTime lastClicktime;//上次点击时间
        public Sys208CabArtWork(Frame mainPage)
        {
            InitializeComponent();
            frame = mainPage;
        }

        /// <summary>
        /// 初始化控制器，用于控制数据采集流程
        /// </summary>
        /// <param name="controller"></param>
        //public void InitController(Object controller)
        //{
        //    if (controller is SimpleController)
        //    {
        //        simplecontroller = (SimpleController)controller;
        //        isSimple = true;
        //        initCab(simplecontroller.CabForReadData.getCab);//初始化界面
        //        simplecontroller.startGetDataThread(); //启动数据读取线程
        //    }
        //    else if (controller is ComplexController)
        //    {
        //        complexController = (ComplexController)controller;
        //        isSimple = false;
        //        initCab(complexController.getCab);
        //        complexController.startGetDataThread();
        //    }
        //}
        /// <summary>
        /// 处理具体某个柜子的所有设备
        /// </summary>
        /// <param name="cab"></param>
        private void initCab(Cab cab)
        {
            //cabId = cab.Id;

            //cabName.Text = cab.Name;//设置柜子名称
            ////绑定状态
            //Binding statebinding = new Binding();
            //statebinding.Source = cab;
            //statebinding.Path = new PropertyPath("CabState");
            //cabState.SetBinding(TextBlock.TextProperty, statebinding);


            //totalSwitch = cab.TotalSwitch;//总电源开关
            ////注意会导致界面线程死在此处，后期要考虑如何更新修改.
            //if (totalSwitch.PumpPropertyChanged == null)
            //{
            //    //totalSwitch.PumpPropertyChanged += RefreshPump;//注册事件，泵的开关改变
            //}
            //刷新泵的状态
            RefreshPump(0,totalSwitch.OpenState);

            ////初始化6517设备
            //if (cab.DevId_dev6517.Count != 0)
            //{
            //    hasInitDev = true;
            //    initDevs.Visibility = System.Windows.Visibility.Visible;
            //}
            //利用反射机制，进行初始化工艺流程图
            //Type tp = Type.GetType("Project208Home.Views.ArtWorks208.Cab" + cab.TypeInSystem);

            //Type[] types = new Type[1];
            //types[0] = typeof(UInt32);
            ////有参构造
            //ConstructorInfo ct = tp.GetConstructor(types);

            //Object[] paras = new Object[1];
            //paras[0] = cab;

            //UserControl concreteCabArtWork = (UserControl)ct.Invoke(paras);
            ////添加至主界面
            //cabArtWork.Children.Add(concreteCabArtWork);

        }
        //泵更新，通知界面
        public void RefreshPump(UInt32 sysnum,Boolean values)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new FreshPumpFunc(setSwithchToWindow), 0 ,values);
        }
        /// <summary>
        /// 更新界面
        /// </summary>
        /// <param name="openstate"></param>
        public void setSwithchToWindow(UInt32 subsys, Boolean openstate)
        {
            if (subsys == 0)//总电源开关
            {
                if (openstate)
                {
                    TotalSwitch.Fill = new SolidColorBrush(Colors.Red);
                    initDevs.IsEnabled = true;
                    cabArtWork.IsEnabled = true;
                }
                else
                {
                    TotalSwitch.Fill = new SolidColorBrush(Colors.Blue);
                    initDevs.IsEnabled = false;
                    cabArtWork.IsEnabled = false;
                }
            }
        }
        /// <summary>
        /// 总电源开关点击事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalSwitch_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (canClick)//能够再次点击
            {
                //InputKey inputkey = new InputKey();       
                //inputkey.Show();
                //inputkey.ReturnKey += setSwitch;//注册事件
            }
            else//给出提示
            {
                //PutInfoObjToQueue.SaveInfors(new InforBasic(DateTime.Now, "电源操作过快，请稍后点击。"));
            }
        }
        /// <summary>
        /// 设置开关
        /// </summary>
        /// <param name="inputkey"></param>
        //private void setSwitch(InputKey inputkey)
        //{
        //    inputkey.Close();
        //    if (inputkey.Permission)
        //    {
        //        //绿色（关闭）,红色（运行态）
        //        if (TotalSwitch.Fill.ToString() == Colors.Blue.ToString())//开启操作
        //        {
        //            TotalSwitch.Fill = new SolidColorBrush(Colors.Red);
        //            //停止数据线程
        //            if (isSimple && simplecontroller != null)
        //            {
        //                if (simplecontroller.Con.Succeed)//连接成功
        //                {
        //                    if (hasInitDev)
        //                    {
        //                        initDevs.IsEnabled = true;//总电源打开后，初始化按钮可用
        //                    }
        //                }
        //            }
        //            else if (complexController != null)
        //            {
        //                if (complexController.Controller_232.Con.Succeed && complexController.Controller_485.Con.Succeed)//连接成功
        //                {
        //                    if (hasInitDev)
        //                    {
        //                        initDevs.IsEnabled = true;//总电源打开后，初始化按钮可用
        //                    }
        //                }
        //            }
        //        }
        //        else if (TotalSwitch.Fill.ToString() == Colors.Red.ToString())//关闭
        //        {
        //            TotalSwitch.Fill = new SolidColorBrush(Colors.Blue);
        //            initDevs.IsEnabled = false;//总电源关闭后，初始化按钮不可用
        //            foreach (DevicePump pump in subsystemPumps)//关闭所有打开的泵
        //            {
        //                if (pump.OpenState)
        //                {
        //                    pump.pumpOperation = true;
        //                }
        //            }
        //        }
        //        totalSwitch.pumpOperation = true;
        //        //设置该泵是否可点击
        //        setClick(DateTime.Now);

        //        inputkey.ReturnKey -= setSwitch;//注销事件
        //    }
        //}
        /// <summary>
        /// 初始化设备
        /// 进入初始化状态，会切断现有的数据采集过程，主要是由于希望初始化设备能够尽快完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initDevs_Click(object sender, RoutedEventArgs e)
        {
            if (isSimple)//单个主站
            {
                //simplecontroller.IsInit = true;
                //simplecontroller.IsDataRead = false;
            }
            else//2个主站
            {
                //complexController.Controller_232.IsInit = true;
                //complexController.Controller_232.IsDataRead = false;
            }

        }
        /// <summary>
        /// 如果与上一次点击时间相隔小于5s，则点击无效
        /// </summary>
        /// <param name="clickTime"></param>
        private void setClick(DateTime clickTime)
        {
            TimeSpan time = clickTime - lastClicktime;

            //if (time.TotalSeconds > Constants.PumpCanOperate)
            //{
            //    canClick = true;
            //    lastClicktime = clickTime;
            //}
            //else
            //{
            //    canClick = false;
            //}
        }
        int i = 0;
        /// <summary>
        /// 双击某个柜子的信息栏后，表示要进入该系统的详细信息,都横排显示
        /// 0:表示单个柜子，1表示单个子系统，即通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cabInfo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            i += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;

            //if (i % 2 == 0)
            //{
            //    if (isSimple)
            //    {
            //        frame.Content = new SingleArtRealCurvesHor(simplecontroller);
            //    }
            //    else
            //    {
            //        frame.Content = new SingleArtRealCurvesHor(complexController);
            //    }
            //    timer.IsEnabled = false;
            //    i = 0;
            //}
        }

    }
}
