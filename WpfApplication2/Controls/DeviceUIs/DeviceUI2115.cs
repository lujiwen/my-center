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
using Project2115Home.Model;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs
{
   public  class DeviceUI2115 : DeviceUI 
    {
       static int x = 1;
       DeviceDataBox_2115 box;

       //float doseNow;//实时值
       //float doseAvg;//平均值
       //float doseStd;//标准差
       //String dataUnit;//单位

       //float rainValue;//雨量值
       //String rainUnit;//雨量单位
       LabelAndText stateLT;
       LabelAndText doseNowValueLT;
       LabelAndText doseAvgValueLT;
       LabelAndText doseStdValueLT;
       LabelAndText rainValueLT;

       private Device2115 device;
       public DeviceUI2115(Device2115 d, Frame fm)
         :base(d, fm)
       {
           device = d;
           DeviceInUI.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           box = (DeviceDataBox_2115)DeviceInUI.Value;


           valueDic = new Dictionary<int, string>();
           valueDic.Add(0, "实时值");
           valueDic.Add(0, "平均值");
           valueDic.Add(0, "标准差");
           valueDic.Add(0, "雨量值");
 
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
           box = (DeviceDataBox_2115)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           values.Clear();
           

           values.Add(box.DoseNow.ToString());
           values.Add(box.DoseAvg.ToString());
           values.Add(box.DoseStd.ToString());
           values.Add(box.RainValue.ToString());
        
           updateChart(values);

           doseNowValueLT.getValueTextBlock().Text = box.DoseNow.ToString();
           doseAvgValueLT.getValueTextBlock().Text = box.DoseAvg.ToString();
           doseStdValueLT.getValueTextBlock().Text = box.DoseStd.ToString();
           rainValueLT.getValueTextBlock().Text = box.RainValue.ToString();
           stateLT.getValueTextBlock().Text = DeviceInUI.State.Equals("Normal") ? "正常" : "异常";
       }

       public override void initlabels()
       {
           float rainValue;//雨量值
           String rainUnit;//雨量单位
           stateLT = new LabelAndText("状态:", "", Colors.White);
           doseNowValueLT = new LabelAndText("实时值:", "", Colors.White);
           doseAvgValueLT = new LabelAndText("中位值:", "", Colors.White);
           doseStdValueLT = new LabelAndText("标准差:", "", Colors.White);
           rainValueLT = new LabelAndText("雨量值:", "", Colors.White);
           //valueLT.getUnitTextBlock().Text = " "+DeviceInUI.DataUnit;

           getInoPanel().Children.Add(stateLT);
           getInoPanel().Children.Add(doseNowValueLT);
           getInoPanel().Children.Add(doseAvgValueLT);
           getInoPanel().Children.Add(doseStdValueLT);
           getInoPanel().Children.Add(rainValueLT);
       }
       public override void startHistoryWindow()
       {
           HistoryWindow w = new HistoryWindow(device);
           w.Show();
       }
    }
}
 
