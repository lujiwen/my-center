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
using System.Windows.Shapes;
using WpfApplication2.Model.Vo;
using WpfApplication2.package;

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// Asm02HistoryTable.xaml 的交互逻辑
    /// </summary>
    public partial class Asm02HistoryTable : Window
    {
        public delegate void BackDataDelegate();
        public delegate void ForwardDataDelegate();

        private DeviceData data;
        public DeviceData DeviceHistoryData { get { return data; } set { data = value; updateChart(data); } }

        private List<DeviceData> deviceData;
        public List<DeviceData> DeviceHistoryDataList { get { return deviceData; } set { deviceData = value; } }
        
        public BackDataDelegate getBackData;
        public ForwardDataDelegate getForwardData;
        private int current;
        public Asm02HistoryTable(List<DeviceData> data)
        {
            InitializeComponent();
            DeviceHistoryDataList = data;
            current = 0;
            init();
            
        }

        private void init()
        {
            DeviceHistoryData = DeviceHistoryDataList[current];
        }

        private void updateChart(DeviceData data)
        {
            time.Text = data.Time;
            DeviceDataASM02Box box = new DeviceDataASM02Box();
            box.AnalysisPavilionData(data.Value_Option);
            
            ab_1.Text = box.Ab_1;
            ab_2.Text = box.Ab_2;
            ab_3.Text = box.Ab_3;
            ab_4.Text = box.Ab_4;
            ab_5.Text = box.Ab_5;
            ab_6.Text = box.Ab_6;
            ab_7.Text = box.Ab_7;
            ab_8.Text = box.Ab_8;

            ec_1.Text = box.Ec_1;
            ec_2.Text = box.Ec_2;
            ec_3.Text = box.Ec_3;
            ec_4.Text = box.Ec_4;
            ec_5.Text = box.Ec_5;
            ec_6.Text = box.Ec_6;
            ec_7.Text = box.Ec_7;
            ec_8.Text = box.Ec_8;

            fl_1.Text = box.Fl_1;
            fl_2.Text = box.Fl_2;
            fl_3.Text = box.Fl_3;
            fl_4.Text = box.Fl_4;
            fl_5.Text = box.Fl_5;
            fl_6.Text = box.Fl_6;
            fl_7.Text = box.Fl_7;
            fl_8.Text = box.Fl_8;
            fl_9.Text = box.Fl_9;
            fl_10.Text = box.Fl_10;

            ga_1.Text = box.Ga_1;
            ga_2.Text = box.Ga_2;
            ga_3.Text = box.Ga_3;
            ga_4.Text = box.Ga_4;
            ga_5.Text = box.Ga_5;
            ga_6.Text = box.Ga_6;
            ga_7.Text = box.Ga_7;

            gi_1.Text = box.Gi_1;
            gi_2.Text = box.Gi_2;
            gi_3.Text = box.Gi_3;
            gi_4.Text = box.Gi_4;
            gi_5.Text = box.Gi_5;
            gi_6.Text = box.Gi_6;
            gi_7.Text = box.Gi_7;

            me_1.Text = box.Me_1;
            me_2.Text = box.Me_2;
            me_3.Text = box.Me_3;
            me_4.Text = box.Me_4;
            me_5.Text = box.Me_5;
            me_6.Text = box.Me_6;
            me_7.Text = box.Me_7;
            me_8.Text = box.Me_8;
            me_9.Text = box.Me_9;
            me_10.Text = box.Me_10;

            rn_1.Text = box.Rn_1;
            rn_2.Text = box.Rn_2;
            rn_3.Text = box.Rn_3;
            rn_4.Text = box.Rn_4;
            rn_5.Text = box.Rn_5;
            rn_6.Text = box.Rn_6;
        }
        private void move_forward_btn_click(object sender, RoutedEventArgs e)
        {
            if (current < DeviceHistoryDataList.Count)
            {
                DeviceHistoryData = DeviceHistoryDataList[current++];
            }
            else
            {
                MessageBox.Show("已经是最后一条记录！");
            }
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            if (current > 0)
            {
                DeviceHistoryData = DeviceHistoryDataList[current--];
            }
            else
            {
                MessageBox.Show("已经是第一条记录！");
            }
        }


    }
}
