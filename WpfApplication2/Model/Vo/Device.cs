using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controller;
using System.ComponentModel;
using WpfApplication2.package;
using WpfApplication2.Util;

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
        private string handleTypeInSystem;
        private string state;
        private string nowValue;
        private List<string> labels = new List<string>();
        
        /**
         * 一下两项用于数据交互
         * */
        private Box value;
        private string type;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// lhs亭子的设备类 所需的属性
        /// </summary>
        public String devPort = " ";
        public String devIp = " ";
        public String devType = " ";  // 子类赋值
       
  /// <summary>
  ///
  /// </summary>
        public Device(DeviceDataBox_Base b,Device deviceInMap)
        {
            fromBoxToDevice(b,deviceInMap);
            judgeState();
        }
        public Device()
        {
    
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

        public virtual void fromBoxToDevice(DeviceDataBox_Base box,Device mapDevice)
        {
            nowValue = box.value;
            state = box.state.ToString();
            Value = box;
            Type = box.className();
            BuildingId = box.systemId;
            CabId = box.cabId;
            DeviceId = box.devId;
            State = box.state.ToString();
            handleTypeInSystem = mapDevice.handleTypeInSystem;
            if (!box.highThreshold.Equals(""))
            {
                Highthreshold = float.Parse(box.highThreshold);
            }
            else
            {
                Highthreshold = mapDevice.Highthreshold;
            }

            if (!box.lowThreshold.Equals(""))
            {
                Lowthreshold = float.Parse(box.lowThreshold);
            }
            else
            {
                Lowthreshold = mapDevice.Lowthreshold;
            }
            if (!box.CorrectFactor.Equals(""))
            {
                CorrectFactor = float.Parse(box.CorrectFactor);
            }
            else
            {
                CorrectFactor = mapDevice.correctFactor;
            }
        }

        public virtual void judgeState()
        {
            if (float.Parse(NowValue) > Highthreshold)
            {
                //state = DeviceDataBox_Base.State.H_Alert;
                State = DeviceDataBox_Base.State.H_Alert.ToString();
            }
            else if (float.Parse(NowValue) < Lowthreshold)
            {
                //tempItem.state = DeviceDataBox_Base.State.L_Alert;
                State = DeviceDataBox_Base.State.L_Alert.ToString();
            }
        }

        public void addLabel(string label)
        {
            labels.Add(label);
        }
        public List<string> getLabels()
        {
            return labels;
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
        public virtual void setDevState(string state)
        {
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
        /// <summary>
        /// 生成阿里云中转更新数据的sql
        /// </summary>
        /// <returns></returns>
        public virtual void getAliyunUpdateStr()
        {
            return;
        }
        public Box Value
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

        public virtual string GenerateSql(string tablename) 
         {
             return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " +  DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" +  DataUnit + "'" + ", " + "'" +  State + "' )";
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
