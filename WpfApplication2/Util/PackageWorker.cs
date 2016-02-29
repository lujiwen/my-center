using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;

namespace WpfApplication2.Controller
{
    public abstract class Box : INotifyPropertyChanged 
    {
        public abstract XmlElement toXmlElement(XmlDocument doc);
        public abstract string className();
        public event PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// 二级协议的包
    /// </summary>
    public class DeviceDataBox_Base : Box,INotifyPropertyChanged
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
        {}

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
        {}

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
            set { 
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
    }

    public class DeviceDataBox_6517AB : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_6517AB";
        public event PropertyChangedEventHandler PropertyChanged;
        
        public override string className()
        {
            return classNameString;
        }
        
        public DeviceDataBox_6517AB()
        {}

        public void loadMore(string _voltValue)
        {
            voltValue = _voltValue;
        }

        protected override void fromXmlElementMore(XmlElement element)
        {
            voltValue = element.GetAttribute("voltValue");
        }

        protected override void toXmlElementMore(ref XmlElement element)
        {
            element.SetAttribute("voltValue", voltValue_);
        }

        public string voltValue
        {
            get { return voltValue_; }
            set { 
                voltValue_ = value;
                if (PropertyChanged!=null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("voltValue"));
                }
            }
        }

        private string voltValue_;
    }

    public class DeviceDataBox_Quality : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_Quality";
        public event PropertyChangedEventHandler PropertyChanged;
        public override string className()
        {
            return classNameString;
        }

        public DeviceDataBox_Quality()
        {}
        
        public void loadMore(string _sumValue, string _sumUnit)
        {
            sumValue = _sumValue;
            sumUnit = _sumUnit;
        }

        protected override void fromXmlElementMore(XmlElement element)
        {
            sumValue = element.GetAttribute("sumValue");
            sumUnit = element.GetAttribute("sumUnit");
        }

        protected override void toXmlElementMore(ref XmlElement element)
        {
            element.SetAttribute("sumValue", sumValue);
            element.SetAttribute("sumUnit", sumUnit);
        }

        public string sumValue
        {
            get { return sumValue_; }
            set { sumValue_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("sumValue"));
                }
            }
        }

        public string sumUnit
        {
            get { return sumUnit_; }
            set { sumUnit_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("sumUnit"));
                }
             }
        }

        private string sumValue_; //累计值
        private string sumUnit_; //累计值单位
    }

    /// <summary>
    /// 第三方数据库的包
    /// </summary>
    public class DeviceDataBox : Box,INotifyPropertyChanged
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
            set { 
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
    }

    public class DeviceCommandBox : Box
    {
        public const string classNameString = "DeviceCommandBox";
        public DeviceCommandBox()
        { }

        public override string className()
        {
            return classNameString;
        }

        public void load(string _devId, string _cabId, string _high_threshold, string _low_threshold, string _param1, string _factor)
        {
            devId_ = _devId;
            cabId_ = _cabId;
            high_threshold_ = _high_threshold;
            low_threshold_ = _low_threshold;
            param1_ = _param1;
            factor_ = _factor;
        }

        public void fromXmlElement(XmlElement element)
        {
            devId = element.GetAttribute("devId");
            cabId = element.GetAttribute("cabId");
            highThreshold = element.GetAttribute("highThreshold");
            lowThreshold = element.GetAttribute("lowThreshold");
            param1 = element.GetAttribute("param1");
            factor = element.GetAttribute("factor");
        }

        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("devId", devId_);
            element.SetAttribute("cabId", cabId_);
            element.SetAttribute("highThreshold", high_threshold_);
            element.SetAttribute("lowThreshold", low_threshold_);
            element.SetAttribute("param1", param1_);
            element.SetAttribute("factor", factor_);
            return element;
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
        public string param1
        {
            get { return param1_; }
            set { param1_ = value; }
        }
        public string factor
        {
            get { return factor_; }
            set { factor_ = value; }
        }


        private string devId_;
        private string cabId_;
        private string high_threshold_;
        private string low_threshold_;
        private string param1_;
        private string factor_;
    }

    public class DeviceCommandEchoBox : Box
    {
        public const string classNameString = "DeviceCommandEchoBox";
        public DeviceCommandEchoBox()
        { }

        public override string className()
        {
            return classNameString;
        }

        public void load(
            //string _devId, string _cabId, string _command,
            string _code)
        {
            //devId_ = _devId;
            //cabId_ = _cabId;
            //command_ = _command;
            code_ = _code;
        }

        public void fromXmlElement(XmlElement element)
        {
            //devId_ = element.GetAttribute("devId");
            //cabId_ = element.GetAttribute("cabId");
            //command_ = element.GetAttribute("command");
            code_ = element.GetAttribute("code");
        }

        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            //element.SetAttribute("devId", devId_);
            //element.SetAttribute("cabId", cabId_);
            //element.SetAttribute("command", command_);
            element.SetAttribute("code", code_);
            return element;
        }

        //public string devId
        //{
        //    get { return devId_; }
        //    set { devId_ = value; }
        //}
        //public string cabId
        //{
        //    get { return cabId_; }
        //    set { cabId_ = value; }
        //}
        //public string command
        //{
        //    get { return command_; }
        //    set { command_ = value; }
        //}
        public string code
        {
            get { return code_; }
            set { this.code_ = value; }
        }

        //private string devId_;
        //private string cabId_;
        //private string command_;
        private string code_;
    }

    public class PackageWorker
    {
        public static string pack(List<Box> boxes)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("package");
            foreach (Box box in boxes)
            {
                XmlElement element = box.toXmlElement(doc);
                root.AppendChild(element);
            }
            doc.AppendChild(root);
            return doc.OuterXml;
        }
        public static List<Box> unpack(string package)
        {
            List<Box> boxes = new List<Box>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(package);

            XmlNodeList nodeList;
            nodeList = doc.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                XmlElement elem = (XmlElement)node;
                if (elem.Name == DeviceDataBox_Base.classNameString)
                {
                    DeviceDataBox_Base box = new DeviceDataBox_Base();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataBox_6517AB.classNameString)
                {
                    DeviceDataBox_6517AB box = new DeviceDataBox_6517AB();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataBox_Quality.classNameString)
                {
                    DeviceDataBox_Quality box = new DeviceDataBox_Quality();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataBox.classNameString)
                {
                    DeviceDataBox box = new DeviceDataBox();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceCommandBox.classNameString)
                {
                    DeviceCommandBox box = new DeviceCommandBox();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceCommandEchoBox.classNameString)
                {
                    DeviceCommandEchoBox box = new DeviceCommandEchoBox();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
            }

            return boxes;
        }
    }
}
