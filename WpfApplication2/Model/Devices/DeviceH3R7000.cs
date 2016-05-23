using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PavilionMonitor
{
    // 超大流量
    class H3R7000 : DevicePavilion, INotifyPropertyChanged
    {

        private int presure;


        public float real_traffic, sample_volume; // 实时流量 采样体积


        private string keep_time = ""; // 降雨时间


        public H3R7000(UInt32 id, String ip, String port)
            : base(id, ip, port)       
        {
            presure = 0;
            real_traffic = 0.0f;
            sample_volume = 0;
            keep_time = "00:00:00";
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
        public int Presure
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


        public override bool isDataRight(byte[] flowBytes,int len )
        {
            bool dataright = true;

            if (flowBytes.Length == 26) // 返回长度固定26 字节
                return dataright;
            else
                return false;
        }


        //2115房间经过RF1000后的数据解析。输入：16进制数组，返回：data2115Packet数据包
        public override void AnalysisPavilionData(byte[] flowBytes,int len)
        {
            byte [] buffer = new byte[flowBytes.Length-3];  // 去除首尾 无效 3字节
            Array.Copy(flowBytes,1,buffer,0,flowBytes.Length-3); // 结果为ASCII，去除头部多余1字节与尾部2字节
            String data_str = Encoding.ASCII.GetString(buffer);
            string[] str_arr = data_str.Split(' ');
            if (str_arr.Length == 4)
            {
                presure = Convert.ToInt32(str_arr[0]); // 压差
                real_traffic = (float)Convert.ToDouble(str_arr[1]); //实时流量
                sample_volume = (float)Convert.ToDouble(str_arr[2]); //采样体积
                keep_time=str_arr[3];
                //keep_time = 0;
                //string[] time_arr = str_arr[3].Split(':');
                //if (time_arr.Length == 3) {
                //    keep_time += Convert.ToInt32(time_arr[0]) * 3600; // 小时数
                //    keep_time += Convert.ToInt32(time_arr[1]) * 60;  //分钟数
                //    keep_time += Convert.ToInt32(time_arr[2]);
                //}
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String getHistoryDataSql()
        {
            //if (rtPaint){ //该设备正在绘制实时曲线
            //    float[] dosevalues = new float[4];
            //    dosevalues[0] = DoseNow;//实时值
            //    dosevalues[1] = DoseAvg;//平均值
            //    dosevalues[2] = DoseStd;//标准差值
            //    dosevalues[3] = RainValue;//雨量

            //    Rtce(dosevalues); 
            ////}
            //DateTime dt = DateTime.Now;
            //String[] colums = {"DevId","DataTime","DoseNow","DoseAvg","DoseStd","RainValue","State"};
            //Object[] values = {DevId,"'"+dt.ToString()+"'",DoseNow,DoseAvg,DoseStd,RainValue,"'"+State+"'"};
            String sql = "";// DBHelper.getInsertCommands("devicehistorydata",colums,values);
            return sql;
        }

        /// <summary>
        /// 2115房间数据或参数读取命令生成。
        /// </summary>
        /// <returns></returns>
        public override byte[] ToReadDataCommand()
        {
            byte[] command = {0x80,0x4D,0x45,0x41,0x20,0x53,0x43,0x41,0x4E,0x20,0x31,0x20,0x34,0x03,0x70}; // 超大流量15字节命令
            // 固定内容
            return command;
        }

        public override void doWork()
        {


        }


    }
}
