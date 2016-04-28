using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WpfApplication2.Util;

namespace WpfApplication2.Daemon
{
    public  class CenterDaemon
    {
        public CenterDaemon()
        { 
        }

        Thread th;
        /// <summary>
        /// daemonPath : @"E:\代码\daemon\Debug\daemon.exe"
        /// </summary>
        /// <param name="daemonPath"></param>
        public void startDaemon(string daemonPath )
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = daemonPath;
            string current_path = Application.ExecutablePath;

            info.Arguments = current_path;
            // 获取程序完整路径 

            info.WindowStyle = ProcessWindowStyle.Minimized;
            try
            {
                // 如果 守护进程 已经存在，则不需再启动。无需重复执行。
                if (daemon_exits_judge())
                {
                  //  MessageBox.Show("daemon already exits !");
                }
                else
                {
                    Process pro = Process.Start(info);
                }
                th = new Thread(new ThreadStart(DaemonCommunicate)); //与守护进程进行 心跳通信                
                th.Start(); //启动线程  
               
            }
            catch (Exception)
            {
                MessageBox.Show("daemon fails to start !");
            }
        }

        static void DaemonCommunicate()
        {
            int port = 7777;
            string host = "127.0.0.1";
            //创建终结点EndPoint
            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint server_ipe = new IPEndPoint(ip, port);   //把ip和端口转化为IPEndPoint的实例
            try
            {
                //创建Socket并连接到服务器
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //  创建Socket
                sock.Connect(server_ipe); //连接到服务器
                string sendStr = "Hello,my daemon server";
                byte[] bs = Encoding.ASCII.GetBytes(sendStr);   //把字符串编码为字节
                int cnt = 0;
                while (true)
                {
                    int len_sent = sock.Send(bs, bs.Length, 0);
                    Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("网络连接异常 " + e.Message); //改为其他记录方式 
                return;
            }
        }

        private bool daemon_exits_judge()
        {
            Process[] allProgresse = System.Diagnostics.Process.GetProcessesByName("center-daemon");
            foreach (Process closeProgress in allProgresse)
            {
                if (closeProgress.ProcessName.Equals("center-daemon"))
                {
                    return true;
                }
            }
            return false;  // 已经存在
        }

        public void  shutdownDaeom()
        {
            Process[] allProgresse = System.Diagnostics.Process.GetProcessesByName("center-daemon");
            foreach (Process closeProgress in allProgresse)
            {
                if (closeProgress.ProcessName.Equals("center-daemon"))
                {
                    if (th != null && th.IsAlive)
                        th.Abort(); //终止通信线程
                    closeProgress.Kill();
                    closeProgress.WaitForExit();
                    break;
                }
            }
        }
    }
}
