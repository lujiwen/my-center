using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace WpfApplication2.package
{
    public class DeviceDataBox_593: DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_593";
        public event PropertyChangedEventHandler PropertyChanged;
        public override string className()
        {
            return classNameString;
        }
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
            set
            {
                date = value;
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
            set
            {
                time = value;
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
            set
            {
                tritiumValueProportionalCounter = value;
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
            set
            {
                tritiumUnitProportionalCounter = value;
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
            set
            {
                tritiumValueIonChamber = value;
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
            set
            {
                tritiumUnitIonChamber = value;
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
            set
            {
                backgroundNotUsed = value;
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
            set
            {
                backgroundUnitNotUsed = value;
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
            set
            {
                humidity1 = value;
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
            set
            {
                humidity2 = value;
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
            set
            {
                flow = value;
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
            set
            {
                flowUnit = value;
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
            set
            {
                tritiumValuePCInCounts = value;
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
            set
            {
                backgroundValuePCInCounts = value;
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
            set
            {
                timeInterval = value;
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
            set
            {
                state = value;
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
            set
            {
                oxidizerTemperature = value;
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
            set
            {
                temperatureUnitForOxidizer = value;
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
            set
            {
                ambientTemperature = value;
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
            set
            {
                temperatureUnitForAmbient = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("temperatureUnitForAmbient"));
                }
            }
        }

     
        public void load(string _systemId, string _cabId, string _devId, State _state,
           string dose_now, string dose_sum , string _unit, string _paraLow, string _paraHigh, string _correctFactor)
        {
            
           
            systemId = "运输部监测亭"; // 部署在不同位置的程序，systemid不一样

            systemId = _systemId;
            cabId = _cabId; //cab id
            devId = _devId; //device id
         //   state = _state; //device state
            unit = _unit; //unit of value
            Parahigh = _paraHigh;//高阈值
            CorrectFactor = _correctFactor;//修正因子
            Paralow = _paraLow;//低阈值

        }
        protected override void fromXmlElementMore(XmlElement element)
        { }
           
        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId);
            element.SetAttribute("cabId", cabId);
            element.SetAttribute("devId", devId);

           
            element.SetAttribute("state", state.ToString());
            element.SetAttribute("unit", unit);
            element.SetAttribute("highThreshold", Parahigh);
            element.SetAttribute("lowThreshold", Paralow);
            element.SetAttribute("factor", CorrectFactor);

            return element;
        }

       

        
    }
}
