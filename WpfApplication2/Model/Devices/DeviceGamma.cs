using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;
using WpfApplication2.package;

namespace Project208Home.Model
{
    public class DeviceGamma: Device,INotifyPropertyChanged
    {
        private double m_gammaTotalDose; //伽马总剂量
        private double m_gammaDoseRate; //伽马剂量率

        public event PropertyChangedEventHandler PropertyChanged;

        DeviceDataBox_Gamma gamma_box;
        public DeviceGamma(DeviceDataBox_Gamma b)
        {
            gamma_box = b;
            fromBoxToDevice((DeviceDataBox_Base)b);
            judgeState();
        }
 
        public DeviceGamma(OracleDataReader odr)
            :base(odr)
        {

        }
        public DeviceGamma( )
        {
        }

        public override void fromBoxToDevice(DeviceDataBox_Base box)
        {
            base.fromBoxToDevice(box);
            if (gamma_box.GammaTotalDose != null && !gamma_box.GammaTotalDose.Equals(""))
            {
                GammaTotalDose = double.Parse(gamma_box.GammaTotalDose);
            }
            if (gamma_box.GammaDoseRate != null && !gamma_box.GammaDoseRate.Equals(""))
            {
                GammaDoseRate = double.Parse(gamma_box.GammaDoseRate);
            }
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

        public override string GenerateAlarmMessage()
        {
            return base.GenerateAlarmMessage();
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1,VALUE2, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + m_gammaTotalDose + ", " +
                m_gammaDoseRate +  " ,  '"  + DataUnit + "'" + ", " + "'" + State + "' )";
        }

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset1 = new List<DeviceData>();
            List<DeviceData> dataset2 = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d1 = new DeviceData();
                DeviceData d2 = new DeviceData();
                Console.WriteLine(odr.GetFloat(5));
                d1.VALUE1 = odr.GetFloat(5).ToString();
                d1.Time = odr.GetString(2);
                dataset1.Add(d1);
                d1.VALUE2 = odr.GetFloat(6).ToString();
                d1.Time = odr.GetString(2);
                dataset2.Add(d2);
                d1 = null;
                d2 = null;
            }
            dataDictionary.Add("伽马总剂量", dataset1);
            dataDictionary.Add("伽马剂量率", dataset2);
            return dataDictionary;
        }

    }
}
