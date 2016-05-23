using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WpfApplication2.Model.Vo;

namespace Project208Home.Model
{
    public class Device593Tritium : Device, INotifyPropertyChanged
    {
        string packetType;//包类型

        public string PacketType
        {
            get { return packetType; }
            set { packetType = value; }
        }
        string versionNumber;//版本号

        public string VersionNumber
        {
            get { return versionNumber; }
            set { versionNumber = value; }
        }
        string sequenceNumber;//序列号

        public string SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }
        string date;

        public string Date
        {
            get { return date; }
            set { date = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("date"));
            }
            }
        }
        string time;

        public string Time
        {
            get { return time; }
            set { time = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("time"));
            }
            }
        }
        double tritiumValueProportionalCounter;//正比计数器

        public double TritiumValueProportionalCounter
        {
            get { return tritiumValueProportionalCounter; }
            set { tritiumValueProportionalCounter = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("tritiumValueProportionalCounter"));
            }
            }
        }
        string tritiumUnitProportionalCounter;//单位

        public string TritiumUnitProportionalCounter
        {
            get { return tritiumUnitProportionalCounter; }
            set { tritiumUnitProportionalCounter = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("tritiumUnitProportionalCounter"));
            }
            }
        }
        double tritiumValueIonChamber;//电离室

        public double TritiumValueIonChamber
        {
            get { return tritiumValueIonChamber; }
            set { tritiumValueIonChamber = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("tritiumValueIonChamber"));
            }
            }
        }
        string tritiumUnitIonChamber;//单位

        public string TritiumUnitIonChamber
        {
            get { return tritiumUnitIonChamber; }
            set { tritiumUnitIonChamber = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("tritiumUnitIonChamber"));
            }
            }
        }
        double backgroundNotUsed;//背景eus

        public double BackgroundNotUsed
        {
            get { return backgroundNotUsed; }
            set { backgroundNotUsed = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("backgroundNotUsed"));
            }
            }
        }
        string backgroundUnitNotUsed;

        public string BackgroundUnitNotUsed
        {
            get { return backgroundUnitNotUsed; }
            set { backgroundUnitNotUsed = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("backgroundUnitNotUsed"));
            }
            }
        }
        double humidity1;

        public double Humidity1
        {
            get { return humidity1; }
            set { humidity1 = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("humidity1"));
            }
            }
        }
        double humidity2;

        public double Humidity2
        {
            get { return humidity2; }
            set { humidity2 = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("humidity2"));
            }
            }
        }
        double flow;

        public double Flow
        {
            get { return flow; }
            set { flow = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("flow"));
            }
            }
        }
        string flowUnit;

        public string FlowUnit
        {
            get { return flowUnit; }
            set { flowUnit = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("flowUnit"));
            }
            }
        }
        double tritiumValuePCInCounts;//tritium值

        public double TritiumValuePCInCounts
        {
            get { return tritiumValuePCInCounts; }
            set { tritiumValuePCInCounts = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("tritiumValuePCInCounts"));
            }
            }
        }
        double backgroundValuePCInCounts;//background值

        public double BackgroundValuePCInCounts
        {
            get { return backgroundValuePCInCounts; }
            set { backgroundValuePCInCounts = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("backgroundValuePCInCounts"));
            }
            }
        }
        double timeInterval;

        public double TimeInterval
        {
            get { return timeInterval; }
            set { timeInterval = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("timeInterval"));
            }
            }
        }
        String state;//状态

        public String State
        {
            get { return state; }
            set { state = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("state"));
            }
            }
        }


        double oxidizerTemperature;

        public double OxidizerTemperature
        {
            get { return oxidizerTemperature; }
            set { oxidizerTemperature = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("oxidizerTemperature"));
            }
            }
        }
        string temperatureUnitForOxidizer;

        public string TemperatureUnitForOxidizer
        {
            get { return temperatureUnitForOxidizer; }
            set { temperatureUnitForOxidizer = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("temperatureUnitForOxidizer"));
            }
            }
        }
        double ambientTemperature;

        public double AmbientTemperature
        {
            get { return ambientTemperature; }
            set { ambientTemperature = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ambientTemperature"));
            }
            }
        }
        string temperatureUnitForAmbient;

        public string TemperatureUnitForAmbient
        {
            get { return temperatureUnitForAmbient; }
            set { temperatureUnitForAmbient = value;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("temperatureUnitForAmbient"));
            }
            }
        }

        ASCIIEncoding encoding = new ASCIIEncoding();

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public Device593Tritium()
        {
        }

        public override Byte[] getParaReadCommands()
        {
            byte[] coms = new byte[10];
            //暂时还不知道下发什么命令
            return coms;
        }

        // 解析数据
        public override void AnalysisData(Byte[] datas)
        {
            string datastr;
            datastr = encoding.GetString(datas);
            string[] dataStrArray = datastr.Split(';');
            Date = dataStrArray[3];
            Time = dataStrArray[4];
            TritiumValueProportionalCounter = Convert.ToDouble(dataStrArray[5]);
            TritiumUnitProportionalCounter = dataStrArray[6];
            TritiumValueIonChamber = Convert.ToDouble(dataStrArray[7]);
            TritiumUnitIonChamber = dataStrArray[8];
            Humidity1 = Convert.ToDouble(dataStrArray[11]);
            humidity2 = Convert.ToDouble(dataStrArray[12]);
            Flow = Convert.ToDouble(dataStrArray[13]);
            FlowUnit = dataStrArray[14];

            OxidizerTemperature = Convert.ToDouble(dataStrArray[28]);
            TemperatureUnitForOxidizer = dataStrArray[29];
            AmbientTemperature = Convert.ToDouble(dataStrArray[30]);
            TemperatureUnitForAmbient = dataStrArray[31];
        }
    }
}
