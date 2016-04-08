using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Model.Vo
{
    public class AlarmMessage
    {
        private String messageContent;
        private DateTime alarmDate;
        private Device alarmDevice;
        private string alarmTime;
        public DateTime AlarmDate { get { return alarmDate ; } set { alarmDate  = value;} }
        public String MessageContent { get { return messageContent; } set { messageContent = value; } }
        public Device AlarmDevice { get { return alarmDevice; } set { alarmDevice = value; } }

        public AlarmMessage(String content)
        {
            messageContent = content ;
        }

        public AlarmMessage(String content,Device alarmDvc)
        {
            alarmDevice = alarmDvc; 
            messageContent = content ;
        }
    }
}
