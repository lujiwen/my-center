using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfApplication2.CustomMarkers;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using WpfApplication2.Controls;
using WpfApplication2.Util;
using WpfApplication2.Controller;
using WpfApplication2.CustomMarkers.Controls;
using WpfApplication2.View.Pages;
using WpfApplication2.Model.Vo;
using WpfApplication2.package;

namespace WpfApplication2.Windows
{
    public delegate void UpdatedToWindowEvent(String systemId);
    public partial class MapWindow : Window
    {
        //PointLatLng sta
        //PointLatLng end;
        public static MainController mc = new MainController();
        public Dictionary<String, CabUI> cabId_cab = new Dictionary<string, CabUI>();
        public Dictionary<String, DeviceUI> devId_dev = new Dictionary<string, DeviceUI>();
        private List<Building> buildings;
        public MainController Mc
        {
            get { return mc; }
            set { mc = value; }
        }
        // marker
        GMapMarker currentMarker;
        // marker1, marker2, marker3, marker4;
        GMapMarker[] markers;

        // zones list
        PointLatLng RedMarkerCickPoint;
        private SHOW_LEVEl showLevel;
        
        
        public MapWindow()
        {
         
            
            InitializeComponent();
            // map events
            MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            //   MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);
            //  MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
            MainMap.OnMapTypeChanged += new MapTypeChanged(MainMap_OnMapTypeChanged);
            //    MainMap.MouseMove += new System.Windows.Input.MouseEventHandler(MainMap_MouseMove);
            MainMap.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(MainMap_MouseLeftButtonDown);
            //  MainMap.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(clickCurrentMarker);
            MainMap.Loaded += new RoutedEventHandler(MainMap_Loaded);
            //MainMap.MouseEnter += new MouseEventHandler(MainMap_MouseEnter);
            MainMap.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(MainMap_MouseWheelScroll);
            //map.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(Map1_MouseWheelScroll);
            //map.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(Map_DoubleClick);
            //   CabList1.MouseDoubleClick += new MouseButtonEventHandler(Cablist_DoubleClick);
            //    CabList1.MouseRightButtonDown += new MouseButtonEventHandler(Delete_Cab);
            //     DeviceList.MouseDoubleClick += new MouseButtonEventHandler(Devicelist_DoubleClick);
            //   initGrid2(RedMarkerCickPoint);
            zoomSlider.Maximum = MainMap.MaxZoom;
            zoomSlider.Minimum = MainMap.MinZoom;

            // perfromance test
            timer.Interval = TimeSpan.FromMilliseconds(44);
            timer.Tick += new EventHandler(timer_Tick);

            // transport demo
            //transport.DoWork += new DoWorkEventHandler(transport_DoWork);
            //transport.ProgressChanged += new ProgressChangedEventHandler(transport_ProgressChanged);
            //transport.WorkerSupportsCancellation = true;
            //transport.WorkerReportsProgress = true;


            showLevel = SHOW_LEVEl.SHOW_CABS;
            home.MouseLeftButtonDown += new MouseButtonEventHandler(home_btn_click);
            back.MouseLeftButtonDown += new MouseButtonEventHandler(back_btn_click);
            next.MouseLeftButtonDown += new MouseButtonEventHandler(next_btn_click);
            page.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            initPoints();
            initGrid1();
            mc.notifyUpdateUI += new UpdateUI(Map_notifyUpdateUI);
            
            
                 
        }

       private  void Map_notifyUpdateUI()
        {
            Console.WriteLine("更新界面！");
        }
        /// <summary>
        /// 初始化所有点
        /// </summary>
        private void initPoints()
        {
            //buildings =
            
            //markers = new GMapMarker[4];
            ////208楼
            //markers[0] = new GMapMarker(new PointLatLng(31.540871, 104.804598));
            //markers[0].Tag = "208";

            //markers[1] = new GMapMarker(new PointLatLng(31.502461, 104.769235));
            //markers[1].Tag = "201";

            //markers[2] = new GMapMarker(new PointLatLng(31.558144, 104.822064));
            //markers[2].Tag = "208烟囱";

            //markers[3] = new GMapMarker(new PointLatLng(31.539865, 104.803595));
            //markers[2].Tag = "208室A楼";

        }
        void MainMap_MouseEnter(object sender, MouseEventArgs e)
        {
            MainMap.Focus();
        }

        #region -- performance test--
        public RenderTargetBitmap ToImageSource(FrameworkElement obj)
        {
            // Save current canvas transform
            Transform transform = obj.LayoutTransform;
            obj.LayoutTransform = null;

            // fix margin offset as well
            Thickness margin = obj.Margin;
            obj.Margin = new Thickness(0, 0, margin.Right - margin.Left, margin.Bottom - margin.Top);

            // Get the size of canvas
            System.Windows.Size size = new System.Windows.Size(obj.Width, obj.Height);

            // force control to Update
            obj.Measure(size);
            obj.Arrange(new Rect(size));

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(obj);

            if (bmp.CanFreeze)
            {
                bmp.Freeze();
            }

            // return values as they were before
            obj.LayoutTransform = transform;
            obj.Margin = margin;

            return bmp;
        }

        double NextDouble(Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }

        Random r = new Random();

        int tt = 0;
        void timer_Tick(object sender, EventArgs e)
        {
            var pos = new PointLatLng(NextDouble(r, MainMap.CurrentViewArea.Top, MainMap.CurrentViewArea.Bottom), NextDouble(r,                                 MainMap.CurrentViewArea.Left, MainMap.CurrentViewArea.Right));
            GMapMarker m = new GMapMarker(pos);
            {
                var s = new Test((tt++).ToString());

                var image = new Image();
                {
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.LowQuality);
                    image.Stretch = Stretch.None;
                    image.Opacity = s.Opacity;

                    image.MouseEnter += new System.Windows.Input.MouseEventHandler(image_MouseEnter);
                    image.MouseLeave += new System.Windows.Input.MouseEventHandler(image_MouseLeave);

                    image.Source = ToImageSource(s);
                }

                m.Shape = image;

                m.Offset = new System.Windows.Point(-s.Width, -s.Height);
            }
            MainMap.Markers.Add(m);

            if (tt >= 333)
            {
                timer.Stop();
                tt = 0;
            }
        }

        void image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = sender as Image;
            img.RenderTransform = null;
        }

        void image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = sender as Image;
            img.RenderTransform = new ScaleTransform(1.2, 1.2, 12.5, 12.5);
        }

        DispatcherTimer timer = new DispatcherTimer();
        #endregion

        #region -- transport demo --
        BackgroundWorker transport = new BackgroundWorker();

        readonly List<VehicleData> trolleybus = new List<VehicleData>();
        readonly Dictionary<int, GMapMarker> trolleybusMarkers = new Dictionary<int, GMapMarker>();

        readonly List<VehicleData> bus = new List<VehicleData>();
        readonly Dictionary<int, GMapMarker> busMarkers = new Dictionary<int, GMapMarker>();

        bool firstLoadTrasport = true;

        void transport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lock (trolleybus)
            {
                foreach (VehicleData d in trolleybus)
                {
                    GMapMarker marker;

                    if (!trolleybusMarkers.TryGetValue(d.Id, out marker))
                    {
                        marker = new GMapMarker(new PointLatLng(d.Lat, d.Lng));
                        marker.Tag = d.Id;
                        marker.Shape = new CircleVisual(marker, Brushes.Red);

                        trolleybusMarkers[d.Id] = marker;
                        MainMap.Markers.Add(marker);
                    }
                    else
                    {
                        marker.Position = new PointLatLng(d.Lat, d.Lng);
                        var shape = (marker.Shape as CircleVisual);
                        {
                            shape.Text = d.Line;
                            shape.Angle = d.Bearing;
                            shape.Tooltip.SetValues("TrolleyBus", d);

                            if (shape.IsChanged)
                            {
                                shape.UpdateVisual(false);
                            }
                        }
                    }
                }
            }

            lock (bus)
            {
                foreach (VehicleData d in bus)
                {
                    GMapMarker marker;

                    if (!busMarkers.TryGetValue(d.Id, out marker))
                    {
                        marker = new GMapMarker(new PointLatLng(d.Lat, d.Lng));
                        marker.Tag = d.Id;

                        var v = new CircleVisual(marker, Brushes.Blue);
                        {
                            v.Stroke = new Pen(Brushes.Gray, 2.0);
                        }
                        marker.Shape = v;

                        busMarkers[d.Id] = marker;
                        MainMap.Markers.Add(marker);
                    }
                    else
                    {
                        marker.Position = new PointLatLng(d.Lat, d.Lng);
                        var shape = (marker.Shape as CircleVisual);
                        {
                            shape.Text = d.Line;
                            shape.Angle = d.Bearing;
                            shape.Tooltip.SetValues("Bus", d);

                            if (shape.IsChanged)
                            {
                                shape.UpdateVisual(false);
                            }
                        }
                    }
                }
            }

            if (firstLoadTrasport)
            {
                firstLoadTrasport = false;
            }
        }

        //void transport_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    while (!transport.CancellationPending)
        //    {
        //        try
        //        {
        //            lock (trolleybus)
        //            {
        //                MainMap.Manager.GetVilniusTransportData(TransportType.TrolleyBus, string.Empty, trolleybus);
        //            }

        //            lock (bus)
        //            {
        //                MainMap.Manager.GetVilniusTransportData(TransportType.Bus, string.Empty, bus);
        //            }

        //            transport.ReportProgress(100);
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine("transport_DoWork: " + ex.ToString());
        //        }
        //        Thread.Sleep(3333);
        //    }
        //    trolleybusMarkers.Clear();
        //    busMarkers.Clear();
        //}

        #endregion

        // add objects and zone around them
        void AddDemoZone(double areaRadius, PointLatLng center, List<PointAndInfo> objects)
        {
            var objectsInArea = from p in objects
                                where GMaps.Instance.GetDistance(center, p.Point) <= areaRadius
                                select new
                                {
                                    Obj = p,
                                    Dist = GMaps.Instance.GetDistance(center, p.Point)
                                };
            if (objectsInArea.Any())
            {
                var maxDistObject = (from p in objectsInArea
                                     orderby p.Dist descending
                                     select p).First();

                // add objects to zone
                //foreach (var o in objectsInArea)
                //{
                //    GMapMarker it = new GMapMarker(o.Obj.Point);
                //    {
                //        it.ZIndex = 55;
                //        //var s = new CustomMarkerDemo(this, it, o.Obj.Info + ", distance from center: " + o.Dist + "km.");
                //        var s = new PositionMarker(this, it, new TrolleyTooltip(),false);
                //        it.Shape = s;
                //    }

                //    MainMap.Markers.Add(it);
                //}

            }
        }

        // center markers on load
        void MainMap_Loaded(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomAndCenterMarkers(null);
            
        }

        void MainMap_OnMapTypeChanged(MapType type)
        {
            //   sliderZoom.Minimum = MainMap.MinZoom;5
            //     sliderZoom.Maximum = MainMap.MaxZoom;
            Console.WriteLine("MainMap_OnMapTypeChanged:" + MainMap.MinZoom + " ; " + MainMap.MaxZoom);
        }

        void MainMap_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(MainMap);
            //currentMarker.Position = MainMap.FromLocalToLatLng((int) p.X, (int) p.Y);
            RedMarkerCickPoint = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
            Console.WriteLine("MainMap_MouseLeftButtonDown");
        }

        // move current marker with left holding
        void MainMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                System.Windows.Point p = e.GetPosition(MainMap);
                currentMarker.Position = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
            }
        }

        // zoo max & center markers
        private void button13_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomAndCenterMarkers(null);
        }

        // current location changed
        void MainMap_OnCurrentPositionChanged(PointLatLng point)
        {
            mapgroup.Header = "gmap: " + point;
            Console.WriteLine("头部的经纬度：" + point);
        }

        // reload
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ReloadMap();
        }
 

        // enable map dragging
        private void checkBoxDragMap_Checked(object sender, RoutedEventArgs e)
        {
            MainMap.CanDragMap = true;
        }

        // disable map dragging
        private void checkBoxDragMap_Unchecked(object sender, RoutedEventArgs e)
        {
            MainMap.CanDragMap = false;
        }

 

        // zoom up
        private void czuZoomUp_Click(object sender, RoutedEventArgs e)
        {
            //if(MainMap.Zoom==MainMap.MinZoom)
            //{
            //    MainMap.Zoom = MainMap.MaxZoom;
            //}
            //Console.WriteLine("map_zoom"+MainMap.Zoom);

            if (MainMap.Zoom < MainMap.MaxZoom)
            {
                MainMap.Zoom += 1;
                //     zoomSlider.SetValue( MainMap.Zoom);
            }
            Console.WriteLine("map_zoom" + MainMap.Zoom);
        }

        // zoom down
        private void czuZoomDown_Click(object sender, RoutedEventArgs e)
        {
            if (MainMap.Zoom > MainMap.MinZoom)
            {
                MainMap.Zoom -= 1;
                //      zoomSlider.SetValue(MainMap.Zoom);
            }
            Console.WriteLine("map_zoom" + MainMap.Zoom);
        }


        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int offset = 22;

            if (MainMap.IsFocused)
            {
                if (e.Key == Key.Left)
                {
                    MainMap.Offset(-offset, 0);
                }
                else if (e.Key == Key.Right)
                {
                    MainMap.Offset(offset, 0);
                }
                else if (e.Key == Key.Up)
                {
                    MainMap.Offset(0, -offset);
                }
                else if (e.Key == Key.Down)
                {
                    MainMap.Offset(0, offset);
                }
                else if (e.Key == Key.Add)
                {
                    czuZoomUp_Click(null, null);
                }
                else if (e.Key == Key.Subtract)
                {
                    czuZoomDown_Click(null, null);
                }
            }
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        /// <summary>
        ///根据获取的点初始化显示界面
        /// </summary>
        /// <param name="point"></param>

        //初始化一级页面
        private void initGrid1()
        {
            
            MainMap.CurrentPosition = new PointLatLng(30.67, 104.06);
            //MainMap.MaxZoom = 14;
            //MainMap.MinZoom = 12;
            MainMap.MaxZoom = 20;
            MainMap.MinZoom = 12;
            MainMap.Zoom = 12;
            MainMap.MapType = MapType.GoogleHybridChina;
            MainMap.Manager.Mode = AccessMode.ServerAndCache;
            MainMap.BoundsOfMap = new RectLatLng(31.540871, 104.804598, 0.025, 0.018); //北纬30.67度，东经104.06度。
            MainMap.CanDragMap = true;


            currentMarker = new GMapMarker(new PointLatLng(31.540871, 104.804598));
            {
                TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
                // trolleyToolTip.setStatus("异常");
                currentMarker.Shape = new PositionMarker(this, currentMarker, trolleyToolTip , new Building());
                currentMarker.Offset = new System.Windows.Point(0, 0);
                currentMarker.ZIndex = int.MaxValue;
                MainMap.Markers.Add(currentMarker);


                for (int i = 0; i < markers.Length; i++)
                {
                    if (i == 2)
                    {
                        trolleyToolTip = new TrolleyTooltip();
                        markers[i].Shape = new  PositionMarker(this, currentMarker, trolleyToolTip , new Building());
                    }
                    else
                    {
                        trolleyToolTip = new TrolleyTooltip();
                        markers[i].Shape = new PositionMarker(this, currentMarker, trolleyToolTip , new Building());
                    }
                    markers[i].Shape.MouseDown += new MouseButtonEventHandler(markerClick);
                    markers[i].Offset = new System.Windows.Point(0, 0);
                    markers[i].ZIndex = int.MaxValue;
                    MainMap.Markers.Add(markers[i]);
                }
            }
        }

        public void markerClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("233333");
            grid1.Visibility = System.Windows.Visibility.Hidden;
            grid2.Visibility = System.Windows.Visibility.Visible;

            System.Windows.Point p = e.GetPosition(MainMap);
            PointLatLng point = MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
            initGrid2(point);
        }


        //初始化二级页面
        private void initGrid2(PointLatLng point)
        {
            map.CurrentPosition = point;
            map.MaxZoom = 10;
            map.MinZoom = 10;
            map.Zoom = 10;
            map.MapType = MapType.GoogleHybridChina;
            map.Manager.Mode = AccessMode.ServerAndCache;
            map.BoundsOfMap = new RectLatLng(point.Lat, point.Lng, 0.025, 0.018); //北纬30.67度，东经104.06度。
            map.CanDragMap = true;

            currentMarker = new GMapMarker(point);
            {
                TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
                currentMarker.Shape = new PositionMarker(this, currentMarker, trolleyToolTip, new Building());
                currentMarker.Offset = new System.Windows.Point(0, 0);
                currentMarker.ZIndex = int.MaxValue;
                //map.Markers.Add(currentMarker);

                for (int i = 0; i < markers.Length; i++)
                {
                    trolleyToolTip = new TrolleyTooltip();
                    if (i == 2)
                    {
                        markers[i].Shape = new PositionMarker(this, markers[i], trolleyToolTip, new Building());
                    }
                    else
                    {
                        markers[i].Shape = new PositionMarker(this, markers[i], trolleyToolTip, new Building());
                    }
                    markers[i].Offset = new System.Windows.Point(0, 0);
                    markers[i].ZIndex = int.MaxValue;
                    map.Markers.Add(markers[i]);
                }
            }

           
        //    DeviceDetailPage deviceDetailPage = new DeviceDetailPage();
            CabsPage cabspage = new CabsPage();
            page.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Visible;
            page.Content = cabspage;
        }
        //初始化二级页面
        private void initGrid2(PointLatLng point, String tag)
        {
            map.CurrentPosition = point;
            map.MaxZoom = 10;
            map.MinZoom = 10;
            map.Zoom = 10;
            map.MapType = MapType.GoogleHybridChina;
            map.Manager.Mode = AccessMode.ServerAndCache;
            map.BoundsOfMap = new RectLatLng(point.Lat, point.Lng, 0.025, 0.018); //北纬30.67度，东经104.06度。
            map.CanDragMap = true;

            currentMarker = new GMapMarker(point);
            {
                TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
                currentMarker.Shape = new PositionMarker(this, currentMarker, trolleyToolTip, new Building());
                currentMarker.Offset = new System.Windows.Point(0, 0);
                currentMarker.ZIndex = int.MaxValue;
                //map.Markers.Add(currentMarker);

                for (int i = 0; i < markers.Length; i++)
                {
                    trolleyToolTip = new TrolleyTooltip();
                    if (i == 2)
                    {
                        markers[i].Shape = new PositionMarker(this, markers[i], trolleyToolTip, new Building());
                    }
                    else
                    {
                        markers[i].Shape = new PositionMarker(this, markers[i], trolleyToolTip, new Building());
                    }

                    markers[i].Offset = new System.Windows.Point(0, 0);
                    markers[i].ZIndex = int.MaxValue;
                    map.Markers.Add(markers[i]);
                }
            }

            cabId_cab.Clear();
            if (GlobalMapForShow.whiteLists.Contains(tag))
            {
                foreach (String name in GlobalMapForShow.globalMapWithCab.Keys)
                {
                    CabUI cab = new CabUI(name);
                    cab.Uid = name;
                 //   cab.setOnCabCloseListner(this);
                 //   CabList1.Items.Add(cab);
                    cabId_cab.Add(name, cab);
                }
            }
            else
            {
                foreach (String name in GlobalMapForShow.globalMapWithoutCab.Keys)
                {
                    List<DeviceDataBox> deviceList = GlobalMapForShow.globalMapWithoutCab[name];
                    foreach (DeviceDataBox deviceData in deviceList)
                    {
                        DeviceUI device = new DeviceUI(deviceData);
                    //    device.setOnDeviceEventListner(this);
                    //    DeviceList.Items.Add(device);
                        devId_dev.Add(deviceData.devId, device);
                    }
                }
            }
       //     mc.DataUpdate += UpdatedGrid;
         //   DeviceDetailPage deviceDetailPage = new DeviceDetailPage();
        //    page.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Visible;
       //     page.Content = deviceDetailPage;
        }

        /// <summary>
        /// 柜子层的更新显示
        /// </summary>
        public void UpdatedGrid(String systemId)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new UpdatedToWindowEvent(UpdateGridToWindows), systemId);
        }
        /// <summary>
        /// 更新到界面
        /// </summary>
        public void UpdateGridToWindows(String systemId)
        {
            try
            {
                if (GlobalMapForShow.whiteLists.Contains(systemId))
                {

                    foreach (String name in GlobalMapForShow.globalMapWithCab.Keys)
                    {

                        List<DeviceDataBox_Base> devdatas = GlobalMapForShow.globalMapWithCab[name];
                        foreach (DeviceDataBox_Base devdata in devdatas)
                        {
                            if (cabId_cab.ContainsKey(name))
                            {
                                //cabId_cab[name].textBlock2.Text = devdata.value.ToString();
                                //cabId_cab[name].textBlock3.Text = devdata.value.ToString();
                                //cabId_cab[name].textBlock4.Text = devdata.value.ToString();
                            }
                        }
                    }
                }
                else
                {
                    foreach (String name in GlobalMapForShow.globalMapWithoutCab.Keys)
                    {

                        List<DeviceDataBox> devdatas = GlobalMapForShow.globalMapWithoutCab[name];
                        foreach (DeviceDataBox devdata in devdatas)
                        {
                            //cabId_cab[name].textBlock2.Text = devdata.value.ToString();
                            //cabId_cab[name].textBlock3.Text = devdata.value.ToString();
                            //cabId_cab[name].textBlock4.Text = devdata.value.ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void Map_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            grid1.Visibility = System.Windows.Visibility.Visible;
            grid2.Visibility = System.Windows.Visibility.Hidden;
            //initGrid1();
            Console.WriteLine("小地图点击双击，回到一级页面！");
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
        void Map1_MouseWheelScroll(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            //10
            map.Zoom = 10;
            Console.WriteLine("map zoom:" + map.Zoom);
        }


       // void Cab.OnCabEventListner.onCabClose(Cab cab)
       // {
       //     Console.WriteLine("点击了柜子关闭按钮");
       //   //  CabList1.Items.Remove(cab);
       //     cabId_cab.Clear();
       //  //   mc.DataUpdate -= UpdatedGrid;
       // }

       // void Device.OnDeviceCloseListner.onDeviceClose(Device cab)
       // {
       //     Console.WriteLine("点击了设别关闭按钮");
       //     devId_dev.Clear();
       // //    mc.DataUpdate -= UpdatedDeviceGrid;
       // }

       // void Cab.OnCabEventListner.onCabShowDevice(Cab cab)
       // {
       //     //Console.WriteLine("进入cab，查看device");
       //     //devId_dev.Clear();
       //     //DeviceList.Items.Clear();
       //     //switchVisibility(DeviceList);
       //     //cabName = cab.getCabName();
       //     //List<DeviceDataBox_Comp> deviceList = GlobalMapForShow.globalMapWithCab[cabName];
       //     //foreach (DeviceDataBox_Comp deviceData in deviceList)
       //     //{
       //     //    Device device = new Device(deviceData);
       //     //    device.setOnDeviceEventListner(this);
       //     //    DeviceList.Items.Add(device);
       //     //    devId_dev.Add(deviceData.devId, device);
       //     //}
       ////     mc.DataUpdate += UpdatedDeviceGrid;

       // }
        /// <summary>
        /// 设备层的更新显示
        /// </summary>
        public void UpdatedDeviceGrid(String systemId)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new UpdatedToWindowEvent(UpdateDeviceGridToWindows), systemId);
        }
        /// <summary>
        /// 更新到界面
        /// </summary>
        public void UpdateDeviceGridToWindows(String systemId)
        {
            try
            {
                if (GlobalMapForShow.whiteLists.Contains(systemId))
                {
                    List<DeviceDataBox_Base> devdatas = GlobalMapForShow.globalMapWithCab["2_3"];
                    foreach (DeviceDataBox_Base devdata in devdatas)
                    {
                   //     devId_dev[devdata.devId].state.Text = devdata.state.ToString();
                        //devId_dev[devdata.devId].realValue.Text = devdata.value;
                  //      devId_dev[devdata.devId].unit.Text = devdata.unit;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        void home_btn_click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("点了home按钮！");
            grid1.Visibility = System.Windows.Visibility.Visible;
            grid2.Visibility = System.Windows.Visibility.Hidden;
         //   initGrid1();
            cabId_cab.Clear();
      //      mc.DataUpdate -= UpdatedGrid;
            devId_dev.Clear();
      //      mc.DataUpdate -= UpdatedDeviceGrid;
        }

        void back_btn_click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("点了back按钮！");
            switch (showLevel)
            {
                case SHOW_LEVEl.SHOW_CABS:
               //     switchVisibility(CabList1);
                    break;
                case SHOW_LEVEl.SHOW_DEVICE:
              //      switchVisibility(CabList1);
                    break;
                case SHOW_LEVEl.SHOW_DEVICE_INFO:
            //        switchVisibility(DeviceList);
                    break;
                default:
                    break;
            }

            Console.WriteLine("show level" + showLevel.ToString());
        }
        private enum SHOW_LEVEl
        {
            SHOW_CABS, //显示的是柜子
            SHOW_DEVICE, //显示的是设别
            SHOW_DEVICE_INFO //显示的是设备信息
        } ;

        void switchVisibility(SHOW_LEVEl l)
        {
            if (l == SHOW_LEVEl.SHOW_CABS)
            {

            }
 
        }
        void switchVisibility(Object view)
        {
            //if (view == CabList1)
            //{
            //    CabList1.Visibility = System.Windows.Visibility.Visible;
            //    DeviceList.Visibility = System.Windows.Visibility.Hidden;
            //    deviceInfo.Visibility = System.Windows.Visibility.Hidden;
            //    showLevel = SHOW_LEVEl.SHOW_CABS;
            //}
            //else if (view == DeviceList)
            //{
            //    DeviceList.Visibility = System.Windows.Visibility.Visible;
            //    CabList1.Visibility = System.Windows.Visibility.Hidden;
            //    deviceInfo.Visibility = System.Windows.Visibility.Hidden;
            //    showLevel = SHOW_LEVEl.SHOW_DEVICE;
            //    grid3.Children.Remove(deviceInfoPanel);
            //}
            //else if (view == deviceInfo)
            //{
            //    //  deviceInfo.Visibility = System.Windows.Visibility.Visible;
            //    DeviceList.Visibility = System.Windows.Visibility.Hidden;
            //    CabList1.Visibility = System.Windows.Visibility.Hidden;
            //    deviceInfoPanel = new DeviceInfoPanel();
            //    grid3.Children.Remove(deviceInfo);
            //    grid3.Children.Add(deviceInfoPanel);
            //    Grid.SetRow(deviceInfoPanel, 1);
            //    Grid.SetColumn(deviceInfoPanel, 0);
            //    //HistoryPanel test = new HistoryPanel();
            //    //grid3.Children.Remove(deviceInfo);
            //    //grid3.Children.Add(test);
            //    //Grid.SetRow(test, 1);
            //    //Grid.SetColumn(test, 0);
            //    //showLevel = SHOW_LEVEl.SHOW_DEVICE_INFO;
            //}
        }

        void next_btn_click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("点了next按钮！");
            switch (showLevel)
            {
                case SHOW_LEVEl.SHOW_CABS:
                   //  switchVisibility(DeviceList);
                    break;
                case SHOW_LEVEl.SHOW_DEVICE:
                   //  switchVisibility(deviceInfo);
                    break;
                case SHOW_LEVEl.SHOW_DEVICE_INFO:
                  //  switchVisibility(DeviceList);
                    break;
                default:
                    break;
            }
        }
 

        //void Device.OnDeviceCloseListner.onDeviceDoubleClick(Device device)
        //{
        //    Console.WriteLine("显示一个设备的详细信息！");
        // //   switchVisibility(deviceInfo);
        //    //DeviceDataBox_Comp data = device.getDeviceData() ;
        //    //devID.Text = data.devId;
        //    //cabId.Text = data.cabId;
        //    //test_para.Text = data.paraMeasure;
        //    //exec_para.Text = data.paraExec;
        //    //real_time_value.Text = data.value;
        //    //if (data.state == WpfApplication2.Controller.DeviceDataBox_Comp.State.Normal)
        //    //{
        //    //      state.Text = "正常";
        //    //}
        //    //else if (data.state == WpfApplication2.Controller.DeviceDataBox_Comp.State.NetError)
        //    //{
        //    //    state.Text = "网络异常";
        //    //}
        //    //else if (data.state == WpfApplication2.Controller.DeviceDataBox_Comp.State.DevError)
        //    //{
        //    //    state.Text = "设备异常";
        //    //}
        //    //unit.Text = data.unit;
        //}

        public void Map_dataReceivedEvent()
        { 
            
        }
     
    }
}
