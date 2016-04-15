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
   public  class DeviceUI6517AB : DeviceUI 
    {
       static int x = 1;
       DeviceDataBox_6517AB box;
       LabelAndText stateLT;
       LabelAndText valueLT;
       private Device6517AB device;
       public DeviceUI6517AB(Device6517AB d, Frame fm)
         :base(d, fm)
       {
           device = d;
           DeviceInUI.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           box = (DeviceDataBox_6517AB)DeviceInUI.Value;
           valueDic = new Dictionary<int, string>();
           valueDic.Add(0, "keep_time");
           dataSeries = new  DataSeries[valueDic.Count];
           values = new List<string>();
           for (int i = 0; i < valueDic.Count; i++)
           {
               dataSeries[i] = new DataSeries();  //数据系列 
               dataSeries[i].Legend = valueDic[i];
               dataSeries[i].RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线     
               device_chart.Series.Add(dataSeries[i]);
           }
       }

       void box_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           box = (DeviceDataBox_6517AB)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           values.Clear();
           values.Add(box.value);
           updateChart(values);

           valueLT.getValueTextBlock().Text = box.value;
           stateLT.getValueTextBlock().Text = DeviceInUI.State.Equals("Normal") ? "正常" : "异常";

           if(MainWindow.getInstance().IsMute)
           {
               //DeviceBuzzer.muteBuzzer();
           }
           //if (DeviceInUI.State.Equals("Normal") && !DeviceBuzzer.IsAlarming)
           //{
           //    startToAlrm();
           //}
       }

       public override void initlabels()
       {
           stateLT = new LabelAndText("状态:", "", Colors.White);
           valueLT = new LabelAndText("浓度:", "", Colors.White);
           valueLT.getUnitTextBlock().Text = " "+DeviceInUI.DataUnit;

           //实时值绑定
           //Binding valueBingding = new Binding();
           //valueBingding.Source = DeviceInUI;
           //valueBingding.Path = new PropertyPath("NowValue");
           //valueLT.getValueTextBlock().SetBinding(TextBlock.TextProperty, valueBingding);

           ////状态绑定
           //Binding stateBinding = new Binding();
           //stateBinding.Source = DeviceInUI;
           //stateBinding.Path = new PropertyPath("State");
           //stateLT.getValueTextBlock().SetBinding(TextBlock.TextProperty, stateBinding);

           getInoPanel().Children.Add(stateLT);
           getInoPanel().Children.Add(valueLT);
       }
       public override void startHistoryWindow()
       {
           HistoryWindow w = new HistoryWindow(device);
           w.Show();
       }
    }
}
 
