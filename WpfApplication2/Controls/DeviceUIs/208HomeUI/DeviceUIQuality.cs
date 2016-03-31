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
using WpfApplication2.package;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs
{
    class DeviceUIQuality : DeviceUI 
    {
        DeviceDataBox_Quality box;
        LabelAndText stateLT;
        LabelAndText valueLT;
        public DeviceUIQuality(Device d, Frame fm)
            : base(d, fm)
        {
            DeviceInUI.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DeviceInUI_PropertyChanged);
        }

        void DeviceInUI_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            box = (DeviceDataBox_Quality)DeviceInUI.Value;
            Dispatcher.BeginInvoke(new Action(updateLabels));
        }

        private void updateLabels()
        {
            updateChart(values);
            valueLT.getValueTextBlock().Text = box.value;
            stateLT.getValueTextBlock().Text = DeviceInUI.State;
            if (MainWindow.getInstance().IsMute)
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
             stateLT = new LabelAndText("状态", "", Colors.White);
             valueLT = new LabelAndText("累计值", "", Colors.White);
             valueLT.getUnitTextBlock().Text = " " + DeviceInUI.DataUnit;
            
             getInoPanel().Children.Add(stateLT);
             getInoPanel().Children.Add(valueLT);

        }
    }
}
