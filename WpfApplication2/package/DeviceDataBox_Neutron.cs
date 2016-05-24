using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;
namespace WpfApplication2.package
{
    public class DeviceDataBox_Neutron : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_Neutron";
        public event PropertyChangedEventHandler PropertyChanged;
        private string m_NeutronRate; //中子计数率
        public override string className()
        {
            return classNameString;
        }

        public DeviceDataBox_Neutron()
        { }

        public void loadMore(string neutronRate)
        {
            m_NeutronRate = neutronRate;
        }

        protected override void fromXmlElementMore(XmlElement element)
        {
            m_NeutronRate = element.GetAttribute("neutronRate");
        }

        protected override void toXmlElementMore(ref XmlElement element)
        {
            element.SetAttribute("neutronRate", m_NeutronRate);
        }

        public string neutronRate
        {
            get { return m_NeutronRate; }
            set
            {
                m_NeutronRate = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NeutronRate"));
                }
            }
        }
    }

}
