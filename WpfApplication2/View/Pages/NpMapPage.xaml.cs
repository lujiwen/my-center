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
using GMap.NET;
using GMap.NET.WindowsPresentation;
using WpfApplication2.Controls;
using WpfApplication2.CustomMarkers;

namespace WpfApplication2.View.Pages
{
    /// <summary>
    /// NpMapPage.xaml 的交互逻辑
    /// </summary>
    public partial class NpMapPage : Page
    {
        private List<GMapMarker> markers;
        private List<Model.Vo.Building> buildings;
        private Windows.MainWindow mainWindow;
        public NpMapPage(List<GMapMarker> markers)
        {
            InitializeComponent();
            this.markers = markers;
            initMap();
            initPoints();
        }

        public NpMapPage(List<Model.Vo.Building> buildings,Windows.MainWindow mainWindow,List<GMapMarker> markers)
        {
                // TODO: Complete member initialization
                InitializeComponent();
                this.buildings = buildings;
                this.mainWindow = mainWindow;
                this.markers = markers;
                initMap();
                initPoints();
        }

        private void initPoints()
        {
            Console.WriteLine("initNpPoints");
            GMapMarker  currentMarker = new GMapMarker(new PointLatLng(31.540871, 104.804598));
            
            TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
            trolleyToolTip.setStatus("异常");
            currentMarker.Offset = new System.Windows.Point(0,0);
            currentMarker.ZIndex = int.MaxValue;
            NpMap.Markers.Add(currentMarker);
          //currentMarker.Shape = new PositionMarker(mainWindow, currentMarker, trolleyToolTip, false);
          //  markers = new List<GMapMarker>();
            for (int i = 0; i <this.buildings.Count; i++)
            {
                markers[i].ZIndex = int.MaxValue;
                trolleyToolTip = new TrolleyTooltip(buildings[i]);
                markers[i].Shape = new PositionMarker(mainWindow, markers[i], trolleyToolTip, buildings[i]);
              //  markers[i].Shape.MouseLeftButtonUp += marker_Click;
                markers[i].Shape.AllowDrop = true;
               //markers[i].Shape.PreviewMouseMove += Shape_PreviewMouseMove;
               //markers[i].Shape.QueryContinueDrag += Shape_QueryContinueDrag;
                 NpMap.Markers.Add(markers[i]);
            }
             
        }

        private void initMap()
        {
            Console.WriteLine("initNpMap");

            //this.MouseMove += new MouseEventHandler(Window_MouseMove);
            //this.MouseUp += new MouseButtonEventHandler(Window_MouseUp);
            //NpMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            //NpMap.MouseMove += new System.Windows.Input.MouseEventHandler(MainMap_MouseMove);
            //NpMap.Loaded += new RoutedEventHandler(MainMap_Loaded);
            //NpMap.MouseEnter += new MouseEventHandler(MainMap_MouseEnter);
            //NpMap.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(MainMap_MouseWheelScroll);
            NpMap.CurrentPosition = new PointLatLng(31.540871, 104.804598);
            NpMap.MaxZoom = 40;
            NpMap.MinZoom = 2;
            NpMap.Zoom = 6;
            NpMap.MapType = MapType.ArcGIS_Map;
            NpMap.Manager.Mode = AccessMode.ServerOnly;
          //  NpMap.CacheLocation = System.Environment.SystemDirectory + "/cache/";
          //  MainMap.BoundsOfMap = new RectLatLng(31.540871, 104.804598, 2.025, 2.018); //北纬30.67度，东经104.06度。
            NpMap.CanDragMap = true;
            NpMap.DragButton = MouseButton.Right;
        }

    }
}
