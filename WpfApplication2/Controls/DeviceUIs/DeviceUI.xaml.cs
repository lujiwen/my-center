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
using WpfApplication2.Controller;
using WpfApplication2.CustomMarkers.Controls;
using WpfApplication2.View.Pages;
using WpfApplication2.Model.Vo;
using WpfApplication2.Util;
using Visifire.Charts;
using WpfApplication2.View.Windows;
using System.ComponentModel;
using System.Windows.Threading;
using WpfApplication2.package;
namespace WpfApplication2.Controls 
{
    /// <summary>
    /// Device.xaml 的交互逻辑
    /// </summary>
    public  partial class DeviceUI : UserControl
    {
       private String  deviceName ;
       private PasswordWindow passwordWindow;
       private DeviceDataBox_Base deviceDataBoxComp;
       private DeviceDataBox deviceDataBox;
       private Frame systemFrame;
       private Device _device;
       private LabelAndText stateLT, valueLT, thresholdLT,typeLT;
       private List<string> label;
       private bool curveEnable;
       public bool CurveEnable { get { return curveEnable; } set { curveEnable = value; } }
       public List<string> LabelsToShow { set { label = value; } get { return label; } }
       private Visifire.Charts.Title title;
       private List<String> nowValues;
       protected DataSeries[] dataSeries;
       protected List<String> values;
       private int maxPointSize;
       protected int lineCount;
       protected Dictionary<int, string> valueDic;
       public Device DeviceInUI { 
           get { return _device; } 
           set { _device = value;
                 _device.Value = value.Value;
           }
       }

       public DeviceUI()
       {
                
       }
        ~ DeviceUI()
        {
            Console.WriteLine("DeviceUI 析构函数 ！");
        }
        public DeviceUI(String name)
        {
            InitializeComponent();
            initEnventListner();
            this.deviceName = name;
        }
        public DeviceUI(String name,Frame fm)
        {
            InitializeComponent();
            initEnventListner();
            this.deviceName = name;
            this.systemFrame = fm;
        }
        public DeviceUI(Device d)
        {
            this._device = d;
            init();
        }
        public DeviceUI(Device d, Frame fm)
        {
            InitializeComponent();
            initEnventListner();

            _device = d;
            this.systemFrame = fm;
            init();
        }
        private void initBindings()
        {
            ////实时值绑定
            //Binding valueBingding = new Binding();
            //valueBingding.Source = _device;
            //valueBingding.Path = new PropertyPath("Value");
            //valueLT.getValueTextBlock().SetBinding(TextBlock.TextProperty, valueBingding);

            ////状态绑定
            //Binding stateBinding = new Binding();
            //stateBinding.Source = _device;
            //stateBinding.Path = new PropertyPath("State");
            //valueLT.getValueTextBlock().SetBinding(TextBlock.TextProperty,stateBinding);
        }
        private void init()
        {
           
           initlabels();
           maxPointSize = 40;
           DeviceBuzzer.MouseLeftButtonDown += new MouseButtonEventHandler(DeviceBuzzer_MouseLeftButtonDown);
           device.MouseLeftButtonDown += new MouseButtonEventHandler(device_MouseLeftButtonDown);
           if(_device!=null)
           {
               Dictionary<string, Cab> cabs = GlobalMapForShow.globalMapForCab;
               device_name.Text = _device.HandleTypeInSystem;
               cab_name.Text = cabs[_device.BuildingId + "_" + _device.CabId].Name; 
           }
 
            initDeviceChart();
 
           
        }

        void DeviceBuzzer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeviceBuzzer.muteBuzzer();
        }

       

        private void device_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DeviceBuzzer.stopAlarm();
        }


        public StackPanel getInoPanel()
        {
            return info_panle;
        }
        public Chart getChart()
        {
            return device_chart;
        }

        public virtual void initlabels()
        { 

        }


        private void initDeviceChart()
        {
            device_chart.Visibility = System.Windows.Visibility.Collapsed;
            title = new Visifire.Charts.Title();
            Axis axisX = new Axis();//图表X轴
            Axis axisY = new Axis(); //图表Y轴
            axisX.Title = "时间";//横坐标单位
            axisX.Interval = 20;
            axisY.Title = "单位:xxx";//纵坐标单位
             
            device_chart.Titles.Add(title);//添加标题
        
            device_chart.AxesX.Add(axisX);//添加x轴
            device_chart.AxesY.Add(axisY);//添加y轴
            //dataSeries = new DataSeries() ;  //数据系列 
            //dataSeries.RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线     
            //device_chart.Series.Add(dataSeries);
            
        }

        public DeviceUI(Box box)
        {
            InitializeComponent();
            
            initEnventListner();
            
            if(box is DeviceDataBox )
            {
                this.deviceDataBox = (DeviceDataBox)box;
            }
            else if(box is DeviceDataBox_Base)
            {
                this.deviceDataBoxComp = (DeviceDataBox_Base)box;
            }
        }

        private void initEnventListner()
        {
        }
      
        public void updateDeviceUI(Device d)
        {
            //更新一个设备
            stateLT.updateValue(d.State.Equals("Normal")?"正常":"异常");
            valueLT.updateValue("  " + d.NowValue + "  ");
            //更新图表
        }
      
        public double MAX = 100;
        public double MIN = 1;  
        private void updateChart()
        {
            if (valueDic != null && valueDic.Count != 0)
            {
                DateTime dt = DateTime.Now;
                string timeStamp = dt.ToString("HH:mm:ss ");
                if (nowValues == null) return;
                for (int i = 0; i < valueDic.Count; i++)
                {
                    try
                    {
                        Decimal nowvalue = Convert.ToDecimal(nowValues[i]);
                        double value = Decimal.ToDouble(nowvalue);

                        //如果超过了最大值，就把该值在图标上限定为最大值
                        if (value >= MAX)
                        {
                            value = MAX;
                        }
                        else if (value <= MIN) //如果低于最小值，就把该值限定在最小值
                        {
                            value = MIN;
                        }

                        //如果还没有到达图表空间上每条曲线上的点数最大值maxPointSize
                        if (dataSeries[i].DataPoints.Count < maxPointSize) //直接添加
                        {
                            DataPoint dataPoint = new DataPoint();//数据点
                            dataPoint.MarkerSize = 8;
                            dataPoint.AxisXLabel = timeStamp;
                            dataPoint.YValue = value;
                            dataSeries[i].DataPoints.Add(dataPoint);//数据点添加到数据系列
                        }
                        else //想左移动
                        {
                            for (int j = 1; j < maxPointSize; j++)
                            {
                                dataSeries[i].DataPoints[j - 1].AxisXLabel = dataSeries[i].DataPoints[j].AxisXLabel;
                                dataSeries[i].DataPoints[j - 1].YValue = dataSeries[i].DataPoints[j].YValue;
                            }
                            dataSeries[i].DataPoints[maxPointSize - 1].AxisXLabel = timeStamp; //(new DateTime().Second).ToString(); //; ;//数据点添加到数据系列
                            dataSeries[i].DataPoints[maxPointSize - 1].YValue = value; //; ;//数据点添加到数据系列
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("DeviceUI chart update value wrong!"+e.Message);
                        break;
                    }
                }
            }  
        }

        public void updateChart(List<String> NowValue)
        {
            nowValues = NowValue;
            Dispatcher.BeginInvoke(new Action(updateChart));
        }
       
        private void historyCurve(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("查看历史曲线");
        }

        private void settings(object sender, RoutedEventArgs e)
        {
            PasswordWindow pw = new PasswordWindow("");
            pw.PasswordCorrect += new isPasswordCorrect(pw_PasswordCorrect);
            pw.Show();
        }

        void pw_PasswordCorrect(User  user)
        {
            //查询数据库中是否存在该用户，返回用户的信息+权限
            
            //有权限修改修改
            ParametersSettingsWindow win = new ParametersSettingsWindow(user,DeviceInUI);
            win.Show();
            //返回修改结果
            MessageBox.Show(DeviceInUI.Type + " 参数修改成功！");
            LogUtil.Log(false,user.Id+"修改"+DeviceInUI.BuildingId+","+ DeviceInUI.Type+DeviceInUI.DeviceId+" 成功！",0);
        }

        private void history_btn_Click(object sender, RoutedEventArgs e)
        {
            startHistoryWindow();
        }

        public virtual void startHistoryWindow()
        {
            HistoryWindow w = new HistoryWindow(this._device);
            w.Show();
        }

        public void  addLabel(string lb)
        {
            if(label!=null)
            {
                this.label.Add(lb);
            }
        }
        public void deleteLabel(string lb)
        {
            if (label != null)
            {
                this.label.Remove(lb);
            }
        }

        public void startToAlrm()
        {
            DeviceBuzzer.startAlarm();
        }

    }
}
