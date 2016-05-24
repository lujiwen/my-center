using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;
namespace WpfApplication2.package
{
    public class DeviceDataBox_Quality : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_Quality";
        public event PropertyChangedEventHandler PropertyChanged;
        public override string className()
        {
            return classNameString;
        }

        public DeviceDataBox_Quality()
        { }

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
            set
            {
                sumValue_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("sumValue"));
                }
            }
        }

        public string sumUnit
        {
            get { return sumUnit_; }
            set
            {
                sumUnit_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("sumUnit"));
                }
            }
        }

        private string sumValue_; //累计值
        private string sumUnit_; //累计值单位
    }
}
