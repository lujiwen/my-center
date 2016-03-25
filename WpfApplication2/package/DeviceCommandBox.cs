using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;

namespace WpfApplication2.package
{
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
}
