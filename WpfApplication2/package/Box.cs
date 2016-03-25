using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;

namespace WpfApplication2.package
{
    public abstract class Box : INotifyPropertyChanged
    {
        public abstract XmlElement toXmlElement(XmlDocument doc);
        public abstract string className();
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
