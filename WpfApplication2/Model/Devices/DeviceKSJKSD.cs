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
    public class DeviceKSJKSD : Device, INotifyPropertyChanged
    {
        double doseNow;//实时值
        double doseSum;//累计值
        String safeColor;
        bool devIsSafe;
        //判定值是否改变，用于实时显示
        public event PropertyChangedEventHandler PropertyChanged;

        public DeviceKSJKSD()
        {
        }
        public DeviceKSJKSD(OracleDataReader odr)
            :base(odr)
        {
             
        }

        public override byte[] ToReadDataCommand()
        {
            return  getReadDoseNowCommands(this.DevLocalAddress);
        }

        //读取累计值，devlocalid为设备自带地址
        public Byte[] getReadDoseSumCommands(int devlocalid)
        {
            byte[] coms = new byte[5];
            char[] devLocalId = new char[2];
            //将devlocalid转为对应的ascll码
            if(devlocalid<10)
            {
                devLocalId[0]='0';
                devLocalId[1]=Convert.ToChar(devlocalid);
            }
            else
            {
                devLocalId = Convert.ToString(devlocalid).ToCharArray();
            }
            coms[0] = 0x23;
            coms[1] = Convert.ToByte(devLocalId[0]);
            coms[2] = Convert.ToByte(devLocalId[1]);
            coms[3] = 0x0D;
            coms[4] = 0x0A;
            return coms;
        }

        //读取瞬时值，devlocalid为设备自带地址
        public Byte[] getReadDoseNowCommands(int devlocalid)
        {
            byte[] coms = new byte[7];
            char[] devLocalId = new char[2];
            //将devlocalid转为对应的ascll码
            if (devlocalid < 10)
            {
                devLocalId[0] = '0';
                devLocalId[1] = Convert.ToChar(devlocalid);
            }
            else
            {
                devLocalId = Convert.ToString(devlocalid).ToCharArray();
            }
            coms[0] = 0x23;
            coms[1] = Convert.ToByte(devLocalId[0]);
            coms[2] = Convert.ToByte(devLocalId[1]);
            coms[3] = 0x30;
            coms[4] = 0x31;
            coms[5] = 0x0D;
            coms[6] = 0x0A;
            return coms;
        }

        // 解析数据
        public override void AnalysisData(Byte[] datas, int len)
        {
            double data;
            Byte[] theData = new Byte[len];
            for (int i = 0; i < len; i++)
            {
                theData[i] = datas[i];
            }
            //判断数据是否符合要求.数据第一位是等号“=”
            if (datas[0]=='=')
            {
                //数据解析
                string datastr = System.Text.Encoding.Default.GetString(theData);
                datastr = datastr.Replace((string)"\n", "");
                datastr = datastr.Replace((string)"\r", "");
                data = Convert.ToDouble(datastr.Substring(1, datastr.Length - 2));
                //if (datas[1] == '-')//判断是否为负数
                //    data = -data;
                DoseNow = data;

                //报警位解析
                devIsSafe = (datastr[datastr.Length - 1] & 0x00001111) == 0;
                if (devIsSafe)
                {
                    devState = "Normal";
                }
                else
                {
                    devState = "Alert";
                }
            }
        }

        public override Box getCommonDataPack()
        {
            DeviceDataBox_KSJKSD box = new DeviceDataBox_KSJKSD();

            box.load(this.BuildingId, this.CabId, DeviceId, this.devState,
             doseNow.ToString(), doseSum.ToString(), this.devUnit, this.Lowthreshold.ToString(), this.Highthreshold.ToString(), this.CorrectFactor.ToString());
            return box;
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + doseNow + ", " + "'" + DataUnit + "'" + ", " + "'" + State + "' )";
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
            dataDictionary.Add("实时值", dataset);
            return dataDictionary;
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

        public bool DevIsSafe
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

    }
}
