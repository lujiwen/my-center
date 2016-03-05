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

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs
{
   public  class DeviceUI6517AB : DeviceUI 
    {
     
       static int x = 1;
       DeviceDataBox_6517AB box;
       LabelAndText stateLT;
       LabelAndText valueLT;
       public DeviceUI6517AB(Device d, Frame fm)
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
           box = (DeviceDataBox_6517AB)DeviceInUI.Value;
           Dispatcher.BeginInvoke(new Action(updateLabels));
       }

       private void updateLabels()
       {
           updateChart(box.value);
           valueLT.getValueTextBlock().Text = box.value;
           stateLT.getValueTextBlock().Text = DeviceInUI.State;
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
           stateLT = new LabelAndText("状态:", "", Colors.White);
           valueLT = new LabelAndText("电压:", "", Colors.White);
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

    }
}
