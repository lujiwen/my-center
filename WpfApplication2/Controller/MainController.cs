using System;
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


namespace WpfApplication2.Controller
{
    public delegate void DataUpdatedEventHandler();
    public delegate void UpdateUI();
    public delegate void alarmEventHandler(AlarmMessage alarmMsg);
    public class MainController : INotifyPropertyChanged
    {
        public static WpfApplication2.Model.Db.DBManager dataOfDevice;
    
        public Configuration config;
        //public List<Device> devices;
        //public List<BaseConnection> connections;
        private SocketConnection sc;
        private String str;
        public event PropertyChangedEventHandler PropertyChanged;
        public event DataUpdatedEventHandler dataUpdate;
        public Queue<Device> bq;  //用于缓存从二级获得的数据的队列
        public static SocketConnection tcpConnection; //控制命令的连接
        public static String tempSendData;
        public event UpdateUI notifyUpdateUI;
        public event alarmEventHandler alarm;
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
            set { dataUpdate = value; }
            get { return dataUpdate; }
        }


        public void InitialData()  //初始化数据
        {
           
            String path = System.Environment.CurrentDirectory;
            String configFile = path + @"\app.config";
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", configFile);

            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            String flag = config.AppSettings.Settings["firstIn"].Value;
             if (flag.Equals("0")) //第一次进入创建数据表和sequence
             {
                DBHelper dbInitial = new DBHelper();
                //dbInitial.createUserAndGrant();
                //System.Threading.Thread.Sleep(3000);
                dbInitial.createAllTableAndSequence(0); //创建正常数据库
                Thread.Sleep(2000);
                //dbInitial.createAllTableAndSequence(1); //创建应急数据库
                AddUpdateAppSettings("firstIn", "1");
                flag = config.AppSettings.Settings["firstIn"].Value;
                Console.Write("after: "+flag);
             }

            DBManager dbOfDevice = new DBManager();
            string errorCode = "";
            try
            {
                  dbOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
            }
            catch(Exception e)
            {
                LogUtil.Log(1,e.ToString() , "数据库连接建立失败！");
                return;
            }
          
            String sql = "select * from buildinginfo";
            OracleDataReader odr = null;
            try
            {
                dbOfDevice.ReadDeviceInfomationFromDb(sql);
            }
            catch(Exception e)
            {
                LogUtil.Log(1,e.ToString(),"查询楼宇信息失败" );
                return;
            }

            while (odr.Read()) //找所有的building
            {
                string state = WpfApplication2.package.DeviceDataBox_Base.State.Normal.ToString();
                Building building = new Building("" + odr.GetInt32(0), odr.GetString(1), odr.GetString(2), odr.GetString(3), odr.GetFloat(4), odr.GetFloat(5), new List<Cab>(), state);
                String sql2 = "select * from cabinfo where buildingid = " + building.SystemId;
                OracleDataReader odr2 = null;
                try
                {
                    dbOfDevice.ReadDeviceInfomationFromDb(sql2);
                }
                catch (Exception e)
                {
                    LogUtil.Log(1, e.ToString(), "查询柜子信息失败！");
                    return;
                }
                   

                while (odr2.Read()) //找每个building对应的cab
                {
                    // public Cab(string cabId,string buildingId,string name, string office, string home, string ip,string port,List<DeviceDataBox_Comp> devices, string state)
                    Cab cab = new Cab("" + odr2.GetInt32(0), "" + odr2.GetInt32(1), "" + odr2.GetString(2), odr2.GetString(3), odr2.GetString(4), odr2.GetString(5), odr2.GetString(6), new List<Device>(), state);
                    string sql3 = "select * from deviceinfo where cabid = " + cab.CabId;

                    OracleDataReader odr3 = null;
                    try
                    {
                        dbOfDevice.ReadDeviceInfomationFromDb(sql3);
                    }
                    catch (Exception e)
                    {
                        LogUtil.Log(1, e.ToString(), "查询设备信息失败！");
                        return;
                    }
                    while (odr3.Read())
                    {
                        //public Device(string deviceId, string cabId, string buildingId, string type, int subSystemSerial, string subSystemName, float highthreshold, float lowthreshold, int devLocalAddress, int interfaceId,
                        //float correctFactor, string dataUnit, float inputArg1, float inputArg2, float inputArg3, string handleTypeInSystem, string state)
                        Device device = new Device("" + odr3.GetInt32(0), "" + odr3.GetInt32(2), building.SystemId, odr3.GetString(1), odr3.GetInt32(3), odr3.GetString(4), odr3.GetFloat(5), odr3.GetFloat(6), odr3.GetInt32(7), odr3.GetInt32(8),
                            odr3.GetFloat(9), odr3.GetString(10), odr3.GetFloat(11), odr3.GetFloat(12), odr3.GetFloat(13), odr3.GetString(15), state);
                        cab.Devices.Add(device);
                        GlobalMapForShow.globalMapForDevice.Add(building.SystemId + "_" + device.DeviceId, device);
                    }
                    GlobalMapForShow.globalMapForCab.Add(building.SystemId + "_" + cab.CabId, cab);
                    odr3.Close();
                    building.Cabs.Add(cab);

                }
                odr2.Close();
                GlobalMapForShow.globalMapForBuiding.Add(building.SystemId, building);
            }
            odr.Close();
            dbOfDevice.CloseConnection();
            //List<Device> devices = GlobalMapForShow.globalMapForCab["2_3"].Devices;
            //String result = "";
            //foreach (Device item in devices)
            //{
            //    result += item.SubSystemName + "\n";
            //}
            //MessageBox.Show(result);
            
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
          
         //    new Thread(new ThreadStart(TakeDataFromQueueThread)).Start();  //启动从队列取数据的线程
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
                        throw;
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
            //PackageGenerator pg = new PackageGenerator();
            //string anaData = pg.generatePackage();
            //Console.WriteLine("收到数据：" + data);
            List<Box> boxes = PackageWorker.unpack(data);
            foreach (Box item in boxes)
            {
                if (item.className() == DeviceCommandEchoBox.classNameString) //控制命令单独处理
                {
                    //if (tcpConnection.ConnectState)
                    //{
                    //    MessageBox.Show("修改成功");
                    //    saveCommandToDataBase();  //如果返回成功，则将下发命令的记录存到数据库
                    //    tcpConnection.exit();
                    //}
                }
                else
                {
                    DeviceDataBox_Base tempItem = (DeviceDataBox_Base)item;
                   
                    WpfApplication2.Model.Vo.Device deviceToChange = GlobalMapForShow.globalMapForDevice[tempItem.systemId + "_" + tempItem.devId];
                    deviceToChange.Value = item;
                    deviceToChange.Type = item.className();
                    deviceToChange.NowValue = tempItem.value;
                    deviceToChange.BuildingId = tempItem.systemId;
                    deviceToChange.CabId = tempItem.cabId;
                    deviceToChange.DeviceId = tempItem.devId;
                    deviceToChange.State = tempItem.state.ToString();
                    deviceToChange.Highthreshold = (float)Convert.ToDouble(tempItem.highThreshold);
                    deviceToChange.Lowthreshold = (float)Convert.ToDouble(tempItem.lowThreshold);

                    if (tempItem.state != DeviceDataBox_Base.State.Normal) //收到状态不正常的数据时，触发警报，并把相应的cab和building的状态更改为相应的报警状态
                    {
                        Alarm(deviceToChange);
                    }
                    GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].State = tempItem.state.ToString();
                    GlobalMapForShow.globalMapForBuiding["2"].State = "Normal";
                    GlobalMapForShow.globalMapForBuiding[tempItem.systemId].State = tempItem.state.ToString();
                    
               
                    /**
                     * 鲁继文这边像下面这么调应该就可以获得所有东西了
                     * */
                    //if (GlobalMapForShow.globalMapForDevice[tempItem.systemId + "_" + tempItem.devId].Type == "6517AB")
                    //{
                    //    DeviceDataBox_6517AB a = (DeviceDataBox_6517AB)deviceToChange.Value;
                    //}

                    if (deviceToChange.Lowthreshold != float.Parse(tempItem.lowThreshold) || deviceToChange.Highthreshold != float.Parse(tempItem.highThreshold))//高低阈值被修改，将修改后的参数入库
                    {
                        MessageBox.Show(deviceToChange.SubSystemName + " 设备" + deviceToChange.DeviceId + "高低阈值被修改");
                        deviceToChange.Lowthreshold = float.Parse(tempItem.lowThreshold);
                        deviceToChange.Highthreshold = float.Parse(tempItem.highThreshold);
                        deviceToChange.CorrectFactor = float.Parse(tempItem.factor);

                        dataOfDevice.UpdateDeviceInfo("deviceInfo", deviceToChange);
                    }
                    //if (!(float.Parse(tempItem.lowThreshold) <= float.Parse(tempItem.value)))  //低报，入异常数据表
                    //{
                    //    GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].State = WpfApplication2.Controller.DeviceDataBox_Base.State.Alert.ToString();
                    //    GlobalMapForShow.globalMapForBuiding[tempItem.systemId].State = WpfApplication2.Controller.DeviceDataBox_Base.State.Alert.ToString();
                    //}
                    //if (!(float.Parse(tempItem.value) <= float.Parse(tempItem.highThreshold)))//高报，入异常数据表
                    //{
                    //    GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].State = WpfApplication2.Controller.DeviceDataBox_Base.State.H_Alert.ToString();
                    //    GlobalMapForShow.globalMapForBuiding[tempItem.systemId].State = WpfApplication2.Controller.DeviceDataBox_Base.State.H_Alert.ToString();
                    //}
                    //if (tempItem.state != WpfApplication2.Controller.DeviceDataBox_Base.State.Normal)
                    //{
                    //    GlobalMapForShow.globalMapForCab[tempItem.systemId + "_" + tempItem.cabId].State = tempItem.state.ToString();
                    //    GlobalMapForShow.globalMapForBuiding[tempItem.systemId].State = tempItem.state.ToString();
                    //}
                    //deviceToChange.NowValue = tempItem.value;
                    //deviceToChange.DataUnit = tempItem.unit;
                    //deviceToChange.State = tempItem.state.ToString();
                    //deviceToChange.Lowthreshold = float.Parse(tempItem.lowThreshold);
                    //deviceToChange.Highthreshold = float.Parse(tempItem.highThreshold);
                    //deviceToChange.CorrectFactor = float.Parse(tempItem.factor);
                    //if (item.className() == DeviceDataBox_6517AB.classNameString)  //如果是6517AB，则添加电压
                    //{
                    //    deviceToChange.NowValue += ";" + ((DeviceDataBox_6517AB)item).voltValue;
                    //    deviceToChange.DataUnit += ";" + "V";

                    //}
                    //if (item.className() == DeviceDataBox_Quality.classNameString) //如果是质量流量计，则添加累计值和累计单位
                    //{
                    //    deviceToChange.NowValue += ";" + ((DeviceDataBox_Quality)item).sumValue;
                    //    deviceToChange.DataUnit += ";" + ((DeviceDataBox_Quality)item).sumUnit;
                    //}
                    bq.Enqueue(deviceToChange);  //将获取到的数据插入队列   
                }
            }
           // notifyUpdateUI();
        }

       private void Alarm(Device d)
       {
           String alertInfomation = "";
           if (d.State.ToString().Equals(DeviceDataBox_Base.State.Alert))
           {
               alertInfomation = "当前值： " + d.NowValue + "低于正常值";
           }
           else if (d.State.ToString().Equals(DeviceDataBox_Base.State.H_Alert))
           {
               alertInfomation = "当前值： " + d.NowValue + "高于正常值";
           }
           else if (d.State.ToString().Equals(DeviceDataBox_Base.State.Fault))
           {
               alertInfomation = "当前值出错 " ;
           }
           String msg = d.NowValue + "," + d.DeviceId + "," + d.BuildingId + "," + d.CabId + " ," + alertInfomation + DateTime.Now;
           dataOfDevice.InsertExceptionToDb("EXCEPTIONINFO", d, msg);
           alarm(new AlarmMessage(msg,new DateTime(),d));
           
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
                   Thread.Sleep(Constants.inputAndOutputFromQueueInterval);
               }
               catch (Exception ex)
               {
                   throw;
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

        public static void changeEmergencyState(int type) //type 0 表示正常状态，1表示应急状态
        {
            dataOfDevice.CloseConnection();
            string errorCode = "";
            switch(type){
                case 0:
                    dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.db_name, ref errorCode);
                    break;
                case 1:
                    dataOfDevice.OpenConnection(DBHelper.db_userName, DBHelper.db_userPassWord, DBHelper.db_ip, DBHelper.db_port, DBHelper.emergencyDb_name, ref errorCode);
                    break;
            }
            

        }

    }

}
