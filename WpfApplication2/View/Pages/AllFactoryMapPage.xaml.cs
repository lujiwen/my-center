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
using WpfApplication2.Controls;
using GMap.NET;
using WpfApplication2.CustomMarkers;
using WpfApplication2.Model.Vo;
using WpfApplication2.View.Windows;

namespace WpfApplication2.View.Pages
{
    /// <summary>
    /// AllFactoryMapPage.xaml 的交互逻辑
    /// </summary>
    public partial class AllFactoryMapPage : Page
    {
        private List<GMapMarker> markers;
        private List<Building> NonewNPBuildings;
        private List<Building> NPBuildings;
        private MainWindow mainWindow;
        private List<GMapMarker> NonewNPmarkers;
        private GMapMarker NpMarker;
        public AllFactoryMapPage(List<GMapMarker> markers)
        {
            InitializeComponent();
            this.markers = markers;
        }

        public AllFactoryMapPage(List<Model.Vo.Building> NonewNPBuildings, Windows.MainWindow mainWindow, List<GMapMarker> NonewNPmarkers,List<Building> npBuilding)
        {
            InitializeComponent();
            this.NonewNPBuildings = NonewNPBuildings;
            this.mainWindow = mainWindow;
            this.NonewNPmarkers = NonewNPmarkers;
            this.NPBuildings = npBuilding;
            initMap();
            initPoints();
        }

        private void initPoints()
        {
            Console.WriteLine("initNpPoints");
            GMapMarker currentMarker = new GMapMarker(new PointLatLng(32.540871, 104.804598));
            TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
            trolleyToolTip.setStatus("异常");
            currentMarker.Offset = new System.Windows.Point(0, 0);
            currentMarker.ZIndex = int.MaxValue;
            AllFactoryMap.Markers.Add(currentMarker);
            //currentMarker.Shape = new PositionMarker(mainWindow, currentMarker, trolleyToolTip, false);
          
            for (int i = 0; i < this.NonewNPBuildings.Count; i++)
            {
                NonewNPmarkers[i].ZIndex = int.MaxValue;
                trolleyToolTip = new TrolleyTooltip(NonewNPBuildings[i]);
                NonewNPmarkers[i].Shape = new PositionMarker(mainWindow, NonewNPmarkers[i], trolleyToolTip, NonewNPBuildings[i]);
                //markers[i].Shape.MouseLeftButtonUp += marker_Click;
                NonewNPmarkers[i].Shape.AllowDrop = true;
                //markers[i].Shape.PreviewMouseMove += Shape_PreviewMouseMove;
                //markers[i].Shape.QueryContinueDrag += Shape_QueryContinueDrag;
                AllFactoryMap.Markers.Add(NonewNPmarkers[i]);
            }


            //在全局地图上增加一个np 点的地图
            NpMarker = new GMapMarker();
            NpMarker.ZIndex = int.MaxValue;
            trolleyToolTip = new TrolleyTooltip(NPBuildings[0]);
            NpMarker.Shape = new PositionMarker(mainWindow, NpMarker, trolleyToolTip, NPBuildings[0]);
            //markers[i].Shape.MouseLeftButtonUp += marker_Click;

            NpMarker.Shape.AllowDrop = true;
            AllFactoryMap.Markers.Add(NpMarker);
        }

        private void initMap()
        {
            Console.WriteLine("initAllFactoryMap");

            //this.MouseMove += new MouseEventHandler(Window_MouseMove);
            //this.MouseUp += new MouseButtonEventHandler(Window_MouseUp);
            //AllFactoryMap.OnCurrentPositionChanged += new CurrentPositionChanged(MainMap_OnCurrentPositionChanged);
            //AllFactoryMap.MouseMove += new System.Windows.Input.MouseEventHandler(MainMap_MouseMove);
            //AllFactoryMap.Loaded += new RoutedEventHandler(MainMap_Loaded);
            //AllFactoryMap.MouseEnter += new MouseEventHandler(MainMap_MouseEnter);
            //AllFactoryMap.MouseWheel += new System.Windows.Input.MouseWheelEventHandler(MainMap_MouseWheelScroll);
            AllFactoryMap.CurrentPosition = new PointLatLng(31.540871, 104.804598);
            AllFactoryMap.MaxZoom = 40;
            AllFactoryMap.MinZoom = 2;
            AllFactoryMap.Zoom = 6;
            AllFactoryMap.MapType = MapType.ArcGIS_Map;
            AllFactoryMap.Manager.Mode = AccessMode.ServerOnly;
           // AllFactoryMap.CacheLocation = System.Environment.SystemDirectory + "/cache/";
           // AllFactoryMap.BoundsOfMap = new RectLatLng(31.540871, 104.804598, 2.025, 2.018); //北纬30.67度，东经104.06度。
            AllFactoryMap.CanDragMap = true;
            AllFactoryMap.DragButton = MouseButton.Right;
        }

    }
}
