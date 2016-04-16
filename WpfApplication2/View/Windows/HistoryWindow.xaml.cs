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


        //绘制一组曲线
        private void drawLines(Dictionary<string,List<DeviceData>> dataList)
        {
            dataSeries = new DataSeries[dataList.Count];
            int i = 0;
            foreach(var item in dataList)
            {
                dataSeries[i] = new DataSeries();
                dataSeries[i].LegendText = item.Key;
                dataSeries[i].RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线    
                for (int j = 0; j < item.Value.Count; j++)
                {
                    DataPoint dataPoint = new DataPoint();//数据点
                    dataPoint.MarkerSize = 8;
                    DateTime t = DateTime.Parse(item.Value[j].Time);
                    string his_time = t.ToString("HH:mm:ss");
                    dataPoint.AxisXLabel = his_time + "";
                    dataPoint.YValue = item.Value[j].VALUE1;
                    dataSeries[i].DataPoints.Add(dataPoint);//数据点添加到数据系列
                }
                history_chart.Series.Add(dataSeries[i]);
            }
        }

        //开始查询
          private void Start_Query_Button_Click(object sender, RoutedEventArgs e)
        {
                String start = "'2016/4/15 0:00:00'";
                String end   = "'2016/4/16 0:00:00'";

                DBManager dataOfDevice = new DBManager();
                string errorCode = "";
                dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
                if (type.Equals(HistoryWindowType.TYPE_DEVICE) && device != null)
                {
                    Dictionary<string, List<DeviceData>> dataDic = dataOfDevice.getDataBetweenStartAndEndTime(device, start, end);
                    dataOfDevice.CloseConnection();
                    if (device.showCurve)
                    {
                        drawLines(dataDic);
                    }
                    else
                    {
                        MessageBox.Show("不给显示！");
                    }
                }
                else if (type.Equals(HistoryWindowType.TYPE_CAB) && cab != null)
                {
                    Dictionary<string, List<DeviceData>> dataDic = dataOfDevice.getDataBetweenStartAndEndTime(cab, start, end);
                    dataOfDevice.CloseConnection();
                    drawLines(dataDic);
                   
                }
              
             
                
            //if(start_time.Value==null||end_time.Value==null)
            //{
            //    MessageBox.Show("起止时间不可缺省！");
            //    return;
            //}
            //else 
            //{
                //String start = "'" + start_time.Value.ToString() + "'";
                //String end = "'" + end_time.Value.ToString() + "'";

              
                // if (type.Equals(HistoryWindowType.TYPE_CAB) && cab != null)
                //{
                //    List<List<DeviceForShow>> datalist = new List<List<DeviceForShow>>();
                   
                //    string errorCode = "";
                //    dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
                //   //dataOfDevice.getDataBetweenStartAndEndTime(cab.BuildingId,cab.Devices[0].DeviceId,start,end);

                //    for (int i = 0; i < cab.Devices.Count;i++ )
                //    {
                //        //List<DeviceForShow> datas = dataOfDevice.getDataBetweenStartAndEndTime(int.Parse(cab.BuildingId), int.Parse(cab.Devices[i].DeviceId), start, end);
                //        datalist.Add(datas);
                //    }
                //    drawLines(datalist);
                ////    Console.WriteLine("查询到数据点一共" + datas.Count);
                 //}
                // if (type.Equals(HistoryWindowType.TYPE_DEVICE) && device != null)
                //{
                   
                //    string errorCode = "";
                    
                //    //dataOfDevice.getDataBetweenStartAndEndTime(device, start, end);

                //   // List<DeviceForShow> datas = dbMng.getDataBetweenStartAndEndTime(int.Parse(device.BuildingId), int.Parse(device.DeviceId), start, end);
                //    //Console.WriteLine("查询到数据点一共" + datas.Count);
                //    //drawLine(datas);
                //}
            }


        //private void Start_Query_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //if(start_time.Value==null||end_time.Value==null)
        //    //{
        //    //    MessageBox.Show("起止时间不可缺省！");
        //    //    return;
        //    //}
        //    //else 
        //    {
        //        //String start = "'" + start_time.Value.ToString() + "'";
        //        //String end = "'" + end_time.Value.ToString() + "'";

        //        String start = "'2016/4/15 0:00:00'";
        //        String end   = "'2016/4/16 0:00:00'";

        //        DBManager dataOfDevice = new DBManager();
        //        // if (type.Equals(HistoryWindowType.TYPE_CAB) && cab != null)
        //        //{
        //        //    List<List<DeviceForShow>> datalist = new List<List<DeviceForShow>>();
                   
        //        //    string errorCode = "";
        //        //    dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
        //        //   //dataOfDevice.getDataBetweenStartAndEndTime(cab.BuildingId,cab.Devices[0].DeviceId,start,end);

        //        //    for (int i = 0; i < cab.Devices.Count;i++ )
        //        //    {
        //        //        //List<DeviceForShow> datas = dataOfDevice.getDataBetweenStartAndEndTime(int.Parse(cab.BuildingId), int.Parse(cab.Devices[i].DeviceId), start, end);
        //        //        datalist.Add(datas);
        //        //    }
        //        //    drawLines(datalist);
        //        ////    Console.WriteLine("查询到数据点一共" + datas.Count);
        //         }
        //         if (type.Equals(HistoryWindowType.TYPE_DEVICE) && device != null)
        //        {
                   
        //            string errorCode = "";
                    
        //            //dataOfDevice.getDataBetweenStartAndEndTime(device, start, end);

        //           // List<DeviceForShow> datas = dbMng.getDataBetweenStartAndEndTime(int.Parse(device.BuildingId), int.Parse(device.DeviceId), start, end);
        //            //Console.WriteLine("查询到数据点一共" + datas.Count);
        //            //drawLine(datas);
        //        }
        //    }

    }

     
}
