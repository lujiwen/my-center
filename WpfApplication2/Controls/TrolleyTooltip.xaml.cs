using System.Windows.Controls;
using GMap.NET;
using WpfApplication2.Model.Vo;

namespace WpfApplication2.Controls
{
   /// <summary>
   /// Interaction logic for TrolleyTooltip.xaml
   /// </summary>
   public partial class TrolleyTooltip : UserControl
   {
       private Building building;
      public TrolleyTooltip()
      {
         InitializeComponent();
      }
      public TrolleyTooltip(Building b)
      {
          InitializeComponent();
          building = b;
          state.Text = b.State;
          position.Text = b.Lat + "," + b.Lng;
          buildName.Text = b.Name;
      }
       public void  setStatus(string status)
       {
           state.Text = status;
       }
      public void SetValues(string type, VehicleData vl)
      {
       //  Device.Text = vl.Id.ToString();
       // LineNum.Text = type + " " + vl.Line;
         TimeGps.Text = vl.Time;
      }
   }
}
