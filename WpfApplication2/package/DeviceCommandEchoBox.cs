using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;

namespace WpfApplication2.package
{
    public class DeviceCommandEchoBox : Box
    {
        public const string classNameString = "DeviceCommandEchoBox";
        public DeviceCommandEchoBox()
        { }

        public override string className()
        {
            return classNameString;
        }

        public void load(
            //string _devId, string _cabId, string _command,
            string _code)
        {
            //devId_ = _devId;
            //cabId_ = _cabId;
            //command_ = _command;
            code_ = _code;
        }

        public void fromXmlElement(XmlElement element)
        {
            //devId_ = element.GetAttribute("devId");
            //cabId_ = element.GetAttribute("cabId");
            //command_ = element.GetAttribute("command");
            code_ = element.GetAttribute("code");
        }

        public override XmlElement toXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(className());
            //element.SetAttribute("devId", devId_);
            //element.SetAttribute("cabId", cabId_);
            //element.SetAttribute("command", command_);
            element.SetAttribute("code", code_);
            return element;
        }

        //public string devId
        //{
        //    get { return devId_; }
        //    set { devId_ = value; }
        //}
        //public string cabId
        //{
        //    get { return cabId_; }
        //    set { cabId_ = value; }
        //}
        //public string command
        //{
        //    get { return command_; }
        //    set { command_ = value; }
        //}
        public string code
        {
            get { return code_; }
            set { this.code_ = value; }
        }

        //private string devId_;
        //private string cabId_;
        //private string command_;
        private string code_;
    }
}
