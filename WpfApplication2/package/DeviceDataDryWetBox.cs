using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WpfApplication2.package;

namespace WpfApplication2.package 
{
    public class DeviceDataDryWetBox :DeviceDataBox
    {
        public const string classNameDryWetString = "DeviceDataBox_DryWet";

        public override string className()
        {
            return classNameDryWetString;
        }
        private string cab_state_, rainy_state_, rain_time_;


        public string cab_state
        {
            get { return cab_state_; }
            set { cab_state_ = value; }
        }

        public string rainy_state
        {
            get { return rainy_state_; }
            set { rainy_state_ = value; }
        }

        public string rain_time
        {
            get { return rain_time_; }
            set { rain_time_ = value; }
        }


        public void load(string _systemId, string _cabId, string _devId, State _state,
          string _cab_state, string _rainy_state, string _rain_time, string _unit, string _paraLow, string _paraHigh, string _correctFactor)
        {
            cab_state_=_cab_state;
            rainy_state_ = _rainy_state;
            rain_time_ = _rain_time;

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
        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId);
            element.SetAttribute("cabId", cabId);
            element.SetAttribute("devId", devId);

            element.SetAttribute("cab_state", cab_state);
            element.SetAttribute("rainy_state", rainy_state);
            element.SetAttribute("rain_time", rain_time);


            element.SetAttribute("state", state.ToString());
            element.SetAttribute("unit", unit);
            element.SetAttribute("highThreshold", Parahigh);
            element.SetAttribute("lowThreshold", Paralow);
            element.SetAttribute("factor", CorrectFactor);

            return element;
        }
    }
}
