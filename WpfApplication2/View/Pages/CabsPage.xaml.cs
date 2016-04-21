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
using Visifire.Charts;
using System.Threading;
using WpfApplication2.Model.Vo;
using WpfApplication2.Controls;
using WpfApplication2.Util;
using System.Collections;

namespace WpfApplication2.View.Pages
{
    /// <summary>
    /// CabsPage.xaml 的交互逻辑
    /// </summary>

    public delegate void DataMonitor(int data);
    public partial class CabsPage : Page
    {
        SystemPage systemPage;
        private List<Cab> cabs = new List<Cab>();
        private List<CabUI> _cabUIs = new List<CabUI>();
        private int x;
        public Building _building;
        private ArrayList cabListSource;
        public List<Building> selectBuildings;
        public List<Cab> selectCabs;
        public CabsPage(SystemPage page, Building b)
        {
            InitializeComponent();
            this.systemPage = page;
            _building = b;
            this.cabs = b.Cabs;
            init();
        }

        public CabsPage(List<Cab> cabs)
        {
            InitializeComponent();
            this.cabs = cabs;
            init();
        }

        public CabsPage(SystemPage sp)
        {
            InitializeComponent();
            systemPage = sp;
            init();
        }

        //添加一个柜子到当前页面
        public void insertCab(Frame sysFram, Cab c)
        {
            selectCabs.Add(c);
            foreach (CabUI cu in CabList.Items)
            {
                if (cu.CabInUI.CabId == c.CabId)
                {
                    return;
                }
            }
            cabListSource.Add(new CabUI(sysFram, c));
            ApplyDataBinding();
        }

        //添加一个楼的柜子到当前页面
        public void insertCab(Frame sysFram, Building b)
        {
            selectBuildings.Add(b);
            for (int i = 0; i < b.Cabs.Count; i++)
            {
                selectCabs.Add(b.Cabs[i]);
                _cabUIs.Add(new CabUI(sysFram, b.Cabs[i]));
            }
            //  
            for (int i = 0; i < b.Cabs.Count; i++)
            {
                cabListSource.Add(new CabUI(sysFram, b.Cabs[i]));
            }

            ApplyDataBinding();
        }

        private void ApplyDataBinding()
        {
            CabList.ItemsSource = null;
            // Bind ArrayList with the ListBox
            CabList.ItemsSource = cabListSource;
        }
        public void deleteCab(Frame sysFram, Building b)
        {
            for (int i = cabListSource.Count - 1; i >= 0; i--)
            {
                if (((CabUI)cabListSource[i]).CabInUI.BuildingId == b.SystemId)
                {
                    cabListSource.RemoveAt(i);
                }
            }
            ApplyDataBinding();
        }

        //删除一个柜子
        public void deleteCab(Frame sysFram, Cab c)
        {
            for (int i = 0; i < CabList.Items.Count; i++)
            {
                if (((CabUI)cabListSource[i]).CabInUI.CabId == c.CabId)
                {
                    cabListSource.RemoveAt(i);
                }
            }
            ApplyDataBinding();
        }

        private void init()
        {
            cabListSource = new ArrayList();
            selectBuildings = new List<Building>();
            selectCabs = new List<Cab>();
            if (_building != null)
            {
                selectBuildings.Add(_building);
            }

            if (cabs != null)
            {
                for (int i = 0; i < cabs.Count; i++)
                {
                    WpfApplication2.Model.Vo.Cab cab = cabs[i];
                   //   WpfApplication2.Model.Vo.Cab cab = GlobalMapForShow.globalMapForCab[cabs[i].BuildingId+"_"+cabs[i].CabId];
                    CabUI cabUI = new CabUI(systemPage.getPageFrame(), cab);
                    cabListSource.Add(cabUI);
                }
            }
            CabList.ItemsSource = cabListSource;
            this.Unloaded += new RoutedEventHandler(CabsPage_Unloaded);
            systemPage.getMainWindowInstance().c.dataChartUpdate += new Controller.DataUpdatedEventHandler(updateCabsCharts);
        }

        private void updateCabsCharts()
        {
            Dispatcher.BeginInvoke(new Action(updateAllCharts));
        }
        private void updateAllCharts()
        {
            if (cabListSource != null)
            {
                foreach (CabUI cu in cabListSource)
                {
                    cu.updateCabUI();
                }
            }
        }

        void CabsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("CabsPage_Unloaded");
        }

        public void addBuildingToShow(Building b)
        {
            selectBuildings.Add(b);
        }
    }
}