using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controller;

namespace WpfApplication2.Controller
{
    /// <summary>
    /// 管理一个检测点的所有连接
    /// </summary>
   public class ConnectionManager :DataReciveListner
    {
        public delegate void MangerReceivedDataHandler(string data);
        public event MangerReceivedDataHandler ManagerReceivedDataEvent;
        public bool isAllConnected;
        private  List<Connection> connections;
        private string connectionErrMessage;
       public ConnectionManager(List<Connection> conList)
       {
           this.connections = conList;
           init();
       }
       public ConnectionManager(Connection con)
       {
           connections = new List<Connection>();
           connections.Add(con);
           init();
       }
       public ConnectionManager()
       {
           init();
       }

       public void init()
       {
           foreach(Connection c in connections)
           {
               c.ReceiveListener = this;
              // c.dataReceivedEvent += managerDataReceivedEvent ;
           }
           isAllConnected = false;
       }
       /// <summary>
       /// 开启管理的每一个连接去请求数据
       /// </summary>
       public bool startConnections()
       {
           isAllConnected = true;
           foreach (Connection c in connections)
           {
               if(!c.StartConnection())
               {
                   isAllConnected = false;
                   connectionErrMessage += c.getErrMessage()+ "; ";
               }
           }
           return isAllConnected;
       }

      public void managerDataReceivedEvent(string data)
       {
           ManagerReceivedDataEvent(data);
       }

       public bool remove(Connection c)
       {
           if (connections.Contains(c))
           {
               connections.Remove(c);
               return true;
           }
           return false;
       }

       public bool add(Connection c)
       {
           if(connections!=null)
           {
               connections.Add(c);
               return true;
           }
           return false;
       }

       public string getConnectiosErr()
       {
           return connectionErrMessage;
       }

       void DataReciveListner.onDataReceive(string message)
       {
           ManagerReceivedDataEvent(message);
       }
    }
}
