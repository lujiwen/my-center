using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using WpfApplication2.Util;


namespace WpfApplication2.Controller
{
    /*
  *SocketConnection类，为应急中心与二级之间的数据传输做服务，client暂定为应急中心端。
  *使用时生成一个SocketConnection对象，然后调用Connect进行连接，如果断开可以使用reconnect进行重连，
  *发送数据需要调用sendData函数，数据接收需要接收dataReceivedEvent事件
 */
   public class SocketConnection:Connection
    {
        string _ip;
        string _port;
        bool connectState = false;

        public delegate void dataReceivedHandler(string data);
        public event dataReceivedHandler dataReceivedEvent;

        StateObject curState;
        const string exitString = "<exit>";

        public SocketConnection()
        {

        }

        public SocketConnection(string ip, string port)
        {
            this._ip = ip;
            this._port = port;
  
        }

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public bool ConnectState
        {
            get {return connectState;}
          
        }

        public override void Connect(){
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(Ip), Convert.ToInt32(Port));
                // Create a TCP/IP socket.     
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Connect to the remote endpoint.     
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);

                connectState = true;
            }
            catch (Exception e)
            {
                connectState = false;
            }
            
        }


        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.     
                Socket client = (Socket)ar.AsyncState;
                // Complete the connection.     
                client.EndConnect(ar);
                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

                // Create the state object.     
                curState = new StateObject();
                curState.workSocket = client;
                // Begin receiving the data from the remote device.     
                client.BeginReceive(curState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), curState);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                string content = string.Empty;
                // Retrieve the state object and the handler socket     
                // from the asynchronous state object.     
                Socket handler = curState.workSocket;
                // Read data from the client socket.     
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.     
                    curState.sb.Append(Encoding.ASCII.GetString(curState.buffer, 0, bytesRead));
                    // Check for end-of-file tag. If it is not there, read     
                    // more data.     
                    content = curState.sb.ToString();
                    if (content.EndsWith("</package>"))
                    {
                        // All the data has been read from the     
                        // client. Display it on the console.     
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                        // Echo the data back to the client.     
                        if (dataReceivedEvent != null)
                        {
                            dataReceivedEvent(curState.sb.ToString());
                        }
                        curState.sb.Remove(0,curState.sb.Length);
                        handler.BeginReceive(curState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), curState);
                    }
                    else if (content.EndsWith(exitString))
                    {
                        curState.workSocket.Shutdown(SocketShutdown.Both);
                        curState.workSocket.Close();
                    }
                    else
                    {
                        handler.BeginReceive(curState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), curState);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(string data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.     
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                // Begin sending the data to the remote device.     
                curState.workSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), curState.workSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.     
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.     
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void exit()
        {
            //Send(exitString);
            curState.workSocket.Disconnect(false);
        }

        public override void GetDataFromServer(){

        }
        public override void SendCommandToServer(string data){
            Send(data);
        }

    }
}
