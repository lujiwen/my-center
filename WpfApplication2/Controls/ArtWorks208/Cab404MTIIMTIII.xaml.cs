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
    /// Cab404MTIIMTIII.xaml 的交互逻辑
    /// </summary>
    public partial class Cab404MTIIMTIII : UserControl
    {
        //Dictionary<UInt32, DeviceGroup> subSystemNum_DeviceGroups;
        Dictionary<UInt32, DevicePump> subSystemNum_DevicePump;
        //防止用户连续点击，无效操作
        Boolean canClick; //能够再次点击
        DateTime lastClicktime;//上次点击时间
        Cab cabInArtwork;
        public Cab404MTIIMTIII(Cab cab)
        {
            InitializeComponent();
            cabInArtwork = cab;
            cabInArtwork.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(update);
            InitCab();
        }

        void initBindings()
        {
            //解体氚测量仪
            Binding nowding1 = new Binding();
            nowding1.Source = cabInArtwork.getDeviceByID(25);
            nowding1.Path = new PropertyPath("NowValue");
            subSys1Qualitytb.SetBinding(TextBlock.TextProperty, nowding1);
            //房间氚测量仪
            Binding nowding2 = new Binding();
            nowding2.Source = cabInArtwork.getDeviceByID(27);
            nowding2.Path = new PropertyPath("NowValue");
            subSys2Qualitytb.SetBinding(TextBlock.TextProperty, nowding2);

            //房间氚测量仪
            Binding nowding3 = new Binding();
            nowding3.Source = cabInArtwork.getDeviceByID(29);
            nowding3.Path = new PropertyPath("NowValue");
            subSys3Qualitytb.SetBinding(TextBlock.TextProperty, nowding3);
            //房间氚测量仪
            Binding nowding4 = new Binding();
            nowding4.Source = cabInArtwork.getDeviceByID(31);
            nowding4.Path = new PropertyPath("NowValue");
            subSys4Qualitytb.SetBinding(TextBlock.TextProperty, nowding4);
        }

        private void update(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                subSys1Qualitytb.Text = cabInArtwork.getDeviceByID(25).NowValue;
                subSys2Qualitytb.Text = cabInArtwork.getDeviceByID(27).NowValue;
                subSys3Qualitytb.Text = cabInArtwork.getDeviceByID(29).NowValue;
                subSys4Qualitytb.Text = cabInArtwork.getDeviceByID(31).NowValue;

            }));
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
