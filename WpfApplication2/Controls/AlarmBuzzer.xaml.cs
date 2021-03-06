﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication2.Model.Vo;
using System.Threading;
using System.Windows.Threading;
using WpfApplication2.Util;

namespace WpfApplication2.CustomMarkers.Controls
{
    /// <summary>
    /// AlarmBuzzer.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmBuzzer : UserControl
    {
        private static  List<string> alarmDevices;
        private bool isAlarming ;
        public bool IsAlarming { get { return isAlarming; } set { isAlarming = value; } }

        private static int count;
        private static List<string> ignoreAlarmDevice;
        private Thread countThread;
        BitmapImage RedBuzzer, BlueBuzzer,redBuzzerMute, blueBuzzerMute ;
        public bool isMute;
        public bool IsMute { get { return isMute; } set { isMute = value; } }
        public AlarmBuzzer()
        {
            InitializeComponent();
            alarmDevices = new List<string>();
            ignoreAlarmDevice = new List<string>();
         // MouseLeftButtonDown += new MouseButtonEventHandler(AlarmBuzzer_MouseLeftButtonDown);
            RedBuzzer = new BitmapImage(new Uri("/Images/red_buzzer.png", UriKind.Relative));
            BlueBuzzer = new BitmapImage(new Uri("/Images/blue_buzzer.png", UriKind.Relative));
            redBuzzerMute = new BitmapImage(new Uri("/Images/red_buzzer_mute.png", UriKind.Relative));
            blueBuzzerMute = new BitmapImage(new Uri("/Images/blue_buzzer_mute.png", UriKind.Relative));
            isMute = false;
            isAlarming = false;
            Visibility = System.Windows.Visibility.Hidden; 
        }

        void AlarmBuzzer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (countThread.IsAlive)
            {
                countThread.Abort();
                countThread = null;
            }
            else  
            {
                countThread = new Thread(new ThreadStart(DispatcherThread));
                countThread.Start();
            }
        }
       
        public void startAlarm()
        {        
            isAlarming = true;
            count = 0;
            if (countThread != null) return;
            countThread = new Thread(new ThreadStart(DispatcherThread));
            countThread.Start();
        
        }
        public void stopAlarm()
        {
            Visibility = System.Windows.Visibility.Hidden;
            isAlarming = false;
            count = 0;
        }
        public void DispatcherThread()
        {
            //可以通过循环条件来控制UI的更新
            while (isAlarming)
            {
                ///线程优先级，最长超时时间，方法委托（无参方法）
                Dispatcher.Invoke( DispatcherPriority.Normal, new Action(UpdateBuzzer));
                Thread.Sleep(2000);
            }
        }

    
        private  void UpdateBuzzer()
        {
            if (isAlarming)
            {
                buzzer.Visibility = System.Windows.Visibility.Visible;
                if (isMute)
                {
                    if (count % 2 == 1)
                    {
                        buzzer.Source = redBuzzerMute;
                        count = 0; 
                        LogUtil.writeErrInFile("buzzer is red ,count is " + count +" "+DateTime.Now.ToString());
                    }
                    else
                    {
                        buzzer.Source = blueBuzzerMute;
                        count = 1;
                        LogUtil.writeErrInFile("buzzer is blue ,count is " + count + " " + DateTime.Now.ToString());
                    }
                }
                else
                {
                    if (count % 2 == 1)
                    {
                        buzzer.Source = RedBuzzer;
                       count = 0;
                       LogUtil.Log(false, "buzzer is red ,count is " + count + " " + DateTime.Now.ToString(),(int)ErrorCode.ERR_CODE.OK);
                    }
                    else
                    {
                        buzzer.Source = BlueBuzzer;
                        count = 1;
                        LogUtil.Log(false, "buzzer is blue ,count is " + count + " " + DateTime.Now.ToString(), (int)ErrorCode.ERR_CODE.OK);
                    }
                }
                Console.WriteLine(count);
            }
        }
 
        
        public void addUnNormalDevice(string deviceId)
        {
            if(!ignoreAlarmDevice.Contains(deviceId))
            {
                alarmDevices.Add(deviceId);
                checkAlarmStatus();
            }
        }
        public void deleteUnNormalDevice(string deviceId)
        {
            alarmDevices.Remove(deviceId);
            checkAlarmStatus();
        }
        public void addIgnoreDevice(string deviveId)
        {
            ignoreAlarmDevice.Add(deviveId);
        }
        public void deleteIgnoreDevice(string deviveId)
        {
            if (ignoreAlarmDevice.Contains(deviveId))
            {
                ignoreAlarmDevice.Remove(deviveId);
            }
        }

        private void checkAlarmStatus()
        {
            if (alarmDevices.Count > 0)
            {
                if (!IsAlarming)
                {
                    if(!isAlarming)
                    {
                        IsAlarming = true;
                    }
                }
            }
            else
            {
                IsAlarming = false;
                count = 0;
            }
        }
        //只闪烁，无声音
        public void muteBuzzer()
        {
            isMute = !isMute;
        }
    }

    
}
