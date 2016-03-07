using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApplication2.CustomMarkers.Controls
{
    class PowerControl
    {
        
        private Boolean isPowerOn;
        private BitmapImage powerOnBm, poweroffBm;
        private Image powerBtn ;

        public PowerControl(Image powerimage)
        {
            powerOnBm = new BitmapImage(new Uri("/Images/running.png", UriKind.Relative));
            poweroffBm = new BitmapImage(new Uri("/Images/stop.png", UriKind.Relative));
            powerBtn = powerimage;
            powerOn();
        }
        public void powerOn()
        {
            powerBtn.Source = powerOnBm;
            isPowerOn = true ;
        }

        public void powerOff()
        {
            powerBtn.Source = poweroffBm;
            isPowerOn = false ;
        }

        public bool getIsPowerOn()
        {
            return isPowerOn;
        }
    }
}
