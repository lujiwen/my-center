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
using Visifire.Charts;
using System.Windows.Threading;
using System.Threading;

namespace WpfApplication2.CustomMarkers.Controls
{
    /// <summary>
    /// MapTest.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceInfoPanel : UserControl
    {
        Title title;//标题
        Axis axisX ;
        Axis axisY ;
        DataSeries ds;
        private DispatcherTimer tm;
        public DeviceInfoPanel()
        {
            InitializeComponent();
            initChart();
        }
 
        private void initChart()
        {
            title = new Title();
            axisX = new Axis();
            axisY = new Axis();
            axisX.Title = "时间" ;
            axisY.Title = "单位" ;
            ds = new DataSeries();
            ds.RenderAs = RenderAs.Line;
            real_time_chart.Series.Add(ds);
            real_time_chart.Titles.Add(title);
            real_time_chart.AxesX.Add(axisX);
            real_time_chart.AxesY.Add(axisY);
         //   deviceInfo.Children.Add(new LabelAndText("123","123"));

            cnt = 0 ;
            tm = new  DispatcherTimer() ;
            tm.Tick += new EventHandler(updateChart);
            tm.Interval = TimeSpan.FromSeconds(0.1);
            tm.Start();
        }

        int cnt = 0;
        int maxNum = 20;
        private void updateChart(object sender, EventArgs e)
        {
            cnt++;
            Random r = new Random();
            if (cnt  <= maxNum)
            {
                DataPoint p = new DataPoint();
                p.MarkerSize = 8;
                p.AxisXLabel = Convert.ToString(cnt);

                p.YValue = Convert.ToDouble(r.Next(50));
                ds.DataPoints.Add(p);
            }
            else 
            {
                DataPoint p = new DataPoint();
                p.MarkerSize = 8;
                p.AxisXLabel = Convert.ToString(cnt);
                p.XValue = Convert.ToDouble(cnt);
                p.YValue = Convert.ToDouble(r.Next(50));
               
                Console.WriteLine(ds.DataPoints.Count);
            
                 for (int i = 0; i <maxNum-1;i++ )
                 {
                    // 曲线数据更新
                     ds.DataPoints[i].YValue = ds.DataPoints[i + 1].YValue;
                     ds.DataPoints[i].XValue = ds.DataPoints[i + 1].XValue;
                     ds.DataPoints[i].AxisXLabel = ds.DataPoints[i + 1].AxisXLabel;
                }

                 ds.DataPoints[19].YValue = p.YValue;
                 p.AxisXLabel = p.AxisXLabel;
            }
        }
    }
}
