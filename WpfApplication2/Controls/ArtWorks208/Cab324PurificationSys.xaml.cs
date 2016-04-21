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
using Project208Home.Model;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using WpfApplication2.Model.Vo;
using WpfApplication2.Util;

namespace Project208Home.Views.ArtWorks208
{
    /// <summary>
    /// Cab324PurificationSys.xaml 的交互逻辑
    /// </summary>       
    public partial class Cab324PurificationSys : UserControl
    {
        //Dictionary<UInt32, DeviceGroup> subSystemNum_DeviceGroups;
        Dictionary<UInt32, DevicePump> subSystemNum_DevicePump;
        //防止用户连续点击，无效操作
        Boolean canClick; //能够再次点击
        DateTime lastClicktime;//上次点击时间
        Cab cabInArtwork;
        public Cab324PurificationSys( )
        {
            InitializeComponent();
        }
        public Cab324PurificationSys(Cab cab)
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
               // Console.WriteLine(" private void update(object sender, System.ComponentModel.PropertyChangedEventArgs e)");
                subSys1Qualitytb.Text = cabInArtwork.Devices[0].NowValue;
                subSys2Qualitytb.Text = cabInArtwork.Devices[2].NowValue;
                subSys3Qualitytb.Text = cabInArtwork.Devices[4].NowValue;
            }));
        }

        void initBindings()
        {
            //解体氚测量仪
            Binding nowding1 = new Binding();
            //nowding1.Source = cabInArtwork.Devices[0];
            nowding1.Source = GlobalMapForShow.globalMapForDevice[cabInArtwork.BuildingId + "_" + cabInArtwork.Devices[0].DeviceId];
            nowding1.Path = new PropertyPath("NowValue");
            subSys1Qualitytb.SetBinding(TextBlock.TextProperty, nowding1);

            //房间氚测量仪
            Binding nowding2 = new Binding();
           // nowding2.Source = cabInArtwork.Devices[2];
            nowding2.Source = GlobalMapForShow.globalMapForDevice[cabInArtwork.BuildingId + "_" + cabInArtwork.Devices[2].DeviceId];
            nowding2.Path = new PropertyPath("NowValue");
            subSys2Qualitytb.SetBinding(TextBlock.TextProperty, nowding2);
            //解吸氚测量仪
            Binding nowding3 = new Binding();
           // nowding3.Source = cabInArtwork.Devices[4];
            nowding3.Source = GlobalMapForShow.globalMapForDevice[cabInArtwork.BuildingId + "_" + cabInArtwork.Devices[4].DeviceId];
            nowding3.Path = new PropertyPath("NowValue");
            subSys3Qualitytb.SetBinding(TextBlock.TextProperty, nowding3);
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
