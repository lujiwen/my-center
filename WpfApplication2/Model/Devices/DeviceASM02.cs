using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using System.Threading;
using WpfApplication2.Model.Vo;
using WpfApplication2.package;
using System.Data.OracleClient;
using WpfApplication2.View.Windows;
using System.Text;

namespace PavilionMonitor
{
    /**
     * 单独的网络通信，获取数据
     * 气溶胶数据解析，单独处理，8组数据
     * 界面也得单独设计
     * 
     * ***/
    public class DeviceASM02 : Device, INotifyPropertyChanged
    {
        private string val_str_set=""; // 存放asm数据字符串
        public DeviceDataASM02Box asm02_box=new DeviceDataASM02Box();

        public DeviceASM02(DeviceDataASM02Box box)
            :base(box)
        {
            asm02_box = box;
            fromBoxToDevice((DeviceDataBox_Base)asm02_box);
            judgeState();
        }
        public DeviceASM02(OracleDataReader odr)
            :base(odr)
        {

        }

     
 
        public override void fromBoxToDevice(DeviceDataBox_Base box)
        {
            base.fromBoxToDevice(box);
            ab_1 = asm02_box.Ab_1;
            ab_2 = asm02_box.Ab_2;
            ab_3 = asm02_box.Ab_3;
            ab_4 = asm02_box.Ab_4;
            ab_5 = asm02_box.Ab_5;
            ab_6 = asm02_box.Ab_6;
            ab_7 = asm02_box.Ab_7;
            ab_8 = asm02_box.Ab_8;

            ec_1 = asm02_box.Ec_1;
            ec_2 = asm02_box.Ec_2;
            ec_3 = asm02_box.Ec_3;
            ec_4 = asm02_box.Ec_4;
            ec_5 = asm02_box.Ec_5;
            ec_6 = asm02_box.Ec_6;
            ec_7 = asm02_box.Ec_7;
            ec_8 = asm02_box.Ec_8;

            fl_1 = asm02_box.Fl_1;
            fl_2 = asm02_box.Fl_2;
            fl_3 = asm02_box.Fl_3;
            fl_4 = asm02_box.Fl_4;
            fl_5 = asm02_box.Fl_5;
            fl_6 = asm02_box.Fl_6;
            fl_7 = asm02_box.Fl_7;
            fl_8 = asm02_box.Fl_8;
            fl_9 = asm02_box.Fl_9;
            fl_10 = asm02_box.Fl_10;

            ga_1 = asm02_box.Ga_1;
            ga_2 = asm02_box.Ga_2;
            ga_3 = asm02_box.Ga_3;
            ga_4 = asm02_box.Ga_4;
            ga_5 = asm02_box.Ga_5;
            ga_6 = asm02_box.Ga_6;
            ga_7 = asm02_box.Ga_7;

            gi_1 = asm02_box.Gi_1;
            gi_2 = asm02_box.Gi_2;
            gi_3 = asm02_box.Gi_3;
            gi_4 = asm02_box.Gi_4;
            gi_5 = asm02_box.Gi_5;
            gi_6 = asm02_box.Gi_6;
            gi_7 = asm02_box.Gi_7;

            me_1 = asm02_box.Me_1;
            me_2 = asm02_box.Me_2;
            me_3 = asm02_box.Me_3;
            me_4 = asm02_box.Me_4;
            me_5 = asm02_box.Me_5;
            me_6 = asm02_box.Me_6;
            me_7 = asm02_box.Me_7;
            me_8 = asm02_box.Me_8;
            me_9 = asm02_box.Me_9;
            me_10 = asm02_box.Me_10;

            rn_1 = asm02_box.Rn_1;
            rn_2 = asm02_box.Rn_2;
            rn_3 = asm02_box.Rn_3;
            rn_4 = asm02_box.Rn_4;
            rn_5 = asm02_box.Rn_5;
            rn_6 = asm02_box.Rn_6;


            oi_1 = asm02_box.oi_1;
            oi_2 = asm02_box.oi_2;
            oi_3 = asm02_box.oi_3;
            oi_4 = asm02_box.oi_4;
            oi_5 = asm02_box.oi_5;
            oi_6 = asm02_box.oi_6;
            oi_7 = asm02_box.oi_7;

        }

        public override void judgeState()
        {
             base.judgeState();
        }

        public override string GenerateAlarmMessage()
        {
            return "Asm02的报警信息！" + DateTime.Now.ToString();
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE_OPTION, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", '" + asm02_box.val_str_set + "', " + "'" + DataUnit + "'" + ", " + "'" + State + "' )";
        }

        public override void startToShowHistoryTable(Dictionary<string, List<DeviceData>> data)
        {
            //base.startToShowHistoryTable(data);
            Asm02HistoryTable w = new Asm02HistoryTable(data["Asm"]);
            w.Show();
        }

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d = new DeviceData();
                d.Value_Option = odr.GetString(4);
                d.Time = odr.GetString(2);
                dataset.Add(d);
                d = null;
            }
            dataDictionary.Add("Asm", dataset);
            return dataDictionary;
        }

        public override List<DeviceData> getHistoryDataList( Dictionary<string, List<DeviceData>> dataDictionary)
        {
            return dataDictionary["Asm"];
        }
 
        public String DevState
        {
            get { return State; }
            set
            {
                State = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("State"));
                }
            }
        }

        public string ab_1, ab_2, ab_3, ab_4, ab_5, ab_6, ab_7, ab_8;

        public string Ab_1
        {
            get { return ab_1; }
            set { ab_1 = value;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_1"));
            }
            }
        }
        public string Ab_2
        {
            get { return ab_2; }
            set
            {
                ab_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_2"));
                }
            }
        }
        public string Ab_3
        {
            get { return ab_3; }
            set
            {
                ab_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_3"));
                }
            }
        }
        public string Ab_4
        {
            get { return ab_4; }
            set
            {
                ab_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_4"));
                }
            }
        }
        public string Ab_5
        {
            get { return ab_5; }
            set
            {
                ab_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_5"));
                }
            }
        }
        public string Ab_6
        {
            get { return ab_6; }
            set
            {
                ab_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_6"));
                }
            }
        }
        public string Ab_7
        {
            get { return ab_7; }
            set
            {
                ab_7 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_7"));
                }
            }
        }
        public string Ab_8
        {
            get { return ab_8; }
            set
            {
                ab_8 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ab_8"));
                }
            }
        }






        public string ec_1, ec_2, ec_3, ec_4, ec_5, ec_6, ec_7, ec_8;

        public string Ec_1
        {
            get { return ec_1; }
            set { ec_1 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_1"));
                }
            }
        }
        public string Ec_2
        {
            get { return ec_2; }
            set
            {
                ec_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_2"));
                }
            }
        }
        public string Ec_3
        {
            get { return ec_3; }
            set
            {
                ec_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_3"));
                }
            }
        }
        public string Ec_4
        {
            get { return ec_4; }
            set
            {
                ec_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_4"));
                }
            }
        }
        public string Ec_5
        {
            get { return ec_5; }
            set
            {
                ec_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_5"));
                }
            }
        }
        public string Ec_6
        {
            get { return ec_6; }
            set
            {
                ec_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_6"));
                }
            }
        }
        public string Ec_7
        {
            get { return ec_7; }
            set
            {
                ec_7 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_7"));
                }
            }
        }
        public string Ec_8
        {
            get { return ec_8; }
            set
            {
                ec_8 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ec_8"));
                }
            }
        }


        public string fl_1, fl_2, fl_3, fl_4, fl_5, fl_6, fl_7, fl_8, fl_9, fl_10;

        public string Fl_1
        {
            get { return fl_1; }
            set { fl_1 = value;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_1"));
            }
            }
        }
        public string Fl_2
        {
            get { return fl_2; }
            set
            {
                fl_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_2"));
                }
            }
        }
        public string Fl_3
        {
            get { return fl_3; }
            set
            {
                fl_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_3"));
                }
            }
        }
        public string Fl_4
        {
            get { return fl_4; }
            set
            {
                fl_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_4"));
                }
            }
        }
        public string Fl_5
        {
            get { return fl_5; }
            set
            {
                fl_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_5"));
                }
            }
        }
        public string Fl_6
        {
            get { return fl_6; }
            set
            {
                fl_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_6"));
                }
            }
        }
        public string Fl_7
        {
            get { return fl_7; }
            set
            {
                fl_7 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_7"));
                }
            }
        }
        public string Fl_8
        {
            get { return fl_8; }
            set
            {
                fl_8 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_8"));
                }
            }
        }
        public string Fl_9
        {
            get { return fl_9; }
            set
            {
                fl_9 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_9"));
                }
            }
        }
        public string Fl_10
        {
            get { return fl_10; }
            set
            {
                fl_10 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fl_10"));
                }
            }
        }



        public string ga_1, ga_2, ga_3, ga_4, ga_5, ga_6, ga_7;

        public string Ga_1
        {
            get { return ga_1; }
            set { ga_1 = value;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ga_1"));
            }
            }
        }
        public string Ga_2
        {
            get { return ga_2; }
            set
            {
                ga_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ga_2"));
                }
            }
        }
        public string Ga_3
        {
            get { return ga_3; }
            set
            {
                ga_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ga_3"));
                }
            }
        }
        public string Ga_4
        {
            get { return ga_4; }
            set
            {
                ga_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ga_4"));
                }
            }
        }
        public string Ga_5
        {
            get { return ga_5; }
            set
            {
                ga_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ga_5"));
                }
            }
        }
        public string Ga_6
        {
            get { return ga_6; }
            set
            {
                ga_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ga_6"));
                }
            }
        }
        public string Ga_7
        {
            get { return ga_7; }
            set
            {
                ga_7 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Ga_7"));
                }
            }
        }

        public string gi_1, gi_2, gi_3, gi_4, gi_5, gi_6, gi_7;

        public string Gi_1
        {
            get { return gi_1; }
            set { gi_1 = value;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gi_1"));
            }
            }
        }
        public string Gi_2
        {
            get { return gi_2; }
            set
            {
                gi_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gi_2"));
                }
            }
        }
        public string Gi_3
        {
            get { return gi_3; }
            set
            {
                gi_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gi_3"));
                }
            }
        }
        public string Gi_4
        {
            get { return gi_4; }
            set
            {
                gi_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gi_4"));
                }
            }
        }
        public string Gi_5
        {
            get { return gi_5; }
            set
            {
                gi_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gi_5"));
                }
            }
        }
        public string Gi_6
        {
            get { return gi_6; }
            set
            {
                gi_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gi_6"));
                }
            }
        }
        public string Gi_7
        {
            get { return gi_7; }
            set
            {
                gi_7 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Gi_7"));
                }
            }
        }

        public string me_1, me_2, me_3, me_4, me_5, me_6, me_7, me_8, me_9,me_10;

        public string Me_1
        {
            get { return me_1; }
            set { me_1 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_1"));
                }
            }
        }
        public string Me_2
        {
            get { return me_2; }
            set
            {
                me_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_2"));
                }
            }
        }
        public string Me_3
        {
            get { return me_3; }
            set
            {
                me_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_3"));
                }
            }
        }
        public string Me_4
        {
            get { return me_4; }
            set
            {
                me_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_4"));
                }
            }
        }
        public string Me_5
        {
            get { return me_5; }
            set
            {
                me_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_5"));
                }
            }
        }
        public string Me_6
        {
            get { return me_6; }
            set
            {
                me_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_6"));
                }
            }
        }
        public string Me_7
        {
            get { return me_7; }
            set
            {
                me_7 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_7"));
                }
            }
        }
        public string Me_8
        {
            get { return me_8; }
            set
            {
                me_8 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_8"));
                }
            }
        }
        public string Me_9
        {
            get { return me_9; }
            set
            {
                me_9 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_9"));
                }
            }
        }
        public string Me_10
        {
            get { return me_10; }
            set
            {
                me_10 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Me_10"));
                }
            }
        }

        public string oi_1, oi_2, oi_3, oi_4, oi_5, oi_6, oi_7;

        public string Oi_1
        {
            get { return oi_1; }
            set { oi_1 = value;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Oi_1"));
            }
            }
        }
        public string Oi_2
        {
            get { return oi_2; }
            set
            {
                oi_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Oi_2"));
                }
            }
        }
        public string Oi_3
        {
            get { return oi_3; }
            set
            {
                oi_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Oi_3"));
                }
            }
        }
        public string Oi_4
        {
            get { return oi_4; }
            set
            {
                oi_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Oi_4"));
                }
            }
        }
        public string Oi_5
        {
            get { return oi_5; }
            set
            {
                oi_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Oi_5"));
                }
            }
        }
        public string Oi_6
        {
            get { return oi_6; }
            set
            {
                oi_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Oi_6"));
                }
            }
        }
        public string Oi_7
        {
            get { return oi_7; }
            set
            {
                oi_7 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Oi_7"));
                }
            }
        }


        public string rn_1, rn_2, rn_3, rn_4, rn_5, rn_6;

        public string Rn_1
        {
            get { return rn_1; }
            set { rn_1 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rn_1"));
                }
            }
        }
        public string Rn_2
        {
            get { return rn_2; }
            set
            {
                rn_2 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rn_2"));
                }
            }
        }
        public string Rn_3
        {
            get { return rn_3; }
            set
            {
                rn_3 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rn_3"));
                }
            }
        }
        public string Rn_4
        {
            get { return rn_4; }
            set
            {
                rn_4 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rn_4"));
                }
            }
        }
        public string Rn_5
        {
            get { return rn_5; }
            set
            {
                rn_5 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rn_5"));
                }
            }
        }
        public string Rn_6
        {
            get { return rn_6; }
            set
            {
                rn_6 = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rn_6"));
                }
            }
        }


        //public ASM02(UInt32 id, String ip, String port)
        //    : base(id, ip, port)
        //{
           
        //}

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override bool isDataRight(byte[] flowBytes, int len)
        {
            String asm_str = Encoding.ASCII.GetString(flowBytes, 0, len);
            string[] items_arr = asm_str.Split(';');
            if (items_arr.Length == 8)
                return true;
            else
                return false;
        }

        //2115房间经过RF1000后的数据解析。输入：16进制数组，返回：data2115Packet数据包
        //Ab:,,,3.13E-003,,,,1.85E-002;Ec:;Fl:01200,002.8,27.0,35.0,000.0,31.7,16.1,01.2,3,401;Ga:,,,,3.60E-001,,;Gi:,,,,1.98E-002,,;Me:0.00e+000,0.0,0,,+0.0,0.0,000,,,;Oi:;Rn:4.82E+000,2.12E-001,,1.31E-001,2.15E-001,
        public override void AnalysisData(byte[] flowBytes,int len)
        {
            devState = "Normal";

            String asm_str = Encoding.ASCII.GetString(flowBytes,0,len);
            val_str_set = asm_str; // 保存最新的数据字符串，数据库存取使用
            string[] items_arr = asm_str.Split(';');
            if (items_arr.Length == 8) {  // 总共8个数据项
                for (int i = 0; i < 8;i++ )
                {
                    int start_index = items_arr[i].IndexOf(':')+1;
                    if(start_index>0){
                        string target_str = items_arr[i].Substring(start_index, items_arr[i].Length - start_index);
                        string[] datas_detail = target_str.Split(',');
                        switch(i){
                            case 0:
                                if (datas_detail.Length == 8) { 
                                    // ab 8
                                    Ab_1 = datas_detail[0];
                                    Ab_2 = datas_detail[1];
                                    Ab_3 = datas_detail[2];
                                    Ab_4 = datas_detail[3];
                                    Ab_5 = datas_detail[4];
                                    Ab_6 = datas_detail[5];
                                    Ab_7 = datas_detail[6];
                                    Ab_8 = datas_detail[7];
                                }
                                break;
                            case 1:
                                if (datas_detail.Length == 8)
                                {
                                    // ec 8
                                    Ec_1 = datas_detail[0];
                                    Ec_2 = datas_detail[1];
                                    Ec_3 = datas_detail[2];
                                    Ec_4 = datas_detail[3];
                                    Ec_5 = datas_detail[4];
                                    Ec_6 = datas_detail[5];
                                    Ec_7 = datas_detail[6];
                                    Ec_8 = datas_detail[7];
                                }
                                break;
                            case 2:
                                if (datas_detail.Length == 10)
                                {
                                    // fl 10
                                    Fl_1 = datas_detail[0];
                                    Fl_2 = datas_detail[1];
                                    Fl_3 = datas_detail[2];
                                    Fl_4 = datas_detail[3];
                                    Fl_5 = datas_detail[4];
                                    Fl_6 = datas_detail[5];
                                    Fl_7 = datas_detail[6];
                                    Fl_8 = datas_detail[7];
                                    Fl_9 = datas_detail[8];
                                    Fl_10 = datas_detail[9];
                                }
                                break;
                            case 3:
                                if (datas_detail.Length == 7)
                                {
                                    // ga 7
                                    Ga_1 = datas_detail[0];
                                    Ga_2 = datas_detail[1];
                                    Ga_3 = datas_detail[2];
                                    Ga_4 = datas_detail[3];
                                    Ga_5 = datas_detail[4];
                                    Ga_6 = datas_detail[5];
                                    Ga_7 = datas_detail[6];
                                }
                                break;
                            case 4:
                                if (datas_detail.Length == 7)
                                {
                                    // gi 7
                                    Gi_1 = datas_detail[0];
                                    Gi_2 = datas_detail[1];
                                    Gi_3 = datas_detail[2];
                                    Gi_4 = datas_detail[3];
                                    Gi_5 = datas_detail[4];
                                    Gi_6 = datas_detail[5];
                                    Gi_7 = datas_detail[6];
                                }
                                break;
                            case 5:
                                if (datas_detail.Length == 10)
                                {
                                    // me 10
                                    Me_1 = datas_detail[0];
                                    Me_2 = datas_detail[1];
                                    Me_3 = datas_detail[2];
                                    Me_4 = datas_detail[3];
                                    Me_5 = datas_detail[4];
                                    Me_6 = datas_detail[5];
                                    Me_7 = datas_detail[6];
                                    Me_8 = datas_detail[7];
                                    Me_9 = datas_detail[8];
                                    Me_10 = datas_detail[9];
                                }
                                break;
                            case 6:
                                if (datas_detail.Length == 7)
                                {
                                    // oi 7
                                    Oi_1 = datas_detail[0];
                                    Oi_2 = datas_detail[1];
                                    Oi_3 = datas_detail[2];
                                    Oi_4 = datas_detail[3];
                                    Oi_5 = datas_detail[4];
                                    Oi_6 = datas_detail[5];
                                    Oi_7 = datas_detail[6];
                                }
                                break;
                            case 7:
                                if (datas_detail.Length == 6)
                                {
                                    // rn 6
                                    Rn_1 = datas_detail[0];
                                    Rn_2 = datas_detail[1];
                                    Rn_3 = datas_detail[2];
                                    Rn_4 = datas_detail[3];
                                    Rn_5 = datas_detail[4];
                                    Rn_6 = datas_detail[5];
                                }
                                break;
                        }
                    }
                }
            }           
        }
        /// <summary>
        /// 2115房间数据或参数读取命令生成。
        /// </summary>
        /// <returns></returns>
        public override byte[] ToReadDataCommand()
        {
            string req="req";
            byte[] command = Encoding.ASCII.GetBytes(req);

            // 固定内容
            return command;
        }

        public override WpfApplication2.package.Box getCommonDataPack()
        {
            DeviceDataASM02Box box = new DeviceDataASM02Box();
            box.load(this.BuildingId, this.CabId, DeviceId,   this.devState ,val_str_set, this.devUnit, this.Lowthreshold.ToString(), this.Highthreshold.ToString(), this.CorrectFactor.ToString());
            return box;
        }

    }
}

