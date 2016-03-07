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
        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <returns></returns>
        //public  override byte[] getInitCommands(int i)
        //{
        //    LinkedList<Byte> _commands = new LinkedList<Byte>();
        //    //标识为网口
        //    Byte length = Convert.ToByte(128 + 'S');
        //    _commands.AddLast(length);
        //    //数量
        //    _commands.AddLast(Convert.ToByte(1));
        //    //对应的小PLC编号
        //    _commands.AddLast(Convert.ToByte(InterfaceId));
           
        //    Byte[] _realCommands = Encoding.ASCII.GetBytes(initCommands[i]);
        //    //偏移量
        //    _commands.AddLast(Convert.ToByte(_realCommands.Length));
        //    //命令
        //    foreach (Byte b in _realCommands)
        //        _commands.AddLast(b);
        //    Byte[] commands = _commands.ToArray();
        //    return commands;
        //}
        /// <summary>
        /// 获取参数读取命令
        /// </summary>
        /// <returns></returns>
        //public override byte[] getParaReadCommands()
        //{
        //    Byte[] tempComms = null;
        //    LinkedList<Byte> tempRealcoms = new LinkedList<byte>();
        //    if (correctNeedUpdate && vlotNeedUpdate)//修正因子和电压需要修改
        //    {
        //        tempComms = Encoding.ASCII.GetBytes(correctComs+" "+(long)CorrectFactor+"\r\n"
        //                                    + voltComs + " " + (long)InputArg1 + "\r\n");
        //    }else if (correctNeedUpdate)//修正因子需要修改
        //    {
        //        tempComms = Encoding.ASCII.GetBytes(correctComs+" "+(long)CorrectFactor+"\r\n");
        //    }
        //    else if (vlotNeedUpdate)//电压需要修改
        //    {
        //        tempComms = Encoding.ASCII.GetBytes(voltComs + " " + (long)InputArg1 + "\r\n");
        //    }
        //    Byte[] readParaComms = Encoding.ASCII.GetBytes(paraReadCommands + dataReadComs);
        //    if (tempComms != null)//如果存在修改命令
        //    {
        //        foreach (Byte b in tempComms)//修改命令
        //            tempRealcoms.AddLast(b);
        //        foreach(Byte b in readParaComms)//读取命令
        //            tempRealcoms.AddLast(b);

        //        return tempRealcoms.ToArray();
        //    }
        //    return readParaComms;
        //}
        /// <summary>
        /// 解析参数,两个12位的byte数组
        /// </summary>
        /// <param name="datas"></param>
        //public override void AnalysisParaData(byte[] datas)
        //{
        //    //先解析参数
        //    //修正因子
        //    byte[] tempdatas = new byte[12];
        //    for (int i = 0; i < 12; i++)
        //        tempdatas[i] = datas[i];
        //    String strData = Encoding.ASCII.GetString(tempdatas, 0, tempdatas.Length);
        //    double newCorrectFactor = getDoubleFromBytes(strData);
        //    //电压
        //    for (int i = 0, j = 12; i < 12 && j < 24; i++)
        //        tempdatas[i] = datas[j];
        //    strData = Encoding.ASCII.GetString(tempdatas, 0, tempdatas.Length);
        //    double newVoltValue = getDoubleFromBytes(strData);
        //    if (newCorrectFactor == CorrectFactor)
        //    {
        //        CorrectFactor = newCorrectFactor;
        //        paraUpdatedNeedSave = true;
        //    }
        //    if (newVoltValue == InputArg1)
        //    {
        //        InputArg1 = newVoltValue;
        //        paraUpdatedNeedSave = true;
        //    }
        //    if (paraUpdatedNeedSave)
        //    {
        //        Object[] values = new Object[3];
        //        values[0] = DevId;
        //        values[1] = newCorrectFactor;
        //        values[2] = newVoltValue;
        //        DevParaUpdatedEvent(values);
        //    }
        //    //解析数据
        //    byte[] devdatas = new byte[datas.Length - 24];
        //    datas.CopyTo(devdatas, 24);
        //    AnalysisData(devdatas);
        //}

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
        /// 6517AB的数据读取命令
        /// </summary>
        /// <returns></returns>
        //public  override byte[] getReadCommands()
        //{
        //    Byte[] coms = Encoding.ASCII.GetBytes(dataReadComs);
        //    return coms;
        //}
        /// <summary>
        /// 6517AB的数据分析
        /// </summary>
        /// <param name="datas"></param>
        //public  override void AnalysisData(byte[] datas)
        //{
        //    if (datas != null && (datas.Length >= 16//有正负号 
        //        &&datas.Length <= 18))//无正负
        //    {
        //        int quato = 1;
        //        string strData = null;
        //        if (datas[0] == '-' || datas[0] == '+')
        //        {
        //            quato = (datas[0] == '-' ? -1 : +1);
        //            strData = Encoding.ASCII.GetString(datas, 1, datas.Length-1);
        //        }
        //        else
        //        {
        //            strData = Encoding.ASCII.GetString(datas, 0, datas.Length-1);
        //        }
                 
        //        string strP = resolveP(strData);//实数部分
        //        string strR = resolveR(strData);//幂部分
        //        double r = 0.0;
        //        double p = 0.0;
        //        if (strP != null && strR != null)
        //        {
        //            r = double.Parse(strR);
        //            p = double.Parse(strP);
        //        }
        //        double pow = Math.Pow(10, r);
        //        double realData = quato * p * pow;

        //        //乘以纠正因子
        //        doseNow = realData * CorrectFactor;//赋值

        //        //显示
        //        DoseNowforPresentation = doseNow.ToString("#.###E-000");

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
        /// 获取参数更新命令
        /// </summary>
        /// <returns></returns>
        //public String getParaUpdatedSql()
        //{
        //    String sql = "";
        //    if (paraUpdatedNeedSave)
        //    {
        //        String[] colums={"CorrectFactor","InputArg1"};//修正因子，电压
        //        Object[] values={CorrectFactor,InputArg1};
        //        String[] condColums = { "DevId" };
        //        Object[] conds = {DevId};
        //        sql = DBHelpler.getUpdateCommands("DeviceInfo", colums, values, condColums, conds);
        //    }
        //    return sql;
        //}
        /// <summary>
        ///获取插入历史数据的sql
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
        //    if (DataIsUpdate)
        //    {
        //        databox.load("2", CabId.ToString(), DevId.ToString(), 
        //            State.Normal, DoseNow.ToString(), DevDataUnit, "电压", InputArg1.ToString());
        //    }
        //    return databox;
        //}
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
            return "INSERT INTO " + tablename + " VALUES(" + "deviceData_" + BuildingId + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + NowValue + ", " + "'" + State + "'" + ", " + "'" + DataUnit + "'" + ", " + "1" + ")"; ;
        }
    }
}
