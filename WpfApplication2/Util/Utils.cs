using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using WpfApplication2.package;
using System.Xml;

namespace WpfApplication2.Util
{
    public class Utils 
    {
        private static string FIRST_IN_FLAG = "0";
        public static string readConfig(string key)
        {
            Configuration config;
            setConfigFile("app.config");
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            String flag = config.AppSettings.Settings[key].Value;
            return flag;
        }
        public static void setConfig(string key,string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException e)
            {
                //Console.WriteLine("Error writing app settings");
                LogUtil.Log(true,e.Message,(int)ErrorCode.ERR_CODE.WIRTE_CONFIG_ERR);
            }
        }
        public static void setConfigFile(string configFileName)
        {
            String path = System.Environment.CurrentDirectory;
            String configFile = path + @"\" + configFileName + "";
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFile);
        }

        public static bool isFirstIn()
        {
            String path = System.Environment.CurrentDirectory;
            String configFile = path + @"\app.config";
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFile);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            String flag = config.AppSettings.Settings["firstIn"].Value;
            if (flag.Equals(FIRST_IN_FLAG))
            {
                return true;
            }
            else return false;
        }

        public static void openDir(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        public static double Max(double a, double b)
        {
            if (a >= b) return a;
            else return b;
        }
        public static double Min(double a, double b)
        {
            if (a <= b) return a;
            else return b;
        }
        /// <summary>
        /// 组包
        /// </summary>
        /// <param name="boxes"></param>
        /// <returns></returns>
        public static string PackBoxes(List<Box> boxes)
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

        public static string PackBox(Box box)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("package");
            XmlElement element = box.toXmlElement(doc);
            root.AppendChild(element);
            doc.AppendChild(root);
            return doc.OuterXml;
        }
    }
}
 