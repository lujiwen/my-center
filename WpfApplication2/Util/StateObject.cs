using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace WpfApplication2.Util
{
    class StateObject
    {
        // Client socket.     
        public Socket workSocket = null;
        // Size of receive buffer.     
        public const int BufferSize = 256;
        // Receive buffer.     
        public byte[] buffer = new byte[BufferSize];
        // Received data string.     
        public StringBuilder sb = new StringBuilder();
    }
}
