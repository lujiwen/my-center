using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;

namespace Project208Home.Model
{
    public class DeviceMARC7000 : Device, INotifyPropertyChanged
    {
        string sDECFranceVersion;//仪器版本号

        public string SDECFranceVersion
        {
            get { return sDECFranceVersion; }
            set { sDECFranceVersion = value;
            }
        }
        string couplernum1;//传感器1

        public string Couplernum1
        {
            get { return couplernum1; }
            set { couplernum1 = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("couplernum1"));
            }
            }
        }
        string couplernum2;//传感器2

        public string Couplernum2
        {
            get { return couplernum2; }
            set { couplernum2 = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("couplernum2"));
            }
            }
        }
        string bauds;//波特率

        public string Bauds
        {
            get { return bauds; }
            set { bauds = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("bauds"));
            }
            }
        }
        string stopbit;//停止位

        public string Stopbit
        {
            get { return stopbit; }
            set { stopbit = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("stopbit"));
            }
            }
        }
        double delay;//延时，单位ms

        public double Delay
        {
            get { return delay; }
            set { delay = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("delay"));
            }
            }
        }
        double ovenTemp;//加热炉温度

        public double OvenTemp
        {
            get { return ovenTemp; }
            set { ovenTemp = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ovenTemp"));
            }
            }
        }
        double coldTemp;//冷却系统温度

        public double ColdTemp
        {
            get { return coldTemp; }
            set { coldTemp = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("coldTemp"));
            }
            }
        }
        double airFlow;//取样流量，单位L/H

        public double AirFlow
        {
            get { return airFlow; }
            set { airFlow = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("airFlow"));
            }
            }
        }
        double vol;//取样总体积，单位L

        public double Vol
        {
            get { return vol; }
            set { vol = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("vol"));
            }
            }
        }
        double time;//取样时间

        public double Time
        {
            get { return time; }
            set { time = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("time"));
            }
            }
        }
        string sequence;//仪器状态编码

        public string Sequence
        {
            get { return sequence; }
            set { sequence = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("sequence"));
            }
            }
        }
        double ovenSettingDeg;//加热炉设置的加热温度

        public double OvenSettingDeg
        {
            get { return ovenSettingDeg; }
            set { ovenSettingDeg = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ovenSettingDeg"));
            }
            }
        }
        double airFlowset;//取样流量设置值

        public double AirFlowset
        {
            get { return airFlowset; }
            set { airFlowset = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("airFlowset"));
            }
            }
        }

        String safeColor;
        String devIsSafe;

        ASCIIEncoding encoding = new ASCIIEncoding();  

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public DeviceMARC7000()
        { 

        }

        public DeviceMARC7000(OracleDataReader odr)
            :base(odr)
        {

        }
        //marc7000无论下发什么都回来数据。所以暂定下发一个“#”
        public override Byte[] getParaReadCommands()
        {
            char comStr = '#';
            byte[] coms = new byte[1];
            coms[0] = Convert.ToByte(comStr);
            return coms;
        }

        // 解析数据
        public override void AnalysisData(Byte[] datas)
        {
            string datastr;
            datastr = encoding.GetString(datas);
            string[] dataStrArray = datastr.Split('\n');
            string data1 = dataStrArray[0];//仪器版本号
            byte[] data2 = encoding.GetBytes(dataStrArray[1]);//传感器1
            byte[] dataForCouplernum1 = CopyBytes(data2,8,8);
            Couplernum1 = encoding.GetString(dataForCouplernum1, 0, dataForCouplernum1.Length);
            byte[] data3 = encoding.GetBytes(dataStrArray[2]);//传感器2
            byte[] dataForCouplernum2 = CopyBytes(data3, 8, 8);
            Couplernum2 = encoding.GetString(dataForCouplernum2, 0, dataForCouplernum2.Length);
            byte[] data4 = encoding.GetBytes(dataStrArray[3]);//波特率
            byte[] dataForBauds = CopyBytes(data4, 8, 8);
            Bauds = encoding.GetString(dataForBauds, 0, dataForBauds.Length);
            byte[] data5 = encoding.GetBytes(dataStrArray[4]);//停止位
            byte[] dataForStopbit = CopyBytes(data5, 8, 8);
            Stopbit = encoding.GetString(dataForStopbit, 0, dataForStopbit.Length);
            byte[] data6 = encoding.GetBytes(dataStrArray[5]);//延时
            byte[] dataForDelay = CopyBytes(data6,58, 8);
            Delay = Convert.ToDouble(encoding.GetString(dataForDelay, 0, dataForDelay.Length));
            byte[] data7 = encoding.GetBytes(dataStrArray[6]);//加热炉温度
            byte[] dataForOvenTemp = CopyBytes(data7, 58, 8);
            OvenTemp = Convert.ToDouble(encoding.GetString(ChangeByte(dataForOvenTemp), 0, dataForOvenTemp.Length));
            byte[] data8 = encoding.GetBytes(dataStrArray[7]);//冷却系统温度
            byte[] dataForColdTemp = CopyBytes(data8, 8, 32);
            ColdTemp = Convert.ToDouble(encoding.GetString(ChangeByte(dataForColdTemp), 0, dataForColdTemp.Length));
            byte[] data9 = encoding.GetBytes(dataStrArray[8]);//取样流量
            byte[] dataForAirFlow = CopyBytes(data9, 8, 32);
            AirFlow = Convert.ToDouble(encoding.GetString(ChangeByte(dataForAirFlow), 0, dataForAirFlow.Length));
            byte[] data10 = encoding.GetBytes(dataStrArray[9]);//取样总体积
            Vol = TotalVol(data10);
            Time = TotalTime(dataStrArray[10]);//取样时间
            byte[] data12 = encoding.GetBytes(dataStrArray[11]);//仪器状态编码
            byte[] dataForSequence = CopyBytes(data12, 8, 8);
            Sequence = encoding.GetString(dataForSequence, 0, dataForSequence.Length);
            byte[] data13 = encoding.GetBytes(dataStrArray[12]);//加热炉设置的加热温度
            byte[] dataForOvenSettingDeg = CopyBytes(data13, 58, 8);
            OvenSettingDeg = Convert.ToDouble(encoding.GetString(ChangeByte(dataForOvenSettingDeg), 0, dataForOvenSettingDeg.Length));
            byte[] data14 = encoding.GetBytes(dataStrArray[13]);//取样流量设置值
            byte[] dataForAirFlowset = CopyBytes(data14, 58, 32);
            AirFlowset = Convert.ToDouble(encoding.GetString(ChangeByte(dataForAirFlowset), 0, dataForAirFlowset.Length));

        }

        //在byte数组中寻找冒号
        public int FindColonInBytes(byte[] data)
        {
            int index = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0x3A)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        //copy指定的byte数组，取两个指定字符之间的数据
        public byte[] CopyBytes(byte[] data,int a,int b)
        {
            byte[] dataByte = null;
            int index1 = 0;
            int index2 = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == a && index1==0)
                {
                    index1 = i+1;
                }
                if (data[i] == b && index1 > 0)
                {
                    index2 = i-1;
                    break;
                }
            }
            for (int i = index1; i <= index2 ; i++)
            {
                dataByte[i - index1] = data[i];
            }
            return dataByte;
        }

        //将字符串中的','换为'.'
        public byte[] ChangeByte(byte[] data)
        {
            byte[] datas = data;
            for (int i = 0; i < datas.Length; i++ )
            {
                if (datas[i] == 0x2C)
                    datas[i] = 0x2E;
            }
            return datas;
        }

        //取样总体积转换
        public double TotalVol(byte[] data)
        {
            double TotalVol;
            byte[] dataByte1=null;
            byte[] dataByte2=null;
            byte[] dataByte3=null;
            double data1;
            double data2;
            double data3;
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;
            int index4 = 0;
            int index5 = 0;
            int index6 = 0;
            int index7 = 0;
            for (int i = 0; i < data.Length; i++ )
            {
                if (data[i] == 8 && index1 == 0)
                {
                    index1 = i;
                }
                else if(data[i] == 8 && index2 == 0)
                {
                    index2 = i;
                }
                else if(data[i] == 8 && index3 == 0)
                {
                    index3 = i;
                }
                else if(data[i] == 8 && index4 == 0)
                {
                    index4 = i;
                }
                else if (data[i] == 8 && index5 == 0)
                {
                    index5 = i;
                }
                else if (data[i] == 8 && index6 == 0)
                {
                    index6 = i;
                }
                else if (data[i] == 8 && index7 == 0)
                {
                    index7 = i;
                }
            }
            for (int i = index1+1; i <= index2-1; i++)
            {
                dataByte1[i - index1-1] = data[i];
            }
            for (int i = index4+1; i <= index5-1; i++)
            {
                dataByte2[i - index4-1] = data[i];
            }
            for (int i = index7+1; i <= index7+3; i++)
            {
                dataByte3[i - index7-1] = data[i];
            }
            data1 = Convert.ToDouble(encoding.GetString(ChangeByte(dataByte1), 0, dataByte1.Length))*1000;
            data2 = Convert.ToDouble(encoding.GetString(ChangeByte(dataByte2), 0, dataByte2.Length)) * 10;
            data3 = Convert.ToDouble(encoding.GetString(ChangeByte(dataByte3), 0, dataByte3.Length));
            TotalVol = data1 + data2 + data3;
            return TotalVol;
        }

        //取样总时间
        public double TotalTime(string data)
        {
            double totalTime;
            string[] datas = data.Split('\b');
            double data1;
            double data2;
            double data3;
            data1 = Convert.ToDouble(datas[1]);
            data2 = Convert.ToDouble(datas[2]);
            data3 = Convert.ToDouble(datas[4]);
            totalTime = data1 * 100 + data2 + data3 / 60;//单位全统一成h
            return totalTime;
        }

        public String SafeColor
        {
            get
            {
                return safeColor;
            }
            set
            {
                safeColor = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("safeColor"));
                }
            }
        }

        public String DevIsSafe
        {
            get
            {
                return devIsSafe;
            }
            set
            {
                devIsSafe = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("devIsSafe"));
                }
            }
        }
    }
}
