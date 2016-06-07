﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WpfApplication2.Model;
using WpfApplication2.Util;
using WpfApplication2.Model.Vo;
using WpfApplication2.Model.Db;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Configuration;
using System.IO;
using System.Threading;
using WpfApplication2.package;
using WpfApplication2.View.Windows;
using Project208Home.Model;
using PavilionMonitor;
using Yancong;
using Project2115Home.Model;


namespace WpfApplication2.Controller
{
    public delegate void DataUpdatedEventHandler();
    public delegate void UpdateUI();
    public delegate void alarmMessageEventHandler(AlarmMessage alarmMsg);
    public delegate void alarmBuzzerEventHandler(bool isStart);
 
    public class MainController : INotifyPropertyChanged
    {
        public static WpfApplication2.Model.Db.DBManager dataOfDevice;
    
        public Configuration config;
        private SocketConnection sc;
        private String str;
        public event PropertyChangedEventHandler PropertyChanged;
        public event DataUpdatedEventHandler dataChartUpdate;
        public Queue<Device> bq;  //用于缓存从二级获得的数据的队列
        public static SocketConnection tcpConnection; //控制命令的连接
        public static String tempSendData;
        public event UpdateUI notifyUpdateUI;
        public event alarmMessageEventHandler alarmMessage;
        public event alarmBuzzerEventHandler alarmBuzzer;
        private ConnectionManager manager207c, manager208, manager209, manager201Chimney,
                 manager207Chimney, manager208Chimney, managerMonitorVehicle,
                 managerPavilionTransport, managerPavilionBridge, managerPavilionInnner, manager2115;
        public MainController()
        {
            InitialData();

            //需要在页面加载之后,在初始化连接,改为在MainWindow当中调用
          //  InitialConnection();
          //  InitialDBConnection();
            InitialThread();

            //while (true)
            //{
            //    List<DeviceForShow> result = dataOfDevice.getDataBetweenStartAndEndTime(2, 5, "'2015/10/8 20:14:25'", "'2016/10/8 20:14:25'");
            //    Console.WriteLine("result.Count: " + result.Count);
            //    System.Threading.Thread.Sleep(2000);
            //}

        }


        public DataUpdatedEventHandler DataUpdate
        {
            set { dataChartUpdate = value; }
            get { return dataChartUpdate; }
        }

        /// <summary>
        /// 第一次进入程序初始化数据库
        /// </summary>
        private void initDatabase()
        {
            DBHelper dbInitial = new DBHelper();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            String flag = config.AppSettings.Settings["firstIn"].Value;
            //dbInitial.createUserAndGrant();
            //System.Threading.Thread.Sleep(3000);
            dbInitial.createAllTableAndSequence(0); //创建正常数据库
            Thread.Sleep(2000);
            //dbInitial.createAllTableAndSequence(1); //创建应急数据库
             Utils.setConfig("firstIn", "1");
            flag = config.AppSettings.Settings["firstIn"].Value;
            Console.Write("after: " + flag);     
        }
        
        /// <summary>
        /// 从数据库当中读取检测点楼宇信息
        /// </summary>
        /// <returns></returns>
        private OracleDataReader readBuidingFromDb()
        {
            int errCode = 0 ;
            try{
                DBManager dbOfDevice = new DBManager();
                string errorCode = "";
                errCode = dbOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
                
                String sql = "select * from buildinginfo Orders ORDER BY B_ID";
                OracleDataReader odr = null;
                odr = dbOfDevice.ReadDeviceInfomationFromDb(sql);
                return odr ;
            }
            catch(Exception e)
            {
                 LogUtil.Log(true, e.ToString(),(int)ErrorCode.ERR_CODE.BUILDING_READ_ERR);
            }
            return null ;
        }

        /// <summary>
        /// 从数据库当中读取柜子信息
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="odr"></param>
        /// <returns></returns>
        private　OracleDataReader　readCabFromDb(DBManager dm,OracleDataReader odr)
        {
             try{
                 string state = "Normal" ;
                Building building = new Building("" + odr.GetInt32(0), odr.GetString(1), odr.GetString(2), odr.GetString(3), odr.GetFloat(4), odr.GetFloat(5), new List<Cab>(), state,odr.GetString(6));
                String sql2 = "select * from cabinfo where buildingid = " + building.SystemId + " ORDER BY C_ID";
                OracleDataReader odr2 = null;
                odr2 =  dm.ReadDeviceInfomationFromDb(sql2);
                return odr2 ;
            }
            catch(Exception e)
            {
                LogUtil.Log(true, e.ToString(), (int)ErrorCode.ERR_CODE.CAB_READ_ERR);
                return null;
            }         
        }

        /// <summary>
        /// 从监测点读取设备信息
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="odr2"></param>
        /// <param name="cab"></param>
        /// <returns></returns>
        private OracleDataReader readDeviceFromDb(DBManager dm,OracleDataReader odr2,Cab cab)
        {
            try{                
                   string sql3 = "select * from deviceinfo where cabid = " + cab.CabId + " ORDER BY D_ID";
                   OracleDataReader odr3 = null; 
                   odr3 = dm.ReadDeviceInfomationFromDb(sql3);
                   return odr3 ;
            }
            catch(Exception e)
            {
                LogUtil.Log(true, e.ToString(), (int)ErrorCode.ERR_CODE.DEVICE_READ_ERR);
                return null;
            }       
        }
    
        public void InitialData()  //初始化数据
        {
            if (Utils.isFirstIn()) //第一次进入创建数据表和sequence
            {
                initDatabase();
            }
            DBManager dbOfDevice = new DBManager();
            string errorCode = "";
            int errCode = (int)ErrorCode.ERR_CODE.OK;
            OracleDataReader odr = null;
            OracleDataReader odr2 = null;
            OracleDataReader odr3 = null;

            try
            {
                errCode = dbOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port,                           DBHelper.db_name, ref errorCode);
                if(errCode!=(int)ErrorCode.ERR_CODE.OK)
                {
                    LogUtil.Log(true, "数据库连接建立异常", (int)ErrorCode.ERR_CODE.CONNECTION_OPEN_ERR);
                    return;
                }
                odr = readBuidingFromDb();
                if (odr.HasRows)
                {
                    while (odr.Read()) //找所有的building
                    {
                        string state ="Normal" ;
                        Building building = new Building("" + odr.GetInt32(0), odr.GetString(1), odr.GetString(2), odr.GetString(3), odr.GetFloat(4), odr.GetFloat(5), new List<Cab>(), state,odr.GetString(6));
                        Console.WriteLine(building.Name);
                        odr2 = readCabFromDb(dbOfDevice, odr);
                        if (odr2.HasRows)
                        {
                            while (odr2.Read()) //找每个building对应的cab5
                            {
                                Cab cab = new Cab("" + odr2.GetInt32(0), "" + odr2.GetInt32(1), "" + odr2.GetString(2), odr2.GetString(3), odr2.GetString(4), odr2.GetString(5), odr2.GetString(6), new List<Device>(), state);
                                odr3 = readDeviceFromDb(dbOfDevice, odr2, cab);
                                Console.WriteLine(building.Name +" :"+cab.Name );
                                while (odr3.Read())
                                {
                                    Device device = null;
                                    switch (odr3.GetString(15))
                                    {
                                        case "XH3125":
                                            device = new DeviceXH31253127(odr3);
                                            break;
                                        case "Pump":
                                            device = new DevicePump(odr3);
                                            break;
                                        case "6517AB":
                                            device = new Device6517AB(odr3);
                                            break;
                                        case "Quality":
                                            device = new DeviceQuality(odr3);
                                            break;
                                        case "DryWet":
                                             device = new DeviceDryWet(odr3);
                                            break;
                                        case "Asm02":
                                            device = new DeviceASM02(odr3);
                                            break;
                                        case "Jl900":
                                            device = new DeviceJL900(odr3);
                                            break;
                                        case "gamma":
                                            device = new DeviceGamma(odr3);
                                            break;
                                        case "neutron":
                                            device = new DeviceNeutron(odr3);
                                            break;
                                        case "XB2401":
                                            device = new DeviceXb2401(odr3);
                                            break;
                                        case "MARC7000":
                                            device = new DeviceMARC7000(odr3);
                                            break;
                                        case "KSJ":
                                            device = new DeviceKSJKSD(odr3);
                                            break;
                                        case "593氚检测系统":
                                            device = new Device593Tritium(odr3);
                                            break;
                                        case "2115":
                                            device = new Device2115(odr3);
                                            break;
                                        default :
                                            device = new Device(odr3);
                                            break;
                                    }
                                    device.devIp = cab.Ip;
                                    cab.Devices.Add(device);
                                    Console.WriteLine(building.Name + " :" + cab.Name+":"+device.Type);
                                    GlobalMapForShow.globalMapForDevice.Add(building.SystemId + "_" + device.DeviceId, device);
                                }
                                GlobalMapForShow.globalMapForCab.Add(building.SystemId + "_" + cab.CabId, cab);
                                odr3.Close();
                                building.Cabs.Add(cab);
                            }
                        }
                        odr2.Close();
                        GlobalMapForShow.globalMapForBuiding.Add(building.SystemId, building);
                    }
                }
                odr.Close();
                dbOfDevice.CloseConnection();
                Console.WriteLine("MainController,InitialData, .........." + GlobalMapForShow.globalMapForBuiding["13"].Cabs[0].Devices[1].CabId);
            }
            catch(Exception e)
            {
                LogUtil.Log(true, e.Message, (int)ErrorCode.ERR_CODE.MAP_POINTS_INIT_ERR);
            }                      
        }

        static void AddUpdateAppSettings(string key, string value)  //第一次进入初始化完成后更新键值对，以后进来就不初始化了。。
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
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private List<ConnectionManager> conManagerList;
        
        /// <summary>
        ///   初始化连接到各个监测点，部分为直连设备，那么不同的设备都要建立一个socket连接（comconnection）
        ///   部分连接二级监测点，收到数据包进行解析，那么所有设备都使用柜子ip和统一的端口号，只建立一个socket（udpconnection）
        ///   收数据
        /// </summary>
        public void InitialConnection() 
        {
            //manager207c = init207cConnection();
            //if (manager207c != null && manager207c.isAllConnected)
            //{
            //    Alarm("207c连接初始化成功！");
            //}
            //else
            //{
            //    Alarm("207c连接初始化失败：" + manager207c.getConnectiosErr());

            //}
        //    manager208 = init208Connection();
        //    if (manager208 != null)
        //    {
        //        Alarm("208连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("208连接初始化失败！"+manager208.getConnectiosErr());
        //    }
        //    manager209 = init209Conection();
        //    if (manager209 != null)
        //    {
        //        Alarm("209连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("209连接初始化失败," + manager209.getConnectiosErr());
        //    }
        //    manager2115 = init2115Connection();
        //    if (manager2115 != null&&manager2115.isAllConnected)
        //    {
        //        Alarm("2115连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("2115连接初始化失败,"+manager2115.getConnectiosErr());
        //    }
        //    manager201Chimney = init201Chimney();
        //    if (manager201Chimney != null && manager201Chimney.isAllConnected)
        //    {
        //        Alarm("201烟囱连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("201烟囱连接初始化失败," + manager201Chimney.getConnectiosErr());
        //    }
        //    manager207Chimney = init207Chimney();
        //    if (manager207Chimney != null && manager207Chimney.isAllConnected)
        //    {
        //        Alarm("207烟囱连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("207烟囱连接初始化失败," + manager207Chimney.getConnectiosErr());
        //    }
        //    manager208Chimney = init208Chimney();
        //    if (manager208Chimney != null && manager208Chimney.isAllConnected)
        //    {
        //        Alarm("208烟囱连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("208烟囱连接初始化失败," + manager208Chimney.getConnectiosErr());
        //    }
        //    if (initPavilionEx1() != null)
        //    {
        //        Alarm("亭子（运输部）连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("亭子（运输部）连接初始化失败！");
        //    }
        //    if (initPavilionEx2() != null)
        //    {
        //        Alarm("亭子（新桥）连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("亭子（新桥）连接初始化失败！");
        //    }
        //    managerPavilionInnner = initPavilionInner();
        //    if (managerPavilionInnner != null)
        //    {
        //        Alarm("亭子（内网）连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("亭子（内网）连接初始化失败！");
        //    }
        //    managerMonitorVehicle = initMonitorVehicle();
        //    if (managerMonitorVehicle != null)
        //    {
        //        Alarm("监测车连接初始化成功！");
        //    }
        //    else
        //    {
        //        Alarm("监测车连接初始化失败！");
        //    }
        }

        /// <summary>
        /// 207c 
        /// 读取方式：串口服务器
        /// 设备：2个XB2401（两个端口），asm
        /// </summary>
        private ConnectionManager init207cConnection()
        {
            Building b = GlobalMapForShow.getBuildingByName("207楼C");
            ConnectionManager manager207c = null;
            if(b!=null)
            {
                List<Connection> cons = new List<Connection>();
                foreach(Device d in b.Cabs[0].Devices)
                {
                    COMConnection con = new COMConnection(d);
                    cons.Add(con);
                }
                manager207c = new ConnectionManager(cons);
                manager207c.ManagerReceivedDataEvent += receiveData;
                manager207c.startConnections();
            }
            return manager207c;
        }

        private ConnectionManager init208Connection()
        {
            Building b = GlobalMapForShow.getBuildingByName("208楼");
            Cab c = b.Cabs[0];
            UdpConnection uc208 = new UdpConnection(c.Ip, c.Port);
            conManagerList = new List<ConnectionManager>();
            ConnectionManager manager208 = new ConnectionManager(uc208);
            manager208.ManagerReceivedDataEvent += receiveData;
            return manager208;
        }

        private ConnectionManager init209Conection()
        {
            Building b = GlobalMapForShow.getBuildingByName("209楼");
            Cab c = b.Cabs[0];
            UdpConnection uc209 = new UdpConnection(c.Ip, c.Port);
            conManagerList = new List<ConnectionManager>();
            ConnectionManager manager209 = new ConnectionManager(uc209);
            manager209.ManagerReceivedDataEvent += receiveData;
            return manager209;
        }

        private ConnectionManager init201Chimney()
        {
            Building b = GlobalMapForShow.getBuildingByName("201烟囱");
            ConnectionManager manager201Chimney = null;
            if (b != null)
            {
                List<Connection> cons = new List<Connection>();
                foreach (Device d in b.Cabs[0].Devices)
                {
                    COMConnection con = new COMConnection(d);
                    cons.Add(con);
                }
                manager201Chimney = new ConnectionManager(cons);
                manager201Chimney.ManagerReceivedDataEvent += receiveData;
                manager201Chimney.startConnections();
            }
            return manager201Chimney;
        }
        private ConnectionManager init207Chimney()
        {
            Building b = GlobalMapForShow.getBuildingByName("207烟囱");
            ConnectionManager manager207Chimney = null;
            if (b != null)
            {
                List<Connection> cons = new List<Connection>();
                foreach (Device d in b.Cabs[0].Devices)
                {
                    COMConnection con = new COMConnection(d);
                    cons.Add(con);
                }
                manager207Chimney = new ConnectionManager(cons);
                manager207Chimney.ManagerReceivedDataEvent += receiveData;
                manager207Chimney.startConnections();
            }
            return manager207Chimney;
        }
        private ConnectionManager init208Chimney()
        {
            Building b = GlobalMapForShow.getBuildingByName("208烟囱");
            Cab c = b.Cabs[0];
            UdpConnection uc208Chimney = new UdpConnection(c.Ip, c.Port);
            conManagerList = new List<ConnectionManager>();
            ConnectionManager manager208Chimney = new ConnectionManager(uc208Chimney);
            manager208Chimney.ManagerReceivedDataEvent += receiveData;
            return manager208Chimney;
        }

        private ConnectionManager initMonitorVehicle()
        {
            Building b = GlobalMapForShow.getBuildingByName("检测车");
            Cab c = b.Cabs[0];
            UdpConnection ucMonitorVehicle = new UdpConnection(c.Ip, c.Port);
            conManagerList = new List<ConnectionManager>();
            ConnectionManager managerMonitorVehicle = new ConnectionManager(ucMonitorVehicle);
            managerMonitorVehicle.ManagerReceivedDataEvent += receiveData;
            return managerMonitorVehicle;
        }

        private ConnectionManager initPavilionEx1()
        {
            Building b = GlobalMapForShow.getBuildingByName("亭子（运输部）");
            Cab c = b.Cabs[0];
            UdpConnection ucPavilionEx1 = new UdpConnection(c.Ip, c.Port);
            conManagerList = new List<ConnectionManager>();
            ConnectionManager managerPavilionEx1 = new ConnectionManager(ucPavilionEx1);
            managerPavilionEx1.ManagerReceivedDataEvent += receiveData;
            return managerPavilionEx1;
        }
        private ConnectionManager initPavilionEx2()
        {
            Building b = GlobalMapForShow.getBuildingByName("亭子（新桥）");
            Cab c = b.Cabs[0];
            UdpConnection ucPavilionEx2 = new UdpConnection(c.Ip, c.Port);
            conManagerList = new List<ConnectionManager>();
            ConnectionManager managerPavilionEx2 = new ConnectionManager(ucPavilionEx2);
            managerPavilionEx2.ManagerReceivedDataEvent += receiveData;
            return managerPavilionEx2;
        }
        private ConnectionManager initPavilionInner()
        {
            Building b = GlobalMapForShow.getBuildingByName("亭子（内网）");
            Cab c = b.Cabs[0];
            UdpConnection ucPavilionInner = new UdpConnection(c.Ip, c.Port);
            conManagerList = new List<ConnectionManager>();
            ConnectionManager managerPavilionInner = new ConnectionManager(ucPavilionInner);
            managerPavilionInner.ManagerReceivedDataEvent += receiveData;
            return managerPavilionInner;
        }
        private ConnectionManager init2115Connection()
        {
            Building b = GlobalMapForShow.getBuildingByName("2115楼");
            ConnectionManager manager2115 = null;
            if (b != null)
            {
                List<Connection> cons = new List<Connection>();
                foreach (Device d in b.Cabs[0].Devices)
                {
                    COMConnection con = new COMConnection(d);
                    cons.Add(con);
                }
                manager2115 = new ConnectionManager(cons);
                manager2115.ManagerReceivedDataEvent += receiveData;
                manager2115.startConnections();
            }
            return manager2115;
        }

        /// <summary>
        ///    数据库连接
        /// </summary>
        /// <returns></returns>
        public bool InitialDBConnection()
        {
            dataOfDevice = new DBManager();
            string errorCode = "";
            dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
            return true;
        }

        /// <summary>
        /// 初始化队列和数据入库线程
        /// </summary>
        public void InitialThread()  
        {
            bq = new Queue<Device>(Constants.BlockQueueSize);
            new Thread(new ThreadStart(TakeDataFromQueueThread)).Start();  //启动从队列取数据的线程
        }

      /// <summary>
        /// 循环从队列里面取数据并存到数据库
      /// </summary>
        public void TakeDataFromQueueThread()  
        {
            while (!Constants.stopFlag)
            {
                if (bq.Count > 0)
                {
                    try
                    {
                        SaveDataToDataBase();
                        Thread.Sleep(Constants.inputAndOutputFromQueueInterval);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Log(false,"设备采集数据入库异常！",(int)ErrorCode.ERR_CODE.DATABASE_INSERT_ERR);
                        Alarm(ex.Message+DateTime.Now.ToString());
                    }
                }
                else
                {
                    //没有任务，休息3秒钟  
                    Thread.Sleep(Constants.getDataFromQueueInterval);
                }
            }  
        }
        /// <summary>
        /// 收到数据包之后进行解析，对二级发过来的数据包package进行解析
        /// 直连设备的数据，组好包之后形成package 也在这里解析
        /// </summary>
        /// <param name="data"></param>
       public void receiveData(string data)  
        {
            Console.WriteLine("收到数据:"+data);
             List<Box> boxes = new List<Box>();
            if(data==null||data.Equals(""))
            {
                return;
            }
            try
            {
                boxes = PackageWorker.unpack(data);
                Console.WriteLine("收到数据包中包含的设备："+boxes.Count);
            }
            catch(Exception e)
            {
                Alarm("收到数据包格式异常，无法解析！");
            }
            if (boxes == null || boxes.Count == 0)
            {
                return;
            }
            Device deviceInMap = null;
            Device deviceToChange = null;
            try
            {
                foreach (DeviceDataBox_Base item in boxes)
                {
                    if (item.className() != DeviceCommandEchoBox.classNameString) //控制命令单独处理
                    {
                        DeviceDataBox_Base tempItem = (DeviceDataBox_Base)item;
                        if (tempItem == null)
                        {
                            break;
                        }

                        deviceInMap = GlobalMapForShow.globalMapForDevice[tempItem.systemId + "_" + tempItem.devId];
                        deviceInMap.Value = item;
                        deviceInMap.Type = item.className();
                        deviceInMap.NowValue = tempItem.value;
                        deviceInMap.BuildingId = tempItem.systemId;
                        deviceInMap.CabId = tempItem.cabId;
                        deviceInMap.DeviceId = tempItem.devId;
                        deviceInMap.State = tempItem.state.ToString();

                        if(!deviceInMap.NowValue.Equals(""))
                        {
                            if (float.Parse(deviceInMap.NowValue) > deviceInMap.Highthreshold)
                            {
                                tempItem.state = "高报";
                                deviceInMap.State = "高报"  ;
                            }
                            else if (float.Parse(deviceInMap.NowValue) < deviceInMap.Lowthreshold)
                            {
                                tempItem.state = "低报" ;//DeviceDataBox_Base.State.L_Alert;
                                deviceInMap.State = "低报"; //DeviceDataBox_Base.State.L_Alert.ToString();
                            }
                        }
                        
                        //高低阈值被修改，将修改后的参数入库
                        if ((!tempItem.lowThreshold.Equals("") && deviceInMap.Lowthreshold != float.Parse(tempItem.lowThreshold)) ||
                            (!tempItem.highThreshold.Equals("") && deviceInMap.Highthreshold != float.Parse(tempItem.highThreshold))||
                            !tempItem.factor.Equals("") && deviceInMap.CorrectFactor != float.Parse(tempItem.factor))
                        {
                            if(!tempItem.lowThreshold.Equals(""))
                            {
                                 deviceInMap.Lowthreshold = float.Parse(tempItem.lowThreshold);
                            }
                            if(!tempItem.highThreshold.Equals(""))
                            {
                                deviceInMap.Highthreshold = float.Parse(tempItem.highThreshold);
                            }
                            if(!tempItem.factor.Equals(""))
                            {
                                deviceInMap.CorrectFactor = float.Parse(tempItem.factor);
                            }
                            if(dataOfDevice.UpdateDeviceInfo("deviceInfo", deviceInMap)==1) //修改成功
                            {
                                LogUtil.Log(true, deviceInMap.SubSystemName + " 设备 " + deviceInMap.Type+" 设备编号："+deviceInMap.DeviceId + 
                                    "高低阈值修改成功！当前高阈值："+deviceInMap.Highthreshold+",低阈值："+deviceInMap.Lowthreshold+",修正因子："+deviceInMap.CorrectFactor, (int)ErrorCode.ERR_CODE.OK);
                            }
                        }

                        deviceToChange = tempItem.fromBoxToDevice();
                        //收到状态不正常的数据时，触发警报，并把相应的cab和building的状态更改为相应的报警状态
                        if (tempItem.state != "Normal")
                        {
                            Alarm(deviceToChange);
                        }
                        //更改柜子的状态
                        GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].State =
                            GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].isStateNormal() ?"Normal" : "Err";

                        //更改监测点的状态
                        GlobalMapForShow.globalMapForBuiding[tempItem.systemId].State = GlobalMapForShow.globalMapForBuiding[tempItem.systemId].isStateNormal() ? "Normal" :"Err";

                        //遍历所有楼宇的状态，存在有一栋楼有异常状态 ，就报警或者维持报警状态，否则停止报警
                        if (GlobalMapForShow.isAllBuildingNormal())
                        {
                            alarmBuzzer(false);
                        }
                        else
                        {
                            alarmBuzzer(true);
                        }

                        bq.Enqueue(deviceToChange);  //将获取到的数据插入队列   
                    }
                }//end for 
            }
            catch (Exception e)
            {
                Alarm("收到数据包格式异常，无法解析！"+"("+DateTime.Now.ToString()+")");
            }
            finally
            {
                boxes = null;
                deviceInMap = null;
                dataChartUpdate();
            }
        }

        /// <summary>
       /// 对设备进行报警，产生报警信息
        /// </summary>
        /// <param name="d"></param>
       private void Alarm(Device d)
       {
           string msg =  d.GenerateAlarmMessage();
           dataOfDevice.InsertExceptionToDb("EXCEPTIONINFO", d, msg);
           AlarmMessage amsg = new AlarmMessage(msg, d);
           alarmMessage(amsg);
           amsg = null;
       }

        /// <summary>
        /// 将报警的字符串，显示到mappage 页面的下方
        /// </summary>
        /// <param name="alrm"></param>
       private void Alarm(String alrm)
       {
           AlarmMessage msg = new AlarmMessage(alrm + "(" + DateTime.Now.ToString() + ")");
           alarmMessage(msg);
           msg = null ;
       }

       /// <summary>
       /// 将数据存入数据库当中的三张表
       /// </summary>
       private void SaveDataToDataBase()
       {
           while (bq.Count > 0)
           {
               try
               {
                   //从队列中取出  
                   Device dataInDevice = bq.Dequeue();
                   int result = dataOfDevice.InsertDataToDb(DBHelper.NORMALDATATABLE + dataInDevice.BuildingId, dataInDevice);
                   int result2 = dataOfDevice.InsertDataToDb(DBHelper.TOTLEDATATABLE + dataInDevice.BuildingId, dataInDevice);  //插入到总表
                   int result3 = dataOfDevice.InsertDataToDb(DBHelper.TEMPDATATABLE + dataInDevice.BuildingId, dataInDevice);  //插入到temp表
                   dataInDevice = null;
                   Thread.Sleep(Constants.inputAndOutputFromQueueInterval);
               }
               catch (Exception ex)
               {
                   LogUtil.Log(false,ex.Message,(int)ErrorCode.ERR_CODE.DATABASE_INSERT_ERR);
               }
           }  
       }

        public String Str
        {
            get { return str; }
            set
            {
                str = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("str"));
                }
            }
        }


       //没用
        public static void sendCommand(String ip, String port, String data)
        {
            tcpConnection = new SocketConnection(ip, port);
            tcpConnection.Connect();
            Thread.Sleep(1000);
            tcpConnection.dataReceivedEvent += receiveCommandData;
            tcpConnection.SendCommandToServer(data);
            tempSendData = data;
        }

        //没用
        public static void receiveCommandData(string data)
        {
            List<Box> boxes = PackageWorker.unpack(data);
            foreach (Box item in boxes)
            {
                if (item.className() == DeviceCommandEchoBox.classNameString) //控制命令单独处理
                {
                    DeviceCommandEchoBox tempItem = (DeviceCommandEchoBox)item;
                    if (tcpConnection.ConnectState)
                    {

                        if (tempItem.code == "1")
                        {
                            MessageBox.Show("修改成功");
                        }
                        else
                        {
                            MessageBox.Show("修改失败");
                        }
                        tcpConnection.exit();
                    }
                }
            }
        }

        public static bool changeEmergencyState(int type) //type 0 表示正常状态，1表示应急状态
        {
            dataOfDevice.CloseConnection();
            string errorCode = "";
            int result = 0 ;
            switch(type){
                case 0:
                   result = (int)dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
                    break;
                case 1:
                   result = (int)dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.emergencyDb_name, ref errorCode);
                    break;
            }
            if (result == (int)ErrorCode.ERR_CODE.OK)
            {
                return true;
            }
            return false;

        }

    }

}
 