using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Project208Home.Model;
using WpfApplication2.Model.Vo;

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

        public DeviceQuality()
        {
        }
        public DeviceQuality(UInt32 id, UInt32 interfaceId, String type)
            //: base(id, interfaceId, type)
        {
            safeColor = "Black";
        }
        /// <summary>
        /// 获取读取命令
        /// </summary>
        /// <returns></returns>
        //public override byte[] getReadCommands()
        //{
        //    Byte[] qualitycoms = new Byte[8];
        //    LinkedList<Byte> _qualityCommands = new LinkedList<byte>();
        //    //地址
        //    _qualityCommands.AddLast(Convert.ToByte(DevLocalAddress));

        //    //中间不变的值
        //    Byte[] command = { 0x03, 0x00, 0x02, 0x00, 0x08 };
        //    for (int i = 0; i < command.Length; i++)
        //    {
        //        _qualityCommands.AddLast(command[i]);
        //    }
        //    //校验码
        //    UInt16 crc = crc_calc(_qualityCommands.ToArray());
        //    Byte[] crcbyte = BitConverter.GetBytes(crc);
        //    _qualityCommands.AddLast(crcbyte[0]); //低位
        //    _qualityCommands.AddLast(crcbyte[1]); //高位

        //    qualitycoms = _qualityCommands.ToArray();
        //    return qualitycoms;
        //}
        ///// <summary>
        ///// 生产CRC校验
        ///// </summary>
        ///// <param name="cmd"></param>
        ///// <returns></returns>
        //private static UInt16 crc_calc(byte[] cmd)
        //{
        //    UInt16 CRC = 0xFFFF;
        //    UInt16 carry = 0;

        //    for (int i = 0; i < cmd.Length; i++)
        //    {
        //        UInt16 tp = (UInt16)(cmd[i] & 0xFF);
        //        CRC = (UInt16)(CRC ^ tp);
        //        for (int j = 0; j < 8; j++)
        //        {
        //            carry = (UInt16)(CRC & 0x0001);
        //            CRC = (UInt16)(CRC >> 1);
        //            if (carry == 0x0001)
        //                CRC = (UInt16)(CRC ^ 0xA001);
        //        }
        //    }
        //    return CRC; // 追加到命令字符串时，先取低位，再取高位
        //}
        ///// <summary>
        ///// 分析数据
        ///// </summary>
        ///// <param name="data"></param>
        ////public override void AnalysisData(byte[] data)
        ////{
        ////    //判断数据是否符合要求
        ////    if (data != null && data.Length == 21)
        ////    {
        ////        //瞬时流量值
        ////        byte[] realtimequality = { data[3], data[4], data[5], data[6] };
        ////        DoseNow = realTimeQualityAnalysis(realtimequality);

        ////        //累积流量
        ////        byte[] totalquality = { data[7], data[8], data[9], data[10], data[11], data[12] };
        ////        DoseSum = totalQualityAnalysis(totalquality);

        ////        //满量程
        ////        byte[] range = { data[13], data[14], data[15], data[16] };
        ////        doseFull = rangAnalysis(range);

        ////        //单位
        ////        byte unit = data[17];
        ////        switch (unit)
        ////        {
        ////            case 0x00:
        ////                NowDataUnit = "SCCM";
        ////                SumDataUnit = "SCC";
        ////                break;
        ////            case 0x01:
        ////                NowDataUnit = "SCCM";
        ////                SumDataUnit = "SL";
        //                break;
        //            case 0x11:
        //                NowDataUnit = "SLM";
        //                SumDataUnit = "SL";
        //                break;
        //        }
        //        //状态判断
        //        byte state = data[18];
        //        switch (state)
        //        {
        //            case 0x00:
        //                devState = "close";
        //                break;
        //            case 0x80:
        //                devState = "valve_control";
        //                break;
        //            case 0xFF:
        //                devState = "clean";
        //                break;
        //        }
        //        //DataIsUpdate  = true;
        //        ////实时曲线更新
        //        //if (RealTimeUpdate)
        //        //{
        //        //    Object[] values = new Object[4];
        //        //    values[0] = DevId;
        //        //    values[1] = DoseNow;
        //        //    values[2] = NowDataUnit;
        //        //    values[3] = DoseSum;
        //        //    values[4] = SumDataUnit;
        //        //    NewMonitorNowData(values);
        //        //}
        //    }
        //}

        ////瞬时流量值
        //private static double realTimeQualityAnalysis(byte[] data)
        //{
        //    double realTimeQuality = 0;
        //    if (data[3] == 0x0A) //情况1
        //    {
        //        if (data[1] == 0x10)
        //            realTimeQuality = -00.0;
        //        else if (data[2] == 0x10)
        //            realTimeQuality = -0.00;
        //        else
        //            realTimeQuality = -0;
        //    }

        //    if (data[2] > 15 && data[3] == 0x0F)//情况2
        //    {
        //        realTimeQuality += data[2] - 16;
        //        realTimeQuality += (double)data[1] / 10;
        //        realTimeQuality += (double)data[0] / 100;
        //    }
        //    if (data[1] > 15 && data[2] != 0x0F && data[3] == 0x0F)//情况3
        //    {
        //        realTimeQuality += data[2] * 10;
        //        realTimeQuality += data[1] - 16;
        //        realTimeQuality += (double)data[0] / 10;
        //    }
        //    if (data[1] > 15 && data[2] == 0x0F)//情况4
        //    {
        //        realTimeQuality += data[1] - 16;
        //        realTimeQuality += (double)data[0] / 10;
        //    }
        //    if (data[0] < 16 && data[1] < 16 && data[2] < 16 && data[3] < 16)//情况5
        //    {
        //        if (data[0] == 0x0F)
        //            realTimeQuality = 0;
        //        else if (data[1] == 0x0F)
        //            realTimeQuality += data[0];
        //        else if (data[2] == 0x0F)
        //        {
        //            realTimeQuality += data[1] * 10;
        //            realTimeQuality += data[0];
        //        }
        //        else if (data[3] == 0x0F)
        //        {
        //            realTimeQuality += data[2] * 100;
        //            realTimeQuality += data[1] * 10;
        //            realTimeQuality += data[0];
        //        }
        //        else
        //        {
        //            realTimeQuality += data[3] * 1000;
        //            realTimeQuality += data[2] * 100;
        //            realTimeQuality += data[1] * 10;
        //            realTimeQuality += data[0];
        //        }
        //    }

        //    if (data[1] > 15 && data[3] < 10)//情况6
        //    {
        //        realTimeQuality += data[3] * 100;
        //        realTimeQuality += data[2] * 10;
        //        realTimeQuality += data[1] - 16;
        //        realTimeQuality += (double)data[0] / 10;
        //    }
        //    else if (data[2] > 15 && data[3] < 10)
        //    {
        //        realTimeQuality += data[3] * 10;
        //        realTimeQuality += data[2] - 16;
        //        realTimeQuality += (double)data[1] / 10;
        //        realTimeQuality += (double)data[0] / 100;
        //    }
        //    else if (data[3] > 15)
        //    {
        //        realTimeQuality += data[3] - 16;
        //        realTimeQuality += (double)data[2] / 10;
        //        realTimeQuality += (double)data[1] / 100;
        //        realTimeQuality += (double)data[0] / 1000;
        //    }


        //    return realTimeQuality;
        //}

        //////累积流量解析
        //private static double totalQualityAnalysis(byte[] data)
        //{
        //    double total=0;
        //    int indexof0F=0, indexof1x=0;
        //    for (int i = 0; i < 6; i++)
        //    {
        //        if (data[i] > 15)
        //            indexof1x = i;
        //        if (data[i] == 0x0F)
        //        {
        //            indexof0F = i; //返回第一个值为0F的index
        //            break;
        //        }
        //    }

        //    if(indexof1x==0 && indexof0F!=0)//整数情况1
        //    {
        //        for (int i = 1; i <= indexof0F;i++ )
        //        {
        //            total += data[indexof0F - i] * (Math.Pow(10, indexof0F - i));
        //        }                
        //    }
        //    if (indexof1x == 0 && indexof0F == 0)//整数情况2
        //    {
        //        for (int i = 5; i >= 0; i-- )
        //        {
        //            total += data[i] * (Math.Pow(10, i));
        //        }
        //    }
        //    if(indexof1x != 0 && indexof0F != 0)//小数情况1
        //    {
        //        for (int i = 1; i <= (indexof0F - indexof1x);i++ )
        //        {
        //            if (data[indexof0F - i] > 15)
        //                data[indexof0F - i] -= 16;
        //            total += data[indexof0F - i] * (Math.Pow(10, indexof0F - indexof1x - i));
        //        }
        //        for (int i = 1; i <= indexof1x;i++ )
        //        {
        //            total += (double)data[indexof1x - i] / (Math.Pow(10, i));
        //        }
        //    }
        //    if (indexof1x != 0 && indexof0F == 0)//小数情况2
        //    {
        //        for (int i = 5; i >= indexof1x; i-- )
        //        {
        //            if (data[i] > 15)
        //                data[i] -= 16;
        //            total += data[i] * (Math.Pow(10, i - indexof1x));
        //        }
        //        for (int i = indexof1x-1; i >= 0; i-- )
        //        {
        //            total += (double)data[i] / (Math.Pow(10, indexof1x - i));
        //        }
        //    }
        //    return total;
        //}

        ////量程解析
        //private static double rangAnalysis(byte[] data)
        //{
        //    double rang = 0;
        //    if (data[3] == 0x0F) //如果里面含有0F,则量程从0.00 - 100
        //    {
        //        if (data[2] > 15)
        //        {
        //            rang += data[2] - 16;
        //            rang += (double)(data[1]) / 10;
        //            rang += (double)(data[0]) / 100;
        //        }
        //        else if (data[1] > 15)
        //        {
        //            rang += data[2] * 10;
        //            rang += data[1] - 16;
        //            rang += (double)(data[0]) / 10;
        //        }
        //        else
        //        {
        //            rang += data[2] * 100;
        //            rang += data[1] * 10;
        //            rang += data[0];
        //        }
        //    }
        //    else if (data[3] > 15)
        //    {
        //        rang += data[3] - 16;
        //        rang += (double)(data[2]) / 10;
        //        rang += (double)(data[1]) / 100;
        //        rang += (double)(data[0]) / 1000;
        //    }
        //    else if (data[2] > 15)
        //    {
        //        rang += data[3] * 10;
        //        rang += data[2] - 16;
        //        rang += (double)(data[1]) / 10;
        //        rang += (double)(data[0]) / 100;
        //    }
        //    else if (data[1] > 15)
        //    {
        //        rang += data[3] * 100;
        //        rang += data[2] * 10;
        //        rang += data[1] - 16;
        //        rang += (double)(data[0]) / 10;
        //    }
        //    else
        //    {
        //        rang += data[3] * 1000;
        //        rang += data[2] * 100;
        //        rang += data[1] * 10;
        //        rang += data[0];
        //    }
        //    return rang;
        //}
        ///// <summary>
        ///// 获取插入历史数据的sql
        ///// </summary>
        ///// <returns></returns>
        ////public override string getHistorySql(DateTime datareceiveTime)
        ////{
        ////    String sql = "";
        ////    if (DataIsUpdate)
        ////    {
        ////        String[] colums = { "DevId", "DataTime", "NowValue", "OtherValue" };
        ////        Object[] values = { DevId, "'" + datareceiveTime.ToString() + "'", doseNow, doseSum };
        ////        sql = DBHelpler.getInsertCommands("devicehistorydata", colums, values);
        ////    }
        ////    return sql;
        ////}
        ///// <summary>
        ///// 获取发送至远端的数据包
        ///// </summary>
        ///// <returns></returns>
        ////public override Box getCommonDataPack()
        //{
        //    DeviceDataBox_Comp databox = new DeviceDataBox_Comp();
        //    //状态要根据实际情况来设置，要传递多个值
        //    if (DataIsUpdate)
        //    {
        //        databox.load("2", CabId.ToString(), DevId.ToString(),
        //            State.Normal, DoseNow.ToString(), NowDataUnit, "", "");
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
        public override string GenerateSql(string tablename)
        {
            return "INSERT INTO " + tablename + " VALUES(" + "deviceData_" + BuildingId + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" + State + "'" + ", " + "'" + DataUnit + "'" + ", " + "1" + ")"; ;
        }
    }
}
