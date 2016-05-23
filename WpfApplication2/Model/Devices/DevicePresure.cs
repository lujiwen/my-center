using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Project208Home.Model;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;

namespace Project208Home.Model
{
    public class DevicePresure : Device , INotifyPropertyChanged
    {
        float doseNow;//实时值
        string devDataUnit;
        String safeColor;
        String devIsSafe;
        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public DevicePresure()
        {
        }
        public DevicePresure(OracleDataReader odr)
            :base(odr)
        {

        }
        public DevicePresure(UInt32 id, UInt32 interfaceId, String type)
            //: base(id, interfaceId, type)
        {
            safeColor = "Black";
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="datas"></param>
        //public override void AnalysisData(Byte[] datas)
        //{
        //    判断数据是否符合要求
        //    if (datas != null && datas.Length == 2)
        //    {
        //        Byte[] ai = { datas[0], datas[1] };
        //        DoseNow = (float)BitConverter.ToInt16(ai, 0);
        //        DataIsUpdate = true;
        //    }
        //    //实时曲线更新
        //    if (RealTimeUpdate)
        //    {
        //        Object[] values = new Object[2];
        //        values[0] = DevId;
        //        values[1] = doseNow;
        //        NewMonitorNowData(values);
        //    }
        //}
        /// <summary>
        /// 获取插入历史数据的sql
        /// </summary>
        /// <returns></returns>
        //public override string getHistorySql(DateTime datareceiveTime)
        //{
        //    String sql = "";
        //    if (DataIsUpdate)
        //    {
        //        String[] colums = { "DevId", "DataTime", "NowValue" };
        //        Object[] values = { DevId, "'" + datareceiveTime.ToString() + "'", doseNow };
        //        sql = DBHelpler.getInsertCommands("devicehistorydata", colums, values);
        //    }
        //    return sql;
        //}
        /// <summary>
        /// 获取发送至远端的数据包
        /// </summary>
        /// <returns></returns>
        //public override Box getCommonDataPack()
        //{
        //    DeviceDataBox_Comp databox = new DeviceDataBox_Comp();
        //    //状态要根据实际情况来设置
        //    if (DataIsUpdate)
        //    {
        //        databox.load("2", CabId.ToString(), DevId.ToString(),
        //            State.Normal, DoseNow.ToString(), DevDataUnit, "", "");
        //    }
        //    return databox;
        //}
        public float DoseNow
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
        public static string GenerateSql(Device d, string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + d.DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + d.NowValue + ", " + "'" + d.DataUnit + "'" + ", " + "'" + d.State + "' )";
        }
    }
}
