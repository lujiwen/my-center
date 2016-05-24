using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;
namespace WpfApplication2.package
{
    public class DeviceDataBox_Gamma : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_Gamma";
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_gammaTotalDose; //伽马总剂量
        private string m_gammaDoseRate; //伽马剂量率  

        public override string className()
        {
            return classNameString;
        }

        public DeviceDataBox_Gamma()
        { }

        public void loadMore(string gammaTotalDose, string gammaDoseRate)
        {
            m_gammaTotalDose = gammaTotalDose;
            m_gammaDoseRate = gammaDoseRate;
        }

        protected override void fromXmlElementMore(XmlElement element)
        {
            m_gammaTotalDose = element.GetAttribute("gammaTotalDose");
            m_gammaDoseRate = element.GetAttribute("gammaDoseRate");
        }

        protected override void toXmlElementMore(ref XmlElement element)
        {
            element.SetAttribute("gammaTotalDose", m_gammaTotalDose);
            element.SetAttribute("gammaDoseRate", m_gammaDoseRate);
        }

        public string GammaDoseRate
        {
            get { return m_gammaDoseRate; }
            set
            {
                m_gammaTotalDose = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("GammaDoseRate"));
                }
            }
        }

        public string GammaTotalDose
        {
            get { return m_gammaTotalDose; }
            set
            {
                m_gammaTotalDose = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("GammaTotalDose"));
                }
            }
        }
    }

}
