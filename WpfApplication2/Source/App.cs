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
         // Create the application.
         Application app = new Application();
      //   MainController c = new MainController();

         // Create the main window.
             MainWindow win = new  MainWindow(); 
          //  MapWindow win = new MapWindow();
         //  Window1 win = new Window1();
          // Launch the application and show the main window.
           app.Run(win);
      }
   }


   public class Dummy
   {

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
