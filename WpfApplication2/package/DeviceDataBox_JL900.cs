using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace WpfApplication2.package
{
    public class DeviceDataJL900Box : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_JL900";
        public event PropertyChangedEventHandler PropertyChanged;
        public override string className()
        {
            return classNameString;
        }
        private string presure_, real_traffic_, sample_volume_, keep_time_;


        public string presure
        {
            get { return presure_; }
            set { presure_ = value; }
        }

        public string real_traffic
        {
            get { return real_traffic_; }
            set { real_traffic_ = value; }
        }

        public string keep_time
        {
            get { return keep_time_; }
            set { keep_time_ = value; }
        }

        public string sample_volume
        {
            get { return sample_volume_; }
            set { sample_volume_ = value; }
        }


        public void load(string _systemId, string _cabId, string _devId, string _state,
           string _presure,string _real_traffic, string _sample_volume, string _keep_time, string _unit, string _paraLow, string _paraHigh, string _correctFactor)
        {
            presure=presure_;
            real_traffic=real_traffic_;
            sample_volume = sample_volume_;
            keep_time = keep_time_;

            systemId = "运输部监测亭"; // 部署在不同位置的程序，systemid不一样

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
            presure = element.GetAttribute("presure");
            real_traffic = element.GetAttribute("real_traffic");
            sample_volume = element.GetAttribute("sample_volume");
            keep_time = element.GetAttribute("keep_time");
        }
        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId);
            element.SetAttribute("cabId", cabId);
            element.SetAttribute("devId", devId);

            element.SetAttribute("presure", presure);
            element.SetAttribute("real_traffic", real_traffic);
            element.SetAttribute("sample_volume", sample_volume);
            element.SetAttribute("keep_time", keep_time);



            element.SetAttribute("state", state.ToString());
            element.SetAttribute("unit", unit);
            element.SetAttribute("highThreshold", Parahigh);
            element.SetAttribute("lowThreshold", Paralow);
            element.SetAttribute("factor", CorrectFactor);

            return element;
        }
       
        public string Real_traffic
        {
            get { return real_traffic; }
            set
            {
                real_traffic = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Real_traffic"));
                }
            }
        }
        public string Presure
        {
            get { return presure; }
            set
            {
                presure = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Presure"));
                }
            }
        }

        public string Sample_volume
        {
            get { return sample_volume; }
            set
            {
                sample_volume = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Sample_volume"));
                }
            }
        }
        public string Keep_time
        {
            get { return keep_time; }
            set
            {
                keep_time = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Keep_time"));
                }
            }
        }
    }
}
