using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.package;
using System.ComponentModel;
using WpfApplication2.Util;
using System.Data.OracleClient;

namespace WpfApplication2.Model.Vo
{
    public class Cab : INotifyPropertyChanged
    {
        private string cabId;
        private string buildingId;
        private string name;
        private string office;
        private string home;
        private string ip;
        private string port;
        private List<Device> devices;  //这里面的DeviceDataBox_Comp直接用的PackageWorker里面以前薛鹏飞写好的
        private string state;
        public event PropertyChangedEventHandler PropertyChanged;
        private String typeInSystem;//利用它进行对应工艺流程图创建
     
         public Cab()
        {
            name = "";
            office = "";
            cabId = "-1";
            buildingId = "-1";
            devices = new List<Device>();
            state = WpfApplication2.package.DeviceDataBox_Base.State.Normal.ToString();
        }

         public Cab(string cabId, string buildingId, string name, string office, string home, string ip, string port, List<Device> devices, string state)
        {
            this.cabId = cabId;
            this.buildingId = buildingId;
            this.name = name;
            this.office = office;
            this.home = home;
            this.ip = ip;
            this.port = port;
            this.devices = devices;
            this.state = state;
            typeInSystem = GlobalMapForShow.cabId_typeInSystem[buildingId +"_"+cabId];

        }
         public Cab(OracleDataReader odr)
         {
             
         }

         public virtual string GenerateSelectSql(string start,string end)
         {
             return "SELECT *  from DEVICEDATA_"+buildingId +" where DEVID in (SELECT D_ID from DEVICEINFO where CABID="+cabId+") ";
         }
         public virtual Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
         {
             Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
              foreach (Device d in devices)
              {
                  Dictionary<string, List<DeviceData>> tmp = new Dictionary<string, List<DeviceData>>();
                  tmp = d.getHistoryDataSet(odr);
                  foreach(var item in tmp)
                  {
                      dataDictionary.Add(item.Key+d.DeviceId,item.Value);
                  }
                  tmp = null;
              }
             return dataDictionary;
         }
         public String TypeInSystem
         {
             get { return typeInSystem; }
             set { typeInSystem = value; }
         }

         public string BuildingId
         {
             get { return buildingId; }
             set { buildingId = value; }
         }

         public string Home
         {
             get { return home; }
             set { home = value; }
         }

         public string Ip
         {
             get { return ip; }
             set { ip = value; }
         }

         public string Port
         {
             get { return port; }
             set { port = value; }
         }

        public string Name
        {
            get { return name; }
            set { name = value; }  
        }

        public string Office
        {
            get { return office; }
            set { office = value; }
        }

        public string CabId
        {
            get { return cabId; }
            set { cabId = value; }
        }

        public List<Device> Devices
        {
            get { return devices; }
            set { devices = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Devices"));
                }    
            }
        }

        public void updateAllDevice()
        {
            foreach (Device d in devices)
            {
                Device tmpDevice = GlobalMapForShow.globalMapForDevice[buildingId + "_" + d.DeviceId];
                tmpDevice.IsUpdate = true;
            }
        }
        public void unUpdateAllDevice()
        {
            foreach (Device d in devices)
            {
                Device tmpDevice = GlobalMapForShow.globalMapForDevice[buildingId + "_" + d.DeviceId];
                tmpDevice.IsUpdate = false ;
            }
        }

        public string State
        {
            get { return state; }
            set { state = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("State"));
                    }
              }
        }

        /// <summary>
        /// 该柜子里面的设备是不是都完全更新了
        /// </summary>
        /// <returns></returns>
        public bool isAllUpdate()
        {
            foreach (Device d in devices)
            {
                Device tmpDevice = GlobalMapForShow.globalMapForDevice[buildingId + "_" + d.DeviceId];
                if (!tmpDevice.IsUpdate)
                {
                    return false;
                }
            }
            return true;
        }
        public bool isStateNormal()
        {
           foreach(Device d in devices)
           {
               if(!d.State.Equals("Normal"))
               {
                   return false;
               }
           }
            return true;
        }
    }
}
