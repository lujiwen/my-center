using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using System.Diagnostics;

namespace Demo.WindowsPresentation.CustomMarkers
{
   /// <summary>
   /// Interaction logic for CustomMarkerDemo.xaml
   /// </summary>
   public partial class CustomMarkerDemo
   {
      Popup Popup;
      Label Label;
      GMapMarker Marker;
      MainWindow MainWindow;

      public CustomMarkerDemo(MainWindow window, GMapMarker marker, UIElement ui)
      {
          this.InitializeComponent();

          this.MainWindow = window;
          this.Marker = marker;

          Popup = new Popup();
          Label = new Label();

          //this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
          //this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
          //this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
          this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
          this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
          this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
          this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
          this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

      }

      void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
      {
         if(e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
         {
            Point p = e.GetPosition(MainWindow.MainMap);
            Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int) (p.X), (int) (p.Y));
         }
      }

      void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if(!IsMouseCaptured)
         {
            Mouse.Capture(this);
         }
      }

      void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
      {
         if(IsMouseCaptured)
         {
            Mouse.Capture(null);
         }
      }
       
      void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
      {
         Marker.ZIndex -= 10000;
         Popup.IsOpen = false;
      }

      void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
      {
         Marker.ZIndex += 10000;
         Popup.IsOpen = true;
      }
   }
}