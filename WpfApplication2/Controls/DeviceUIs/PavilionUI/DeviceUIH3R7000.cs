using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controls;
using WpfApplication2.Model.Vo;
using System.Windows.Controls;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs.PavilionUI
{
    public class DeviceUIH3R7000 : DeviceUI 
    {
  
        private LabelAndText presureLT;
        private LabelAndText sampleVolumeLT;
        private LabelAndText keepTimeLT;
        private LabelAndText realTraffic;
        public DeviceUIH3R7000(Device d, Frame fm)
            : base(d, fm)
        {
            DeviceInUI.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DeviceInUI_PropertyChanged);
        }

        void DeviceInUI_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //box = (DeviceDataBox_Quality)DeviceInUI.Value;
            //Dispatcher.BeginInvoke(new Action(updateLabels));
        }

        public override void initlabels()
        {
            getInoPanel().Children.Add(presureLT);
            //getInoPanel().Children.Add(realTrafficLT);
            getInoPanel().Children.Add(sampleVolumeLT);
            getInoPanel().Children.Add(keepTimeLT);

        }
    }
}
