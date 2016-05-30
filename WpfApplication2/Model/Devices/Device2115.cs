using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;
using WpfApplication2.package;

namespace Project2115Home.Model
{
 
    public class Device2115 : Device, INotifyPropertyChanged
    {
       private Int32 detetorType;//探头类型
       private Int32 monitorNumber;//检测仪编号
       private Int32 flag;//标志位
       private float doseNow;//实时值
       private float doseAvg;//平均值
       private float doseStd;//标准差
       private String dataUnit;//单位

       private float rainValue;//雨量值
       private String rainUnit;//雨量单位

        float errorThreshold;//失效阈值

        String state = " "; //状态

        Boolean paraChanged;//参数是否被修改
        Boolean paraChangedSuccess;//参数修改成功
        UpdateSuccessEvent use;

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public Device2115(UInt32 id, String ip, String port)
            : base(id, ip, port)
        {
        }

        public Device2115(OracleDataReader odr)
            :base(odr)
        {

        }

        DeviceDataBox_2115  box2115;
        public Device2115(DeviceDataBox_2115 b)
        {
            box2115 = b;
            fromBoxToDevice((DeviceDataBox_2115)b);
            judgeState();
        }

        //2115房间经过RF1000后的数据格式是否正确
        public override bool isDataRight(byte[] flowBytes, int len)
        {
            bool dataright = true;
            //判断数据包长度是否为0x14，否则设置为错误
            if (0x14 != flowBytes[1])
                dataright = false;
            //判断数据应当命令是否为0x71，否则设置为错误
            if (0x71 != flowBytes[4])
                dataright = false;
            return dataright;
        }

        
        //2115房间经过RF1000后的修改参数的确认信息是否是否正确
        public override bool isParaSetRight(byte[] paraBytes)
        {
            bool parasetright = true;
            //判断数据包长度是否为0x05，否则设置为错误
            if (0x05 != paraBytes[1])
                parasetright = false;
            //判断数据应当命令是否为0x60，否则设置为错误
            if (0x60 != paraBytes[4])
                parasetright = false;
            return parasetright;
        }

        public override void fromBoxToDevice(DeviceDataBox_Base box)
        {
            base.fromBoxToDevice(box);
            if (box2115.DoseNow != null && !box2115.DoseNow.Equals(""))
            {
                DoseNow =  box2115.DoseNow ;
            }
            if (box2115.DoseAvg != null && !box2115.DoseAvg.Equals(""))
            {
                DoseAvg = box2115.DoseAvg;
            }
            if (box2115.DoseStd != null && !box2115.DoseStd.Equals(""))
            {
                DoseStd =  box2115.DoseStd;
            }
            if (box2115.RainValue != null && !box2115.RainValue.Equals(""))
            {
                RainValue = box2115.RainValue;
            }
        }
 
        //2115房间经过RF1000后的数据解析。输入：16进制数组，返回：data2115Packet数据包
        public override void AnalysisData(byte[] flowBytes,int len)
        {
            //获取包长度
            int size = (int)flowBytes[1];
            //获取探头类型
            int detetorType = (int)flowBytes[2];
            //获取检测仪编号
            int monitorNumber = (int)flowBytes[3];
            //获取标志位
            int flag = (int)flowBytes[5];

            byte[] tempbytes = new byte[4];
            //获取剂量率瞬时值
            int i = 6;
            tempbytes[0] = flowBytes[i + 3];
            tempbytes[1] = flowBytes[i + 2];
            tempbytes[2] = flowBytes[i + 1];
            tempbytes[3] = flowBytes[i];
            float tempInstantValue = BitConverter.ToSingle(tempbytes, 0);
            int instantValue = (int)(tempInstantValue * 1000);
            //获取5分钟平均
            i += 4;
            tempbytes[0] = flowBytes[i + 3];
            tempbytes[1] = flowBytes[i + 2];
            tempbytes[2] = flowBytes[i + 1];
            tempbytes[3] = flowBytes[i];
            float tempAverageValue = BitConverter.ToSingle(tempbytes, 0);
            int averageValue = (int)(tempAverageValue * 1000);
            //获取标准差值
            i += 4;
            tempbytes[0] = flowBytes[i + 3];
            tempbytes[1] = flowBytes[i + 2];
            tempbytes[2] = flowBytes[i + 1];
            tempbytes[3] = flowBytes[i];
            float tempSdValue = BitConverter.ToSingle(tempbytes, 0);
            Console.WriteLine("sd:" + tempSdValue);
            int sdValue = (int)(tempSdValue * 1000);
            //获取雨量
            UInt16 temprainValue = 0;
            if (detetorType == 1)
            {
                temprainValue = BitConverter.ToUInt16(flowBytes, i);
            }
           
            if (flag == Convert.ToInt32(0x08))
            {
                state = "正常";
            }
            else if (flag == Convert.ToInt32(0x80))
            {
                state = "故障";
            }
            else if (flag == Convert.ToInt32(0x40))
            {
                state = "报警";
            }
            else if (flag == Convert.ToInt32(0x20))
            {
                state = "失效";
            }
            devState = "Normal";
            doseNow = instantValue;//剂量率瞬时值
            doseAvg = averageValue;//5分钟平均值
            doseStd = sdValue;//标准差值
            rainValue = temprainValue;//雨量
        }

        public override WpfApplication2.package.Box getCommonDataPack()
        {
            DeviceDataBox_2115 box2115 = new DeviceDataBox_2115();
             
            box2115.load(this.BuildingId, this.CabId, DeviceId,  this.devState,
              doseNow, doseAvg,doseStd,rainValue,rainUnit, this.devUnit,this.Lowthreshold.ToString(), this.Highthreshold.ToString(), this.CorrectFactor.ToString());
            return box2115;
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1,VALUE2, VALUE3,VALUE4,VALUE_OPTION,UNITS,SAFESTATE)" + 
                " VALUES(" + tablename + "_sequence" + ".nextval" + ", " +
                DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + DoseNow + ", " + DoseAvg + ", " + DoseStd + ", " + RainValue + ", " + "'"
                + State + "'" + ", " + "'" 
                + DataUnit + "'" + ", " + "'" + State + "' )";
        }

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset1 = new List<DeviceData>();
            List<DeviceData> dataset2 = new List<DeviceData>();
            List<DeviceData> dataset3 = new List<DeviceData>();
            List<DeviceData> dataset4 = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d1 = new DeviceData();
                DeviceData d2 = new DeviceData();
                DeviceData d3 = new DeviceData();
                DeviceData d4 = new DeviceData();
                d1.VALUE1 = odr.GetFloat(5).ToString();
                d2.VALUE1 = odr.GetFloat(6).ToString();
                d3.VALUE1 = odr.GetFloat(7).ToString();
                d4.VALUE1 = odr.GetFloat(8).ToString();
                d1.Time = odr.GetString(2);
                d2.Time = odr.GetString(2);
                d3.Time = odr.GetString(2);
                d4.Time = odr.GetString(2);
                dataset1.Add(d1);
                dataset2.Add(d2);
                dataset3.Add(d3);
                dataset4.Add(d4);
                d1 = null;
                d2 = null;
                d3 = null;
                d4 = null;
            }
            dataDictionary.Add("实时值", dataset1);
            dataDictionary.Add("平均值", dataset2);
            dataDictionary.Add("标准差", dataset3);
            dataDictionary.Add("雨量", dataset4);
            return dataDictionary;
        }

       

        /// <summary>
        /// 2115房间数据或参数读取命令生成。
        /// </summary>
        /// <returns></returns>
        public override byte[] ToReadDataCommand()
        {
            byte[] command = new byte[6];

            command[0] = 0xfe;//包头
            command[1] = 0x05;//包长
            command[2] = 0x00;//探头类型
            command[3] = Convert.ToByte(monitorNumber);//监测仪编号
            //读数据指令
            command[4] = 0xf0;
            //生成校验码
            for (int i = 0; i < 5; i++)
                command[5] ^= command[i];

            return command;
        }

        /// <summary>
        /// 读参数命令
        /// </summary>
        /// <returns></returns>
        public override Byte[] ToReadParaCommands()
        {
            byte[] command = new byte[6];

            command[0] = 0xfe;//包头
            command[1] = 0x05;//包长
            command[2] = 0x00;//探头类型
            command[3] = Convert.ToByte(monitorNumber);//监测仪编号
            //读参数指令
            command[4] = 0xf6;
            //生成校验码
            for (int i = 0; i < 5; i++)
                command[5] ^= command[i];

            return command;
        }
        /// <summary>
        /// 设置参数命令
        /// </summary>
        /// <returns></returns>
        public override Byte[] ToSetParaCommands()
        {
            byte[] command = new byte[18];

            command[0] = 0xfe;//包头
            command[1] = 0x10;//包长
            command[2] = 0x02;//探头类型
            command[3] = Convert.ToByte(monitorNumber);//监测仪编号
            //设置参数指令
            command[4] = 0xf7;

            //高值阈值
            Byte[] tempvalue = BitConverter.GetBytes(Highthreshold);
            command[5] = tempvalue[0];
            command[6] = tempvalue[1];
            command[7] = tempvalue[2];
            command[8] = tempvalue[3];
            //低值阈值
            tempvalue = BitConverter.GetBytes(Lowthreshold);
            command[9] = tempvalue[0];
            command[10] = tempvalue[1];
            command[11] = tempvalue[2];
            command[12] = tempvalue[3];
            //失效阈值
            tempvalue = BitConverter.GetBytes(errorThreshold);
            command[13] = tempvalue[0];
            command[14] = tempvalue[1];
            command[15] = tempvalue[2];
            command[16] = tempvalue[3];

            //生成校验码
            for (int i = 0; i < 17; i++)
                command[17] ^= command[i];

            return command;
        }
        
        public Boolean ParaChanged
        {
            get { return paraChanged; }
            set { paraChanged = value;}
        }
        public Boolean ParaChangedSuccess
        {
            get { return paraChangedSuccess; }
            set
            {
                paraChangedSuccess = value;
                use(paraChangedSuccess);
            }
        }

        public Int32 DetetorType
        {
            get { return detetorType; }
            set { detetorType = value;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DetetorType"));
            }
            }
        }
        public float DoseNow
        {
            get { return doseNow; }
            set
            {
                doseNow = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DoseNow"));
                }
            }
        }

        public float DoseAvg
        {
            get { return doseAvg; }
            set
            {
                doseAvg = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DoseAvg"));
                }
            }
        }

        public float DoseStd
        {
            get { return doseStd; }
            set
            {
                doseStd = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DoseStd"));
                }
            }
        }

        public float RainValue
        {
            get { return rainValue; }
            set
            {
                rainValue = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("RainValue"));
                }
            }
        }

        public float ErrorThreshold
        {
            get { return errorThreshold; }
            set { errorThreshold = value; }
        }

        public String State
        {
            get { return state; }
            set
            {
                state = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("state"));
                }
            }
        }
    }
}
