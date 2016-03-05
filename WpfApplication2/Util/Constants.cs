using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Controller
{
    class Constants
    {
        
        public static int BlockQueueSize = 1000;  //缓存队列大小
        public static bool stopFlag = false; //全局停止的标志
        public static int getDataFromQueueInterval = 3000;  //当队列没有数据时，过多久从队列里面取数据（毫秒）
        public static int inputAndOutputFromQueueInterval = 20; //当队列有数据或忘队列放数据时的时间间隔
    }
}
