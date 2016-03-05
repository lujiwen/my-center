using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.package;
using System.ComponentModel;
using WpfApplication2.Util;

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
    }
}
