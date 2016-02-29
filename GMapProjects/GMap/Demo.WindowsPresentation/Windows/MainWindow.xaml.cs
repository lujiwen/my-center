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
using Demo.WindowsPresentation.CustomMarkers;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using Demo.WindowsPresentation.Controls;
using Demo.WindowsPresentation.Model ;
using Demo.WindowsPresentation.Presatation.CabsWindow;


namespace Demo.WindowsPresentation
{
    public partial class MainWindow : Window, Demo.WindowsPresentation.CustomMarkers.CustomMarkerRed.onRedMarkerClickLisener, Demo.WindowsPresentation.CustomMarkers.CustomMarkerGreen.onGreenMarkerClickListener
   {
      //PointLatLng start;
      //PointLatLng end;

       // marker
       GMapMarker currentMarker; 
        // marker1, marker2, marker3, marker4;
       GMapMarker[] markers; 
      // zones list
       List<GMapMarker> Circles = new List<GMapMarker>();
       PointLatLng RedMarkerCickPoint;

      public MainWindow()
      {
         InitializeComponent();
         // map events
         MainMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
         MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);
         MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
         MainMap.OnMapTypeChanged += new MapTypeChanged(MainMap_OnMapTypeChanged);
   //    MainMap.MouseMove += new System.Windows.Input.MouseEventHandler(MainMap_MouseMove);
         MainMap.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(MainMap_MouseLeftButtonDown);
     //  MainMap.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(clickCurrentMarker);
         MainMap.Loaded += new RoutedEventHandler(MainMap_Loaded);
       //MainMap.MouseEnter += new MouseEventHandler(MainMap_MouseEnter);
         MainMap.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(MainMap_MouseWheelScroll);
         map.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(Map1_MouseWheelScroll); 
         map.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(Map_DoubleClick);
         CabList.MouseDoubleClick += new  MouseButtonEventHandler(Cablist_DoubleClick);
         CabList.MouseRightButtonDown += new MouseButtonEventHandler(Delete_Cab);


         RedMarkerCickPoint = new PointLatLng();
         initGrid1();
         initGrid2(RedMarkerCickPoint);   
         zoomSlider.Maximum = MainMap.MaxZoom;
         zoomSlider.Minimum = MainMap.MinZoom;

         //if(false)
         {
            // add my city location for demo
            GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;

            PointLatLng? city = GMaps.Instance.GetLatLngFromGeocoder("Lithuania, Vilnius", out status);
            if(city != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            {
               GMapMarker it = new GMapMarker(city.Value);
               {
                  it.ZIndex = 55;
                    TrolleyTooltip trolleyToolTip1 = new TrolleyTooltip();
                    it.Shape = new CustomMarkerGreen(this, it, trolleyToolTip1);
               }
               MainMap.Markers.Add(it);

               #region -- add some markers and zone around them --
               {
                  List<PointAndInfo> objects = new List<PointAndInfo>();
                  {
                     string area = "Antakalnis";
                     PointLatLng? pos = GMaps.Instance.GetLatLngFromGeocoder("Lithuania, Vilnius, " + area, out status);
                     if(pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
                     {
                        objects.Add(new PointAndInfo(pos.Value, area));
                     }
                  }
                  {
                     string area = "Senamiestis";
                     PointLatLng? pos = GMaps.Instance.GetLatLngFromGeocoder("Lithuania, Vilnius, " + area, out status);
                     if(pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
                     {
                        objects.Add(new PointAndInfo(pos.Value, area));
                     }
                  }
                  {
                     string area = "Pilaite";
                     PointLatLng? pos = GMaps.Instance.GetLatLngFromGeocoder("Lithuania, Vilnius, " + area, out status);
                     if(pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
                     {
                        objects.Add(new PointAndInfo(pos.Value, area));
                     }
                  }
                  AddDemoZone(8.8, city.Value, objects);
               }
               #endregion
            }
         }

         // perfromance test
         timer.Interval = TimeSpan.FromMilliseconds(44);
         timer.Tick += new EventHandler(timer_Tick);

         // transport demo
         transport.DoWork += new DoWorkEventHandler(transport_DoWork);
         transport.ProgressChanged += new ProgressChangedEventHandler(transport_ProgressChanged);
         transport.WorkerSupportsCancellation = true;
         transport.WorkerReportsProgress = true;
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

         RenderTargetBitmap bmp = new RenderTargetBitmap((int) size.Width, (int) size.Height, 96, 96, PixelFormats.Pbgra32);
         bmp.Render(obj);

         if(bmp.CanFreeze)
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
         var pos = new PointLatLng(NextDouble(r, MainMap.CurrentViewArea.Top, MainMap.CurrentViewArea.Bottom), NextDouble(r, MainMap.CurrentViewArea.Left, MainMap.CurrentViewArea.Right));
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

         if(tt >= 333)
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
         lock(trolleybus)
         {
            foreach(VehicleData d in trolleybus)
            {
               GMapMarker marker;

               if(!trolleybusMarkers.TryGetValue(d.Id, out marker))
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

                     if(shape.IsChanged)
                     {
                        shape.UpdateVisual(false);
                     }
                  }
               }
            }
         }

         lock(bus)
         {
            foreach(VehicleData d in bus)
            {
               GMapMarker marker;

               if(!busMarkers.TryGetValue(d.Id, out marker))
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

                     if(shape.IsChanged)
                     {
                        shape.UpdateVisual(false);
                     }
                  }
               }
            }
         }

         if(firstLoadTrasport)
         {
            firstLoadTrasport = false;
         }
      }

      void transport_DoWork(object sender, DoWorkEventArgs e)
      {
         while(!transport.CancellationPending)
         {
            try
            {
               lock(trolleybus)
               {
                  MainMap.Manager.GetVilniusTransportData(TransportType.TrolleyBus, string.Empty, trolleybus);
               }

               lock(bus)
               {
                  MainMap.Manager.GetVilniusTransportData(TransportType.Bus, string.Empty, bus);
               }

               transport.ReportProgress(100);
            }
            catch(Exception ex)
            {
               Debug.WriteLine("transport_DoWork: " + ex.ToString());
            }
            Thread.Sleep(3333);
         }
         trolleybusMarkers.Clear();
         busMarkers.Clear();
      }

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
         if(objectsInArea.Any())
         {
            var maxDistObject = (from p in objectsInArea
                                 orderby p.Dist descending
                                 select p).First();

            // add objects to zone
            foreach(var o in objectsInArea)
            {
               GMapMarker it = new GMapMarker(o.Obj.Point);
               {
                  it.ZIndex = 55;
                  //var s = new CustomMarkerDemo(this, it, o.Obj.Info + ", distance from center: " + o.Dist + "km.");
                  var s = new CustomMarkerGreen(this, it, new TrolleyTooltip() );
                  it.Shape = s;
               }

               MainMap.Markers.Add(it);
            }

            // add zone circle
            {
               GMapMarker it = new GMapMarker(center);
               it.ZIndex = -1;

               Circle c = new Circle();
               c.Center = center;
               c.Bound = maxDistObject.Obj.Point;
               c.Tag = it;
               c.IsHitTestVisible = false;

               UpdateCircle(c);
               Circles.Add(it);

               it.Shape = c;
               MainMap.Markers.Add(it);
            }
         }
      }

      // calculates circle radius
      void UpdateCircle(Circle c)
      {
         var pxCenter = MainMap.FromLatLngToLocal(c.Center);
         var pxBounds = MainMap.FromLatLngToLocal(c.Bound);

         double a = (double) (pxBounds.X - pxCenter.X);
         double b = (double) (pxBounds.Y - pxCenter.Y);
         var pxCircleRadius = Math.Sqrt(a * a + b * b);

         c.Width = 55 + pxCircleRadius * 2;
         c.Height = 55 + pxCircleRadius * 2;
         (c.Tag as GMapMarker).Offset = new System.Windows.Point(-c.Width / 2, -c.Height / 2);
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
         if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
         {
            System.Windows.Point p = e.GetPosition(MainMap);
            currentMarker.Position = MainMap.FromLocalToLatLng((int) p.X, (int) p.Y);
         }
      }

      // zoo max & center markers
      private void button13_Click(object sender, RoutedEventArgs e)
      {
         MainMap.ZoomAndCenterMarkers(null);
      } 

      // tile louading starts
      void MainMap_OnTileLoadStart()
      {
         System.Windows.Forms.MethodInvoker m = delegate()
         {
          //  progressBar1.Visibility = Visibility.Visible;
         };

         try
         {
             this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, m);
         }
         catch
         {
         }
      } 

      // tile loading stops
      void MainMap_OnTileLoadComplete(long ElapsedMilliseconds)
      {
         MainMap.ElapsedMilliseconds = ElapsedMilliseconds;

         System.Windows.Forms.MethodInvoker m = delegate()
         {
         //   progressBar1.Visibility = Visibility.Hidden;
        //    groupBox3.Header = "loading, last in " + MainMap.ElapsedMilliseconds + "ms";
         };

         try
         {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, m);
         }
         catch
         {
         }
      }

      // current location changed
      void MainMap_OnCurrentPositionChanged(PointLatLng point)
      {
         mapgroup.Header = "gmap: " + point;
         Console.WriteLine("头部的经纬度："+ point);
      }

      // reload
      private void button1_Click(object sender, RoutedEventArgs e)
      {
         MainMap.ReloadMap();
      }

      // enable current marker
      private void checkBoxCurrentMarker_Checked(object sender, RoutedEventArgs e)
      {
         if(currentMarker != null)
         {
            MainMap.Markers.Add(currentMarker);
         }
      }

      // disable current marker
      private void checkBoxCurrentMarker_Unchecked(object sender, RoutedEventArgs e)
      {
         if(currentMarker != null)
         {
            MainMap.Markers.Remove(currentMarker);
         }
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
 
      // zoom changed
      private void sliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
         // updates circles on map
         foreach(var c in Circles)
         {
            UpdateCircle(c.Shape as Circle);
         }
      }

      // zoom up
      private void czuZoomUp_Click(object sender, RoutedEventArgs e)
      {
          //if(MainMap.Zoom==MainMap.MinZoom)
          //{
          //    MainMap.Zoom = MainMap.MaxZoom;
          //}
          //Console.WriteLine("map_zoom"+MainMap.Zoom);

          if (MainMap.Zoom <MainMap.MaxZoom)
          {
              MainMap.Zoom += 1  ;
         //     zoomSlider.SetValue( MainMap.Zoom);
          }
          Console.WriteLine("map_zoom" + MainMap.Zoom);
      }

      // zoom down
      private void czuZoomDown_Click(object sender, RoutedEventArgs e)
      {
          if(MainMap.Zoom>MainMap.MinZoom)
          {
              MainMap.Zoom -= 1;
        //      zoomSlider.SetValue(MainMap.Zoom);
          }
          Console.WriteLine("map_zoom"+MainMap.Zoom);
      }
 

      private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
      {
         int offset = 22;

         if(MainMap.IsFocused)
         {
            if(e.Key == Key.Left)
            {
               MainMap.Offset(-offset, 0);
            }
            else if(e.Key == Key.Right)
            {
               MainMap.Offset(offset, 0);
            }
            else if(e.Key == Key.Up)
            {
               MainMap.Offset(0, -offset);
            }
            else if(e.Key == Key.Down)
            {
               MainMap.Offset(0, offset);
            }
            else if(e.Key == Key.Add)
            {
               czuZoomUp_Click(null, null);
            }
            else if(e.Key == Key.Subtract)
            {
               czuZoomDown_Click(null, null);
            }
         }
      }

      private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {

      }


      public void onRedMarkerclick(PointLatLng point)
      {
          grid1.Visibility = System.Windows.Visibility.Collapsed;
          grid2.Visibility = System.Windows.Visibility.Visible;
          initGrid2(point);
          Console.WriteLine("点击了marker,切换到第二级页面！");
      }
      public void onGreenMarkerClick(PointLatLng point)
       {
           grid1.Visibility = System.Windows.Visibility.Collapsed;
           grid2.Visibility = System.Windows.Visibility.Visible;
           initGrid2(point);
           System.Console.WriteLine("绿色marker ,被点击了一下");
       }

     //初始化一级页面
      private void initGrid1( )
      {
          MainMap.CurrentPosition = new PointLatLng(30.67, 104.06);
          MainMap.MaxZoom = 14;
          MainMap.MinZoom = 12;
          MainMap.Zoom = 12;
          MainMap.MapType = MapType.GoogleHybridChina;
          MainMap.Manager.Mode = AccessMode.CacheOnly;
          MainMap.BoundsOfMap = new RectLatLng(30.67, 104.06, 0.025, 0.018); //北纬30.67度，东经104.06度。
          MainMap.CanDragMap = true;


          markers = new GMapMarker[4];

          markers[0] = new GMapMarker(new PointLatLng(30.6614600000, 104.0835370000));
          markers[1] = new GMapMarker(new PointLatLng(30.6660890000, 104.0360580000));
          markers[2] = new GMapMarker(new PointLatLng(30.6614600000, 104.0835370000));
          markers[3] = new GMapMarker(new PointLatLng(30.7686670000, 103.9919190000));

          currentMarker = new GMapMarker(MainMap.CurrentPosition);
          {
              TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
             // trolleyToolTip.setStatus("异常");
              currentMarker.Shape = new CustomMarkerRed(this, currentMarker, trolleyToolTip, this);
              currentMarker.Offset = new System.Windows.Point(0, 0);
              currentMarker.ZIndex = int.MaxValue;
              MainMap.Markers.Add(currentMarker);

             
              for (int i = 0; i < markers.Length; i++)
              {
                  
                  if (i % 2 == 0)
                  {
                      trolleyToolTip = new TrolleyTooltip();
                      markers[i].Shape = new CustomMarkerRed(this, markers[i], trolleyToolTip, this);
                  }
                  else
                  {
                      trolleyToolTip = new TrolleyTooltip();
                      markers[i].Shape = new CustomMarkerGreen(this, markers[i], trolleyToolTip, this);
                  }
                  markers[i].Offset = new System.Windows.Point(0, 0);
                  markers[i].ZIndex = int.MaxValue;
                  MainMap.Markers.Add(markers[i]);
              }
          }
      }

        //初始化二级页面
      private void initGrid2(PointLatLng point)
      {
          map.CurrentPosition = point;
          map.MaxZoom = 10;
          map.MinZoom = 10;
          map.Zoom = 10;
          map.MapType = MapType.GoogleHybridChina;
          map.Manager.Mode = AccessMode.CacheOnly;
          map.BoundsOfMap = new RectLatLng(point.Lat, point.Lng, 0.025, 0.018); //北纬30.67度，东经104.06度。
          map.CanDragMap = true;
          markers = new GMapMarker[4];

          markers[0] = new GMapMarker(new PointLatLng(30.6614600000, 104.0835370000));
          markers[1] = new GMapMarker(new PointLatLng(30.6660890000, 104.0360580000));
          markers[2] = new GMapMarker(new PointLatLng(30.6614600000, 104.0835370000));
          markers[3] = new GMapMarker(new PointLatLng(30.7686670000, 103.9919190000));
          currentMarker = new GMapMarker(point);
          {
              TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
              currentMarker.Shape = new CustomMarkerRed(this, currentMarker, trolleyToolTip, this);
              currentMarker.Offset = new System.Windows.Point(0, 0);
              currentMarker.ZIndex = int.MaxValue;
              map.Markers.Add(currentMarker);
            
              for (int i = 0; i < markers.Length; i++)
              {
                  trolleyToolTip = new TrolleyTooltip();
                  if (i % 2 == 0)
                  {
                      markers[i].Shape = new CustomMarkerRed(this, markers[i], trolleyToolTip, this);
                  }
                  else
                  {
                      markers[i].Shape = new CustomMarkerGreen(this, markers[i], trolleyToolTip, this);
                  }
                
                  markers[i].Offset = new System.Windows.Point(0, 0);
                  markers[i].ZIndex = int.MaxValue;
                  map.Markers.Add(markers[i]);
              }
          }
      }
      void Map_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
          grid1.Visibility = System.Windows.Visibility.Visible;
          grid2.Visibility = System.Windows.Visibility.Hidden;
        //  initGrid1();
          Console.WriteLine("小地图点击双击，回到一级页面！");
      }

      void MainMap_MouseWheelScroll(object sender, System.Windows.Input.MouseWheelEventArgs e)
      {
          //12 14

           if(MainMap.Zoom>14)
          {
              MainMap.Zoom = 14;
          }
          else if(MainMap.Zoom<=MainMap.MinZoom)
          {
              MainMap.Zoom = MainMap.MinZoom;
          }

          Console.WriteLine("main.maxzoom:"+MainMap.MaxZoom+"   MainMap zoom:"+MainMap.Zoom+"MainMap Minzoom"+MainMap.MinZoom);
      }
      void Map1_MouseWheelScroll(object sender, System.Windows.Input.MouseWheelEventArgs e)
      {
          //10
          map.Zoom = 10;
          Console.WriteLine("map zoom:"+map.Zoom);
      }

      void Cablist_DoubleClick(object sender, MouseEventArgs e)
      {
          Console.WriteLine("添加一个柜子");
          Cab cab = new Cab();
          //添加在界面上
          SingleCab cabinfo = new SingleCab(cab);
          CabList.Items.Add(cabinfo);
      }

      void Delete_Cab(object sender, MouseEventArgs e)
      {
          Console.WriteLine("删除一个柜子");
          if(!CabList.Items.IsEmpty)
          {
              CabList.Items.RemoveAt(0);
          }
      }
   }
}
