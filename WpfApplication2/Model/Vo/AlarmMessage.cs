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

        public DateTime AlarmDate { get { return alarmDate ; } set { alarmDate  = value;} }
        public String MessageContent { get { return messageContent; } set { messageContent = value; } }
        public Device AlarmDevice { get { return alarmDevice; } set { alarmDevice = value; } }

        public AlarmMessage()
        {
            messageContent = "";
            alarmDate = new DateTime();
        }

        public AlarmMessage(String content,DateTime d)
        {
            messageContent = content;
            alarmDate = d;
        }
        public AlarmMessage(String content, DateTime d,Device alarmDvc)
        {
            messageContent = content;
            alarmDate = d;
            alarmDevice = alarmDvc;
        }
    }
}
