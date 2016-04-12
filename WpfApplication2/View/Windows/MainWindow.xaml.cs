using System.Windows;
using System.Windows.Controls;
using WpfApplication2.View.Pages;
using GMap.NET.WindowsPresentation;
using GMap.NET;
using WpfApplication2.Controller;
using WpfApplication2.Model.Db;
using System.Windows.Input;
using System;
using WpfApplication2.Util;
using System.Collections.Generic;
using WpfApplication2.Model.Vo;
using WpfApplication2.CustomMarkers.Controls;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Documents;
using System.Diagnostics;
namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// d
    
    public partial class MainWindow : Window
    {
        // marker
        GMapMarker currentMarker;
        // marker1, marker2, marker3, marker4;
        GMapMarker[] markers;
        PointLatLng clickPoint;
        public  MainController c ;
        public PageChooserWindow chooser;
        private Boolean[] _roomChoosed;
        public Boolean[] RoomChoosed { get { return _roomChoosed; } set { _roomChoosed = value; } }
        private Boolean isEmergencyStatus;
        private String[] _roomNames = { "208室", "201室", "208烟囱", "208室A楼" };
        public String[] RoomNames { get { return _roomNames; }  set { _roomNames = value; } }
        private List<Building> _buildings ;
        public List<Building> Buildings { get { return _buildings; } set { _buildings = value; } }
        private int pageNO;
        private static  MainWindow instance;
        private Boolean isMute;
        public  Boolean IsMute { get { return isMute; } set { isMute = value; } }
        private MapPage mapPage;
        private SystemPage systemPage;
        public  MapPage MainWindowMapPage { get { return mapPage; } set { mapPage = value; } }
        public  SystemPage MainWindowSyspage { get { return systemPage; } set { systemPage = value; } }

        public static  MainWindow getInstance()
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                return new MainWindow();
            }
        }

        public MainWindow()
        {
        
            InitializeComponent();
            init();
        }

        /// <summary>
        /// 初始化所有点
        /// </summary>
        private void initPoints()
        {
            Dictionary<string, Building> buildings = GlobalMapForShow.globalMapForBuiding;
            markers = new GMapMarker[buildings.Count];
            _buildings = new List<Building>();
            try { 
                for (int i = 0; i < buildings.Count;i++ )
                {
                    if (buildings.ContainsKey((i + 1) + ""))
                    {
                        Building b = buildings[(i + 1) + ""];
                        _buildings.Add(b);
                        markers[i] = new GMapMarker(new PointLatLng(b.Lat, b.Lng));
                        markers[i].Tag = b.Name;
                    }
                    else
                    {
                        continue;
                    }
                }            
            }
            catch(Exception e)
            {
                LogUtil.Log(true, e.Message , (int)ErrorCode.ERR_CODE.MAP_POINTS_INIT_ERR);
            }
            
        }
        private void  init()
        {
            instance = this ;
            c = new MainController();
            initPoints();
            //弹出窗，选择展示页面
          //  this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            c.alarmMessage += new alarmMessageEventHandler(MainWindowShowAlarm );
            c.alarmBuzzer += new alarmBuzzerEventHandler(buzzerAlarm);
            isEmergencyStatus = false;
           // alarmer = new AlarmBuzzer();
            isMute = false;
            mapPage = new MapPage(this);
            MainPage.Content = mapPage;
           // systemPage = new SystemPage(this);
        }
      
        void MainWindowShowAlarm(AlarmMessage alarmMsg)
        {
            //添加 显示报警信息
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate() {
             
                if (mapPage != null)
                {
                    mapPage.updateAlarmMessage(alarmMsg.MessageContent);
                    Console.WriteLine(alarmMsg.MessageContent);
                }  
            });
        }

      
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            w_pageClick();
        }

        private void chooser_pageClick(int pageNum)
        {
            pageNO = pageNum;
        }
        private void changePage(int num)
        {
            if (num == 1)
            {
                mapPage = new MapPage(this);
                MainPage.Content = mapPage;
            }
            else
            {
                systemPage  = new SystemPage(this);
                MainPage.Content = systemPage ; 
             }   
        }

        public Frame getMainPage()
        {
            return MainPage;
        }
        public GMapMarker getCurrentMarker()
        {
            return currentMarker;
        }
        public void setCurrentMarker(GMapMarker mk)
        {
              currentMarker = mk;
        }
        public GMapMarker[] getMapMarkers()
        {
            return markers;
        }
        public void setMapMarkers(GMapMarker[] mks)
        {
             this.markers = mks ;
        }
        public void setClickPoint(PointLatLng p)
        {
            this.clickPoint = p;
        }
        public PointLatLng getClickPoint()
        {
            return clickPoint;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        { 
            MenuItem item = (MenuItem)sender ;
            switch(item.Name)
            {
                case "MultiRoomshow":
                    MutiHomeChooserWindow window = new MutiHomeChooserWindow();
                    window.roomChose += new ChooseRoom(window_roomChose);
                    window.Show();
                    break;
                case "pageChooser":
                    PageChooserWindow w = new PageChooserWindow();
                    w.pageClick += new PageBtnClick(changePage);
                   // w_pageClick
                    w.Show();
                    break; 
                case"emergency_status":
                   string alerString  ;
                    if(!isEmergencyStatus)
                    {
                        alerString = "确定切换到应急状态";
                    }
                    else
                    {
                        alerString = "当前是应急状态,确定关闭？" ;
                    }
                    MessageBoxResult r = MessageBox.Show(this, alerString, "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (r == MessageBoxResult.Yes)
                    {
                        switchEmergentStatus(!isEmergencyStatus);
                    }
                   
                    break; 
                default: 
                    break;
             }
        }

        private void switchEmergentStatus(Boolean e)
        {
            this.isEmergencyStatus = e;
            Console.WriteLine(isEmergencyStatus+"");
            string str ;
            if (isEmergencyStatus)
            {
                str = "应急状态已经打开" ;
            }
            else 
            {
                str = "应急状态已经关闭";
            }

            if (resetEmergentStatus(isEmergencyStatus))
            {
                MessageBox.Show(this, str, "通知", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                str = "状态切换失败";
                MessageBox.Show(this, str, "通知", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            MainWindowShowAlarm(new AlarmMessage(str + "(" + DateTime.Now.ToString() + ")"));
        }

        private bool resetEmergentStatus(bool isEmergencyStatus)
        {
           return  MainController.changeEmergencyState(isEmergencyStatus ? 1 : 0);
            //添加屏幕打印 todo
        }

        void w_pageClick()
        {
              PageChooserWindow w = new PageChooserWindow();
              w.pageClick += new PageBtnClick(changePage);
              w.Show();
             
        }

        void window_roomChose(bool[] chooseArr)
        {
            _roomChoosed = chooseArr;    
            MainPage.Content = new SystemPage(this,true);
        }

        private void soft_settings_Click_1(object sender, RoutedEventArgs e)
        {
            //PasswordWindow w = new PasswordWindow("");
            //w.PasswordCorrect += new isPasswordCorrect(w_PasswordCorrect);
            //w.Show();
            ParametersSettingsWindow w = new ParametersSettingsWindow();
            w.Show();
        }

        void w_PasswordCorrect(bool isCorrect)
        {
            ParametersSettingsWindow w = new ParametersSettingsWindow();
            w.Show();
        }


        private void exit(object sender, RoutedEventArgs e)
        {
           Application.Current.Shutdown();
        }

        private void buzzerAlarm(bool isStart)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                if (isStart)
                {
                    if (!alarmer.IsAlarming)
                    {
                        Console.WriteLine("  alarmer.startAlarm();");
                        alarmer.Visibility = System.Windows.Visibility.Visible;
                        alarmer.startAlarm();
                    }
                }
                else  //停止报警
                {
                    if (alarmer.IsAlarming)
                    {
                        Console.WriteLine("    alarmer.stopAlarm();");
                        alarmer.Visibility = System.Windows.Visibility.Hidden;
                        alarmer.stopAlarm();
                    }
                }
            });
        }

        private void alarmMute(object sender, RoutedEventArgs e)
        {
            if(alarmer.isMute)
            {
                MessageBox.Show("当前并不处于报警状态！");
            }
            else 
            {
                alarmer.muteBuzzer();
                isMute = true ;
            }
         
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
           
            String baseDir = Environment.CurrentDirectory;
           
            String rootpath = baseDir.Substring(0, baseDir.LastIndexOf("\\"));
            rootpath = rootpath.Substring(0, rootpath.LastIndexOf("\\")).Replace("\\","/");
            String path = "file:///" + rootpath + "/Help/" + "help.html";
            System.Diagnostics.Process.Start("IExplore.exe", path);
           
        }
    }
}

