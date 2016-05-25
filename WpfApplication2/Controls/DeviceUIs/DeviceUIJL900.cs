using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controls;
using WpfApplication2.Model.Vo;
using System.Windows.Controls;
using System.ComponentModel;
using WpfApplication2.package;
using WpfApplication2.View.Windows;
using System.Windows.Media;
using Visifire.Charts;
using PavilionMonitor;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs.PavilionUI
{
    public class DeviceUIJL900 : DeviceUI 
    {
        DeviceDataJL900Box box;
        LabelAndText presureLT;
        LabelAndText realTrafficLT;
        LabelAndText sampleVolumeLT;
        LabelAndText keepTimeLT;
        DeviceJL900 device; 
        public DeviceUIJL900(DeviceJL900 d, Frame fm)
         :base(d, fm)
       {
           DeviceInUI.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           CurveEnable = false;
           device = d;
           valueDic = new Dictionary<int, string>();
           valueDic.Add(0, "压差");
           valueDic.Add(1, "瞬时流量");
           valueDic.Add(2, "采样体积");
           valueDic.Add(3, "采样时间");
           dataSeries = new DataSeries[valueDic.Count];
           values = new List<string>();
           for (int i = 0; i < valueDic.Count; i++)
           {
               dataSeries[i] = new DataSeries();  //数据系列 
              // dataSeries[i].Legend = valueDic[i];
               dataSeries[i].RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线     
               dataSeries[i].LegendText = valueDic[i];
               device_chart.Series.Add(dataSeries[i]);
           }
       }

       void box_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           box = (DeviceDataJL900Box)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           values.Clear();
           values.Add(box.presure);
           values.Add(box.real_traffic);
           values.Add(box.keep_time);
           values.Add(box.sample_volume);
           updateChart(values);

           presureLT.getValueTextBlock().Text = box.presure;
           realTrafficLT.getValueTextBlock().Text = box.real_traffic;
           sampleVolumeLT.getValueTextBlock().Text = box.sample_volume;
           keepTimeLT.getValueTextBlock().Text = box.keep_time;
           
       }

       public override void initlabels()
       {
           presureLT = new LabelAndText("压差:", "","pa", Colors.White);
           realTrafficLT = new LabelAndText("瞬时流量:", "", "m^3/h",Colors.White);
           sampleVolumeLT = new LabelAndText("采样体积:", "", "m^3", Colors.White);
           keepTimeLT = new LabelAndText("采样时间:", "", "h", Colors.White);

           getInoPanel().Children.Add(presureLT);
           getInoPanel().Children.Add(realTrafficLT);
           getInoPanel().Children.Add(sampleVolumeLT);
           getInoPanel().Children.Add(keepTimeLT);

       }

    }
}
