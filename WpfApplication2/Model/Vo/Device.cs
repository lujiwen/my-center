using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controller;
using System.ComponentModel;
using WpfApplication2.package;

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
  ///
  /// </summary>
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
         //   return "";//"INSERT INTO " + tablename + " VALUES(" + "deviceData_" + BuildingId + "_sequence" + ".nextval" + ", " + value.DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + value.NowValue + ", " + "'" + value.State + "'" + ", " + "'" + value.DataUnit + "'" + ", " + "1" + ")"; ;
            return "INSERT INTO " + tablename + " VALUES(" + "deviceData_" + BuildingId + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" + State + "'" + ", " + "'" + DataUnit + "'" + ", " + "1" + ")"; 
        }
    }
}
