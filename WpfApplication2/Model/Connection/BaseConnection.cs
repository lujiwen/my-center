using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Controller
{
    public class BaseConnection
    {
        //protected String _ip;
       // protected String _port;
        
        //abstract String Ip{get;set;}
        //abstract String Port { get; set; }
         public BaseConnection() { }
         public virtual void Connect() { }
         public virtual void GetDataFromServer() { }
         public virtual void SendCommandToServer(string data) { }

        
    }
}
