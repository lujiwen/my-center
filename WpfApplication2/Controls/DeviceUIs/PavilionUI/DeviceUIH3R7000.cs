<<<<<<< HEAD
﻿using System;
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
        LabelAndText stateLT;
        LabelAndText valueLT;
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
    }
}
=======
﻿using System;
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
            //presureLT = new LabelAndText("压差:", "", "pa", Colors.White);
            //realTrafficLT = new LabelAndText("瞬时流量:", "", "m^3/h", Colors.White);
            //sampleVolumeLT = new LabelAndText("采样体积:", "", "m^3", Colors.White);
            //keepTimeLT = new LabelAndText("采样时间:", "", "h", Colors.White);
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
            //getInoPanel().Children.Add(realTrafficLT);
            getInoPanel().Children.Add(sampleVolumeLT);
            getInoPanel().Children.Add(keepTimeLT);

        }
    }
}
>>>>>>> 48a16ab2994032e16b6b4afa0cc66a66ea589e9c
