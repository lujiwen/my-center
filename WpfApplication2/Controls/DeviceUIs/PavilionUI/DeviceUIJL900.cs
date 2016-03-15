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
    public class DeviceUIJL900 : DeviceUI 
    {
        DeviceDataJL900Box box;
        LabelAndText presureLT;
        LabelAndText realTrafficLT;
        LabelAndText sampleVolumeLT;
        LabelAndText keepTimeLT;

        public DeviceUIJL900(Device d, Frame fm)
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
           box = (DeviceDataJL900Box)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           updateChart(box.value);
           presureLT.getValueTextBlock().Text = box.presure;
           realTrafficLT.getValueTextBlock().Text = box.real_traffic;
           sampleVolumeLT.getValueTextBlock().Text = box.sample_volume;
           keepTimeLT.getValueTextBlock().Text = box.keep_time;
           if(MainWindow.getInstance().IsMute )
           {
                DeviceBuzzer.muteBuzzer();
           }
           //if (DeviceInUI.State.Equals("Normal") && !DeviceBuzzer.IsAlarming)
           //{
           //    startToAlrm();
           //}
       }

       public override void initlabels()
       {
           presureLT = new LabelAndText("压差:", "","pa", Colors.White);
           realTrafficLT = new LabelAndText("瞬时流量:", "", "m^3/h",Colors.White);
           sampleVolumeLT = new LabelAndText("采样体积:", "", "m^3", Colors.White);
           keepTimeLT = new LabelAndText("采样时间:", "", "h", Colors.White);
           //valueLT.getUnitTextBlock().Text = " "+DeviceInUI.DataUnit;

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

           getInoPanel().Children.Add(presureLT);
           getInoPanel().Children.Add(realTrafficLT);
           getInoPanel().Children.Add(sampleVolumeLT);
           getInoPanel().Children.Add(keepTimeLT);

       }

    }
}
