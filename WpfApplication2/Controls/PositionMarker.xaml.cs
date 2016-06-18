using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using GMap.NET;
using WpfApplication2.Controls;
using System.Windows.Media.Imaging;
using System;
using WpfApplication2.View.Pages;
using WpfApplication2.Model.Vo;
using System.ComponentModel;
using WpfApplication2.Controller;
using System.Windows.Threading;
using System.Threading;
using WpfApplication2.package;
using System.Collections.Generic;


namespace WpfApplication2.CustomMarkers
{
    /// <summary>
    /// Interaction logic for CustomMarkerDemo.xaml
    /// </summary>
    public partial class PositionMarker
    {
        Popup Popup;
        Label Label;
        GMapMarker Marker;
        Window MapWindow;
        PointLatLng point;
        private GMapMarker marker;
        private GMapMarker[] markers;
        private BitmapImage MarkerBm;

        public delegate void MarkerClick(object args);
        public event MarkerClick EventName;
        private SystemPage systemPage;
        private GMapMarker currentMarker;
        private TrolleyTooltip trolleyToolTip;
        private bool p;

        private Building _building;
        public Building building { get { return _building; } set { _building = value; } }

        private bool _IsNormal;

        public bool IsNormal
        {
            get { return _IsNormal; }

            set { _IsNormal = value; }
        }

        public PositionMarker(PointLatLng point)
        {
            this.InitializeComponent();
            point = Marker.Position;
        }

        public PositionMarker(List<Building> buildings,List<GMapMarker> markers)
        {
            InitializeComponent();
            point = markers[0].Position;

        }

        public PositionMarker(Window window, List<GMapMarker> markers, UIElement ui, List<Building> builds)
        {
            InitializeComponent();
            MarkerBm = new BitmapImage();
            building = new Building("NP基地"); 
            MarkerBm = new BitmapImage(new Uri("/WpfApplication2;component/Images/bigMarkerGreen.png", UriKind.Relative));
            if (MarkerBm != null)
            {
                icon.Source = MarkerBm;
            }
            this.MapWindow = window;
            this.Marker = markers[0];

            Popup = new Popup();
            Label = new Label();
            point = markers[0].Position;

            this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);

            if (ui != null)
            {
                ((TrolleyTooltip)ui).setStatus("异常");
                Popup.Placement = PlacementMode.Mouse;
                {
                    Label.Background = Brushes.Blue;
                    Label.Foreground = Brushes.White;
                    Label.BorderBrush = Brushes.WhiteSmoke;
                    Label.BorderThickness = new Thickness(2);
                    Label.Padding = new Thickness(5);
                    Label.FontSize = 22;
                    // Label.Content = title;
                    Label.Content = "lable content!";
                }
                Popup.AllowsTransparency = true;
                Popup.Child = ui;
            }
        }

        public PositionMarker(Window window, GMapMarker marker, UIElement ui, Building b)
        {
            this.InitializeComponent();
            MarkerBm = new BitmapImage();
            building = b;
            building.PropertyChanged += buildingStateChangeMarkerColor;
            if (!b.State.Equals("Normal"))
            {
                MarkerBm = new BitmapImage(new Uri("/WpfApplication2;component/Images/red-dot.png", UriKind.Relative));
            }
            else
            {
                MarkerBm = new BitmapImage(new Uri("/WpfApplication2;component/Images/bigMarkerGreen.png", UriKind.Relative));
            }
            if (MarkerBm != null)
            {
                icon.Source = MarkerBm;
            }
            this.MapWindow = window;
            this.Marker = marker;

            Popup = new Popup();
            Label = new Label();
            point = marker.Position;

            this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);

            if (ui != null)
            {
                ((TrolleyTooltip)ui).setStatus("异常");
                Popup.Placement = PlacementMode.Mouse;
                {
                    Label.Background = Brushes.Blue;
                    Label.Foreground = Brushes.White;
                    Label.BorderBrush = Brushes.WhiteSmoke;
                    Label.BorderThickness = new Thickness(2);
                    Label.Padding = new Thickness(5);
                    Label.FontSize = 22;
                    // Label.Content = title;
                    Label.Content = "lable content!";
                }
                Popup.AllowsTransparency = true;
                Popup.Child = ui;

            }
        }

        public PositionMarker(SystemPage systemPage, GMapMarker currentMarker, TrolleyTooltip trolleyToolTip, bool p)
        {
            // TODO: Complete member initialization
            this.systemPage = systemPage;
            this.currentMarker = currentMarker;
            this.trolleyToolTip = trolleyToolTip;
            this.p = p;
        }
        private void buildingStateChangeMarkerColor(object sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine("buildingStateChangeMarkerColor");
            
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
           {
              Building b = (Building)sender;
              if (!b.State.Equals("Normal"))
              {
                  MarkerBm = new BitmapImage(new Uri("/WpfApplication2;component/Images/red-dot.png", UriKind.Relative));
                  ((TrolleyTooltip)Popup.Child).setStatus("异常");
              }
              else
              {
                  MarkerBm = new BitmapImage(new Uri("/WpfApplication2;component/Images/bigMarkerGreen.png", UriKind.Relative));
                  ((TrolleyTooltip)Popup.Child).setStatus("正常");
              }
              if (MarkerBm != null)
              {
                  icon.Source = MarkerBm;
              }
          });

        }
        void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
        {
            if (icon.Source.CanFreeze)
            {
                icon.Source.Freeze();
            }
        }

        void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new System.Windows.Point(-e.NewSize.Width / 2, -e.NewSize.Height);
        }

        //void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
        //{
        //   if(e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
        //   {
        //      System.Windows.Point p = e.GetPosition(MapWindow.MainMap);
        //      Marker.Position = MapWindow.MainMap.FromLocalToLatLng((int) p.X, (int) p.Y);
        //   }
        //}

        //void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //   if(!IsMouseCaptured)
        //   {
        //      Mouse.Capture(this);
        //   }
        //   System.Console.WriteLine("marker Red MouseLeftButtonDown :" );

        //   //Window1 window = new Window1(point);
        //   //window.Show();

        //   System.Windows.Point p = e.GetPosition(MapWindow.MainMap);
        //   Marker.Position = MainMapPage.MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
        //   RedMarkerClickLisener.onRedMarkerclick(Marker.Position);

        //}

        void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
        }

        void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 10000;
            Popup.IsOpen = false;
        }


        //鼠标移动到红点，弹出提示窗体popup 
        void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 10000;
            Popup.IsOpen = true;
        }

        private void icon_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
       

        public GMapMarker getGmapMarker()
        {
            return Marker;
        }

        //protected override void onmousedown(mousebuttoneventargs e)
        //{
        //    base.onmousedown(e);
        //    this.capturemouse();
        //    console.writeline(" protected override void onmousedown(mousebuttoneventargs e) ");
        //}
    }

}