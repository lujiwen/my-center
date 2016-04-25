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
    using WpfApplication2.View.Windows;
    using GMap.NET;
    using GMap.NET.WindowsPresentation;
    using WpfApplication2.Controls;
    using WpfApplication2.CustomMarkers;
    using WpfApplication2.Model.Vo;
    using WpfApplication2.Controller;
    using WpfApplication2.CustomMarkers.Controls;
    using WpfApplication2.Util;

    namespace WpfApplication2.View.Pages
    {
    /// <summary>
    /// SystemView.xaml 的交互逻辑
    /// </summary>
    public partial class SystemPage : Page
    {
        MainWindow mainWindow;
        GMapMarker[] markers;
        GMapMarker currentMarker;
        Building building;  //map 上点击 需要查看的楼 
        private Boolean[] roomchoose;
        private CabsPage cabspage;
        private DevicePage devicePage;
        public SystemPage(MainWindow w,Building b)
        {
            InitializeComponent();
            this.mainWindow = w;
            this.building = b;
            initSystemTreeView();
            if (building != null)
            {
                switch(building.Name)
                {
                    case "亭子":
                        devicePage = new DevicePage(this.getPageFrame(), building);
                        page.Content = devicePage;
                        break;
                    case "检测车":
                        devicePage = new DevicePage(this.getPageFrame(), building);
                        page.Content = devicePage;
                        break; 
                    default:
                        cabspage = new CabsPage(this, building);
                        page.Content = cabspage;
                        break;
                }
              
            }
            else
            {
                cabspage = new CabsPage(this);
                page.Content = cabspage;
            }
           
            init();
           
        }
        public SystemPage(MainWindow w )
        {
            InitializeComponent();
            this.mainWindow = w;
            initSystemTreeView();
            if (building != null)
            {
                cabspage = new CabsPage(this, building);
            }
            else
            {
                cabspage = new CabsPage(this);
            }
            page.Content = cabspage;
            init();

        }
        
        public SystemPage(MainWindow w,bool isMutilChoose)
        {
            InitializeComponent();
            this.mainWindow = w;
            if (building != null)
            {
                cabspage = new CabsPage(this, building);
            }
            else
            {
                cabspage = new CabsPage(this);
            }
            page.Content = cabspage;
            if (isMutilChoose)
            {
                roomchoose = w.RoomChoosed;
                initSystemTreeView(roomchoose);
            }
            else
            {
                initSystemTreeView();
            }
            init();
        }
        private void init()
        {
            markers = mainWindow.getMapMarkers();
            currentMarker = mainWindow.getCurrentMarker();
           
            initEventListener();
            initLittleMap();
            this.Unloaded += new RoutedEventHandler(SystemPage_Unloaded);
        }

        void SystemPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("SystemPage_Unloaded");
        }

        /// <summary>
        /// 由选择多个检测点进来 看到的左侧的树状结构
        /// </summary>
        /// <param name="choosedArr"></param>
        private void initSystemTreeView(Boolean[] choosedArr)
        {
            Dictionary<string, Building> buildings = GlobalMapForShow.globalMapForBuiding;
            MyTreeViewItem mainNode = new MyTreeViewItem(null);
            mainNode.IsExpanded = true;
            mainNode.Header = createTreeViewItem("请选择需要查看的柜子", null, false, 0, mainNode);
            system_tree.Items.Add(mainNode);
            
            for (int i = 0; i < buildings.Count; i++) //不同的房间
            {
                if(choosedArr[i])
                {
                    Building b= buildings[(i + 1)+""];
                    MyTreeViewItem roomNode = new MyTreeViewItem(b);
                    roomNode.Header = createTreeViewItem(b.Name, null, true, 1, roomNode);
                    roomNode.MouseDoubleClick +=  mainNode_MouseDoubleClick ;
                    mainNode.Items.Add(roomNode);
                    if ((page.Content !=null) && (page.Content is CabsPage)&&(cabspage!=null))
                    {
                        cabspage.addBuildingToShow(b);
                    }
                    for (int j = 0; j < b.Cabs.Count;j++ ) // 每个房间的柜子层
                    {
                        Cab c = b.Cabs[j];
                        MyTreeViewItem cabNode = new MyTreeViewItem(c);
                        cabNode.Header = createTreeViewItem(c.Name, new BitmapImage(new Uri("/Images/rack.png", UriKind.Relative)), true, 2, cabNode);
                        roomNode.Items.Add(cabNode);
                        //显示通道
                        Dictionary<String, List<Device>> subSystemNames = sortSubsystem(c);
                        foreach (var dic in subSystemNames)
                        {
                            //  Device device = cab.Devices[j];
                            MyTreeViewItem subSystemNode = new MyTreeViewItem(dic.Value);
                            subSystemNode.Header = createTreeViewItem(dic.Key, new BitmapImage(new Uri("/Images/home8.png", UriKind.Relative)), false, -1, subSystemNode);
                            subSystemNode.MouseDoubleClick += subSystemNode_MouseLeftButtonDown;
                            roomNode.Items.Add(subSystemNode);
                        }
                        //List<string> subSystemNames = sortSubsystem(c);
                        //for (int k = 0; k < subSystemNames.Count; k++)
                        //{
                        //    //  Device device = cab.Devices[j];
                        //    MyTreeViewItem treeItem = new MyTreeViewItem(subSystemNames[k]);
                        //    treeItem.Header = createTreeViewItem(subSystemNames[k], new BitmapImage(new Uri("/Images/home8.png", UriKind.Relative)), false, -1, treeItem);
                        //    treeItem.Items.Add(treeItem);
                        //}
                    }
                }
            }
        }

         
        //点击一个点 进行的树的初始化
        private void initSystemTreeView()
        {
            if (building != null)
            {
                MyTreeViewItem mainNode = new MyTreeViewItem(building);
                mainNode.Header = createTreeViewItem(building.Name, null, false, -1, mainNode);
                mainNode.MouseDoubleClick += new MouseButtonEventHandler(mainNode_MouseDoubleClick);
                mainNode.IsExpanded = true;
                system_tree.Items.Add(mainNode);
                // globalMapFor
                for (int i = 0; i < building.Cabs.Count; i++)
                {
                    WpfApplication2.Model.Vo.Cab cab = building.Cabs[i];
                    MyTreeViewItem treeNode = new MyTreeViewItem(building.Cabs[i]);
                    treeNode.Header = createTreeViewItem(cab.Name, new BitmapImage(new Uri("/Images/rack.png", UriKind.Relative)), false, -1, treeNode);
                    treeNode.MouseDoubleClick += cab_MouseLeftButtonDown;
                    mainNode.Items.Add(treeNode);
                    //显示通道
                    Dictionary<String ,List<Device>> subSystemNames = sortSubsystem(cab);
                    foreach(var dic in subSystemNames)
                    {
                            //  Device device = cab.Devices[j];
                        MyTreeViewItem subSystemNode = new MyTreeViewItem(dic.Value);
                        subSystemNode.Header = createTreeViewItem(dic.Key, new BitmapImage(new Uri("/Images/home8.png", UriKind.Relative)), false, -1, subSystemNode);
                        subSystemNode.MouseDoubleClick += subSystemNode_MouseLeftButtonDown ;
                        treeNode.Items.Add(subSystemNode);
                    }
                }
            }
        }


        void mainNode_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled) return;
            MyTreeViewItem item = (MyTreeViewItem)sender;
            Building b = (Building)item.NodeObject;
            if (page.Content is CabsPage)
            {
                
            }
            else if (page.Content is DevicePage) //当前在设备页面跳转到柜子页面
            {
                    page.Content = new CabsPage(this, b);
            }
        }

        void subSystemNode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled) return;

            MyTreeViewItem treeNode = (MyTreeViewItem)sender;
            MyTreeViewItem cabNode =  (MyTreeViewItem)treeNode.Parent;
            if (cabNode.NodeObject is Building)
                return;
            Cab cab = (Cab)cabNode.NodeObject ;
            List<Device> subsys = (List<Device>)treeNode.NodeObject;
            if (page.Content is CabsPage) //当前在cab页面 ，跳转到设备页面
            {
                page.Content = new DevicePage(page, cab);
            }
            else if (page.Content is DevicePage)
            {

            }
           
        }
        void cab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled) return;
            MyTreeViewItem treeNode = (MyTreeViewItem)sender;
            Cab cab = (Cab)treeNode.NodeObject;
            
            if (page.Content is CabsPage)  
            {
                 
            }
            else if (page.Content is DevicePage) //当前在设备页面跳转到柜子页面
            {
                page.Content = new DevicePage(page,cab);
            }
            e.Handled = true;
        }

        public Dictionary<String, List<Device>> sortSubsystem(Cab cab)
        {
            Dictionary<String, List<Device>> subSystemNames = new Dictionary<String, List<Device>>();
            foreach(Device d in cab.Devices)
            {
                if (subSystemNames.ContainsKey(d.SubSystemName))
                {
                    subSystemNames[d.SubSystemName].Add(d);
                }
                else
                {
                    subSystemNames[d.SubSystemName] = new List<Device>();
                    subSystemNames[d.SubSystemName].Add(d);
                }
            }
            return subSystemNames ;
        }
      
        // 0  1
        private StackPanel createTreeViewItem(string value, BitmapImage btmapImg,bool hasMyCheckBox,int level,MyTreeViewItem item)
        {
            StackPanel panel = new StackPanel();
            panel.Height = 45;
            panel.Width = 200;
            panel.Orientation = Orientation.Horizontal;

            if (btmapImg != null)
            {
                Image img = new Image();
                img.Source = btmapImg;
                panel.Children.Add(img); //加入图片
            }

            Label lb = new Label();
            lb.FontSize = 17;
            lb.Foreground = new SolidColorBrush(Color.FromRgb(255, 59, 59));
            lb.Content = value;
            lb.VerticalAlignment = VerticalAlignment.Center;
            lb.HorizontalAlignment = HorizontalAlignment.Center;
            lb.Foreground = new SolidColorBrush(Colors.White);
            panel.Children.Add(lb); //加入文字标签
        
            if (hasMyCheckBox)
            {
                MyCheckBox.CheckBoxType t =  MyCheckBox.CheckBoxType.unknow ;
                switch(level)
                {
                        case -1:
                        break;
                        case 0:                    
                        break; 
                        case  1:
                        t = MyCheckBox.CheckBoxType.buildingType;
                        break; 
                        case 2:
                        t = MyCheckBox.CheckBoxType.cabTyp;
                        break;
                        case 3:
                        t = MyCheckBox.CheckBoxType.deviceType;
                        break; 
                }
                MyCheckBox cb = new MyCheckBox(t, item);
                cb.Click += cb_Checked;
                panel.Children.Add(cb); //加入最后一个checkbox
            }   
            return panel;
        }

        private void checkAllChild(TreeViewItem node ,bool isChecked)
        {
            if (node.Items.Count == 0)
            {
                return;
            }
            else
            {
                //遍历node的每一个子node  == stackpanel 
                foreach (TreeViewItem item in node.Items)
                {
                     StackPanel nodePanel = (StackPanel)item.Header;
                    //checkbox 放在第三个位置上，判断是否有最后的checkbox
                     if (nodePanel.Children.Count==3)
                    {
                          CheckBox c = (CheckBox)nodePanel.Children[2];
                        c.IsChecked = isChecked;
                        //  c.Checked += new RoutedEventHandler(cb_Checked);
                        checkAllChild(item, isChecked);
                    }
                    
                }
            }
        }

        void cb_Checked(object sender, RoutedEventArgs e)
        {
            MyCheckBox c = (MyCheckBox)sender;
           
            switch (c.type)
            {
                case MyCheckBox.CheckBoxType.buildingType:
                    Console.WriteLine(" MyCheckBox.CheckBoxType.buildingType");
                    
                    break;
                case MyCheckBox.CheckBoxType.cabTyp:
                    Console.WriteLine(" MyCheckBox.CheckBoxType.cabTyp");
                    break;
                case MyCheckBox.CheckBoxType.deviceType:
                    Console.WriteLine(" MyCheckBox.CheckBoxType.deviceType");
                    break;
                default:
                    break;
            }

            if (c.IsChecked ?? false) //添加
            {
                Console.WriteLine("柜子 " + c.Name + "! ");
                if (page.Content is CabsPage)  //当前 柜子页面
                {
                    Console.WriteLine("当前 柜子页面");
                    checkAllChild(c.TreeNode,true);
                    if (c.type.Equals(MyCheckBox.CheckBoxType.buildingType)) //插入一个楼的全部柜子
                    {

                        cabspage.insertCab(getPageFrame(), (Building)c.NodeObject);
                        //  cabspage.selectBuildings.Add((Building)c.NodeObject);
                    }
                    else 
                    {
                        cabspage.insertCab(getPageFrame(), (Cab)c.NodeObject);
                    }
                    
                }
                else if (page.Content is DevicePage) //当前实在 设备页面
                {
                    Console.WriteLine("当前设备页面");
                    checkAllChild(c.TreeNode, false);
                }
            }
            else //删除 
            {
                if (page.Content is CabsPage)  //当前 柜子页面
                {
                    Console.WriteLine("当前 柜子页面");
                    checkAllChild(c.TreeNode, true);
                    if (c.type.Equals(MyCheckBox.CheckBoxType.buildingType)) //插入一个楼的全部柜子
                    {
                        cabspage.deleteCab(getPageFrame(), (Building)c.NodeObject);
                    }
                    else
                    {
                        cabspage.deleteCab(getPageFrame(), (Cab)c.NodeObject);
                    }
                    checkAllChild(c.TreeNode, false);
                }
                else if (page.Content is DevicePage) //当前实在 设备页面
                {
                    Console.WriteLine("当前设备页面");
                    checkAllChild(c.TreeNode, false);
                }
            //     cabspage.deleteCab();
            }
        }

        private void initEventListener()
        {
            map.MouseDoubleClick     += new MouseButtonEventHandler(map_double_click);
            home.MouseLeftButtonDown += new MouseButtonEventHandler(home_btn_click);
            back.MouseLeftButtonDown += new MouseButtonEventHandler(back_btn_click);
            next.MouseLeftButtonDown += new MouseButtonEventHandler(next_btn_click);
        }

        private void initLittleMap()
        {
            PointLatLng point = mainWindow.getClickPoint();
            map.CurrentPosition =new PointLatLng(point.Lat,point.Lng);
            map.MaxZoom = 40;
            map.MinZoom = 1;
            map.Zoom = 7;
            //  map.MapType = MapType.GoogleHybridChina ;
            map.MapType = MapType.ArcGIS_Map;
            map.Manager.Mode = AccessMode.ServerAndCache;
            //   map.BoundsOfMap = new RectLatLng(29.540871 , 104.804598 , 20.025, 20.018); //北纬30.67度，东经104.06度。
            map.CanDragMap = true;
            map.MouseWheel += new MouseWheelEventHandler(map_MouseWheel);
            map.DragButton = MouseButton.Right;
            currentMarker = new GMapMarker(new PointLatLng(31.540871, 104.804598));
            {
                TrolleyTooltip trolleyToolTip = new TrolleyTooltip();
                currentMarker.ZIndex = int.MaxValue;
                map.Markers.Add(currentMarker);
                //currentMarker.Shape = new PositionMarker(mainWindow, currentMarker, trolleyToolTip, false);

                for (int i = 0; i < markers.Length; i++)
                {
                    if(markers[i]!=null)
                    {
                        break;
                    }
                    markers[i].ZIndex = int.MaxValue;
                    trolleyToolTip = new TrolleyTooltip(mainWindow.Buildings[i]);
                    markers[i].Shape = new PositionMarker(mainWindow, markers[i], trolleyToolTip, mainWindow.Buildings[i]);
                    markers[i].Shape.AllowDrop = false;
                    map.Markers.Add(markers[i]);
                }
            }
        }

        void map_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Console.WriteLine("map zoom :"+map.Zoom );
        }

        void map_double_click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("点了map两下！");
            GoBackToMapPage();
        }

        private void GoBackToMapPage()
        {
            MapPage page = null;
            if (mainWindow.MainWindowMapPage != null)
            {
                page = mainWindow.MainWindowMapPage;
            }
            else
            {
                page = new MapPage(mainWindow);
            }
            mainWindow.getMainPage().Content = page;
        }

        void home_btn_click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("点了home按钮！");
            GoBackToMapPage();
        }

        void back_btn_click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("点了back按钮！");
            if (page.NavigationService.CanGoBack)
            {
                page.NavigationService.GoBack();
            }
        }

        void next_btn_click(object sender, MouseEventArgs e)
        {
            Console.WriteLine("点了next按钮！");
            if (page.NavigationService.CanGoForward)
            {
                page.NavigationService.GoForward();
            }
        }

        public Frame getPageFrame()
        {
            return page;
        }
        public MainWindow getMainWindowInstance()
        {
            return mainWindow;
        }

    }
}
