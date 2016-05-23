using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.ObjectModel;
using WpfApplication2.Util;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.Controller
{
    public delegate void NewMonitorDevsDataSqlEvent(String s);//更新实时曲线使用的委托
    public class COMConnection : Connection
    {
        /**
         * 亭子对应的一个设备，一个设备，一条tcp连接
         * **/
        private Device device;
        Byte[] commands; // 待下发命令，pdevice获取
        int MAX_SLEEP_CNT = 3600; // 异常睡眠等待次数  暂定30分钟重复扫描
        int MAX_UNRECV_CNT = 50; // 最大连续 50次没有收到数据，视为掉线。
        int PERIOD_INTERVAL = 2000; // 数据请求的周期间隔，pdevice获取


        public Device Pdevice
        {
            get { return device; }
            set { device = value; }
        }
         
        Socket socket;//socket连接
        Byte[] receiveBuffer = new Byte[512];//接受数据缓冲区 每个设备的接收缓冲区  待确定？
        Thread rdthread; //线程

        NewMonitorDevsDataSqlEvent newMonitorData;
        public COMConnection(Device device)
        {
            Succeed = false;
            this.Ip = device.devIp;
            this.Port = device.devPort;
            this.device = device; // 设备关联
            PERIOD_INTERVAL = device.periodInterval*1000; // 不同设备请求数据周期可能不一样 ，S--->ms
        }


        // IP 重连 机制
        public override Boolean StartConnection() // 不同于PLC接入，一个设备对应一个连接。
        {
            try
            {
                //建立socket连接   TCP Server模式 
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ie = new IPEndPoint(IPAddress.Parse(Ip), Convert.ToInt32(Port));
                socket.Connect(ie);
                Succeed = true;
                // 开启读取数据线程 
                rdthread = new Thread(new ThreadStart(this.Work));
                rdthread.Start();
            }
            catch (SocketException se)
            {
                LogUtil.Log(false, "Socket 连接异常", (int)ErrorCode.ERR_CODE.SOCKET_CONNECT_ERR);
                err = "Socket 连接异常： " + device.SubSystemName + ", " + device.Type + device.DeviceId;
                Console.WriteLine(err);
                Succeed = false;
            }
            catch (InvalidOperationException ipe)
            {
                LogUtil.Log(false, "Socket 连接异常", (int)ErrorCode.ERR_CODE.SOCKET_CONNECT_ERR);
                err = "Socket 连接异常： " + device.SubSystemName + "; " + device.Type + device.DeviceId;
                Console.WriteLine(err);
                Succeed = false;
            }
            return Succeed;
        }
        public override string getErrMessage()
        {
            return err;
        }

        /// <summary>
        /// 关闭连接,不是暂停连接
        /// </summary>
        public override Boolean StopConnection()
        {
            if (Succeed)//正处于连接状态
            {
                Succeed = false;
                if (rdthread != null) // 终止线程
                    rdthread.Abort();
             
                socket.Close();
            }
            return Succeed; // 返回当前链接状态
        }

        public void Work() {
            int error_count = 0;
            int exception_sleep_cnt = 0; // 异常等待次数

            while (Succeed) {

                Thread.Sleep(PERIOD_INTERVAL); // 读取数据周期间隔
                /** 设备正常，读取有效数据 **/
                if (exception_sleep_cnt <= 0)
                {
                    if (sendCommands() == 0) // 下发数据请求命令
                    {
                        if (getDevsData())// 获取设备数据 并解析
                        {
                            error_count = 0; //成功一次就清0
                            device.DState = " "; //自动上线更新
                            continue; // 数据请求成功，开始下一次
                        }
                    }
                        /** 读写异常，开始计数 **/
                        error_count++;
                        /** 连续5次没有请求到数据，向上报异常状态 **/
                        if (error_count > MAX_UNRECV_CNT)
                        {
                            device.DState="掉线"; // 设备掉线，如何更新状态？？？
                            exception_sleep_cnt = MAX_SLEEP_CNT; //30分钟试探一次？？？待修正 
                        }
                        Thread.Sleep(1000); // 更长的错误延时 
                }
                /** 进入异常sleep周期,周期过后重新请求数据  **/
                else {
                    exception_sleep_cnt--; 
                }
            }
            return;
        }
        /// <summary>
        /// 下发命令,一次只处理一个设备 
        /// </summary>
        public override int sendCommands()
        {
            try {
                if (device.ParaChanged)//参数设置命令
                {
                     commands = device.ToSetParaCommands();
                }
                else
                {
                    commands = device.ToReadDataCommand();
                }
                //发送数据
                socket.Send(commands, commands.Length, 0);
                Console.WriteLine(device.SubSystemName+":"+device.Type+", "+device.DeviceId+" 连接放送命令："+commands);
            }
            catch (Exception ex)
            {
                LogUtil.Log(false, ex.ToString(),(int)ErrorCode.ERR_CODE.SOCKET_CONNECT_ERR);
                return -1; // 返回异常
            }
            return 0;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public override bool getDevsData()
        {
            try
            {             
                //发送次数
                UInt32 count = 0;
                //没有数据更新,跳出等待
                while (socket.Available <= 0 && count < receiveNullMaxCount)
                {
                    //等待0.1s
                    Thread.Sleep(500);
                    count++;
                } 
                // 等待有数据或者预置时间，直接一次性读数据
                if (Succeed && count < receiveNullMaxCount)//正常接收
                {
                    int recv_len = socket.Receive(receiveBuffer);
                    /** 参数解析 **/
                    if (device.ParaChanged) 
                    {
                        device.ParaChanged = false;
                        if (!device.isParaSetRight(receiveBuffer))//继续设置,设置错误,上层应做判断
                        {
                            device.ParaChangedSuccess = false;
                            return false;
                        }
                        else//已经修改成功
                        {
                            device.ParaChangedSuccess = true;
                            return true;
                        }
                    }
                    /** 解析接收的设备实时数据 **/
                    else
                    {
                        if (!device.isDataRight(receiveBuffer, recv_len)) //判断是否数据正常？如果不正常，跳出该次数据解析
                        {
                            return false; //数据异常
                        }
                        //解析数据
                        device.AnalysisPavilionData(receiveBuffer,recv_len);
                        //产生新的数据入库事件
                        newMonitorData(device.getHistoryDataSql());

                        //产生新的阿里云数据更新事件
                        device.getAliyunUpdateStr();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Log(false, ex.ToString(), (int)ErrorCode.ERR_CODE.SOCKET_CONNECT_ERR);
                return false;
            }
            return false; // 没有数据返回 

        }
      

        public NewMonitorDevsDataSqlEvent NewMonitorData
        {
            get { return newMonitorData; }
            set { newMonitorData = value; }
        }
    }
}
