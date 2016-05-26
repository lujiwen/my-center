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
    // 超大流量
    public class DeviceJL900 : Device, INotifyPropertyChanged
    {

        private float presure;

        private float real_traffic, sample_volume; // 实时流量 采样体积


        private string keep_time = ""; // 降雨时间

        public DeviceDataJL900Box jl900_box = new DeviceDataJL900Box();

        public DeviceJL900(OracleDataReader odr)
            :base(odr)
         {

         }

        public DeviceJL900(UInt32 id, String ip, String port)
        {
            presure = 0;
            real_traffic = 0.0f;
            sample_volume = 0;
            keep_time = "00:00:00";

        }

        public DeviceJL900(DeviceDataJL900Box box)
            : base(box )
        {
            jl900_box = box;
            fromBoxToDevice((DeviceDataBox_Base)jl900_box );
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1,VALUE2,VALUE3,VALUE_OPTION, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + Presure + ", " + Real_traffic + ", " + Sample_volume + ", " + Keep_time + ", " + "'" + DataUnit + "'" + ", " + "'" + State + "' )";
        }

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> pressureDataset = new List<DeviceData>();
            List<DeviceData> realTrafficDataset = new List<DeviceData>();
            List<DeviceData> sampleDataset = new List<DeviceData>();
            List<DeviceData> keepTimeDataset = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d1 = new DeviceData();
                DeviceData d2 = new DeviceData();
                DeviceData d3 = new DeviceData();
                DeviceData d4 = new DeviceData();
                d1.VALUE1 = odr.GetFloat(5).ToString();
                d2.VALUE1 = odr.GetFloat(6).ToString();
                d3.VALUE1 = odr.GetFloat(7).ToString();
                d4.Value_Option = odr.GetString(4) ;
                d1.Time = odr.GetString(2);
                d2.Time = odr.GetString(2);
                d3.Time = odr.GetString(2);
                d4.Time = odr.GetString(2);
                pressureDataset.Add(d1);
                realTrafficDataset.Add(d2);
                sampleDataset.Add(d3);
                keepTimeDataset.Add(d4);
                d1 = null;
                d2 = null;
                d3 = null;
                d4 = null;
            }
            dataDictionary.Add("压力", pressureDataset);
            dataDictionary.Add("实时流量", realTrafficDataset);
            dataDictionary.Add("抽样体积", sampleDataset);
            dataDictionary.Add("持续时间", keepTimeDataset);
            return dataDictionary;
        }

        public override string GenerateAlarmMessage()
        {
            return base.GenerateAlarmMessage();
        }

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override void fromBoxToDevice(DeviceDataBox_Base box)
        {
            base.fromBoxToDevice(box);
            if(jl900_box.presure!=null&&!jl900_box.presure.Equals(""))
            {
                Presure = float.Parse(jl900_box.presure);
            }
            if (jl900_box.real_traffic != null && !jl900_box.real_traffic.Equals(""))
            {
                Real_traffic = float.Parse(jl900_box.real_traffic);
            }
            if (jl900_box.sample_volume != null && !jl900_box.sample_volume.Equals(""))
            {
                Sample_volume = float.Parse(jl900_box.sample_volume);
            }
            if (jl900_box.keep_time != null && !jl900_box.keep_time.Equals(""))
            {
                Keep_time = jl900_box.keep_time;
            }
        }
        public override void judgeState()
        {
            base.judgeState();
        }

 
        public float Real_traffic
        {
            get { return real_traffic; }
            set { real_traffic = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Real_traffic"));
                }
            }
        }
        public float Presure
        {
            get { return presure; }
            set { presure = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Presure"));
                }
            }
        }

        public float Sample_volume
        {
            get { return sample_volume; }
            set
            {
                sample_volume = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Sample_volume"));
                }
            }
        }
        public string Keep_time
        {
            get { return keep_time; }
            set { keep_time = value;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Keep_time"));
            }
            }
        }


        public override bool isDataRight(byte[] flowBytes,int len)
        {
            bool dataright = true;

            if ( len == 26) // 不能仅仅用长度判断
                return dataright;
            else
                return false;
        }

        public override void AnalysisData(byte[] flowBytes,int len)
        {
            byte [] buffer = new byte[24];  // 去除首尾 无效 1字节
            Array.Copy(flowBytes,1,buffer,0,23); // 结果为ASCII，去除头部多余1字节与尾部2字节
            buffer[23] = 0;

            String data_str = Encoding.ASCII.GetString(buffer);
            string[] str_arr = data_str.Split(' ');
            if (str_arr.Length == 4)
            {
                Presure = Convert.ToInt32(str_arr[0]); // 压差
                Real_traffic = (float)Convert.ToDouble(str_arr[1]); //实时流量
                Sample_volume = (float)Convert.ToDouble(str_arr[2]); //采样体积
                if(str_arr[3].Length>=7)
                    Keep_time = str_arr[3].Substring(0,7) ; // 只有7个长度
                //keep_time = 0;
                //string[] time_arr = str_arr[3].Split(':');
                //if (time_arr.Length == 3) {
                //    keep_time += Convert.ToInt32(time_arr[0]) * 3600; // 小时数
                //    keep_time += Convert.ToInt32(time_arr[1]) * 60;  //分钟数
                //    keep_time += Convert.ToInt32(time_arr[2]);
                //}
            }


        }

        // 更新最新的数据
        //public override void getAliyunUpdateStr()
        //{
        //    State current_dev_state = State.Normal;
        //    if (DevState.Equals("掉线"))
        //        current_dev_state = State.DevError;
        //    //只更新数据
        //    jl900_box.load("运输部亭子", "1", "" + devId,current_dev_state,""+Presure,""+Real_traffic,""+Sample_volume,Keep_time,"","","","");

        //}
        //public override string gethistorydatasql()
        //{
        //    if (rtpaint)
        //    { //该设备正在绘制实时曲线
        //        float[] dosevalues = new float[3];
        //         如何绑定显示 ？？？？ 显示变量统一用字符串？？？？？

        //        dosevalues[0] = presure;//压力值 
        //        dosevalues[1] = real_traffic;//实时流量
        //        dosevalues[2] = sample_volume; //采样体积

        //        rtce(dosevalues);
        //    }
        //    datetime dt = datetime.now;
        //    string[] colums = { "devid", "val1", "val2", "val3", "val4", "str_val5", "datatime", "state" }; // 4个float+ 1个字符串 +单位 状态 
        //    object[] values = { devid, presure,real_traffic,sample_volume, 0, "'" + keep_time + "'", "'" + dt.tostring() + "'", "'" + devstate + "'" };
        //    string sql = dbhelper.getinsertcommands("monthhistorydata", colums, values);
        //    sql += ";";
        //    sql += dbhelper.getinsertcommands("historydata", colums, values); // 复制两条sql ？？？  历史数据库

        //    return sql;
        //}

        
    }
}
