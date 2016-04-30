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
    public class DeviceXH31253127 : Device, INotifyPropertyChanged
    {
        double doseNow;//实时值
        string doseNowforPresentation;//用于显示
        string devDataUnit;
        String safeColor;
        String devIsSafe;

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public DeviceXH31253127()
        {
           
        }

        public DeviceXH31253127(DeviceDataBox_Base b)
            :base(b)
        {

        }
        public DeviceXH31253127(OracleDataReader odr)
         :base(odr)
        {

        }
 

        public DeviceXH31253127(UInt32 id, UInt32 interfaceId, String type)
            //: base(id, interfaceId, type)
        {
            safeColor = "Black";
            DoseNowforPresentation ="0";
        }
        
        public double DoseNow
        {
            get { return doseNow; }
            set
            {
                doseNow = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("doseNow"));
                }
            }
        }
        public string DoseNowforPresentation
        {
            get { return doseNowforPresentation; }
            set
            {
                doseNowforPresentation = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("doseNowforPresentation"));
                }
            }
        }

        public string DevDataUnit
        {
            get { return devDataUnit; }
            set
            {
                devDataUnit = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("devDataUnit"));
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

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d = new DeviceData();
                d.VALUE1 = odr.GetString(5);
                d.Time = odr.GetString(2);
                dataset.Add(d);
                d = null;
            }
            dataDictionary.Add("XH31252127浓度", dataset);
            return dataDictionary;
        }

        public override string GenerateInsertSql(string tablename)
        {
            string[] values =  NowValue.Trim().Split(',');
          //  return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, VALUE2, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + values[0] + ", " + values[1] + ", " + "'" + DataUnit + "'" + ", " + "'" +  State + "' )";
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1 , UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + values[0]  + ", " + "'" + DataUnit + "'" + ", " + "'" + State + "' )";
        }
     
    }
}
