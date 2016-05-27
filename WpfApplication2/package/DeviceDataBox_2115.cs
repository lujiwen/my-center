using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;
namespace WpfApplication2.package
{
    public class DeviceDataBox_2115 : DeviceDataBox_Base, INotifyPropertyChanged
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


        public const string classNameString = "DeviceDataBox_2115";
        public event PropertyChangedEventHandler PropertyChanged;

        public string className()
        {
            return classNameString;
        }

        public DeviceDataBox_2115()
        { }


        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId);
            element.SetAttribute("cabId", cabId);
            element.SetAttribute("devId", devId);

            element.SetAttribute("doseNow", doseNow.ToString());
            element.SetAttribute("doseAvg", doseAvg.ToString());
            element.SetAttribute("doseStd", doseStd.ToString());
            element.SetAttribute("rainValue", rainValue.ToString());
            element.SetAttribute("rainUnit", rainUnit);

            element.SetAttribute("state", state.ToString());
            element.SetAttribute("unit", dataUnit);
            element.SetAttribute("highThreshold", Parahigh);
            element.SetAttribute("lowThreshold", Paralow);
            element.SetAttribute("factor", CorrectFactor);

            return element;
        }

        protected override void fromXmlElementMore(XmlElement element)
        {
            doseNow = float.Parse(element.GetAttribute("doseNow"));
            doseAvg = float.Parse(element.GetAttribute("doseAvg"));
            doseStd = float.Parse(element.GetAttribute("doseStd"));
            rainValue =  float.Parse(element.GetAttribute("rainValue"));
            rainUnit = element.GetAttribute("rainUnit");
        }

        public void load(string _systemId, string _cabId, string _devId, string _state,
      float dose_now, float dose_avg, float dose_std, float rain_value, string rain_unit, string dev_unit,string _paraLow, string _paraHigh, string _correctFactor)
        {
            systemId = _systemId; // 部署在不同位置的程序，systemid不一样

            systemId = _systemId;
            cabId = _cabId; //cab id
            devId = _devId; //device id

            doseNow = dose_now;
            doseAvg = dose_avg;
            doseStd = dose_std;
            rainValue = rain_value;
            rainUnit = rain_unit;
            unit = dev_unit;

            Parahigh = _paraHigh;//高阈值
            CorrectFactor = _correctFactor;//修正因子
            Paralow = _paraLow;//低阈值
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
            set { highThreshold = value; }
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
