using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using GMap.NET ;
using Demo.WindowsPresentation.Controls;

namespace Demo.WindowsPresentation.CustomMarkers
{
   /// <summary>
   /// Interaction logic for CustomMarkerDemo.xaml
   /// </summary>
   public partial class CustomMarkerRed
   {
      Popup Popup;
      Label Label;
      GMapMarker Marker;
      MainWindow MainWindow;
      PointLatLng point;
      private GMapMarker marker;
      private GMapMarker[] markers;
     public  onRedMarkerClickLisener RedMarkerClickLisener ;
      public CustomMarkerRed(PointLatLng point)
      {
          point = Marker.Position;
      }


      public CustomMarkerRed(MainWindow window, GMapMarker marker, UIElement ui,onRedMarkerClickLisener listener)
      {
         this.InitializeComponent();

         this.MainWindow = window;
         this.Marker = marker;

         Popup = new Popup();
         Label = new Label();
         point = marker.Position;

         this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);
         this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
         this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
         this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
         this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
         this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
         this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);
      //   this.MouseDoubleClick += new
         this.RedMarkerClickLisener = listener;
          if(ui!=null)
          {
              ((TrolleyTooltip)ui).setStatus("异常");
                Popup.Placement = PlacementMode.Mouse;
                {
                    Label.Background = Brushes.Blue;
                    Label.Foreground = Brushes.White;
                    Label.BorderBrush = Brushes.WhiteSmoke;
                    Label.BorderThickness = new Thickness(2);
                    Label.Padding = new Thickness(5);
                    Label.FontSize = 22;
                    // Label.Content = title;
                    Label.Content = "lable content!";
                }
              Popup.AllowsTransparency = true;
              Popup.Child =ui;
              
          }
          
        
      }
           
      void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
      {
         if(icon.Source.CanFreeze)
         {
            icon.Source.Freeze();
         }
      }

      void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
      {
         Marker.Offset = new System.Windows.Point(-e.NewSize.Width/2, -e.NewSize.Height);
      }

      void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
      {
         if(e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured)
         {
            System.Windows.Point p = e.GetPosition(MainWindow.MainMap);
            Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int) p.X, (int) p.Y);
         }
      }

      void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {
         if(!IsMouseCaptured)
         {
            Mouse.Capture(this);
         }
         System.Console.WriteLine("marker Red MouseLeftButtonDown :" );

         //Window1 window = new Window1(point);
         //window.Show();

         System.Windows.Point p = e.GetPosition(MainWindow.MainMap);
         Marker.Position = MainWindow.MainMap.FromLocalToLatLng((int)p.X, (int)p.Y);
         RedMarkerClickLisener.onRedMarkerclick(Marker.Position);
         
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


       //鼠标移动到红点，弹出提示窗体popup 
      void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
      {
         Marker.ZIndex += 10000;
         Popup.IsOpen = true;
      }

      private void icon_ImageFailed(object sender, ExceptionRoutedEventArgs e)
      {

      }

      private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
      {

      }
      public interface onRedMarkerClickLisener
      {
            void onRedMarkerclick(PointLatLng point);
      }
      public void setOnRedMarkerClick(onRedMarkerClickLisener listener)
      {
          this.RedMarkerClickLisener = listener;
      }
   }
  
 
 }