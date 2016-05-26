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
using System.Windows.Data;
using System.Windows;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs.PavilionUI
{
    public class DeviceUIDryWet: DeviceUI 
    {
        DeviceDataBox_DryWet box;
        LabelAndText rainyStateLT;
        LabelAndText cabStateLT;
        LabelAndText rainTimeLT;

        public DeviceUIDryWet(Device d, Frame fm)
         :base(d, fm)
       {
           DeviceInUI.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           CurveEnable = false;
           box = (DeviceDataBox_DryWet)DeviceInUI.Value;
       }

       void box_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           box = (DeviceDataBox_DryWet)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           CurveEnable = false;
           cabStateLT.getValueTextBlock().Text = box.cab_state.Equals("Normal")?"正常":"异常";
           rainyStateLT.getValueTextBlock().Text = box.rainy_state;
           rainTimeLT.getValueTextBlock().Text = box.rain_time;
       }

       public override void initlabels()
       {
           cabStateLT = new LabelAndText("盖子状态:", "", Colors.White);
           rainyStateLT = new LabelAndText("下雨状态:", "", Colors.White);
           rainTimeLT = new LabelAndText("降雨时间（分钟）:", "",Colors.White);

           getInoPanel().Children.Add(cabStateLT);
           getInoPanel().Children.Add(rainyStateLT);
           getInoPanel().Children.Add(rainTimeLT);
       }
    }

}

