using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.ComponentModel;

namespace WpfApplication2.package
{
    public class PackageWorker
    {
        public static string pack(List<Box> boxes)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("package");
            foreach (Box box in boxes)
            {
                XmlElement element = box.toXmlElement(doc);
                root.AppendChild(element);
            }
            doc.AppendChild(root);
            return doc.OuterXml;
        }
        public static List<Box> unpack(string package)
        {
            List<Box> boxes = new List<Box>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(package);

            XmlNodeList nodeList;
            nodeList = doc.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                XmlElement elem = (XmlElement)node;
                if (elem.Name == DeviceDataBox_Base.classNameString)
                {
                    DeviceDataBox_Base box = new DeviceDataBox_Base();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataBox_6517AB.classNameString)
                {
                    DeviceDataBox_6517AB box = new DeviceDataBox_6517AB();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataBox_Quality.classNameString)
                {
                    DeviceDataBox_Quality box = new DeviceDataBox_Quality();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataBox.classNameString)
                {
                    DeviceDataBox box = new DeviceDataBox();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceCommandBox.classNameString)
                {
                    DeviceCommandBox box = new DeviceCommandBox();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceCommandEchoBox.classNameString)
                {
                    DeviceCommandEchoBox box = new DeviceCommandEchoBox();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataASM02Box.classNameString)
                {
                    DeviceDataASM02Box box = new DeviceDataASM02Box();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataDryWetBox.classNameString)
                {
                    DeviceDataDryWetBox box = new DeviceDataDryWetBox();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                else if (elem.Name == DeviceDataJL900Box.classNameString)
                {
                    DeviceDataJL900Box box = new DeviceDataJL900Box();
                    box.fromXmlElement(elem);
                    boxes.Add(box);
                }
                Console.WriteLine("设备类型："+boxes[boxes.Count-1].ToString() );
            }
            return boxes;
        }
    }
}
