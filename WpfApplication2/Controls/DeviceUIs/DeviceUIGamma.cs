using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controls;
using WpfApplication2.Model.Vo;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApplication2.Controller;
using System.Windows.Data;
using System.Windows;
using WpfApplication2.View.Windows;
using System.ComponentModel;
using WpfApplication2.package;
using Visifire.Charts;
using Project208Home.Model;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs
{
   public  class DeviceUIGamma : DeviceUI 
    {
       static int x = 1;
       DeviceDataBox_Gamma box;
       LabelAndText stateLT;
       LabelAndText gammaTotalLT;
       LabelAndText gammaDoseRateLT;
       private DeviceGamma device;
       public DeviceUIGamma(DeviceGamma d, Frame fm)
         :base(d, fm)
       {
           device = d;
           DeviceInUI.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           box = (DeviceDataBox_Gamma)DeviceInUI.Value;
           valueDic = new Dictionary<int, string>();
           valueDic.Add(0, "伽马总剂量");
           valueDic.Add(1, "伽马剂量率");
           dataSeries = new  DataSeries[valueDic.Count];
           values = new List<string>();
           for (int i = 0; i < valueDic.Count; i++)
           {
               dataSeries[i] = new DataSeries();  //数据系列 
               dataSeries[i].LegendText = valueDic[i];
               dataSeries[i].RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线     
               device_chart.Series.Add(dataSeries[i]);
           }
       }

       void box_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           box = (DeviceDataBox_Gamma)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           values.Clear();
           values.Add(box.GammaTotalDose);
           values.Add(box.GammaDoseRate);
           updateChart(values);

           gammaTotalLT.getValueTextBlock().Text = box.GammaTotalDose;
           gammaDoseRateLT.getValueTextBlock().Text = box.GammaDoseRate;
           stateLT.getValueTextBlock().Text = DeviceInUI.State.Equals("Normal") ? "正常" : "异常";
       }

       public override void initlabels()
       { 
           stateLT = new LabelAndText("状态:", "", Colors.White);
           gammaTotalLT = new LabelAndText("伽马总剂量:", "", Colors.White);
           gammaDoseRateLT = new LabelAndText("伽马剂量率:", "", Colors.White);
           gammaTotalLT.getUnitTextBlock().Text = " "+DeviceInUI.DataUnit;

           getInoPanel().Children.Add(stateLT);
           getInoPanel().Children.Add(gammaTotalLT);
           getInoPanel().Children.Add(gammaDoseRateLT);
       }
       public override void startHistoryWindow()
       {
           HistoryWindow w = new HistoryWindow(device);
           w.Show();
       }
    }
}
 
