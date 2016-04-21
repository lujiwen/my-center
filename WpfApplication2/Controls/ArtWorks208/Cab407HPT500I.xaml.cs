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
using System.Windows.Threading;
using Project208Home.Model;
using WpfApplication2.Model.Vo;

namespace Project208Home.Views.ArtWorks208
{
    /// <summary>
    /// Cab407HPT500I.xaml 的交互逻辑
    /// </summary>
    public partial class Cab407HPT500I : UserControl
    {
        //Dictionary<UInt32, DeviceGroup> subSystemNum_DeviceGroups;
        Dictionary<UInt32, DevicePump> subSystemNum_DevicePump;
        //防止用户连续点击，无效操作
        Boolean canClick; //能够再次点击
        DateTime lastClicktime;//上次点击时间
        Cab cabInArtwork;
        public Cab407HPT500I(Cab cab)
        {
            InitializeComponent();
            cabInArtwork = cab;
            cabInArtwork.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(update);
            InitCab();
        }

        private void update(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                subSys16517ABtb.Text  = cabInArtwork.Devices[0].NowValue;
                subSys26517ABtb.Text  = cabInArtwork.Devices[4].NowValue;
                subSys36517ABtb.Text  = cabInArtwork.Devices[8].NowValue;
                subSys46517ABtb.Text  = cabInArtwork.Devices[12].NowValue;

                subSys1Qualitytb.Text = cabInArtwork.Devices[1].NowValue;
                subSys2Qualitytb.Text = cabInArtwork.Devices[5].NowValue;
                subSys3Qualitytb.Text = cabInArtwork.Devices[9].NowValue;
                subSys4Qualitytb.Text = cabInArtwork.Devices[13].NowValue;

            }));
        }

        private void initBindings()
        {
            //压力计
            Binding nowding1 = new Binding();
            nowding1.Source = cabInArtwork.Devices[0];
            nowding1.Path = new PropertyPath("NowValue");
            subSys1Presuretb.SetBinding(TextBlock.TextProperty, nowding1);
            //压力计
            Binding nowding2 = new Binding();
            nowding2.Source = cabInArtwork.Devices[0];
            nowding2.Path = new PropertyPath("NowValue");
            subSys2Presuretb.SetBinding(TextBlock.TextProperty, nowding2);
            //压力计
            Binding nowding3 = new Binding();
            nowding3.Source = cabInArtwork.Devices[0];
            nowding3.Path = new PropertyPath("NowValue");
            subSys3Presuretb.SetBinding(TextBlock.TextProperty, nowding3);
            //压力计
            Binding nowding4 = new Binding();
            nowding4.Source = cabInArtwork.Devices[0];
            nowding4.Path = new PropertyPath("NowValue");
            subSys4Presuretb.SetBinding(TextBlock.TextProperty, nowding4);

            //特排氚测量仪
            Binding nowding5 = new Binding();
            nowding5.Source = cabInArtwork.Devices[0];
            nowding5.Path = new PropertyPath("NowValue");
            subSys16517ABtb.SetBinding(TextBlock.TextProperty, nowding5);
            //特排氚测量仪
            Binding nowding6 = new Binding();
            nowding6.Source = cabInArtwork.Devices[4];
            nowding6.Path = new PropertyPath("NowValue");
            subSys26517ABtb.SetBinding(TextBlock.TextProperty, nowding6);
            //特排氚测量仪
            Binding nowding7 = new Binding();
            nowding7.Source = cabInArtwork.Devices[8];
            nowding7.Path = new PropertyPath("NowValue");
            subSys36517ABtb.SetBinding(TextBlock.TextProperty, nowding7);
            //特排氚测量仪
            Binding nowding8 = new Binding();
            nowding8.Source = cabInArtwork.Devices[12];
            nowding8.Path = new PropertyPath("NowValue");
            subSys46517ABtb.SetBinding(TextBlock.TextProperty, nowding8);

             //特排氚测量仪
            Binding nowding9 = new Binding();
            nowding9.Source = cabInArtwork.Devices[1];
            nowding9.Path = new PropertyPath("NowValue");
            subSys1Qualitytb.SetBinding(TextBlock.TextProperty, nowding9); 
            //特排氚测量仪
            Binding nowding10 = new Binding();
            nowding10.Source = cabInArtwork.Devices[5];
            nowding10.Path = new PropertyPath("NowValue");
            subSys2Qualitytb.SetBinding(TextBlock.TextProperty, nowding10);
            //特排氚测量仪
            Binding nowding11 = new Binding();
            nowding11.Source = cabInArtwork.Devices[9];
            nowding11.Path = new PropertyPath("NowValue");
            subSys3Qualitytb.SetBinding(TextBlock.TextProperty, nowding11); 
            //特排氚测量仪
            Binding nowding12 = new Binding();
            nowding12.Source = cabInArtwork.Devices[13];
            nowding12.Path = new PropertyPath("NowValue");
            subSys4Qualitytb.SetBinding(TextBlock.TextProperty, nowding12);
        }
        /// <summary>
        /// 初始化柜子工艺图
        /// </summary>
        private void InitCab()
        {
            initBindings();
        }
    }
}
