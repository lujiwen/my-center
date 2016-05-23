using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using Project208Home.Model;
using System.Windows.Threading;
using WpfApplication2.Model.Vo;
using WpfApplication2.package;
using System.Data.OracleClient;

namespace Project208Home.Model
{
    public delegate void Dev6517ParaUpdatedEvent(Object[] values);
    public class Device6517AB : Device , INotifyPropertyChanged
    {
        double doseNow;//实时值
        string doseNowforPresentation;//用于显示
        string devDataUnit;//数据单位
        String safeColor;
        String devIsSafe;
        //用于界面显示的参数更新
        private Dev6517ParaUpdatedEvent devParaUpdatedEvent;

        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public Device6517AB()
        {
           // InitComsLength = initCommands.Length;
        }
        public Device6517AB(OracleDataReader odr)
            :base(odr)
        {

        }
        public Device6517AB(string deviceId, string cabId, string buildingId, string type, int subSystemSerial, string subSystemName, float highthreshold, float lowthreshold, int devLocalAddress, int interfaceId,
    float correctFactor, string dataUnit, float inputArg1, float inputArg2, float inputArg3, string handleTypeInSystem, string state)
        {

        }
        public Device6517AB(DeviceDataBox_Base b)
            :base(b )
        {

        }

        public Dev6517ParaUpdatedEvent DevParaUpdatedEvent
        {
            get { return devParaUpdatedEvent; }
            set { devParaUpdatedEvent = value; }
        }
        public Device6517AB(UInt32 id, UInt32 interfaceId, String type)
        {
            safeColor = "Black";
            //this.DevId = id;
            //this.InterfaceId = interfaceId;
            //this.DevType = type;
            //InitComsLength = initCommands.Length;
            DoseNowforPresentation = "30";
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
            dataDictionary.Add("浓度", dataset);
            return dataDictionary;
        }

        private Double getDoubleFromBytes(String str)
        {
            string strP = resolveP(str);//实数部分
            string strR = resolveR(str);//幂部分
            double r = double.Parse(strR); ;
            double p = double.Parse(strP);
            double pow = Math.Pow(10, r);
            double realData = p * pow;
            return realData;
        }

        /// <summary>
        /// 获取实数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string resolveP(string str)
        {
            try
            {
                int indexE = str.IndexOf('E');
                return str.Substring(0, indexE);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取幂数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string resolveR(string str)
        {
            try
            {
                int indexE = str.IndexOf('E');
                return str.Substring(indexE + 1, 3);
            }
            catch
            {
                return null;
            }
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean updateCorrectValue(double value)
        {
            Boolean updateSuccedd = false;
            if (CorrectFactor == value)//修改成功
            {
                updateSuccedd = true;
            }
            else
            {
                //CorrectFactor = value;
            }

            return updateSuccedd;
        }
        public double DoseNow
        {
            get { return doseNow; }
            set { doseNow = value; }
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
        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" + DataUnit + "'" + ", " + "'" + State + "' )";
        }
    }
}
