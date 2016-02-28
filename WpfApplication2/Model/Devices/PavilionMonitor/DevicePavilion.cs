using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WpfApplication2.package;


namespace PavilionMonitor
{
    //实时曲线显示的委托
    public delegate void RealTimeCurveEvent(float[] value); 
    public delegate void UpdateSuccessEvent(Boolean success);

    // 子类再派生类，数据更新做到子类上？？？？？？？？？？？？？？？？？？？？？？？？？？ 
    public class DevicePavilion 
    {
       public UInt32 devId=0;
       public String devPort=" ";
       public String devIp=" ";
       public String devType=" ";  // 子类赋值
       public UInt32 cabId; //所属柜子/接入点 编号
       public String cabName=" "; //所属柜子名称 （接入点名称描述）
       public string devState=" "; // 设备状态 枚举 变量表示，全局统一

       public string DState
       {
           get { return devState; }
           set { devState = value;
             setDevState(devState);
           }
       }
       public virtual void setDevState(string state) { 
       }
       //public float doseNow1, doseNow2, doseNow3, doseNow4, doseNow5, doseNow6, doseNow7, doseNow8; // 8个单位值

       //public string unit1, unit2, unit3, unit4, unit5, unit6, unit7, unit8; // 数据库存储，单位用‘_’拼接 

     
        /** 具体某个子类去拆分数据，用户数据判断与预警处理 **/
       public double thresholdHigh, thresholdLow; // 阈值拼接
       public double correctFactor;//纠正因子
       public int periodInterval; //该设备请求数据周期间隔时间 毫秒单位，不同设备请求数据周期可能不一样
       public string devUnit; // 单位 

        //当有新数据产生时，通知实时曲线更新
        public Boolean rtPaint;//为true时，表示正在绘制实时曲线
        private RealTimeCurveEvent rtce;

        public RealTimeCurveEvent Rtce
        {
            get { return rtce; }
            set { rtce = value; }
        }

        public Boolean paraChanged;//参数是否被修改
        public Boolean paraChangedSuccess;//参数修改成功
        public UpdateSuccessEvent use;


        public DevicePavilion(UInt32 id, String ip, String port)         
        {
            devId = id;
            devPort = port;
            devIp = ip;
            paraChanged = false;
        }
    

        //2115房间经过RF1000后的数据格式是否正确
        public virtual bool isDataRight(byte[] flowBytes,int len)
        {
            bool dataright = true;
            return dataright;
        }
        //2115房间经过RF1000后的修改参数的确认信息是否是否正确
        public virtual bool isParaSetRight(byte[] paraBytes)
        {
            bool parasetright = true;
            //判断数据包长度是否为0x05，否则设置为错误
         
            return parasetright;
        }

        /// <summary>
        /// 亭子数据解析
        /// </summary>
        /// <param name="flowBytes"> 原始字节流 </param>
        public virtual void AnalysisPavilionData(byte[] flowBytes,int len) { 
        }
        /// <summary>
        /// 生成插入数据的sql
        /// </summary>
        /// <returns></returns>
        public virtual String getHistoryDataSql() {
            return "";
        }


        /// <summary>
        /// 生成阿里云中转更新数据的sql
        /// </summary>
        /// <returns></returns>
        public virtual void getAliyunUpdateStr()
        {
            return ;
        }

        /// <summary>
        /// 生成插入异常数据库的 插入sql
        /// </summary>
        /// <returns></returns>
        public virtual String getExcepDataSql()
        {
            return "";
        }


        /// <summary>
        /// 初始化连接，循环扫描 请求数据 ,
        /// </summary>
        /// <returns></returns>
        public virtual void doWork() { 
               

        }
        /// <summary>
        /// 亭子设备 数据或参数读取命令生成。
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToReadDataCommand() {
            return null;
        }

        /// <summary>
        /// 读参数命令
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] ToReadParaCommands() {
            return null;
        }
        /// <summary>
        /// 生成设置参数命令
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] ToSetParaCommands() {
            return null;
        }
        /// <summary>
        /// 更改当前设备参数的值
        /// </summary>
        public virtual void ChangeDevPara(float[] values) {
            return;
        }

        public Boolean ParaChanged
        {
            get { return paraChanged; }
            set { paraChanged = value;}
        }
        public Boolean ParaChangedSuccess
        {
            get { return paraChangedSuccess; }
            set
            {
                paraChangedSuccess = value;
                use(paraChangedSuccess);
            }
        }
        public UpdateSuccessEvent Use
        {
            get { return use; }
            set { use = value; }
        }



      
    }
}
