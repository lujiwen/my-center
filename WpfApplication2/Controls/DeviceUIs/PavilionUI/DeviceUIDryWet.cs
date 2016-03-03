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

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs.PavilionUI
{
    public class DeviceUIDryWet: DeviceUI 
    {
        DeviceDataDryWetBox box;
        LabelAndText rainyStateLT;
        LabelAndText cabStateLT;
        LabelAndText rainTimeLT;
        public DeviceUIDryWet(Device d, Frame fm)
         :base(d, fm)
       {
         // MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(DeviceUI6517AB_MouseLeftButtonDown);
          //getChart().SetBinding();
         // box.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           DeviceInUI.PropertyChanged += new PropertyChangedEventHandler(box_PropertyChanged);
           
           //DeviceBuzzer.startAlarm();
       }

       void box_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           box = (DeviceDataDryWetBox)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           CurveEnable = false;
           updateChart(box.value);
           cabStateLT.getValueTextBlock().Text = box.cab_state;
           rainyStateLT.getValueTextBlock().Text = box.rainy_state;
           rainTimeLT.getValueTextBlock().Text = box.rain_time;
           if(MainWindow.getInstance().IsMute )
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
           cabStateLT = new LabelAndText("状态:", "", Colors.White);
           rainyStateLT = new LabelAndText("下雨状态:", "", Colors.White);
           rainTimeLT = new LabelAndText("下雨时间:", "",Colors.White);
           
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

           getInoPanel().Children.Add(cabStateLT);
           getInoPanel().Children.Add(rainyStateLT);
           getInoPanel().Children.Add(rainTimeLT);
           //getInoPanel().Children.Add(keepTimeLT);

       }
    }

}
