﻿using System;
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
   public  class DeviceUINeutron : DeviceUI 
    {
       static int x = 1;
       DeviceDataBox_Neutron box;
       LabelAndText stateLT;
       LabelAndText valueLT;
       private DeviceNeutron device;
       public DeviceUINeutron(DeviceNeutron d, Frame fm)
         :base(d, fm)
       {
           device = d;
           DeviceInUI.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           box = (DeviceDataBox_Neutron)DeviceInUI.Value;

           valueDic = new Dictionary<int, string>();
           valueDic.Add(0, "中子计数率");
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
           box = (DeviceDataBox_Neutron)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           values.Clear();
           values.Add(box.neutronRate);
           updateChart(values);

           valueLT.getValueTextBlock().Text = box.neutronRate;
           stateLT.getValueTextBlock().Text = DeviceInUI.State.Equals("Normal") ? "正常" : "异常";
       }

       public override void initlabels()
       {
           stateLT = new LabelAndText("状态:", "", Colors.White);
           valueLT = new LabelAndText("中子计数率:", "", Colors.White);
           valueLT.getUnitTextBlock().Text = " "+DeviceInUI.DataUnit;

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
 
