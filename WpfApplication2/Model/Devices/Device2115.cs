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
    //实时曲线显示的委托
    public delegate void RealTimeCurveEvent(float[] value); 
    public delegate void UpdateSuccessEvent(Boolean success);
    public class Device2115 : Device, INotifyPropertyChanged
    {
        Int32 detetorType;//探头类型
        Int32 monitorNumber;//检测仪编号
        Int32 flag;//标志位
        float doseNow;//实时值
        float doseAvg;//平均值
        float doseStd;//标准差
        String dataUnit;//单位

        float rainValue;//雨量值
        String rainUnit;//雨量单位

        float highThreshold;//高阈值
        float lowThreshold;//低阈值
        float errorThreshold;//失效阈值
        float correctFactor;//纠正因子

        String state = " "; //状态

        //当有新数据产生时，通知实时曲线更新
        Boolean rtPaint;//为true时，表示正在绘制实时曲线
        RealTimeCurveEvent rtce;

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
        DeviceDataBox_2115 gamma_box;
        public Device2115(DeviceDataBox_2115 b)
        {
            gamma_box = b;
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
            DoseNow = instantValue;//剂量率瞬时值
            DoseAvg = averageValue;//5分钟平均值
            DoseStd = sdValue;//标准差值
            RainValue = temprainValue;//雨量
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
        }

        public override WpfApplication2.package.Box getCommonDataPack()
        {
            DeviceDataBox_2115 box2115 = new DeviceDataBox_2115();
             
            box2115.load(this.BuildingId, this.CabId, DeviceId,  this.devState,
              doseNow, doseAvg,doseStd,rainValue,rainUnit, this.devUnit,this.Lowthreshold.ToString(), this.Highthreshold.ToString(), this.CorrectFactor.ToString());
            return box2115;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String getHistoryDataSql()
        {
            if (rtPaint){ //该设备正在绘制实时曲线
                float[] dosevalues = new float[4];
                dosevalues[0] = DoseNow;//实时值
                dosevalues[1] = DoseAvg;//平均值
                dosevalues[2] = DoseStd;//标准差值
                dosevalues[3] = RainValue;//雨量

                Rtce(dosevalues); 
            }
            //DateTime dt = DateTime.Now;
            //String[] colums = {"DevId","DataTime","DoseNow","DoseAvg","DoseStd","RainValue","State"};
            //Object[] values = {DevId,"'"+dt.ToString()+"'",DoseNow,DoseAvg,DoseStd,RainValue,"'"+State+"'"};
            //String sql = DBHelper.getInsertCommands("devicehistorydata",colums,values);
            String sql = "";
            return sql;
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
            Byte[] tempvalue = BitConverter.GetBytes(highThreshold);
            command[5] = tempvalue[0];
            command[6] = tempvalue[1];
            command[7] = tempvalue[2];
            command[8] = tempvalue[3];
            //低值阈值
            tempvalue = BitConverter.GetBytes(lowThreshold);
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
        /// <summary>
        /// 更改当前参数的值
        /// </summary>
        public void ChangeDevPara(float[] values)
        {
            if (highThreshold != values[0] || lowThreshold != values[1] || errorThreshold != values[2])//修改的参数
            {
                highThreshold = values[0];
                lowThreshold = values[1];
                errorThreshold = values[2];
                paraChanged = true;
            }
            else//修改的修正因子
            {
                rainValue = values[3];
                ParaChangedSuccess = true;
                Thread.Sleep(500);
            }
  
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
        public UpdateSuccessEvent Use
        {
            get { return use; }
            set { use = value; }
        }


        public Int32 DetetorType
        {
            get { return detetorType; }
            set { detetorType = value; }
        }
        public float DoseNow
        {
            get { return doseNow; }
            set
            {
                doseNow = value;
                if (PropertyChanged != null)
                {
  
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

        public float HighThreshold
        {
            get { return highThreshold; }
            set { highThreshold = value;}
        }
        public float LowThreshold
        {
            get { return lowThreshold; }
            set { lowThreshold = value; }
        }
        public float ErrorThreshold
        {
            get { return errorThreshold; }
            set { errorThreshold = value; }
        }

        public RealTimeCurveEvent Rtce
        {
            get { return rtce; }
            set { rtce = value; }
        }
        public Boolean RtPaint
        {
            get { return rtPaint; }
            set { rtPaint = value; }
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
