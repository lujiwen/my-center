using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controller;
using System.ComponentModel;
using WpfApplication2.package;
using WpfApplication2.Util;
using System.Data.OracleClient;

namespace WpfApplication2.Model.Vo
{
    public class Device : INotifyPropertyChanged
    {
        private string deviceId;
        private string cabId;
        private string buildingId;
       
        private int subSystemSerial;
        private string subSystemName;
        private float highthreshold;
        private float lowthreshold;
        private int devLocalAddress;
        private int interfaceId;
        private float correctFactor;
        private string dataUnit;
        private float inputArg1;
        private float inputArg2;
        private float inputArg3;
        private float inputArg4;
        private float inputArg5;
        private float inputArg6;
        private float inputArg7;
        private float inputArg8;
        private string handleTypeInSystem;
        private string state;
        private string nowValue;
        private List<string> labels = new List<string>();
        public  bool showCurve;

        /**
         * 一下两项用于数据交互
         * */
        private DeviceDataBox_Base value;
        private string type;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// lhs亭子的设备类 所需的属性
        /// </summary>
        public String devPort = " ";
        public String devIp = " ";
        public String devType = " ";  // 子类赋值
        public  Device DeviceInMap;
  /// <summary>
  ///
  /// </summary>
        public Device(DeviceDataBox_Base b)
        {
            fromBoxToDevice(b);
            judgeState();
        }
        public Device()
        {
 
        }
        //string D_ID, string TYPE, string CABID, string SUBSYSTEMSERIAL, string SUBSYSTEMNAME, int HIGHTHRESHOLD, string LOWTHRESHOLD, float DEVLOCALADDRESS, float INTERFACEID, int CORRECTFACTOR, int DATAUNIT, float INPUTARG1, string INPUTARG2, float INPUTARG3, float BUILDINGID, float HANDLETYPEINSYSTEM
        
        public Device(OracleDataReader odr)
        {
            deviceId = odr.GetInt32(0).ToString();
            type = odr.GetString(1);
            cabId = odr.GetInt32(2).ToString();
            subSystemSerial = odr.GetInt32(3);
            subSystemName = odr.GetString(4);
            highthreshold = odr.GetFloat(5);
            lowthreshold = odr.GetFloat(6);
            devLocalAddress = odr.GetInt32(7);
            interfaceId = odr.GetInt32(8);
            correctFactor = odr.GetFloat(9);
            dataUnit = odr.GetString(10);
            inputArg1 = odr.GetFloat(11);
            inputArg2 = odr.GetFloat(12);
            inputArg3 = odr.GetFloat(13);
            buildingId = odr.GetInt32(14).ToString();
            handleTypeInSystem = odr.GetString(15);
            state = "Normal";
            showCurve = true;
        }

        public Device(string deviceId, string cabId, string buildingId, string type, int subSystemSerial, string subSystemName, float highthreshold, float lowthreshold, int devLocalAddress, int interfaceId,
            float correctFactor, string dataUnit, float inputArg1, float inputArg2, float inputArg3, string handleTypeInSystem, string state)
        {
            this.deviceId = deviceId;
            this.cabId = cabId;
            this.buildingId = buildingId;
            this.type = type;
            this.subSystemSerial = subSystemSerial;
            this.subSystemName = subSystemName;
            this.highthreshold = highthreshold;
            this.lowthreshold = lowthreshold;
            this.devLocalAddress = devLocalAddress;
            this.interfaceId = interfaceId;
            this.correctFactor = correctFactor;
            this.dataUnit = dataUnit;
            this.inputArg1 = inputArg1;
            this.inputArg2 = inputArg2;
            this.inputArg3 = inputArg3;
            this.handleTypeInSystem = handleTypeInSystem;
            this.state = state;
        }

        public virtual void fromBoxToDevice(DeviceDataBox_Base box)
        {
            DeviceInMap = GlobalMapForShow.globalMapForDevice[box.systemId+"_"+box.devId];
            nowValue = box.value;
            state = box.state.ToString();
            Value = box;
            Type = box.className();
            BuildingId = box.systemId;
            CabId = box.cabId;
            DeviceId = box.devId;
            State = box.state.ToString();
            handleTypeInSystem = DeviceInMap.handleTypeInSystem;
            if (!box.highThreshold.Equals(""))
            {
                Highthreshold = float.Parse(box.highThreshold);
            }
            else
            {
                Highthreshold = DeviceInMap.Highthreshold;
            }

            if (!box.lowThreshold.Equals(""))
            {
                Lowthreshold = float.Parse(box.lowThreshold);
            }
            else
            {
                Lowthreshold = DeviceInMap.Lowthreshold;
            }
            if (!box.CorrectFactor.Equals(""))
            {
                CorrectFactor = float.Parse(box.CorrectFactor);
            }
            else
            {
                CorrectFactor = DeviceInMap.correctFactor;
            }
        }

        public virtual void judgeState()
        {
            if (NowValue!=null&&!NowValue.Equals(""))
            if (float.Parse(NowValue) > Highthreshold)
            {
                State = DeviceDataBox_Base.State.H_Alert.ToString();
            }
            else if (float.Parse(NowValue) < Lowthreshold)
            {
                State = DeviceDataBox_Base.State.L_Alert.ToString();
            }
        }

        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }

        public string BuildingId
        {
            get { return buildingId; }
            set { buildingId = value; }
        }
      
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        

        public int SubSystemSerial
        {
            get { return subSystemSerial; }
            set { subSystemSerial = value; }
        }
       

        public string SubSystemName
        {
            get { return subSystemName; }
            set { subSystemName = value; }
        }
       

        public float Highthreshold
        {
            get { return highthreshold; }
            set { highthreshold = value; }
        }
      

        public float Lowthreshold
        {
            get { return lowthreshold; }
            set { lowthreshold = value; }
        }
        

        public int DevLocalAddress
        {
            get { return devLocalAddress; }
            set { devLocalAddress = value; }
        }
        

        public int InterfaceId
        {
            get { return interfaceId; }
            set { interfaceId = value; }
        }
      

        public float CorrectFactor
        {
            get { return correctFactor; }
            set { correctFactor = value; }
        }
        

        public string DataUnit
        {
            get { return dataUnit; }
            set { dataUnit = value; }
        }
       

        public float InputArg1
        {
            get { return inputArg1; }
            set { inputArg1 = value; }
        }
        public float InputArg2
        {
            get { return inputArg2; }
            set { inputArg2 = value; }
        }
          public float InputArg3
        {
            get { return inputArg3; }
            set { inputArg3 = value; }
        }
          public float InputArg4
          {
              get { return inputArg4; }
              set { inputArg4 = value; }
          }
          public float InputArg5
          {
              get { return inputArg5; }
              set { inputArg5 = value; }
          }
          public float InputArg6
          {
              get { return inputArg6; }
              set { inputArg6 = value; }
          }
          public float InputArg7
          {
              get { return inputArg7; }
              set { inputArg7 = value; }
          }
          public float InputArg8
          {
              get { return inputArg8; }
              set { inputArg8 = value; }
          }
          
      
        public string HandleTypeInSystem
        {
            get { return handleTypeInSystem; }
            set { handleTypeInSystem = value; }
        }
        
        public string State
        {
            get { return state; }
            set { 
                state = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("State"));
                }
            }
        }
      
        public string CabId
        {
            get { return cabId; }
            set { cabId = value; }
        }

        public string NowValue
        {
            get { return nowValue; }
            set { 
                nowValue = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NowValue"));
                }
            }
        }
 
        //2115房间经过RF1000后的数据格式是否正确
        public virtual bool isDataRight(byte[] flowBytes, int len)
        {
            bool dataright = true;
            return dataright;
        }
        //2115房间经过RF1000后的修改参数的确认信息是否是否正确
        public virtual bool isParaSetRight(byte[] paraBytes)
        {
            bool parasetright = true;
            //判断数据包长度是否为0x05，否则设置为错误

            return parasetright;
        }

        /// <summary>
        /// 亭子数据解析
        /// </summary>
        /// <param name="flowBytes"> 原始字节流 </param>
        public virtual void AnalysisPavilionData(byte[] flowBytes, int len)
        {
        }
        /// <summary>
        /// 生成插入数据的sql
        /// </summary>
        /// <returns></returns>
        public virtual String getHistoryDataSql()
        {
            return "";
        }

        /// <summary>
        /// 初始化连接，循环扫描 请求数据 ,
        /// </summary>
        /// <returns></returns>
        public virtual void doWork()
        {


        }
        /// <summary>
        /// 亭子设备 数据或参数读取命令生成。
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToReadDataCommand()
        {
            return null;
        }

        /// <summary>
        /// 读参数命令
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] ToReadParaCommands()
        {
            return null;
        }
        /// <summary>
        /// 生成设置参数命令
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] ToSetParaCommands()
        {
            return null;
        }
        /// <summary>
        /// 更改当前设备参数的值
        /// </summary>
        public virtual void ChangeDevPara(float[] values)
        {
            return;
        }
 
        public DeviceDataBox_Base Value
        {
            get {
                return this.value; 
            }
            set { 
                this.value = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        public virtual string GenerateInsertSql(string tablename) 
         {
             return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " +  DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" +  DataUnit + "'" + ", " + "'" +  State + "' )";
         }

        public virtual string GenerateSelectSql(string tablename, string start, string end)
        {
            return "select * from devicedata_" + this.buildingId + " where devid = " + deviceId + " and DATATIME between " + start + " and " + end; 
        }


        public delegate string ReadDatabaseDelegate();
        public virtual Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
          //  ReadDatabaseDelegate readDatabase = new ReadDatabaseDelegate(OracleDataReader odr);
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d = new DeviceData();
                d.VALUE1 = odr.GetFloat(5);
                d.Time = odr.GetString(2);
                dataset.Add(d);
                d = null;
            }
            dataDictionary.Add("nowValue", dataset);
            return dataDictionary;
        }

        public virtual string GenerateAlarmMessage()
        {
            String alertInfomation = "";
            if (State.ToString().Equals(DeviceDataBox_Base.State.L_Alert.ToString()))
            {
                alertInfomation = "当前值： \"" + NowValue + "\" 低于正常值";
            }
            else if (State.ToString().Equals(DeviceDataBox_Base.State.H_Alert.ToString()))
            {
                alertInfomation = "当前值： \"" + NowValue + "\"高于正常值";
            }
            else if (State.ToString().Equals(DeviceDataBox_Base.State.Fault.ToString()))
            {
                alertInfomation = "当前值出错 ";
            }

            String msg = GlobalMapForShow.globalMapForBuiding[BuildingId].Name + " 监测点" + "," +
                GlobalMapForShow.globalMapForCab[BuildingId + "_" + CabId].Name + " ," +
                GlobalMapForShow.globalMapForDevice[BuildingId + "_" + DeviceId].SubSystemName + alertInfomation +
                "  当前值为：\"" + NowValue + "\" (" + DateTime.Now.ToString() + ")";
            return msg;
        }
    }
}
