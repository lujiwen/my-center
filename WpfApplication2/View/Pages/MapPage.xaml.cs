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
using GMap.NET.WindowsPresentation;
using GMap.NET;
using WpfApplication2.Controls;
using WpfApplication2.CustomMarkers;
using WpfApplication2.View.Windows;
using WpfApplication2.CustomMarkers.Controls;
using WpfApplication2.Util;
using WpfApplication2.Model.Vo;
using DragDrop;
using System.ComponentModel;
using WpfApplication2.Controller;
using System.Windows.Threading;
using System.Threading;
using System.Net;
using WpfApplication2.package;

namespace WpfApplication2.View.Pages
{
    delegate System.Windows.Point GetPositionDelegate(IInputElement element);

    /// <summary>
    /// MainmapPage.xaml 的交互逻辑
    /// </summary>
    public partial class MapPage : Page
    {
        static int count_item = 0;
        GMapMarker[] markers;
        GMapMarker currentMarker;
        MainWindow mainWindow;
        PositionMarker dragMarker;
        bool IsMouseDown = false;
        System.Windows.Point mousePoint;
        object mouseCtrl = null;
        private List<Building> buildings;
        private QueueFixedLength<AlarmMessage> alarmMessages;
         
        public MapPage(MainWindow w)
        {
            InitializeComponent();
            this.mainWindow = w ;
            init();
            initPoints();
            initMap();
            //listBox1.Items.Add("this is listbox");
            Thread th1 = new Thread(new ThreadStart(DispatcherThread));
            //th1.Start();
          
        }
        public void DispatcherThread()
        {
            //可以通过循环条件来控制UI的更新
            while (true)
            {
                //this.listBox1.ScrollIntoView(this.listBox1.Items[listBox1.Items.Count - 1]);
                ///线程优先级，最长超时时间，方法委托（无参方法）
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(UpdateOutput));
                //this.listBox1.ScrollIntoView(this.listBox1.Items[listBox1.Items.Count - 1]);
                Thread.Sleep(500);
            }
        }
        private void UpdateOutput()
        {
            //listBox1.Items.Add("itemThread");
            count_item++;
            listBox1.Items.Insert(0, "最前面" + count_item);
            if (listBox1.Items.Count > 10)
                listBox1.Items.Clear();
            //清除所有内容
            //listBox1.Items.Clear();
            //listBox1.ScrollIntoView(listBox1.Items[listBox1.Items.Count - 1]);
            //System.Console.WriteLine("*********************"+listBox1.Items.Count);
            //listBox1.Items.MoveCurrentToPosition(listBox1.Items.Count-1);
            //listBox1.SelectedIndex = listBox1.Items.Count - 1;
            //this.listBox1.ScrollIntoView(this.listBox1.SelectedItem);
        }
        private void init()
        {
           // info.MouseEnter += new MouseEventHandler(info_MouseEnter);
           // info.MouseLeave += new MouseEventHandler(info_MouseLeave);
        //    info_panel.MouseEnter += new MouseEventHandler(info_panel_MouseEnter);
            //whole_status.Text += "异常点：1/20";
            buildings = mainWindow.Buildings;
            alarmMessages = new QueueFixedLength<AlarmMessage>(10);
            listBox1.ItemsSource = alarmMessages.Queue;
            //listBox1.SelectedValuePath = "messageContent";
            //listBox1.DisplayMemberPath = "messageContent";
            //Binding binding = new Binding();
            //binding.Path = new PropertyPath("messageContent");
            //listBox1.SetBinding(ListBox.SelectedValuePathProperty, binding);
        }

        private void initMap()
        {
            // add your custom map db provider
            //MySQLPureImageCache ch = new MySQLPureImageCache();
            //ch.ConnectionString = @"server=sql2008;User Id=trolis;Persist Security Info=True;database=gmapnetcache;password=trolis;";
            //MainMap.Manager.ImageCacheSecond = ch;

            // set your proxy here if need
       //    MainMap.Manager.Proxy = new WebProxy("10.2.0.100", 8080);
        //   MainMap.Manager.Proxy.Credentials = new NetworkCredential("ogrenci@bilgeadam.com", "bilgeadam");

            // set cache mode only if no internet avaible
            //try
            //{

            //    System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("www.google.com");
            //}
            //catch
            //{
            //    MainMap.Manager.Mode = AccessMode.CacheOnly;
            //    MessageBox.Show("No internet connection avaible, going to CacheOnly mode.", "GMap.NET - Demo.WindowsPresentation", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            Console.WriteLine("initMap");
            
            this.MouseMove += new MouseEventHandler(Window_MouseMove);
          //  this.MouseUp += new MouseButtonEventHandler(Window_MouseUp);
            MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            MainMap.MouseMove += new System.Windows.Input.MouseEventHandler(MainMap_MouseMove);
            MainMap.Loaded += new RoutedEventHandler(MainMap_Loaded);
            MainMap.MouseEnter += new MouseEventHandler(MainMap_MouseEnter);
            MainMap.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(MainMap_MouseWheelScroll);
            MainMap.CurrentPosition = new PointLatLng(31.540871, 104.804598);
            MainMap.MaxZoom = 40;
            MainMap.MinZoom = 2;
            MainMap.Zoom = 6;
           // MainMap.MapType = MapType.GoogleHybridChina;
             MainMap.MapType = MapType.ArcGIS_Map;
            MainMap.Manager.Mode = AccessMode.ServerAndCache;
          //  MainMap.BoundsOfMap = new RectLatLng(31.540871, 104.804598, 2.025, 2.018); //北纬30.67度，东经104.06度。
            MainMap.CanDragMap = true;
            MainMap.DragButton = MouseButton.Right;
            currentMarker = new GMapMarker(new PointLatLng(31.540871, 104.804598));
            {
                TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
                // trolleyToolTip.setStatus("异常");
             //   currentMarker.Offset = new System.Windows.Point(0,0);
                currentMarker.ZIndex = int.MaxValue;
                MainMap.Markers.Add(currentMarker);
              //  currentMarker.Shape = new PositionMarker(mainWindow, currentMarker, trolleyToolTip, false);

                for (int i = 0; i < markers.Length; i++)
                {
                    markers[i].ZIndex = int.MaxValue;
                    trolleyToolTip = new TrolleyTooltip(buildings[i]);
                    markers[i].Shape = new PositionMarker(mainWindow, markers[i], trolleyToolTip, buildings[i]);
                 //   markers[i].Shape.MouseRightButtonDown += new MouseButtonEventHandler(Marker_MouseRightButtonDown);
                    // markers[i].Shape.MouseDown += new MouseButtonEventHandler(marker_Click);
                    markers[i].Shape.MouseLeftButtonUp += marker_Click;
                    markers[i].Shape.AllowDrop = true;
                    markers[i].Shape.PreviewMouseMove += Shape_PreviewMouseMove;
                    markers[i].Shape.QueryContinueDrag += Shape_QueryContinueDrag;
                    MainMap.Markers.Add(markers[i]);
                }
            }
        }

        void OnDragOver(object sender, DragEventArgs e)
        {
            Console.WriteLine("OnDragOver");
            e.Effects = DragDropEffects.None;

            System.Windows.Point pos = e.GetPosition(info_panel);
            HitTestResult result = VisualTreeHelper.HitTest(info_panel, pos);
            if (result == null)
                return;

            e.Effects = DragDropEffects.Copy;
        }

        
        private void OnDrop(object sender, DragEventArgs e)
        {
            System.Windows.Point pos = e.GetPosition(info_panel);
            HitTestResult result = VisualTreeHelper.HitTest(info_panel, pos);
            if (result == null)
                return;

            PositionMarker marker = e.Data.GetData(typeof(PositionMarker)) as PositionMarker;
            Building b = marker.building;
            b.PropertyChanged += LeftBuidingInfoChange; 
            room.Text = b.Name;
            position.Text = b.Location;
            status.Text = b.State.Equals("Nomal")? "正常" : "异常";
            cab_num.Text = "柜子总数：" + b.Cabs.Count;
            manager.Text = "负责人："+ b.Manager;
            group_panel.Children.Clear();
            foreach(Cab c in b.Cabs)
            {
                group_panel.Children.Add(new DeviceGroup(c));
            }
            

            //for (int i = 0; i < b.Cabs.Count; i++)
            //{
            //    group_panel.Children.Add(new DeviceGroup(i)); //LabelAndText("氚浓度","0.00007668")
            //}
        }

        private void LeftBuidingInfoChange(object sender, PropertyChangedEventArgs e)
        { 
            Building b = (Building)sender ;
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {
                if (!b.State.Equals(DeviceDataBox_Base.State.Normal.ToString()))
                {
                    status.Text = "异常";
                }
                else
                {
                    status.Text = "正常";
                }
            });
           
        }

        void Shape_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            mAdornerLayer.Update();
        }

        void Shape_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            System.Windows.Point pos = e.GetPosition(MainMap);
            HitTestResult result = VisualTreeHelper.HitTest(MainMap, pos);
            if (result == null)
                return;

            PositionMarker marker = (PositionMarker)sender;
            DragDropAdorner adorner = new DragDropAdorner(marker);
            mAdornerLayer = AdornerLayer.GetAdornerLayer(grid1); // Window class do not have AdornerLayer
            mAdornerLayer.Add(adorner);

            System.Windows.DragDrop.DoDragDrop(marker, marker, DragDropEffects.Copy);

            mStartHoverTime = DateTime.MinValue;
          //  mHoveredItem = null;
            mAdornerLayer.Remove(adorner);
            mAdornerLayer = null;
        }
        DateTime mStartHoverTime = DateTime.MinValue;
     // TreeViewItem mHoveredItem = null;
        AdornerLayer mAdornerLayer = null;


        void info_panel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                Console.WriteLine("进入详细信息panel！");

                room.Text = "串口设备名+TX系列";
              //  ip.Text = "192.168.103.145:5002";
                position.Text = "基础楼六栋八楼502C";
                status.Text = dragMarker.IsNormal ? "正常" : "异常";
                cab_num.Text = "柜子总数：" + "5";
                  
                group_panel.Children.Clear();
                for (int i = 0; i <15;i++ )
                {
                    group_panel.Children.Add(new DeviceGroup(i)); //LabelAndText("氚浓度","0.00007668")
                }
            }
        }

        void info_MouseLeave(object sender, MouseEventArgs e)
        {
            Console.WriteLine("离开详细信息！");
        }


        //void info_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    if(IsMouseDown)
        //    {
        //        Console.WriteLine("进入详细信息！");
        //     //   info.Text += "进入详细信息！"+Environment.NewLine;
        //    }
        //}
        private void Marker_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                IsMouseDown = true;
                mousePoint = e.GetPosition(MainMap);
                mouseCtrl = sender;
                dragMarker = (PositionMarker)mouseCtrl;
                currentMarker = dragMarker.getGmapMarker();
               // currentMarker.Position = MainMap.FromLocalToLatLng((int)mousePoint.X, (int)mousePoint.Y);
            }
        }

        // move current marker with left holding
        void MainMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                System.Windows.Point p = e.GetPosition(MainMap);
                if(currentMarker!=null)
                {
                    currentMarker.Position = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
                }
            }
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    System.Windows.Point theMousePoint = e.GetPosition(MainMap);
                    Canvas.SetLeft((UIElement)mouseCtrl, theMousePoint.X - (mousePoint.X - Canvas.GetLeft(((UIElement)mouseCtrl))));
                    Canvas.SetTop((UIElement)mouseCtrl, theMousePoint.Y - (mousePoint.Y - Canvas.GetTop(((UIElement)mouseCtrl))));
                    mousePoint = theMousePoint;
                    Console.WriteLine("x:"+ mousePoint.X+"  y:"+mousePoint.Y);
                }
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Window_MouseUp");
            if (IsMouseDown)
            {
                currentMarker.GoBack();
                IsMouseDown = false;
                currentMarker = null;
            }
        }

        void MainMap_MouseEnter(object sender, MouseEventArgs e)
        {
            MainMap.Focus();
        }

        public void marker_Click(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("点击了一下marker");
            if (e.LeftButton == MouseButtonState.Released)
            {
                Dictionary<string, Building> globalMapForBuiding = GlobalMapForShow.globalMapForBuiding;  
                PositionMarker marker = (PositionMarker)sender;
                System.Windows.Point p = e.GetPosition(MainMap);
                mainWindow.setClickPoint(MainMap.FromLocalToLatLng((int)p.X, (int)p.Y));
                mainWindow.setCurrentMarker(marker.getGmapMarker());
                Building b = marker.building;
           //   globalMapForBuiding.TryGetValue("208",out b);
                SystemPage page = new SystemPage(mainWindow,b);
                mainWindow.getMainPage().Content = page;
            }
        }

        void MainMap_OnMapTypeChanged(MapType type)
        {
            //   sliderZoom.Minimum = MainMap.MinZoom;5
            //     sliderZoom.Maximum = MainMap.MaxZoom;
            
            Console.WriteLine("MainMap_OnMapTypeChanged:" + MainMap.MinZoom + " ; " + MainMap.MaxZoom);
        }

        // current location changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
          //  mapgroup.Header = "gmap: " + point;
            Console.WriteLine("头部的经纬度：" + point);
        }

        void MainMap_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
             System.Windows.Point p = e.GetPosition(MainMap);
             mainWindow.setClickPoint(MainMap.FromLocalToLatLng((int)p.X, (int)p.Y));
          //  RedMarkerCickPoint = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
         //   Console.WriteLine("MainMap_MouseLeftButtonDown");
        }
        void MainMap_MouseWheelScroll(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            //12 14
            if (MainMap.Zoom > 14)
            {
                MainMap.Zoom = 14;
            }
            else if (MainMap.Zoom <= MainMap.MinZoom)
            {
                MainMap.Zoom = MainMap.MinZoom;
            }

            Console.WriteLine("main.maxzoom:" + MainMap.MaxZoom + "   MainMap zoom:" + MainMap.Zoom + "MainMap Minzoom" + MainMap.MinZoom);
        }
        // center markers on load 
        void MainMap_Loaded(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomAndCenterMarkers(null);
        }

        /// <summary>
        /// 初始化所有点
        /// </summary>
        private void initPoints()
        {
            markers = new GMapMarker[buildings.Count];
            for (int i = 0; i < buildings.Count;i++ )
            {

                Building b = buildings[i];
                markers[i] = new GMapMarker(new PointLatLng(b.Lat, b.Lng));
                markers[i].Tag = b.Name;
            }
         }

        // zoo max & center markers
        private void button13_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomAndCenterMarkers(null);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem i = (MenuItem)sender ;
            switch(i.Name)
            {
                case "clear":
                    clearLeftPanelInfo();
                    break ;
            }
        }

        private void clearLeftPanelInfo()
        {
            for(int i = 0;i<6;i++)
            {
                TextBox t = (TextBox)info_panel.Children[i];
                t.Text = "";
            }
            group_panel.Children.Clear();
        }

        //更新地图界面数据
        public void updateMapUI()
        {
            Dictionary<string, Building> buildings = GlobalMapForShow.globalMapForBuiding;
            if(markers!=null&&markers.Length>0)
            {
                for (int i = 0; i < markers.Length;i++ )
                {
                    Building b = ((PositionMarker)(markers[i].Shape)).building;
                    Building b2 = buildings[b.SystemId + ""];
                    if (b2.State != b.State) //异常和正常状态的切换
                    {
                        TrolleyTooltip trolleyToolTip = new TrolleyTooltip(b);
                        markers[i].Shape = new PositionMarker(mainWindow, markers[i], trolleyToolTip, buildings[b.SystemId + ""]);
                    }
                }
            }
        }


        public void updateAlarmMessage(String alarmStr)
        {
            alarmMessages.add(new AlarmMessage(alarmStr,new DateTime()));
            //存储报警信息


            //alarmMessages.Capacity = 10;
            //count_item = count_item + 1;
            //double x = SystemParameters.WorkArea.Width;        
            //listBox1.Width = x;
        }

        private void listBox1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            updateAlarmMessage("1234");
        }
       
        
    }
}
