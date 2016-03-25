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
    /// 二级协议的包
    /// </summary>
    public class DeviceDataBox_Base : Box, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_Base";
        public event PropertyChangedEventHandler PropertyChanged;
        public DeviceDataBox_Base()
        { }

        public override string className()
        {
            return classNameString;
        }

        public void load(string _systemId, string _cabId, string _devId, State _state,
            string _value, string _unit, string _high_threshold, string _low_threshold, string _factor)
        {
            systemId_ = _systemId;
            cabId_ = _cabId; //cab id
            devId_ = _devId; //device id
            state_ = _state; //device state
            value_ = _value; //real-time value of device
            unit_ = _unit; //unit of value
            high_threshold_ = _high_threshold;
            low_threshold_ = _low_threshold;
            factor_ = _factor;
        }

        public void fromXmlElement(XmlElement element)
        {
            systemId = element.GetAttribute("systemId");
            cabId = element.GetAttribute("cabId");
            devId = element.GetAttribute("devId");
            state = (State)Enum.Parse(typeof(State), element.GetAttribute("state"));
            value = element.GetAttribute("value");
            unit = element.GetAttribute("unit");
            highThreshold = element.GetAttribute("highThreshold");
            lowThreshold = element.GetAttribute("lowThreshold");
            factor = element.GetAttribute("factor");
            fromXmlElementMore(element); //让子类读取更多变量
        }

        protected virtual void fromXmlElementMore(XmlElement element)
        { }

        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId_);
            element.SetAttribute("cabId", cabId_);
            element.SetAttribute("devId", devId_);
            element.SetAttribute("state", state_.ToString());
            element.SetAttribute("value", value_);
            element.SetAttribute("unit", unit_);
            element.SetAttribute("highThreshold", high_threshold_);
            element.SetAttribute("lowThreshold", low_threshold_);
            element.SetAttribute("factor", factor_);
            toXmlElementMore(ref element); //让子类的变量写进element
            return element;
        }

        protected virtual void toXmlElementMore(ref XmlElement element)
        { }

        public enum State { Normal, Alert, H_Alert, Fault };

        public string systemId
        {
            get { return systemId_; }
            set
            {
                systemId_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("systemId"));
                }

            }
        }
        public virtual String GenerateSql()
        {
            return "";
        }
        public string devId
        {
            get { return devId_; }
            set { devId_ = value; }
        }
        public string cabId
        {
            get { return cabId_; }
            set { cabId_ = value; }
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
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("value"));
                }
            }
        }
        public string unit
        {
            get { return unit_; }
            set { unit_ = value; }
        }
        public string highThreshold
        {
            get { return high_threshold_; }
            set { high_threshold_ = value; }
        }
        public string lowThreshold
        {
            get { return low_threshold_; }
            set { low_threshold_ = value; }
        }
        public string factor
        {
            get { return factor_; }
            set { factor_ = value; }
        }

        protected string systemId_;
        protected string cabId_;
        protected string devId_;
        protected State state_;
        protected string value_;
        protected string unit_;
        protected string high_threshold_;
        protected string low_threshold_;
        protected string factor_;

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
