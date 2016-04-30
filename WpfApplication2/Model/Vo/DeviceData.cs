using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Model.Vo
{
    public class DeviceData
    {
        public DeviceData()
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

        private string v1, v2, v3, v4, v5, v6, v7, v8;

        public string VALUE1
        {
            get { return v1; }
            set { v1 = value; }
        }

        public string VALUE2
        {
            get { return v2; }
            set { v2 = value; }
        }

        public string VALUE3
        {
            get { return v3; }
            set { v3 = value; }
        }
        //public float VALUE4
        //{
        //    get { return v4; }
        //    set { v4 = value; }
        //}
        //public float VALUE5
        //{
        //    get { return v5; }
        //    set { v5 = value; }
        //}
        //public float VALUE6
        //{
        //    get { return v6; }
        //    set { v6 = value; }
        //}
        //public float VALUE7
        //{
        //    get { return v7; }
        //    set { v7 = value; }
        //}

        //public float VALUE8
        //{
        //    get { return v8; }
        //    set { v8 = value; }
        //}
        private string value_option;
        public string Value_Option
        {
            get { return value_option; }
            set { value_option = value; }
        }

    }
}
