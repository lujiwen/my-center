﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.Controller
{ 
    class State
    {
        public State(Socket socket)
        {
            this.Buffer = new byte[1024*10];
            this.Socket = socket;
            this.RemoteEP = new IPEndPoint(IPAddress.Any, 0);
        }
        /// <summary>
        /// 获取本机（服务器）Socket
        /// </summary>
        public Socket Socket { get; private set; }
        /// <summary>
        /// 获取接收缓冲区
        /// </summary>
        public byte[] Buffer { get; private set; }
        /// <summary>
        /// 获取/设置客户端终结点
        /// </summary>
        public EndPoint RemoteEP;
    }

   public  class UdpConnection:Connection
    {
        public event dataReceivedHandler dataReceivedEvent;
        public UdpConnection(String ip, String port):base(ip,port)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Convert.ToInt32(Port));
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(ipep);

                State state = new State(socket);
                socket.BeginReceiveFrom(
                    state.Buffer, 0, state.Buffer.Length,
                    SocketFlags.None,
                    ref state.RemoteEP,
                    EndReceiveFromCallback,
                    state);
            }
            catch (Exception e)
            {

            }
        }
        public UdpConnection(Device device)
            : base(device)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Convert.ToInt32(Port));
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(ipep);

                State state = new State(socket);
                socket.BeginReceiveFrom(
                    state.Buffer, 0, state.Buffer.Length,
                    SocketFlags.None,
                    ref state.RemoteEP,
                    EndReceiveFromCallback,
                    state);
            }
            catch (Exception e)
            {

            }
        }
        private void EndReceiveFromCallback(IAsyncResult iar)
        {
            State state = iar.AsyncState as State;
            Socket socket = state.Socket;
            try
            {
                int byteRead = socket.EndReceiveFrom(iar, ref state.RemoteEP);
                string message = Encoding.Default.GetString(state.Buffer, 0, byteRead);

                //if (dataReceivedEvent != null)
                //{
                //    dataReceivedEvent(message);
                //}
                if(ReceiveListener!=null)
                {
                    ReceiveListener.onDataReceive(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                 socket.BeginReceiveFrom(
                    state.Buffer, 0, state.Buffer.Length,
                    SocketFlags.None,
                    ref state.RemoteEP,
                    EndReceiveFromCallback,
                    state);
            }
        }

        public override bool StartConnection()
        {
            return base.StartConnection();
        }
   }
}
