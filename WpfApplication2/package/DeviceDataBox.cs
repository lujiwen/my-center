using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;


namespace WpfApplication2.package
{
    /// <summary>
    /// 第三方数据库的包
    /// </summary>
    public class DeviceDataBox : Box, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox";
        public event PropertyChangedEventHandler PropertyChanged;
        public DeviceDataBox()
        { }

        public override string className()
        {
            return classNameString;
        }

        public void load(string _systemId, string _devId, string _value, State _state)
        {
            systemId_ = _systemId;
            devId_ = _devId; //device id
            value_ = _value; //real-time value of device
            state_ = _state; //device state
        }

        public void fromXmlElement(XmlElement element)
        {
            systemId = element.GetAttribute("systemId");
            devId = element.GetAttribute("devId");
            value = element.GetAttribute("value");
            state = (State)Enum.Parse(typeof(State), element.GetAttribute("state"));
        }

        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId_);
            element.SetAttribute("devId", devId_);
            element.SetAttribute("value", value_);
            element.SetAttribute("state", state_.ToString());
            return element;
        }
        public enum State { Normal, Alert, H_Alert, Fault };

        public string systemId
        {
            get { return systemId_; }
            set { systemId_ = value; }
        }
        public string devId
        {
            get { return devId_; }
            set { devId_ = value; }
        }
        public State state
        {
            get { return state_; }
            set { state_ = value; }
        }
        public string value
        {
            get { return value_; }
            set
            {
                this.value_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("value_"));
                }
            }
        }

        private string systemId_;
        private string devId_;
        private string value_;
        private State state_;
        private string unit_;
        public string unit
        {
            get { return unit_; }
            set { unit_ = value; }
        }
        private string cabId_;
        public string cabId
        {
            get { return cabId_; }
            set { cabId_ = value; }
        }
        private string paralow;

        public string Paralow
        {
            get { return paralow; }
            set { paralow = value; }
        }
        private string parahigh;

        public string Parahigh
        {
            get { return parahigh; }
            set { parahigh = value; }
        }
        private String correctFactor;

        public String CorrectFactor
        {
            get { return correctFactor; }
            set { correctFactor = value; }
        }
    }
}
