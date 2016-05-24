using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;
namespace WpfApplication2.package
{
    public class DeviceDataBox_XH3125 : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_XH3125";
        public event PropertyChangedEventHandler PropertyChanged;

        public override string className()
        {
            return classNameString;
        }

        public DeviceDataBox_XH3125()
        { }

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
            set
            {
                voltValue_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("voltValue"));
                }
            }
        }

        private string voltValue_;
    }

}
