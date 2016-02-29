using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Collections.ObjectModel;


namespace Demo.WindowsPresentation.Model
{
    /// <summary>
    /// 柜子类：包含所有的设备信息
    /// 新增：包含几套设备 DeviceGroup
    /// </summary>
    public class Cab  
    {
        //private UInt32 id;//柜子编号
        //private String office; //科室
        //private String home; //房间号
        //private String name;//名称
        //private String ip;//ip地址
        //private String port;//端口号
        ////所有的设备
        //private DeviceCollection devices = new DeviceCollection();

        ////几套系统设备
        //private UInt32 subSystemNum;

        ////每套系统的id
        //private Dictionary<UInt32, DeviceGroup> subSystemNum_deviceGroups = new Dictionary<uint, DeviceGroup>();

        ////泵设备集合
        //private DevicePumpCollection devPumps = new DevicePumpCollection();

        ////需要初始化的设备类型
        //private LinkedList<String> devNeedInitTypes = new LinkedList<string>();

        ////数字量的设备类型集合：走小PLC的数据，不存在设备地址
        //private LinkedList<String> devSR20Types = new LinkedList<string>();

        ////数字量的设备类型集合：走小PLC的数据,且存在设备地址
        //private LinkedList<String> devSR20LocalAddressTypes = new LinkedList<string>();

        ////模拟量设备类型集合
        //private LinkedList<String> devAnalogTypes = new LinkedList<string>();

        ////是否存在泵操作
        //Boolean pumpOperation = new Boolean();
        //Boolean powerSwitch = true;

        //String cabState; //状态1未连接：存在设备未连接，状态2连接中：所有设备正在初始化，状态3运行正常：所有设备运行正常，状态4运行出错：存在          设备出现异常
        //public event PropertyChangedEventHandler PropertyChanged;


        public Cab()
        {

        }
        /// <summary>
        /// 初始化
        /// </summary>
        //    public void init()
        //    {
        //        foreach (Device dev in devices)
        //        {
        //            if (!subSystemNum_deviceGroups.Keys.Contains(dev.SubSystemSerial))
        //            {
        //                DeviceGroup devGroup = new DeviceGroup();
        //                devGroup.SubSystemId = dev.SubSystemSerial; //设置子系统编号
        //                devGroup.SubSystemName = dev.SubSystemName;
        //                devGroup.AddSingleDevice(dev);
        //                subSystemNum_deviceGroups.Add(dev.SubSystemSerial,devGroup);
        //            }
        //            else
        //            {
        //                subSystemNum_deviceGroups[dev.SubSystemSerial].AddSingleDevice(dev);
        //            }
        //            switch (dev.DevType)
        //            {
        //                case Constants.Dev_6517A:
        //                    dev.HandleTypeInSystem = Constants.Dev_6517;
        //                    if (!devSR20Types.Contains(Constants.Dev_6517))
        //                    {
        //                        devSR20Types.AddLast(Constants.Dev_6517);
        //                        DevNeedInitTypes.AddLast(Constants.Dev_6517);
        //                    }
        //                    break;
        //                case Constants.Dev_6517B:
        //                    dev.HandleTypeInSystem = Constants.Dev_6517;
        //                    if (!devSR20Types.Contains(Constants.Dev_6517))
        //                    {
        //                        devSR20Types.AddLast(Constants.Dev_6517);
        //                        DevNeedInitTypes.AddLast(Constants.Dev_6517);
        //                    }
        //                    break;
        //                case Constants.Dev_Quality:
        //                    dev.HandleTypeInSystem = Constants.Dev_Quality;
        //                    if (!devSR20LocalAddressTypes.Contains(Constants.Dev_Quality))
        //                    {
        //                        devSR20LocalAddressTypes.AddLast(Constants.Dev_Quality);
        //                    }

        //                    break;
        //                case Constants.Dev_XH3125:
        //                    dev.HandleTypeInSystem = Constants.Dev_XH;
        //                    if (!devSR20LocalAddressTypes.Contains(Constants.Dev_XH))
        //                    {
        //                        devSR20LocalAddressTypes.AddLast(Constants.Dev_XH);
        //                    }
        //                    break;
        //                case Constants.Dev_XH3127:
        //                    dev.HandleTypeInSystem = Constants.Dev_XH;
        //                    if (!devSR20LocalAddressTypes.Contains(Constants.Dev_XH))
        //                    {
        //                        devSR20LocalAddressTypes.AddLast(Constants.Dev_XH);
        //                    }
        //                    break;
        //                case Constants.Dev_Presure:
        //                    dev.HandleTypeInSystem = Constants.Dev_Presure;
        //                    if (!devAnalogTypes.Contains(Constants.Dev_Presure))
        //                    {
        //                        devAnalogTypes.AddLast(Constants.Dev_Presure);
        //                    }
        //                    break;
        //                case Constants.Dev_Pump:
        //                    devPumps.Add((DevicePump)dev);
        //                    break;
        //            }
        //        }
        //        subSystemNum = (UInt32)subSystemNum_deviceGroups.Count;
        //    }
        //    /// <summary>
        //    /// 响应事件
        //    /// </summary>
        //    /// <param name="devsId"></param>
        //    public void SetNeedUpdateRealTimeDevsId(UInt32[] devsId)
        //    {
        //        NeedDevsId = devsId;
        //    }
        //    //柜子编号
        //    public UInt32 Id
        //    {
        //        get { return id; }
        //        set { id = value; }
        //    }
        //    //柜子名称
        //    public String Name
        //    {
        //        get { return name; }
        //        set { name = value; }
        //    }
        //    //科室
        //    public String Office
        //    {
        //        get { return office; }
        //        set { office = value; }
        //    }
        //    //房间号
        //    public String Home
        //    {
        //        get { return home; }
        //        set { home = value; }
        //    }
        //    //Ip
        //    public String Ip
        //    {
        //        get { return ip; }
        //        set { ip = value; }
        //    }
        //    //端口号
        //    public String Port
        //    {
        //        get { return port; }
        //        set { port = value; }
        //    }
        //    //设备集合
        //    public DeviceCollection Devices
        //    {
        //        get { return devices; }
        //        set { devices = value; }
        //    }
        //    public DevicePumpCollection DevPumps
        //    {
        //        get { return devPumps; }
        //        set { devPumps = value; }
        //    }
        //    public UInt32 SubSystemNum
        //    {
        //        get { return subSystemNum; }
        //        set { subSystemNum = value; }
        //    }
        //    public Dictionary<UInt32, DeviceGroup> SubSystemNum_deviceGroups
        //    {
        //        get { return subSystemNum_deviceGroups; }
        //        set { subSystemNum_deviceGroups = value; }
        //    }


        //    public LinkedList<String> DevNeedInitTypes
        //    {
        //        get { return devNeedInitTypes; }
        //        set { devNeedInitTypes = value; }
        //    }

        //    public LinkedList<String> DevSR20Types
        //    {
        //        get { return devSR20Types; }
        //        set { devSR20Types = value; }
        //    }

        //    public LinkedList<String> DevSR20LocalAddressTypes
        //    {
        //        get { return devSR20LocalAddressTypes; }
        //        set { devSR20LocalAddressTypes = value; }
        //    }


        //    public LinkedList<String> DevAnalogTypes
        //    {
        //        get { return devAnalogTypes; }
        //        set { devAnalogTypes = value; }
        //    }

        //    public String CabState
        //    {
        //        get { return cabState; }
        //        set
        //        {
        //            cabState = value;
        //            if (PropertyChanged != null)
        //            {
        //                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("cabState"));
        //            }
        //        }
        //    }
        //    public Boolean PumpOperation
        //    {
        //        get { return pumpOperation; }
        //        set { pumpOperation = value; }
        //    }
        //}
    }
}
