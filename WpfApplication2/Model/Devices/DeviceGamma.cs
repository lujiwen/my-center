using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;

namespace Project208Home.Model
{
    public class DeviceGamma: Device,INotifyPropertyChanged
    {
        private double m_gammaTotalDose; //伽马总剂量
        private double m_gammaDoseRate; //伽马剂量率

        public event PropertyChangedEventHandler PropertyChanged;

        //public DeviceGamma(UInt32 localaddress, UInt32 uid)
        //    : base(localaddress, uid)
        //{
        //}
        public DeviceGamma(OracleDataReader odr)
            :base(odr)
        {

        }
        public DeviceGamma( )
        {
        }
        public double GammaTotalDose
        {
            get { return m_gammaTotalDose; }
            set
            {
                m_gammaTotalDose = value;
                if (PropertyChanged != null)
                {

                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("GammaTotalDose"));
                }
            }
        }

        public double GammaDoseRate
        {
            get { return m_gammaDoseRate; }
            set
            {
                m_gammaDoseRate = value;
                if (PropertyChanged != null)
                {

                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("GammaDoseRate"));
                }
            }
        }
    }
}
