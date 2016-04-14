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
        public MainController()
        {
            InitialData();
            InitialConnection();
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
        
        private OracleDataReader readBuidingFromDb()
        {
            int errCode = 0 ;
            try{
                DBManager dbOfDevice = new DBManager();
                string errorCode = "";
                errCode = dbOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
                
                /////read buildings////
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

        private　OracleDataReader　readCabFromDb(DBManager dm,OracleDataReader odr)
        {
             try{
                string state = WpfApplication2.package.DeviceDataBox_Base.State.Normal.ToString();
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
                        string state = WpfApplication2.package.DeviceDataBox_Base.State.Normal.ToString();
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
                                    Device device = new Device("" + odr3.GetInt32(0), "" + odr3.GetInt32(2), building.SystemId, odr3.GetString(1), odr3.GetInt32(3), odr3.GetString(4), odr3.GetFloat(5), odr3.GetFloat(6), odr3.GetInt32(7), odr3.GetInt32(8), odr3.GetFloat(9), odr3.GetString(10), odr3.GetFloat(11), odr3.GetFloat(12), odr3.GetFloat(13), odr3.GetString(15), state);
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

        public void InitialConnection()  //初始化连接
        {
            UdpConnection uc = new UdpConnection();
            uc.dataReceivedEvent += receiveData;
            dataOfDevice = new DBManager();
            string errorCode = "";
            dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);


            /**
             * 发送控制命令到二级的调用方法
             * */
            //DeviceCommandBox dcb = new DeviceCommandBox();
            //dcb.load("deviceId","cabId","high","low","param1","factor");
            //List<Box> lb = new List<Box>();
            //lb.Add(dcb);
            //String data = PackageWorker.pack(lb);
            //sendCommand("192.168.0.105","6003",data);
        }

        public void InitialThread()  //初始化队列和线程
        {
            bq = new Queue<Device>(Constants.BlockQueueSize);
          
             new Thread(new ThreadStart(TakeDataFromQueueThread)).Start();  //启动从队列取数据的线程
        }
      
        public void TakeDataFromQueueThread()  //循环从队列里面取数据并存到数据库
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
                    }
                }
                else
                {
                    //没有任务，休息3秒钟  
                    Thread.Sleep(Constants.getDataFromQueueInterval);
                }
            }  
        }

       public void receiveData(string data)  //收到二级发送过来的数据后触发
        {
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
                foreach (Box item in boxes)
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
                                tempItem.state = DeviceDataBox_Base.State.H_Alert;
                                deviceInMap.State = DeviceDataBox_Base.State.H_Alert.ToString();
                            }
                            else if (float.Parse(deviceInMap.NowValue) < deviceInMap.Lowthreshold)
                            {
                                tempItem.state = DeviceDataBox_Base.State.L_Alert;
                                deviceInMap.State = DeviceDataBox_Base.State.L_Alert.ToString();
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
                        if (tempItem.state != DeviceDataBox_Base.State.Normal)
                        {
                            Alarm(deviceToChange);
                        }

                        GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].State =
                            GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].isStateNormal() ? DeviceDataBox_Base.State.Normal.ToString() : DeviceDataBox_Base.State.Alert.ToString();
                        Console.WriteLine(GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].State.ToString());
                        GlobalMapForShow.globalMapForBuiding[tempItem.systemId].State = GlobalMapForShow.globalMapForBuiding[tempItem.systemId].isStateNormal() ? DeviceDataBox_Base.State.Normal.ToString() : DeviceDataBox_Base.State.Alert.ToString();
                        Console.WriteLine(GlobalMapForShow.globalMapForBuiding[tempItem.systemId].State.ToString());
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

       private void Alarm(Device d)
       {
           string msg =  d.GenerateAlarmMessage();
           dataOfDevice.InsertExceptionToDb("EXCEPTIONINFO", d, msg);
           AlarmMessage amsg = new AlarmMessage(msg, d);
           alarmMessage(amsg);
           amsg = null;
       }

       private void Alarm(String alrm)
       {
           AlarmMessage msg = new AlarmMessage(alrm + "(" + DateTime.Now.ToString() + ")");
           alarmMessage(msg);
           msg = null ;
       }

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

       private void saveCommandToDataBase()
       {

       }

        private void Update(String s)
        {
            // Dispatcher.Invoke(DispatcherPriority.Normal,new DelegateStrUpdateEvent(dataReceive), s);
            Str = s;
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


        public static void sendCommand(String ip, String port, String data)
        {
            tcpConnection = new SocketConnection(ip, port);
            tcpConnection.Connect();
            Thread.Sleep(1000);
            tcpConnection.dataReceivedEvent += receiveCommandData;
            tcpConnection.SendCommandToServer(data);
            tempSendData = data;
        }


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
 