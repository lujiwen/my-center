using PavilionMonitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;
using WpfApplication2.package;
// code by liuhuashan 2016/04/26

// 2401b 485接入 
// 6个字节的数据部分不仅包含了 4个字节的浮点数实时值，还包含了状态和单位值。

namespace Yancong
{
    public class DeviceXb2401:Device
    {

        private string deviceId;
        private string cabId;
        private string buildingId;
       
        private int subSystemSerial;
        private string subSystemName;
        private float highthreshold;
        private float lowthreshold;

        private int devLocalAddress; //本地地址

        private int interfaceId;
        private float correctFactor;

        private string dataUnit;
        private double nowValue;

        /// <summary>
        /// 初始化设备地址，设备数据读取命令需要
        /// addr :设备本地地址，数据请求命令中使用
        /// </summary>
        /// <param name="addr"></param>
        public DeviceXb2401(int addr,UInt32 id, String ip, String port): base(id,ip,port){
            devLocalAddress=addr;
        }
        public DeviceXb2401(OracleDataReader odr)
            :base(odr)
        {

        }

        DeviceDataBox_Xb2401  box2401;
        public DeviceXb2401(DeviceDataBox_Xb2401 b)
        {
            box2401 = b;
            fromBoxToDevice((DeviceDataBox_Xb2401)b);
            judgeState();
        }
        public override void fromBoxToDevice(DeviceDataBox_Base box)
        {
            base.fromBoxToDevice(box);
            if (box2401.value != null && !box2401.value.Equals(""))
            {
                nowValue = double.Parse(box2401.value);
                devState = box2401.state;
            }
            
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override byte[] ToReadDataCommand()
        {
            // 05 02 00 F0 F7
          byte[] command = {0x05,0x02,0x00,0xF0,0xF7}; // 超大流量15字节命令
            command[1]=(byte)devLocalAddress; // 暂时的定义，该设备标准定义的地址是第二三字节，都是地址标号；可能高位在前，地位在后吧，现场验证看看？
            command[4]=0;
            for(int i=0;i<4;i++){  // 累计求和计算校验码
                command[4]+=command[i];
            }

            return command;
        }

        /// <summary>
        /// 读参数命令
        /// </summary>
        /// <returns></returns>
        public override Byte[] ToReadParaCommands()
        {
            byte[] command = new byte[6];

            command[0] = 0xfe;//包头
            command[1] = 0x05;//包长
            command[2] = 0x00;//探头类型
            command[3] = Convert.ToByte(2401);//监测仪编号
            //读数据指令
            command[4] = 0xf0;
            //生成校验码
            for (int i = 0; i < 5; i++)
                command[5] ^= command[i];

            return command;
        }

                /// <summary>
        /// 生成设置参数命令
        /// </summary>
        /// <returns></returns>
        public override Byte[] ToSetParaCommands() {
            return null;
        }

         /// <summary>
        /// 设备数据解析
        /// </summary>
        /// 默认只有一个通道数据吧
        /// <param name="flowBytes"> 原始字节流 </param>
        /// 字节数（标准数据块）
            /*
            名  称	字节数（压缩数据块）	字节数（标准数据块）
            字节数	1字节	1字节
            设备号	2字节	2字节
            特征字	1字节（0x70）	1字节（0x71）
            1通道实时数据块	4字节	6字节
            ……	……×4字节	6字节
            N通道实时数据块	4字节	6字节
            校验码		1字节
            */
        public override void AnalysisData(byte[] flowBytes,int len) { 
            // 0B 02 00 71 00 70 2D 46 01 80 E2   目前回来的数据只有一个通道的数据，首字节就是命令长度 
            // 0B 02 00 71 99 99 81 41 14 80 06
            // 首字节为命令长度，最后一字节未校验码
            int recv_len=flowBytes[0];
            if(recv_len!=len){
                return ; // 格式有误
            }
            byte sum=0;
            for(int i=0;i<recv_len-1;i++){
                sum+=flowBytes[i];
            }
            if(sum != flowBytes[recv_len-1])
                return ; // 校验位不匹配

            if(flowBytes[3]==0x71) //只处理标准格式的数据 
            {  // 解析数据部分的6字节
                byte[] f_bytes = new byte[4];
                f_bytes[0] = flowBytes[4];
                f_bytes[1] = flowBytes[5];
                f_bytes[2] = flowBytes[6];
                f_bytes[3] = flowBytes[7];
                nowValue=BitConverter.ToSingle(flowBytes,4); // 浮点数转换 
            }
            // 状态和单位分析 
            DState="";
            for(int i=0;i<5;i++){
                byte mask=0x80;
                uint r=(uint)( (flowBytes[8])&mask );
                if(r>0){
                    if(i==0)
                        DState+="高报 ";
                    else if(i==1)
                         DState+="警告 ";
                    else if(i==2)
                         DState+="失效 ";
                    else if(i==3)
                         DState+="源检 ";
                    else if(i==4)
                         DState+="禁止 ";
                }
                flowBytes[8]=(byte)(flowBytes[8]<<1);
            }
            // 后3位单位分析，已经跑到前三位了
            if(flowBytes[8]==0x20) // 001 0 
            {
                devUnit="Bq / m3";
            }else if(flowBytes[8]==0x40) // 010 0
            {
                 devUnit="μGy / h";
            }else if(flowBytes[8]==0x60) // 010 0
            {
                 devUnit="μSv / h";
            }else if(flowBytes[8]==0x80) // 100 0
            {
                 devUnit="Hz";
            }
            this.devState = "Normal";
        }
        /// <summary>
        /// 生成插入数据的sql
        /// </summary>
        /// <returns></returns>
        public override String getHistoryDataSql() {

            return "";
        }

        public override Dictionary<string, List<DeviceData>> getHistoryDataSet(OracleDataReader odr)
        {
            Dictionary<string, List<DeviceData>> dataDictionary = new Dictionary<string, List<DeviceData>>();
            List<DeviceData> dataset = new List<DeviceData>();
            while (odr.Read())
            {
                DeviceData d = new DeviceData();
                d.VALUE1 = odr.GetFloat(5).ToString();
                d.Time = odr.GetString(2);
                dataset.Add(d);
                d = null;
            }
            dataDictionary.Add("实时值", dataset);
            return dataDictionary;
        }

        public override string GenerateInsertSql(string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + nowValue + ", " + "'" + DataUnit + "'" + ", " + "'" + devState + "' )";
        }

        public override WpfApplication2.package.Box getCommonDataPack()
        {
            DeviceDataBox_Xb2401 box = new DeviceDataBox_Xb2401();
            box.load(this.BuildingId, this.CabId, DeviceId,this.devState, nowValue.ToString(), this.devUnit, this.Lowthreshold.ToString(), this.Highthreshold.ToString(), this.CorrectFactor.ToString());
            return box;
        }
    }



}
