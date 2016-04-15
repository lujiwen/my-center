using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Project208Home.Model;
using WpfApplication2.Model.Vo;
using WpfApplication2.package;
using System.Data.OracleClient;

namespace Project208Home.Model
{
    public class DeviceQuality : Device, INotifyPropertyChanged
    {
        double doseNow;//实时值
        string nowDataUnit;//实时值单位
        double doseSum;//累计值
        string sumDataUnit;//累积值单位
        double doseFull;//满量程
        string devState;//状态
        String safeColor;
        String devIsSafe;
        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;
        private DeviceDataBox_Quality box;

        public DeviceQuality(DeviceDataBox_Base b)
            :base(b)
        {
           // box = (DeviceDataBox_Quality);

        }
        public DeviceQuality(OracleDataReader odr)
            :base(odr)
        {

        }
 
        public double DoseNow
        {
            get { return doseNow; }
            set
            {
                doseNow = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DoseNow"));
                }
            }
        }
        public string NowDataUnit
        {
            get { return nowDataUnit; }
            set
            {
                nowDataUnit = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("nowDataUnit"));
                }
            }
        }
        public double DoseSum
        {
            get { return doseSum; }
            set
            {
                doseSum = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DoseSum"));
                }
            }
        }
        public string SumDataUnit
        {
            get { return sumDataUnit; }
            set
            {
                sumDataUnit = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("sumDataUnit"));
                }
            }
        }
        public String SafeColor
        {
            get
            {
                return safeColor;
            }
            set
            {
                safeColor = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("safeColor"));
                }
            }
        }

        public String DevIsSafe
        {
            get
            {
                return devIsSafe;
            }
            set
            {
                devIsSafe = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("devIsSafe"));
                }
            }
        }
        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" + DataUnit + "'" + ", " + "'" + State + "' )";
        }

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
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
            dataDictionary.Add("质量流量计浓度", dataset);
            return dataDictionary;
        }
    }
}
