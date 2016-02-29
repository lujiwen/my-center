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
    public partial class HistoryPanel : UserControl
    {
        Title title;//标题
        Axis axisX ;
        Axis axisY ;
        DataSeries ds;
        int maxNum;
        public HistoryPanel()
        {
            InitializeComponent();
            initChart();
        }
 
        private void initChart()
        {
            maxNum = 20 ;
            title = new Title();
            axisX = new Axis();
            axisY = new Axis();
            axisX.Title = "时间" ;
            axisY.Title = "单位" ;
            ds = new DataSeries();
            ds.RenderAs = RenderAs.Line;
           
            Random r = new Random();
            for (int i = 0; i < maxNum;i++ )
            {
                DataPoint p = new DataPoint();
                p.MarkerSize = 8;
                p.AxisXLabel = Convert.ToString(i);
                p.YValue = r.Next(20);
                ds.DataPoints.Add(p);
            }
            history_chart.Series.Add(ds);
         
        }

     
    }
}
