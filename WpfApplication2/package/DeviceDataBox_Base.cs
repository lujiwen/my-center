﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;
using WpfApplication2.Model.Vo;
using WpfApplication2.Util;
using Project208Home.Model;
using PavilionMonitor;
using Project2115Home.Model;
using Yancong;

namespace WpfApplication2.package
{
    /// <summary>
    /// 二级协议的包
    /// </summary>
    public class DeviceDataBox_Base : Box, INotifyPropertyChanged
    {
        public const string classNameString = "DeviceDataBox_Base";
        public event PropertyChangedEventHandler PropertyChanged;
        public DeviceDataBox_Base()
        { }

        public override string className()
        {
            return classNameString;
        }

        public void load(string _systemId, string _cabId, string _devId, string _state,
            string _value, string _unit, string _high_threshold, string _low_threshold, string _factor)
        {
            systemId_ = _systemId;
            cabId_ = _cabId; //cab id
            devId_ = _devId; //device id
            state_ = _state; //device state
            value_ = _value; //real-time value of device
            unit_ = _unit; //unit of value
            high_threshold_ = _high_threshold;
            low_threshold_ = _low_threshold;
            factor_ = _factor;
        }

        public void fromXmlElement(XmlElement element)
        {
            systemId = element.GetAttribute("systemId");
            cabId = element.GetAttribute("cabId");
            devId = element.GetAttribute("devId");
            state =   element.GetAttribute("state");
            value = element.GetAttribute("value");
            unit = element.GetAttribute("unit");
            highThreshold = element.GetAttribute("highThreshold");
            lowThreshold = element.GetAttribute("lowThreshold");
            factor = element.GetAttribute("factor");
            correctFactor = element.GetAttribute("factor");
            fromXmlElementMore(element); //让子类读取更多变量
        }

        protected virtual void fromXmlElementMore(XmlElement element)
        { }

        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            element.SetAttribute("systemId", systemId_);
            element.SetAttribute("cabId", cabId_);
            element.SetAttribute("devId", devId_);
            element.SetAttribute("state", state_.ToString());
            element.SetAttribute("value", value_);
            element.SetAttribute("unit", unit_);
            element.SetAttribute("highThreshold", high_threshold_);
            element.SetAttribute("lowThreshold", low_threshold_);
            element.SetAttribute("factor", factor_);
            toXmlElementMore(ref element); //让子类的变量写进element
            return element;
        }

        protected virtual void toXmlElementMore(ref XmlElement element)
        { }

        public Device fromBoxToDevice()
        {
                Device deviceToChange = null ;
                        switch (this.className())
                        {
                            case "DeviceDataBox_Quality":
                                deviceToChange = new DeviceQuality((DeviceDataBox_Quality)this );
                                break;
                            case "DeviceDataBox_XH3125":
                                deviceToChange = new DeviceXH31253127((DeviceDataBox_XH3125)this);
                                break;
                            case "DeviceDataBox_6517AB":
                                deviceToChange = new Device6517AB((DeviceDataBox_6517AB)this);
                                break;
                            case "DeviceDataBox_JL900":
                                deviceToChange = new DeviceJL900((DeviceDataJL900Box)this);
                                break;
                            case "DeviceDataASM02Box":
                                deviceToChange = new DeviceASM02((DeviceDataASM02Box)this);
                                break;
                            case "DeviceDataBox_DryWet":
                                deviceToChange = new DeviceDryWet((DeviceDataBox_DryWet)this);
                                break;
                            case "DeviceDataBox_Neutron":
                                deviceToChange = new DeviceNeutron((DeviceDataBox_Neutron)this);
                                break;
                            case "DeviceDataBox_Gamma":
                                deviceToChange = new DeviceGamma((DeviceDataBox_Gamma)this);
                                break;
                            case "DeviceDataBox_593":
                                deviceToChange = new Device593Tritium((DeviceDataBox_593)this);
                                break;
                            case "DeviceDataBox_2115":
                                deviceToChange = new Device2115((DeviceDataBox_2115)this);
                                break;
                            case "DeviceDataBox_Xb2401":
                                deviceToChange = new DeviceXb2401((DeviceDataBox_Xb2401)this);
                                break;
                            default :
                                deviceToChange = new Device(this );
                                break ;
                        }
                        return deviceToChange;
        }

     //   public enum State { Normal, Alert, H_Alert, L_Alert,Fault };

        public string systemId
        {
            get { return systemId_; }
            set
            {
                systemId_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("systemId"));
                }

            }
        }
        public virtual String GenerateSql()
        {
            return "";
        }
        public string devId
        {
            get { return devId_; }
            set { devId_ = value; }
        }
        public string cabId
        {
            get { return cabId_; }
            set { cabId_ = value; }
        }
        public string state
        {
            get { return state_; }
            set { state_ = value; }
        }
        public string value
        {
            get { return value_; }
            set
            {
                this.value_ = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("value"));
                }
            }
        }
        public string unit
        {
            get { return unit_; }
            set { unit_ = value; }
        }
        public string highThreshold
        {
            get { return high_threshold_; }
            set { high_threshold_ = value; }
        }
        public string lowThreshold
        {
            get { return low_threshold_; }
            set { low_threshold_ = value; }
        }
        public string factor
        {
            get { return factor_; }
            set { factor_ = value; }
        }

        protected string systemId_;
        protected string cabId_;
        protected string devId_;
        protected string state_;
        protected string value_;
        protected string unit_;
        protected string high_threshold_;
        protected string low_threshold_;
        protected string factor_;

        private string paralow;

        public string Paralow
        {
            get { return paralow; }
            set { paralow = value; }
        }
        private string parahigh;

        public string Parahigh
        {
            get { return parahigh; }
            set { parahigh = value; }
        }
        private String correctFactor;

        public String CorrectFactor
        {
            get { return correctFactor; }
            set { correctFactor = value; }
        }
    }
}
