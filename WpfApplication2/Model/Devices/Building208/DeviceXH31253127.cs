using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Project208Home.Model;
using WpfApplication2.Model.Vo;

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
        public DeviceXH31253127(UInt32 id, UInt32 interfaceId, String type)
            //: base(id, interfaceId, type)
        {
            safeColor = "Black";
            DoseNowforPresentation ="0";
        }
        /// <summary>
        /// 获取读取命令
        /// </summary>
        /// <returns></returns>
        //public override byte[] getReadCommands()
        //{
        //    Byte[] xhcoms = new Byte[8];
        //    LinkedList<Byte> _xhCommands = new LinkedList<byte>();
        //    _xhCommands.AddLast(0x40);
        //    _xhCommands.AddLast(0x23);
        //    //地址
        //    _xhCommands.AddLast(Convert.ToByte(DevLocalAddress));
        //    _xhCommands.AddLast(0x02);
        //    _xhCommands.AddLast(0x10);
        //    //校验码
        //    _xhCommands.AddLast(crc_calc(_xhCommands.ToArray()));

        //    xhcoms = _xhCommands.ToArray();
        //    return xhcoms;
        //}
        /// <summary>
        /// 获取校验码
        /// </summary>
        /// <param name="coms"></param>
        ///// <returns></returns>
        //private byte crc_calc(Byte[] coms)
        //{
        //    Byte crc = new byte();
        //    foreach (Byte b in coms)
        //    {
        //        crc += b;
        //    }
        //    return crc;
        //}
        ///// <summary>
        /// 分析数据
        /// </summary>
        /// <param name="datas"></param>
        //public override void AnalysisData(byte[] datas)
        //{
        //    //判断数据是否符合要求
        //    if (datas != null && datas.Length == 11)
        //    {
        //        byte[] buf = new byte[4];
        //        for (int i = 0; i < 4; i++)
        //        {
        //            buf[i] = datas[i + 5];
        //        }
        //        float realData = BitConverter.ToSingle(buf, 0);
        //        //更新
        //        DoseNow = realData;
        //        DoseNowforPresentation = doseNow.ToString("#.###E+000");

        //        DataIsUpdate = true;
        //        //实时曲线更新
        //        if (RealTimeUpdate)
        //        {
        //            Object[] values = new Object[2];
        //            values[0] = DevId;
        //            values[1] = doseNow;
        //            NewMonitorNowData(values);
        //        }
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
        public static string GenerateSql(Device d, string tablename)
        {
            string[] values = d.NowValue.Trim().Split(',');
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, VALUE2, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + d.DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + values[0] + ", " + values[1] + ", " + "'" + d.DataUnit + "'" + ", " + "'" + d.State + "' )";
        }
    }
}
