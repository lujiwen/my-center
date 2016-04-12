using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using Project208Home.Model;
using System.Windows.Threading;
using WpfApplication2.Model.Vo;

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

        public override string GenerateSql(string tablename)
        {
            return "INSERT INTO " + tablename + " VALUES(" + "deviceData_" + BuildingId + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" + State + "'" + ", " + "'" + DataUnit + "'" + ", " + "1" + ")"; 
        }
    }
}
