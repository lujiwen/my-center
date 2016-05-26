using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;
namespace WpfApplication2.package
{
    public class DeviceDataBox_Xb2401 : DeviceDataBox_Base, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_Xb2401";
        public event PropertyChangedEventHandler PropertyChanged;

        public override string className()
        {
            return classNameString;
        }

        public DeviceDataBox_Xb2401()
        { }

        public void load(string _systemId, string _cabId, string _devId, State _state,
        string _value, string _unit, string _paraLow, string _paraHigh, string _correctFactor )
        {
            systemId = _systemId;
            cabId = _cabId; //cab id
            devId = _devId; //device id
            state = _state; //device state
            value = _value; //real-time value of device
            unit = _unit; //unit of value
            Parahigh = _paraHigh;//高阈值
            CorrectFactor = _correctFactor;//修正因子
            Paralow = _paraLow;//低阈值
        }
 
 
    }

}
