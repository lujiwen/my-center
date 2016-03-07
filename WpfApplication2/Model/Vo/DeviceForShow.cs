using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Model.Vo
{
    public class DeviceForShow
    {
        public DeviceForShow()
        {
        }
        int deviceId;

        public int DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }
        string time;

        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        String value;

        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        String state;

        public String State
        {
            get { return state; }
            set { state = value; }
        }
        String unit;

        public String Unit
        {
            get { return unit; }
            set { unit = value; }
        }
    }
}
