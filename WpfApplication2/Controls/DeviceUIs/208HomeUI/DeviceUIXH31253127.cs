using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controls;
using System.Windows.Controls;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs
{
    public class DeviceUIXH31253127 : DeviceUI 
    {
        public DeviceUIXH31253127(Device d, Frame fm)
               :base(d,fm)
           
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
