using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using Project208Home.Model;
using PavilionMonitor;
using Yancong;
using Project2115Home.Model;

namespace WpfApplication2.Model.Vo
{
    public class DeviceFactory
    {
        public static Device createDevice(string type, OracleDataReader odr)
        {
            Device device;
            switch (type)
            {
                case "XH3125":
                    device = new DeviceXH31253127(odr);
                    break;
                case "Pump":
                    device = new DevicePump(odr);
                    break;
                case "6517AB":
                    device = new Device6517AB(odr);
                    break;
                case "Quality":
                    device = new DeviceQuality(odr);
                    break;
                case "DryWet":
                    device = new DeviceDryWet(odr);
                    break;
                case "Asm02":
                    device = new DeviceASM02(odr);
                    break;
                case "Jl900":
                    device = new DeviceJL900(odr);
                    break;
                case "gamma":
                    device = new DeviceGamma(odr);
                    break;
                case "neutron":
                    device = new DeviceNeutron(odr);
                    break;
                case "XB2401":
                    device = new DeviceXb2401(odr);
                    break;
                case "MARC7000":
                    device = new DeviceMARC7000(odr);
                    break;
                case "KSJ":
                    device = new DeviceKSJKSD(odr);
                    break;
                case "593氚检测系统":
                    device = new Device593Tritium(odr);
                    break;
                case "2115":
                    device = new Device2115(odr);
                    break;
                default:
                    device = new Device(odr);
                    break;
            }
            return device;
        }
    }
}
