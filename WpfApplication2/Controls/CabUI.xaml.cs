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
using WpfApplication2.CustomMarkers.Controls;
using WpfApplication2.View.Pages;
using Visifire.Charts;
using WpfApplication2.Model.Vo;
using WpfApplication2.View.Windows;
using WpfApplication2.Util;
using Microsoft.Samples.KMoore.WPFSamples.DateControls;
using Project208Home.Views.ArtWorks208;
using System.Windows.Threading;
using System.Reflection;
 
namespace WpfApplication2.Controls
{
    /// <summary>
    /// cab.xaml 的交互逻辑
    /// </summary>
    public partial class CabUI : UserControl
    {
        private String cabName;
        private List<DeviceUI> deviceList;
  
        private Frame systemframe;
        private Cab _cab;
        private int _pointCount = 50 ;
        public int PointCount { get { return _pointCount; } set { _pointCount = value;} }
        private Visifire.Charts.Title title;
        private DataSeries[] dataSeries;
        public Cab CabInUI { get { return _cab; } set { _cab = value; } }
        private LabelAndText deviceCountLT, stateLT;

        public CabUI()
        {
            InitializeComponent();
            init();
            initEnventListner();
            this.deviceList = new List<DeviceUI>(); 
        }

        ~CabUI()
        {
            
        }

        public CabUI(Frame frame, Cab cab)
        {
            InitializeComponent();
            systemframe = frame;
            this._cab = cab;
            init();
            initEnventListner();
            this.deviceList = new List<DeviceUI>();
        }
 
        public CabUI(String name)
        {
            InitializeComponent();
            initEnventListner();
            init();
            this.deviceList = new List<DeviceUI>(); 
            //设置柜子名
            this.cabName = name;
            cab_name.Text = name;
        }
      
        private void init()
        {
            initChart();
            both.IsChecked = true;
            Dictionary<string, Device> devices =  GlobalMapForShow.globalMapForDevice;
            if (_cab!=null)
            {
                cab_name.Text = _cab.Name;
                system_name.Text = MainWindow.getInstance().Buildings[int.Parse(_cab.BuildingId)-1].Name;
                
                deviceCountLT   = new LabelAndText("设备总数:",_cab.Devices.Count+"","");
                deviceCountLT.setForeGround(Colors.White);
                info_panel.Children.Add(deviceCountLT);

                stateLT= new LabelAndText("状态:", _cab.State.Equals("Nomal") ? "正常" : "异常", "");
                stateLT.setForeGround(Colors.White);
                info_panel.Children.Add(stateLT);
            }
            initArtWork();
            cabAlarm.Visibility = System.Windows.Visibility.Hidden;
        }

        private void initArtWork()
        {
            try
            {
                 //利用反射机制，进行初始化工艺流程图
                Type tp = Type.GetType("Project208Home.Views.ArtWorks208.Cab" + CabInUI.TypeInSystem);
                Console.WriteLine(tp.ToString());
                if(tp==null)
                {
                    return;
                }
                Type[] types = new Type[1];
                types[0] = typeof(Cab);
                //有参构造
                ConstructorInfo ct = tp.GetConstructor(types);

                Object[] paras = new Object[1];
                paras[0] = CabInUI;

                UserControl concreteCabArtWork = (UserControl)ct.Invoke(paras);
                cabArtWork.Children.Add(concreteCabArtWork);
            }
            catch(Exception e)
            {

            }
          
        }

        void c_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            Console.WriteLine(" Unchecked :  " + c.Name);
            if (c.IsChecked ?? false)
            {

            }

            real_time_cab_chart.Series.RemoveAt( 0);
            Console.WriteLine("还有曲线：" + real_time_cab_chart.Series.Count);
        }

        void c_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            Console.WriteLine("Checked"+c.Name);
            if (c.IsChecked ?? false)
            {

            }
        }

         
        
        private void initChart()
        {
            title = new Visifire.Charts.Title();
            Axis axisX = new Axis();//图表X轴
            Axis axisY = new Axis(); //图表Y轴
            axisX.Title = "时间";//横坐标单位
            axisX.Interval = 10;
            axisY.Title = "单位:xxx";//纵坐标单位
        
            real_time_cab_chart.Titles.Add(title);//添加标题
            real_time_cab_chart.AxesX.Add(axisX);//添加x轴
            real_time_cab_chart.AxesY.Add(axisY);//添加y轴
            dataSeries = new DataSeries[_cab.Devices.Count];
            for (int i = 0; i < _cab.Devices.Count;i++ )
            {
                if(!_cab.Devices[i].Type.Equals("Pump"))
                {
                    dataSeries[i] = new DataSeries();  //数据系列 
                    dataSeries[i].RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线     
                    dataSeries[i].LegendText = _cab.Devices[i].HandleTypeInSystem;
                    real_time_cab_chart.Series.Add(dataSeries[i]);
                }
            }
        }

        private void initEnventListner()
        {
            this.MouseDoubleClick += new MouseButtonEventHandler(cabDoubleClick);
            CabInUI.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(CabInUI_PropertyChanged);
        }

        void CabInUI_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine("CabInUI_PropertyChanged");
         //   Dispatcher.BeginInvoke(new Action(updateCabUI));
        }

        public double MAX = 1000000;
        public double MIN = 1;  

        public void UpdateChart(Cab c)
        {
            List<Device> devices = c.Devices ;
            DateTime dt =   DateTime.Now;
            string timeStamp = dt.ToString("HH:mm:ss");
            for (int i = 0; i <c.Devices.Count;i++ )
            {
                if ((devices[i].NowValue != null)&&(!devices[i].NowValue.Equals("")))
                {
                    try
                    {
                        Decimal nowvalue = Convert.ToDecimal(devices[i].NowValue);
                        double value = Decimal.ToDouble(nowvalue);
                        if (value >= MAX  )
                        {
                            value = MAX;
                        }
                        else if(value<=MIN)
                        {
                            value = MIN;
                        }

                        if (dataSeries[i].DataPoints.Count < _pointCount) //直接添加
                        {
                            LightDataPoint dataPoint = new LightDataPoint();//数据点
                            //  dataPoint = 8;

                            dataPoint.AxisXLabel = timeStamp; //dataSeries[i].DataPoints.Count + "";
                            dataPoint.YValue = value;
                            Console.WriteLine("X：" + dataPoint.AxisXLabel + "   Y:" + dataPoint.YValue);
                            dataSeries[i].DataPoints.Add(dataPoint);//数据点添加到数据系列
                            dataPoint = null;
                        }
                        else //想左移动
                        {
                            for (int j = 1; j < _pointCount; j++)
                            {
                                dataSeries[i].DataPoints[j - 1].AxisXLabel = dataSeries[i].DataPoints[j].AxisXLabel;
                                dataSeries[i].DataPoints[j - 1].YValue = dataSeries[i].DataPoints[j].YValue;
                            }
                            dataSeries[i].DataPoints[_pointCount - 1].AxisXLabel = timeStamp; //(new DateTime().Second).ToString();
                            dataSeries[i].DataPoints[_pointCount - 1].YValue = value; //; ;//数据点添加到数据系列
                        }
                    }
                    catch(Exception e)
                    {
                        LogUtil.Log(false, "数值过大或过小:" + devices[i].NowValue +" : "+ DateTime.Now.ToString(), 0);
                        Console.WriteLine("数值过大或过小:" + devices[i].NowValue + " : " + DateTime.Now.ToString());
                    }
                }
            }
        }
       
        public void cabDoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("双击了柜子一下");
            if(systemframe!=null)
            {
               DevicePage page = new DevicePage(systemframe,_cab);
               systemframe.Content = page;
            }
        }


        public void updateCabUI()
        {
             //CabInUI = GlobalMapForShow.globalMapForCab[CabInUI.BuildingId + "_" + CabInUI.CabId];
             Cab c = GlobalMapForShow.globalMapForCab[CabInUI.BuildingId + "_" + CabInUI.CabId];
             if (c.State.Equals("Normal"))
            {
                cabAlarm.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                cabAlarm.Visibility = System.Windows.Visibility.Visible;
            }
            //更新状态
             stateLT.updateValue(c.State.Equals("Normal") ? "正常" : "异常");
            //更新曲线图
             UpdateChart(c);

        }

 
        private void radio_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            Console.WriteLine(r.Name);
            switch(r.Name)
            {
                case "tech":
                    tech.IsChecked =  true;
                    cabArtWork.Visibility = System.Windows.Visibility.Visible;
                    real_time_cab_chart.Visibility = System.Windows.Visibility.Collapsed;
                    curve_pannel.Visibility = System.Windows.Visibility.Collapsed;
                    break ;
                case "curve":
                    tech.IsChecked = false;
                    cabArtWork.Visibility = System.Windows.Visibility.Collapsed;
                    real_time_cab_chart.Visibility = System.Windows.Visibility.Visible;
                    curve_pannel.Visibility = System.Windows.Visibility.Visible;
                    break ;
               case "both":
                    tech.IsChecked = false;
                    cabArtWork.Visibility = System.Windows.Visibility.Visible;
                    real_time_cab_chart.Visibility = System.Windows.Visibility.Visible;
                    curve_pannel.Visibility = System.Windows.Visibility.Visible;
                    break ;

            }
        }

        private void history_btn_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow w = new HistoryWindow(CabInUI);
            w.Show();
        }

         private void settings_btn_Click(object sender, RoutedEventArgs e)
         {
            PasswordWindow pw = new PasswordWindow("");
            pw.PasswordCorrect += new isPasswordCorrect(pw_PasswordCorrect);
            pw.Show();
         }

         void pw_PasswordCorrect(bool isCorrect)
         {
             if(isCorrect)
             {
                 ParametersSettingsWindow w = new ParametersSettingsWindow(CabInUI);
                 w.Show();
             }
         }

         public String CabName
         {
             get { return cabName; }
             set
             {
                 this.cabName = value;
                 cab_name.Text = " " + value + " ";
             }
         }
    }
}
