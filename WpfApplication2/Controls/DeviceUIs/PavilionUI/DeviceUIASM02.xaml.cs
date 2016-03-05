using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PavilionMonitor;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.CustomMarkers.Controls.DeviceUIs.PavilionUI
{
    /// <summary>
    /// DeviceUIASM02.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceUIASM02 : UserControl
    {
       private ASM02 asm02;

       public DeviceUIASM02(Device d, Frame fm)
        {
            InitializeComponent();
            this.asm02 = (ASM02)d;
            init();
        }
        private void init()
        {
            initBingdings();
        }
        

        /// <summary>
        /// 初始化界面的数据绑定
        /// </summary>
        private void initBingdings()
        {
            // asm02   ab 8
            Binding bind_asm02_status = new Binding();
            bind_asm02_status.Source = asm02;
            bind_asm02_status.Path = new PropertyPath("DevState");
            ASM02_Status.SetBinding(TextBlock.TextProperty, bind_asm02_status);


            Binding bind_ab_1 = new Binding();
            bind_ab_1.Source = asm02;
            bind_ab_1.Path = new PropertyPath("Ab_1");
            ab_1.SetBinding(TextBlock.TextProperty, bind_ab_1);
            Binding bind_ab_2 = new Binding();
            bind_ab_2.Source = asm02;
            bind_ab_2.Path = new PropertyPath("Ab_2");
            ab_2.SetBinding(TextBlock.TextProperty, bind_ab_2);
            Binding bind_ab_3 = new Binding();
            bind_ab_3.Source = asm02;
            bind_ab_3.Path = new PropertyPath("Ab_3");
            ab_3.SetBinding(TextBlock.TextProperty, bind_ab_3);
            Binding bind_ab_4 = new Binding();
            bind_ab_4.Source = asm02;
            bind_ab_4.Path = new PropertyPath("Ab_4");
            ab_4.SetBinding(TextBlock.TextProperty, bind_ab_4);
            Binding bind_ab_5 = new Binding();
            bind_ab_5.Source = asm02;
            bind_ab_5.Path = new PropertyPath("Ab_5");
            ab_5.SetBinding(TextBlock.TextProperty, bind_ab_5);
            Binding bind_ab_6 = new Binding();
            bind_ab_6.Source = asm02;
            bind_ab_6.Path = new PropertyPath("Ab_6");
            ab_6.SetBinding(TextBlock.TextProperty, bind_ab_6);
            Binding bind_ab_7 = new Binding();
            bind_ab_7.Source = asm02;
            bind_ab_7.Path = new PropertyPath("Ab_7");
            ab_7.SetBinding(TextBlock.TextProperty, bind_ab_7);
            Binding bind_ab_8 = new Binding();
            bind_ab_8.Source = asm02;
            bind_ab_8.Path = new PropertyPath("Ab_8");
            ab_8.SetBinding(TextBlock.TextProperty, bind_ab_8);

            // ec  8
            Binding bind_ec_1 = new Binding();
            bind_ec_1.Source = asm02;
            bind_ec_1.Path = new PropertyPath("Ec_1");
            ec_1.SetBinding(TextBlock.TextProperty, bind_ec_1);
            Binding bind_ec_2 = new Binding();
            bind_ec_2.Source = asm02;
            bind_ec_2.Path = new PropertyPath("Ec_2");
            ec_2.SetBinding(TextBlock.TextProperty, bind_ec_2);
            Binding bind_ec_3 = new Binding();
            bind_ec_3.Source = asm02;
            bind_ec_3.Path = new PropertyPath("Ec_3");
            ec_3.SetBinding(TextBlock.TextProperty, bind_ec_3);
            Binding bind_ec_4 = new Binding();
            bind_ec_4.Source = asm02;
            bind_ec_4.Path = new PropertyPath("Ec_4");
            ec_4.SetBinding(TextBlock.TextProperty, bind_ec_4);
            Binding bind_ec_5 = new Binding();
            bind_ec_5.Source = asm02;
            bind_ec_5.Path = new PropertyPath("Ec_5");
            ec_5.SetBinding(TextBlock.TextProperty, bind_ec_5);
            Binding bind_ec_6 = new Binding();
            bind_ec_6.Source = asm02;
            bind_ec_6.Path = new PropertyPath("Ec_6");
            ec_6.SetBinding(TextBlock.TextProperty, bind_ec_6);
            Binding bind_ec_7 = new Binding();
            bind_ec_7.Source = asm02;
            bind_ec_7.Path = new PropertyPath("Ec_7");
            ec_7.SetBinding(TextBlock.TextProperty, bind_ec_7);
            Binding bind_ec_8 = new Binding();
            bind_ec_8.Source = asm02;
            bind_ec_8.Path = new PropertyPath("Ec_8");
            ec_8.SetBinding(TextBlock.TextProperty, bind_ec_8);

            // fl  10
            Binding bind_fl_1 = new Binding();
            bind_fl_1.Source = asm02;
            bind_fl_1.Path = new PropertyPath("Fl_1");
            fl_1.SetBinding(TextBlock.TextProperty, bind_fl_1);
            Binding bind_fl_2 = new Binding();
            bind_fl_2.Source = asm02;
            bind_fl_2.Path = new PropertyPath("Fl_2");
            fl_2.SetBinding(TextBlock.TextProperty, bind_fl_2);
            Binding bind_fl_3 = new Binding();
            bind_fl_3.Source = asm02;
            bind_fl_3.Path = new PropertyPath("Fl_3");
            fl_3.SetBinding(TextBlock.TextProperty, bind_fl_3);
            Binding bind_fl_4 = new Binding();
            bind_fl_4.Source = asm02;
            bind_fl_4.Path = new PropertyPath("Fl_4");
            fl_4.SetBinding(TextBlock.TextProperty, bind_fl_4);
            Binding bind_fl_5 = new Binding();
            bind_fl_5.Source = asm02;
            bind_fl_5.Path = new PropertyPath("Fl_5");
            fl_5.SetBinding(TextBlock.TextProperty, bind_fl_5);
            Binding bind_fl_6 = new Binding();
            bind_fl_6.Source = asm02;
            bind_fl_6.Path = new PropertyPath("Fl_6");
            fl_6.SetBinding(TextBlock.TextProperty, bind_fl_6);
            Binding bind_fl_7 = new Binding();
            bind_fl_7.Source = asm02;
            bind_fl_7.Path = new PropertyPath("Fl_7");
            fl_7.SetBinding(TextBlock.TextProperty, bind_fl_7);
            Binding bind_fl_8 = new Binding();
            bind_fl_8.Source = asm02;
            bind_fl_8.Path = new PropertyPath("Fl_8");
            fl_8.SetBinding(TextBlock.TextProperty, bind_fl_8);
            Binding bind_fl_9 = new Binding();
            bind_fl_9.Source = asm02;
            bind_fl_9.Path = new PropertyPath("Fl_9");
            fl_9.SetBinding(TextBlock.TextProperty, bind_fl_9);
            Binding bind_fl_10 = new Binding();
            bind_fl_10.Source = asm02;
            bind_fl_10.Path = new PropertyPath("Fl_10");
            fl_10.SetBinding(TextBlock.TextProperty, bind_fl_10);

            // ga 7
            Binding bind_ga_1 = new Binding();
            bind_ga_1.Source = asm02;
            bind_ga_1.Path = new PropertyPath("Ga_1");
            ga_1.SetBinding(TextBlock.TextProperty, bind_ga_1);
            Binding bind_ga_2 = new Binding();
            bind_ga_2.Source = asm02;
            bind_ga_2.Path = new PropertyPath("Ga_2");
            ga_2.SetBinding(TextBlock.TextProperty, bind_ga_2);
            Binding bind_ga_3 = new Binding();
            bind_ga_3.Source = asm02;
            bind_ga_3.Path = new PropertyPath("Ga_3");
            ga_3.SetBinding(TextBlock.TextProperty, bind_ga_3);
            Binding bind_ga_4 = new Binding();
            bind_ga_4.Source = asm02;
            bind_ga_4.Path = new PropertyPath("Ga_4");
            ga_4.SetBinding(TextBlock.TextProperty, bind_ga_4);
            Binding bind_ga_5 = new Binding();
            bind_ga_5.Source = asm02;
            bind_ga_5.Path = new PropertyPath("Ga_5");
            ga_5.SetBinding(TextBlock.TextProperty, bind_ga_5);
            Binding bind_ga_6 = new Binding();
            bind_ga_6.Source = asm02;
            bind_ga_6.Path = new PropertyPath("Ga_6");
            ga_6.SetBinding(TextBlock.TextProperty, bind_ga_6);
            Binding bind_ga_7 = new Binding();
            bind_ga_7.Source = asm02;
            bind_ga_7.Path = new PropertyPath("Ga_7");
            ga_7.SetBinding(TextBlock.TextProperty, bind_ga_7);

            // gi  7
            Binding bind_gi_1 = new Binding();
            bind_gi_1.Source = asm02;
            bind_gi_1.Path = new PropertyPath("Gi_1");
            gi_1.SetBinding(TextBlock.TextProperty, bind_gi_1);
            Binding bind_gi_2 = new Binding();
            bind_gi_2.Source = asm02;
            bind_gi_2.Path = new PropertyPath("Gi_2");
            gi_2.SetBinding(TextBlock.TextProperty, bind_gi_2);
            Binding bind_gi_3 = new Binding();
            bind_gi_3.Source = asm02;
            bind_gi_3.Path = new PropertyPath("Gi_3");
            gi_3.SetBinding(TextBlock.TextProperty, bind_gi_3);
            Binding bind_gi_4 = new Binding();
            bind_gi_4.Source = asm02;
            bind_gi_4.Path = new PropertyPath("Gi_4");
            gi_4.SetBinding(TextBlock.TextProperty, bind_gi_4);
            Binding bind_gi_5 = new Binding();
            bind_gi_5.Source = asm02;
            bind_gi_5.Path = new PropertyPath("Gi_5");
            gi_5.SetBinding(TextBlock.TextProperty, bind_gi_5);
            Binding bind_gi_6 = new Binding();
            bind_gi_6.Source = asm02;
            bind_gi_6.Path = new PropertyPath("Gi_6");
            gi_6.SetBinding(TextBlock.TextProperty, bind_gi_6);
            Binding bind_gi_7 = new Binding();
            bind_gi_7.Source = asm02;
            bind_gi_7.Path = new PropertyPath("Gi_7");
            gi_7.SetBinding(TextBlock.TextProperty, bind_gi_7);

            //me  9
            Binding bind_me_1 = new Binding();
            bind_me_1.Source = asm02;
            bind_me_1.Path = new PropertyPath("Me_1");
            me_1.SetBinding(TextBlock.TextProperty, bind_me_1);
            Binding bind_me_2 = new Binding();
            bind_me_2.Source = asm02;
            bind_me_2.Path = new PropertyPath("Me_2");
            me_2.SetBinding(TextBlock.TextProperty, bind_me_2);
            Binding bind_me_3 = new Binding();
            bind_me_3.Source = asm02;
            bind_me_3.Path = new PropertyPath("Me_3");
            me_3.SetBinding(TextBlock.TextProperty, bind_me_3);
            Binding bind_me_4 = new Binding();
            bind_me_4.Source = asm02;
            bind_me_4.Path = new PropertyPath("Me_4");
            me_4.SetBinding(TextBlock.TextProperty, bind_me_4);
            Binding bind_me_5 = new Binding();
            bind_me_5.Source = asm02;
            bind_me_5.Path = new PropertyPath("Me_5");
            me_5.SetBinding(TextBlock.TextProperty, bind_me_5);
            Binding bind_me_6 = new Binding();
            bind_me_6.Source = asm02;
            bind_me_6.Path = new PropertyPath("Me_6");
            me_6.SetBinding(TextBlock.TextProperty, bind_me_6);
            Binding bind_me_7 = new Binding();
            bind_me_7.Source = asm02;
            bind_me_7.Path = new PropertyPath("Me_7");
            me_7.SetBinding(TextBlock.TextProperty, bind_me_7);
            Binding bind_me_8 = new Binding();
            bind_me_8.Source = asm02;
            bind_me_8.Path = new PropertyPath("Me_8");
            me_8.SetBinding(TextBlock.TextProperty, bind_me_8);
            Binding bind_me_9 = new Binding();
            bind_me_9.Source = asm02;
            bind_me_9.Path = new PropertyPath("Me_9");
            me_9.SetBinding(TextBlock.TextProperty, bind_me_9);
            Binding bind_me_10 = new Binding();
            bind_me_10.Source = asm02;
            bind_me_10.Path = new PropertyPath("Me_10");
            me_10.SetBinding(TextBlock.TextProperty, bind_me_10);

            //oi 7
            Binding bind_oi_1 = new Binding();
            bind_oi_1.Source = asm02;
            bind_oi_1.Path = new PropertyPath("Oi_1");
            oi_1.SetBinding(TextBlock.TextProperty, bind_oi_1);
            Binding bind_oi_2 = new Binding();
            bind_oi_2.Source = asm02;
            bind_oi_2.Path = new PropertyPath("Oi_2");
            oi_2.SetBinding(TextBlock.TextProperty, bind_oi_2);
            Binding bind_oi_3 = new Binding();
            bind_oi_3.Source = asm02;
            bind_oi_3.Path = new PropertyPath("Oi_3");
            oi_3.SetBinding(TextBlock.TextProperty, bind_oi_3);
            Binding bind_oi_4 = new Binding();
            bind_oi_4.Source = asm02;
            bind_oi_4.Path = new PropertyPath("Oi_4");
            oi_4.SetBinding(TextBlock.TextProperty, bind_oi_4);
            Binding bind_oi_5 = new Binding();
            bind_oi_5.Source = asm02;
            bind_oi_5.Path = new PropertyPath("Oi_5");
            oi_5.SetBinding(TextBlock.TextProperty, bind_oi_5);
            Binding bind_oi_6 = new Binding();
            bind_oi_6.Source = asm02;
            bind_oi_6.Path = new PropertyPath("Oi_6");
            oi_6.SetBinding(TextBlock.TextProperty, bind_oi_6);
            Binding bind_oi_7 = new Binding();
            bind_oi_7.Source = asm02;
            bind_oi_7.Path = new PropertyPath("Oi_7");
            oi_7.SetBinding(TextBlock.TextProperty, bind_oi_7);

            // rn 6
            Binding bind_rn_1 = new Binding();
            bind_rn_1.Source = asm02;
            bind_rn_1.Path = new PropertyPath("Rn_1");
            rn_1.SetBinding(TextBlock.TextProperty, bind_rn_1);
            Binding bind_rn_2 = new Binding();
            bind_rn_2.Source = asm02;
            bind_rn_2.Path = new PropertyPath("Rn_2");
            rn_2.SetBinding(TextBlock.TextProperty, bind_rn_2);
            Binding bind_rn_3 = new Binding();
            bind_rn_3.Source = asm02;
            bind_rn_3.Path = new PropertyPath("Rn_3");
            rn_3.SetBinding(TextBlock.TextProperty, bind_rn_3);
            Binding bind_rn_4 = new Binding();
            bind_rn_4.Source = asm02;
            bind_rn_4.Path = new PropertyPath("Rn_4");
            rn_4.SetBinding(TextBlock.TextProperty, bind_rn_4);
            Binding bind_rn_5 = new Binding();
            bind_rn_5.Source = asm02;
            bind_rn_5.Path = new PropertyPath("Rn_5");
            rn_5.SetBinding(TextBlock.TextProperty, bind_rn_5);
            Binding bind_rn_6 = new Binding();
            bind_rn_6.Source = asm02;
            bind_rn_6.Path = new PropertyPath("Rn_6");
            rn_6.SetBinding(TextBlock.TextProperty, bind_rn_6);

        }
    }
}
