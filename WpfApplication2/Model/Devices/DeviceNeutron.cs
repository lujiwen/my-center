using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;

namespace Project208Home.Model
{
    class DeviceNeutron: Device,INotifyPropertyChanged
    {
        private double m_NeutronRate; //中子计数率
        public event PropertyChangedEventHandler PropertyChanged;

        //public DeviceNeutron(UInt32 localaddress, UInt32 uid): base(localaddress, uid)
        //{
        //}
        public DeviceNeutron( )
        {
        }
        public DeviceNeutron(OracleDataReader odr)
            :base(odr)
        {

        }
        public double NeutronRate
        {
            get { return m_NeutronRate; }
            set
            {
                m_NeutronRate = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("NeutronRate"));
                }
            }
        }
    }
}
