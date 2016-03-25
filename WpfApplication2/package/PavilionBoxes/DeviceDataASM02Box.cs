using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using WpfApplication2.package;
 

namespace WpfApplication2.package
{
    public class DeviceDataASM02Box : DeviceDataBox_Base
    {
        public const string classNameString = "DeviceDataASM02Box";

        private string val_str_set_; // 需要用 asm02.cs代码中的 AnalysisPavilionData 进行拆分数据 


        public override string className()
        {
            return classNameString;
        }

        public string val_str_set
        {
            get { return val_str_set_; }
            set { val_str_set_ = value; }
        }

        

        public void load(string _systemId, string _cabId, string _devId, State _state,
         string _val_str_set, string _unit, string _paraLow, string _paraHigh, string _correctFactor)
        {
            val_str_set=_val_str_set;

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

            element.SetAttribute("val_str_set", val_str_set);


            element.SetAttribute("state", state.ToString());
            element.SetAttribute("unit", unit);
            element.SetAttribute("highThreshold", Parahigh);
            element.SetAttribute("lowThreshold", Paralow);
            element.SetAttribute("factor", CorrectFactor);

            return element;
        }
    }
}
