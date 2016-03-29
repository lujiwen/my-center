using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.package;
using System.ComponentModel;

namespace WpfApplication2.Model.Vo
{
    public class Building : INotifyPropertyChanged
    {
        private string systemId;
        private string name;
        private string office;
        private string location;
        private double lat;
        private double lng;
        private string manager;
        private List<Cab> cabs;
        private string state;
        public event PropertyChangedEventHandler PropertyChanged;
        public Building()
        {
            name = "";
            systemId = "-1";
            cabs = new List<Cab>();
            state = WpfApplication2.package.DeviceDataBox_Base.State.Normal.ToString();
        }
        
        public Building(string systemId, string name, string office, string location, double lat, double lng, List<Cab> cabs, string state,string mng)
        {
            this.systemId = systemId;
            this.name = name;
            this.office = office;
            this.location = location;
            this.lat = lat;
            this.lng = lng;
            this.cabs = cabs;
            this.state = state;
            this.manager = mng;
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

        public string Location
        {
            get { return location; }
            set
            {
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Location"));
                }

                location = value;
            }
        }

        public double Lat
        {
            get { return lat; }
            set
            {
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Lat"));
                }

                lat = value;
            }
        }

        public double Lng
        {
            get { return lng; }
            set
            {
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Lng"));
                }
                lng = value;
            }
        }

        public string Manager
        {
            get { return manager; }
            set { manager = value; }
        }

        public string SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }

        public List<Cab> Cabs
        {
            get { return cabs; }
            set { cabs = value; }
        }
        public string State
        {
            get { return state; }
            set
            {
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("State"));
                }
                state = value;
            }
        }



    }
}
