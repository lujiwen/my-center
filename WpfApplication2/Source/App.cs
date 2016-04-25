using System;
using GMap.NET;
using WpfApplication2.View.Windows;
using WpfApplication2.Controller;
using System.Windows;
 

namespace WpfApplication2
{
   public partial class App : Application
   {
      [STAThread()]
      static void Main()
      {
         Application app = new Application();
         MainWindow win = new  MainWindow();
         app.Run(win);
      }
   }

   public struct PointAndInfo
   {
      public PointLatLng Point;
      public string Info;

      public PointAndInfo(PointLatLng point, string info)
      {
         Point = point;
         Info = info;
      }
   }
}
