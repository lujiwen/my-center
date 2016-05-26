using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.Controller
{
    /// <summary>
    /// connection 绑定不到界面上，无法使用代理,先使用回调函数的方式
    /// </summary>
    public interface DataReciveListner
    {
        void onDataReceive(String message);
    }
    /**
     * 尽可能做到 PLC 串口服务器 二级子系统 3种接入方式 接口一样。
     * 
     * **/
    public class Connection
    {
        private String ip;
        private String port;
        private bool succeed; //是否连接成功
        private bool test;//是否为测试状态
        private DataReciveListner receiveListener;
        public  DataReciveListner ReceiveListener { get { return receiveListener; } set { receiveListener = value; } }
        public  UInt32 receiveNullMaxCount = 5;//每个读取数据周期，最长等待时间 5*500 ms=2.5 S 
        private Device device;
        public delegate void dataReceivedHandler(string data);
        public event dataReceivedHandler dataReceivedEvent;
        public string err;
        public Connection()
        {
        }
        public Connection(String ip,String port) 
        {
            this.ip = ip;
            this.port = port;
            this.succeed = false;
        }

        public Connection(Device device)
        {
            this.ip = device.devIp;
            this.port = device.devPort;
            this.succeed = false;
            this.device = device;
        }

         public virtual void Connect() { }
         public virtual void GetDataFromServer() { }
         public virtual void SendCommandToServer(string data) { }

         public virtual string getErrMessage()
         {
             if (device == null) return "";
             err = device.SubSystemName + " " + device.Type + " " + device.DeviceId + "连接异常！";
             return err;
         }

        /// <summary>
        /// 启动连接
        /// </summary>
        public virtual Boolean StartConnection() { return false; }
        /// <summary>
        /// 断开连接
        /// </summary>
        public virtual Boolean StopConnection() { return false; }

        /// <summary>
        /// 下发命令
        /// </summary>
        public virtual int sendCommands()
        {
            return 0;
        }
        /// <summary>
        /// 结束数据
        /// </summary>
        /// <returns></returns>
        public virtual bool getDevsData()
        {
            return true;
        }
        public String Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        public String Port
        {
            get { return port; }
            set { port = value; }
        }
        public bool Succeed
        {
            get { return succeed; }
            set { succeed = value; }
        }
        public bool Test
        {
            get { return test; }
            set { test = value; }
        }
        public Device DeviceInConnection 
        { 
            get { return device; } 
            set { device = value;}
        }
        public void setReceiveListener(DataReciveListner l)
        {
            ReceiveListener = l;
        }
    }
}
