using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

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

    }
}
