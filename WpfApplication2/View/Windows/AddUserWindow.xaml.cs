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
using System.Windows.Shapes;
using WpfApplication2.Model.Vo;
using WpfApplication2.Util;
using WpfApplication2.CustomMarkers.Controls;

namespace WpfApplication2.View.Windows
{
    /// <summary>
    /// AddUser.xaml 的交互逻辑
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public delegate void AddUser(User user);
        public event AddUser adduser;
        private Dictionary<string, Building> buidings;
        public AddUserWindow()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            Topmost = true;
            buidings = GlobalMapForShow.globalMapForBuiding;
            if (buidings != null)
            {
                for (int i = 0; i < buidings.Count; i++)
                {
                    if (buidings.ContainsKey((i + 1) + ""))
                    {
                        MyCheckBox c = new MyCheckBox();
                        c.Margin = new Thickness(10, 10, 10, 10);
                        c.Name = "building" + buidings[(i + 1) + ""].SystemId;
                        c.Content = buidings[(i + 1) + ""].Name;
                        c.FontSize = 15;
                        c.NodeObject = buidings[(i + 1) + ""];
                        c.Foreground = new SolidColorBrush(Colors.White);
                        building_panel.Children.Add(c);
                    }
                }
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Comfirm_Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> privilege = new List<string>();
            int i = 0;
            foreach (MyCheckBox b in building_panel.Children)
            {
                if (b.IsChecked ?? false)
                {
                    //buildings.Add((Building)b.NodeObject);
                    //arr[i] = true;
                    privilege.Add(((Building)b.NodeObject).Name);
                }
                i++;
            }
           
            User user = new User(usernameTB.Text, passwordTB.Password, "normal", privilege);
            adduser(user);
            Close();
        }
    }
}
