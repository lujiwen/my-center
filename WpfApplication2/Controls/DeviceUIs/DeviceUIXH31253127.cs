using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controls;
using System.Windows.Controls;
using WpfApplication2.Model.Vo;
using Visifire.Charts;
using WpfApplication2.package;
using System.Windows.Media;
using WpfApplication2.View.Windows;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs
{
    public class DeviceUIXH31253127 : DeviceUI 
    {
        DeviceDataBox_XH3125 box;
        LabelAndText stateLT;
        LabelAndText valueLT;
        public DeviceUIXH31253127(Device d, Frame fm)
               :base(d,fm)          
        {
            DeviceInUI.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DeviceInUI_PropertyChanged);
            valueDic = new Dictionary<int, string>();
            valueDic.Add(0, "浓度");
            dataSeries = new DataSeries[valueDic.Count];
            values = new List<string>();
            for (int i = 0; i < valueDic.Count; i++)
            {
                dataSeries[i] = new DataSeries();  //数据系列 
                dataSeries[i].LegendText = valueDic[i];
                dataSeries[i].RenderAs = RenderAs.Line;      //Spline : 平滑曲线 Line : 折线     
                device_chart.Series.Add(dataSeries[i]);
            }
        }
        public override void initlabels()
        {
             stateLT = new LabelAndText("状态：", "", Colors.White);
             valueLT = new LabelAndText("浓度：", "", Colors.White);

            ////实时值绑定
            //Binding valueBingding = new Binding();
            //valueBingding.Source = DeviceInUI;
            //valueBingding.Path = new PropertyPath("Box");
            //valueLT.getValueTextBlock().SetBinding(TextBlock.TextProperty, valueBingding);

            ////状态绑定
            //Binding stateBinding = new Binding();
            //stateBinding.Source = DeviceInUI;
            //stateBinding.Path = new PropertyPath("State");
            //valueLT.getValueTextBlock().SetBinding(TextBlock.TextProperty, stateBinding);

             getInoPanel().Children.Add(stateLT);
             getInoPanel().Children.Add(valueLT);

        }
        void DeviceInUI_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            box = (DeviceDataBox_XH3125)DeviceInUI.Value;
             Dispatcher.BeginInvoke(new Action(updateLabels));
        }
        private void updateLabels()
        {
            values.Clear();
            values.Add(box.value);
            updateChart(values);

            stateLT.getValueTextBlock().Text = DeviceInUI.State.Equals("Normal")?"正常":"异常";
            valueLT.getValueTextBlock().Text = box.value ;
            if (MainWindow.getInstance().IsMute)
            {
                //DeviceBuzzer.muteBuzzer();
            }
            //if (DeviceInUI.State.Equals("Normal") && !DeviceBuzzer.IsAlarming)
            //{
            //    startToAlrm();
            //}
        }
    }
}
