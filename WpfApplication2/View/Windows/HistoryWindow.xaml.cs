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
using System.Windows.Shapes;
using WpfApplication2.Model.Vo;
using WpfApplication2.Model.Db;
using Visifire.Charts;
using Microsoft.Windows.Controls;

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// HistoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public enum HistoryWindowType { TYPE_DEVICE, TYPE_CAB } ;
        private Cab cab;
        private Device device;
        private HistoryWindowType type;
        public delegate Dictionary<string, List<DeviceData>> ReadDeviceDataDelegate(Device device, string start, string end);
        public delegate Dictionary<string, List<DeviceData>> ReadCabDataDelegate(Cab cab, string start, string end);
        public HistoryWindow()
        {
            InitializeComponent();
        }
        public HistoryWindow(Cab c)
        {
            InitializeComponent();
            this.cab = c;
            Title = c.Name;
            type = HistoryWindowType.TYPE_CAB;
        }
        public HistoryWindow(Device d)
        {
            InitializeComponent();
            this.device = d;
            Title = d.SubSystemName;
            type = HistoryWindowType.TYPE_DEVICE;
        }
        private Visifire.Charts.Title title;
        private DataSeries[] dataSeries;

        private void initChart()
        {
            title = new Visifire.Charts.Title();
            Axis axisX = new Axis();//图表X轴
            Axis axisY = new Axis(); //图表Y轴
            axisX.Title = "时间";//横坐标单位
            axisX.Interval = 20;
            axisY.Title = "单位:xxx";//纵坐标单位

            history_chart.Titles.Add(title);//添加标题
            history_chart.AxesX.Add(axisX);//添加x轴
            history_chart.AxesY.Add(axisY);//添加y轴
        }

        //绘制一条线
        private void drawLine(List<DeviceData> datas)
        {
            DataSeries series = new DataSeries();
            series.RenderAs = RenderAs.Line;
            foreach(DeviceData d in datas)
            {
                DataPoint dataPoint = new DataPoint();//数据点
                dataPoint.MarkerSize = 8;
                DateTime t = DateTime.Parse(d.Time);
                string his_time = t.ToString("HH:mm:ss");
                dataPoint.AxisXLabel = his_time+ "";
                dataPoint.YValue = Double.Parse(d.Value);
                series.DataPoints.Add(dataPoint);//数据点添加到数据系列
            }
            history_chart.Series.Add(series);
        }

        public double MAX = 100;
        public double MIN = 1;  
        //绘制一组曲线
        private void drawLines(Dictionary<string,List<DeviceData>> dataList)
        {
            MessageBox.Show("数据查询完毕，正在绘制历史曲线 ，请稍等...");
            history_chart.Series.Clear();
            dataSeries = new DataSeries[dataList.Count];
            int i = 0;
            foreach(var item in dataList)
            {
                dataSeries[i] = new DataSeries();
                dataSeries[i].LegendText = item.Key;
                dataSeries[i].RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线    
                for (int j = 0; j < item.Value.Count; j++)
                {
                    try
                    {
                        Decimal value = Convert.ToDecimal(item.Value[j].VALUE1);
                        double v = Decimal.ToDouble(value);
                        if (v > MAX)
                        {
                            v = MAX;
                        }
                        else if(v<MIN)
                        {
                            v = MIN;
                        }
                        DataPoint dataPoint = new DataPoint();//数据点
                        dataPoint.MarkerSize = 8;
                        DateTime t = DateTime.Parse(item.Value[j].Time);
                        string his_time = t.ToString("HH:mm:ss");
                        dataPoint.AxisXLabel = his_time + "";
                        //将数据库中的高精度数值的strin转换为double
                        dataPoint.YValue = v ;
                        dataSeries[i].DataPoints.Add(dataPoint);//数据点添加到数据系列
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("HistoryWindow 数据绘制有误" + item.Value[j].VALUE1+DateTime.Now.ToString());
                    }
                }
                history_chart.Series.Add(dataSeries[i]);
                dataSeries[i] = null;
                i++;
            }
            MessageBox.Show("查询结果：\r\n"
                       + getHistoryResult( dataList));
        }

        private string getHistoryResult(Dictionary<string, List<DeviceData>>  dataList)
        {
            string str = "";
            foreach(var v in dataList)
            {
                str += v.Key + ": " + v.Value.Count+"条记录！\r\n";
            }
            return str;
        }
        public static void TestCallback(IAsyncResult data)
        {

        }
        MessageBox box;
        //开始查询
        private void Start_Query_Button_Click(object sender, RoutedEventArgs e)
        {
            String start = "'" + start_time.Value.ToString() + "'";
            String end = "'" + end_time.Value.ToString() + "'";

            //String start = "'2016/5/25 19:30:00'";
            //String end = "'2016/5/26 0:00:00'";
            if (start_time.Value == null || end_time.Value == null)
            {
                MessageBox.Show("起止时间不可缺省！");
                return;
            }
            else if (start_time.Value >= end_time.Value)
            {
                MessageBox.Show("开始时间应早于结束时间！");
                return;
            }
           
            DBManager dataOfDevice = new DBManager();
            string errorCode = "";
            dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
            //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            //{
            //    MessageBox.Show("正在查询数据，请稍等......");
            //}));
            if (type.Equals(HistoryWindowType.TYPE_DEVICE) && device != null)
            {
                ReadDeviceDataDelegate deviceDelegate = new ReadDeviceDataDelegate(dataOfDevice.getDataBetweenStartAndEndTime);
                IAsyncResult result = deviceDelegate.BeginInvoke(device, start, end, TestCallback, "call back");
              //  MessageBox.Show("正在查询数据，请稍等......");
                Dictionary<string, List<DeviceData>> dataDic = deviceDelegate.EndInvoke(result);
                if(dataDic==null)
                {
                    MessageBox.Show("访问数据库错误！");
                    return;
                }
                dataOfDevice.CloseConnection();
                device.startToShowHistoryTable(dataDic);
                //绘制图表 
                drawLines(dataDic);
            }
            else if (type.Equals(HistoryWindowType.TYPE_CAB) && cab != null)
            {
                ReadCabDataDelegate cabDelegate = new ReadCabDataDelegate(dataOfDevice.getDataBetweenStartAndEndTime);
                IAsyncResult result = cabDelegate.BeginInvoke(cab, start, end, TestCallback, "call back");
              //  MessageBox.Show("正在查询数据，请稍等......");
                Dictionary<string, List<DeviceData>> dataDic = cabDelegate.EndInvoke(result);
                dataOfDevice.CloseConnection();
               // device.startToShowHistoryTable(dataDic);
                //绘制图表 
                drawLines(dataDic);
            }
        }
    }
}
