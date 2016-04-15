using WpfApplication2.package;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;

namespace PavilionMonitor
{
    // 干湿沉降设备类
    class DeviceDryWet : DevicePavilion, INotifyPropertyChanged
    {
        private String cab_state="开盖", rainy_state="未下雨"; // 开盖状态，是否下雨？？？
        private int rain_time = 0; // 降雨时间

        // 用于生成阿里云中转数据的对象
        public DeviceDataDryWetBox drywet_box ;

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public DeviceDryWet(DeviceDataDryWetBox b)
        {
            drywet_box = b;
            fromBoxToDevice((DeviceDataBox_Base)b);
            judgeState();
        }
        public DeviceDryWet(OracleDataReader odr)
            :base(odr)
        {

        }
        public override string GenerateAlarmMessage()
        {
            return base.GenerateAlarmMessage();
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1,VALUE_OPTION, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + Rain_time + ", '" + Cab_state+";"+Rainy_state + "' , " + "'" + DataUnit + "'" + ", " + "'" + State + "' )";
        }

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d = new DeviceData();
                d.VALUE1 = odr.GetFloat(5);
                d.Value_Option = odr.GetString(4);
                d.Time = odr.GetString(2);
                dataset.Add(d);
                d = null;
            }
            dataDictionary.Add("干湿检测仪", dataset);
            return dataDictionary;
        }

        public override void fromBoxToDevice(DeviceDataBox_Base box)
        {
            base.fromBoxToDevice(box);
            Cab_state = drywet_box.Cab_state;
            Rainy_state = drywet_box.Rainy_state;
            if (drywet_box.Rain_time!=null&&!drywet_box.Rain_time.Equals(""))
            {
                Rain_time = int.Parse(drywet_box.Rain_time);
            }
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override void setDevState(string state)
        {
            DevState = state;  // 糟糕的设计。父类引用更新子类成员，触发界面更新
        }
        public String DevState
        {
            get { return devState; }
            set
            {
                devState = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DevState"));
                }
            }
        }
        public int Rain_time
        {
            get { return rain_time; }
            set { rain_time = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rain_time"));
                }
            }
        }
        public String Rainy_state
        {
            get { return rainy_state; }
            set { rainy_state = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rainy_state"));
                }
            }
        }

        public String Cab_state
        {
            get { return cab_state; }
            set { cab_state = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Cab_state"));
                }
            }
        }

       



        public DeviceDryWet(UInt32 id, String ip, String port): base(id,ip,port)
        {
            rain_time = 0;
            cab_state = " ";
            rainy_state = " ";
        }

        public override bool isDataRight(byte[] flowBytes,int len)
        {
            bool dataright = true;

            if (len > 18&& len<25) // 返回长度18-20之间
                return dataright;
            else
                return false;
        }

        public override void AnalysisPavilionData(byte[] flowBytes,int len)
        {
            switch (flowBytes[5]) { 
                case 0x30:
                    Cab_state = "关盖";
                    break;
                case 0x31:
                    Cab_state = "开盖";
                    break;
                case 0x32:
                    Cab_state = "错误";
                    break;
                case 0x33:
                    Cab_state = "正在开盖或关盖";
                    break;
                default:
                    Cab_state = "未知";
                    break;
            }

            switch (flowBytes[9]) { 
                case 0x30:
                    Rainy_state = "无雨";
                    break;
                case 0x31:
                    Rainy_state = "降雨";
                    break;
                default:
                    Rainy_state = "未知";
                    break;
            }
            Byte [] time_bytes = new Byte[5];
             time_bytes[0]=flowBytes[11];
             time_bytes[1]=flowBytes[12];
             time_bytes[2]=flowBytes[13];
             time_bytes[3]=flowBytes[14];
             time_bytes[4]=flowBytes[15];

             try
             {
                 String time = Encoding.Default.GetString(time_bytes);
                 Rain_time = Convert.ToInt32(time);
             }
             catch (FormatException e)
             {
                 Rain_time = -1;
             }
             catch (OverflowException e) {
                 Rain_time = -1;
             }


        }
        /// <summary>
        /// 生成该设备的sql 插入语句代码 
        /// </summary>
        /// <returns></returns>
        /// 
        //public override String getHistoryDataSql()
        //{
        //    if (rtPaint){ //该设备正在绘制实时曲线
        //        float[] dosevalues = new float[4];
        //        // 如何绑定显示 ？？？？ 显示变量统一用字符串？？？？？
                
        //        //dosevalues[0] = DoseNow;//实时值
        //        //dosevalues[1] = DoseAvg;//平均值
        //        //dosevalues[2] = DoseStd;//标准差值
        //        //dosevalues[3] = RainValue;//雨量

        //        Rtce(dosevalues); 
        //    }
        //    DateTime dt = DateTime.Now;
        //    String[] colums = {"DevId","val1","val2","val3","val4","str_val5","DataTime","State"}; // 4个float+ 1个字符串 +单位 状态 
        //    // 字符串值的拼接
        //    string str_value=cab_state+"_"+rainy_state;
        //    Object[] values = { devId, rain_time, 0, 0, 0, "'" + str_value + "'", "'" + dt.ToString() + "'",  "'" + DevState + "'" };
        //    String sql = DBHelper.getInsertCommands("monthhistorydata", colums, values);
        //    sql += ";";
        //    sql += DBHelper.getInsertCommands("historydata", colums, values); // 复制两条sql ？？？  历史数据库

        //    return sql;
        //}

        //// 更新最新的数据
        //public override void getAliyunUpdateStr()
        //{
        //    State current_dev_state = State.Normal;
        //    if (DevState.Equals("掉线"))
        //        current_dev_state = State.DevError;
        //    //只更新数据
        //    drywet_box.load("运输部亭子", "1", "" + devId, current_dev_state, Cab_state, Rainy_state, "" + Rain_time, "", "", "", "");

        //}
        //public override String getExcepDataSql()
        //{
        //    DateTime dt = DateTime.Now;
        //    String[] colums = { "DevId", "val1", "val2", "val3", "val4", "str_val5", "DataTime", "State" }; // 4个float+ 1个字符串 +单位 状态 
        //    // 字符串值的拼接
        //    string str_value = cab_state + "_" + rainy_state;
        //    Object[] values = { devId, rain_time, 0, 0, 0, "'" + str_value + "'", "'" + dt.ToString() + "'", "'" + DevState + "'" };
        //    String sql = DBHelper.getInsertCommands("exceptionhistorydata", colums, values);  //异常数据库 
        //    return sql;
        //}


        /// <summary>
        /// 2115房间数据或参数读取命令生成。
        /// </summary>
        /// <returns></returns>
        public override byte[] ToReadDataCommand()
        {
            byte[] command = {0x00,0x32,0xcd,0xa0,0x3b,0x30,0x30,0x30,0x38,0x30,0x31,0x30,0x30,0x30,0x31,0x32,0x02,0x57,0x01}; // 干湿沉降读取数据命令16字节
            // 固定内容
            return command;
        }

        public override void doWork()
        {


        }

    }
}
