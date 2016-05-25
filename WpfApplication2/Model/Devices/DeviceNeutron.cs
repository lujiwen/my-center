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
   public class DeviceNeutron: Device,INotifyPropertyChanged
    {
        private double m_NeutronRate; //中子计数率
        public event PropertyChangedEventHandler PropertyChanged;
        DeviceDataBox_Neutron neutron_box;
        public DeviceNeutron(DeviceDataBox_Neutron b)
        {
            neutron_box = b;
            fromBoxToDevice((DeviceDataBox_Neutron)b);
            judgeState();
        }
        public DeviceNeutron( )
        {
        }
        public DeviceNeutron(OracleDataReader odr)
            : base(odr)
        {

        }
        public override void fromBoxToDevice(DeviceDataBox_Base box)
        {
            base.fromBoxToDevice(box);
            if (neutron_box.neutronRate != null && !neutron_box.neutronRate.Equals(""))
            {
                NeutronRate = double.Parse(neutron_box.neutronRate);
            }
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

        public override string GenerateAlarmMessage()
        {
            return base.GenerateAlarmMessage();
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + m_NeutronRate + 
                " ,  '" + DataUnit + "'" + ", " + "'" + State + "' )";
        }

        public override string GenerateSelectSql(string tablename, string start, string end)
        {
            return base.GenerateSelectSql(tablename, start, end);
        }
        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset1 = new List<DeviceData>();
            List<DeviceData> dataset2 = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d1 = new DeviceData();
                d1.VALUE1 = odr.GetFloat(5).ToString();
                d1.Time = odr.GetString(2);
                dataset1.Add(d1);
                d1 = null;
            }
            dataDictionary.Add("中子计数率", dataset1);
            return dataDictionary;
        }
    }
}
