using System;
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

namespace WpfApplication2.CustomMarkers.Controls
{
    /// <summary>
    /// LabelAndText.xaml 的交互逻辑
    /// </summary>
    public partial class LabelAndText : UserControl
    {
        public static readonly DependencyProperty LTForeGround;
      //  private Color foregroundColor; 
        public LabelAndText()
        {
            InitializeComponent();
        }
        public LabelAndText(String label,String value,String unit)
        {
            InitializeComponent();
            Label.Content = label;
            Value.Text = "  " + value + "  ";
            units.Text = unit;
        }
        public LabelAndText(String label, String value)
        {
            InitializeComponent();
            Label.Content = label;
            Value.Text = "  " + value + "  ";
        }

        public LabelAndText(String label, String value, String unit,Color c)
        {
            InitializeComponent();
            Label.Content = label;
            Value.Text = "  " + value + "  ";
            units.Text = unit;
            setForeGround(c);
        }
        public LabelAndText(String label, String value ,Color c)
        {
            InitializeComponent();
            Label.Content = label;
            Value.Text = "  " + value + "  ";
            units.Text = "";
            setForeGround(c);
        }
        public void setForeGround(Color c)
        {
            this.Label.Foreground = new SolidColorBrush(c);
            this.Value.Foreground = new SolidColorBrush(c);
            this.units.Foreground = new SolidColorBrush(c);
        }
        public void updateValue(String value)
        {
           Value.Text = "  " + value + "  ";
        }
        public Label getLabel()
        {
            return Label;
        }
        public TextBlock getValueTextBlock()
        {
            return Value;
        }
        public TextBlock getUnitTextBlock()
        {
            return units;
        }
    }
}
