using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controls;
using WpfApplication2.Model.Vo;
using System.Windows.Controls;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs
{
    class DeviceUIPresure : DeviceUI 
    {
        public DeviceUIPresure(Device d, Frame fm)
            : base(d, fm)
        {
 
        }
        public override void initlabels()
        {
            //LabelAndText stateLT = new LabelAndText("状态", "", Colors.White);
            //LabelAndText valueLT = new LabelAndText("电压", "", Colors.White);

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

            //getInoPanel().Children.Add(stateLT);
            //getInoPanel().Children.Add(valueLT);

        }
    }
}
