using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace WpfApplication2.package
{
    public class DeviceDataBox_KSJKSD : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_KSJKSD";
        public event PropertyChangedEventHandler PropertyChanged;
        public override string className()
        {
            return classNameString;
        }
        private string doseNow;//实时值
        private string doseSum;//累计值

        public void load(string _systemId, string _cabId, string _devId, string _state,
           string dose_now, string dose_sum , string _unit, string _paraLow, string _paraHigh, string _correctFactor)
        {
            doseNow = dose_now;
            doseSum = dose_sum;
           
            systemId = _systemId; // 部署在不同位置的程序，systemid不一样

            systemId = _systemId;
            cabId = _cabId; //cab id
            devId = _devId; //device id
            state = _state; //device state
            unit = _unit; //unit of value
            Parahigh = _paraHigh;//高阈值
            CorrectFactor = _correctFactor;//修正因子
            Paralow = _paraLow;//低阈值

        }
        protected override void fromXmlElementMore(XmlElement element)
        {
            doseNow = element.GetAttribute("doseNow");
            doseSum = element.GetAttribute("doseSum");
        }
        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId);
            element.SetAttribute("cabId", cabId);
            element.SetAttribute("devId", devId);

            element.SetAttribute("doseNow", doseNow);
            element.SetAttribute("doseSum", doseSum);

            element.SetAttribute("state", state.ToString());
            element.SetAttribute("unit", unit);
            element.SetAttribute("highThreshold", Parahigh);
            element.SetAttribute("lowThreshold", Paralow);
            element.SetAttribute("factor", CorrectFactor);

            return element;
        }

        public string DoseSum
        {
            get { return doseSum; }
            set
            {
                doseSum = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DoseSum"));
                }
            }
        }
        public string DoseNow
        {
            get { return doseNow; }
            set
            {
                doseNow = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DoseNow"));
                }
            }
        }
    }
}
