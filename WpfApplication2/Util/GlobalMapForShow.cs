using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WpfApplication2.Controller;
using WpfApplication2.Model.Vo;
using WpfApplication2.package;

namespace WpfApplication2.Util
{
    class GlobalMapForShow
    {
        public static Dictionary<string, List<DeviceDataBox_Base>> globalMapWithCab = new Dictionary<string, List<DeviceDataBox_Base>>();
        public static Dictionary<string, List<DeviceDataBox>> globalMapWithoutCab = new Dictionary<string, List<DeviceDataBox>>();
        public static List<string> whiteLists = new List<string>() { "2013" };

        /**
         * 上面三个弃用，后期用下面的三个Dictionary
         *
         */
        public static Dictionary<string, Building> globalMapForBuiding = new Dictionary<string, Building>();  //这个map键值分别是“systemId”，Building对象
        public static Dictionary<string, Cab> globalMapForCab = new Dictionary<string, Cab>();//这个map键值分别是“systemId_cabId”，Cab对象
        public static Dictionary<string, Device> globalMapForDevice = new Dictionary<string, Device>();//这个map键值分别是“systemId_deviceId”，device对象


        public static Dictionary<String, String> cabId_typeInSystem = new Dictionary<String, string>() { 
        { "2_2", "324PurificationSys" },{  "2_1", "324WasteProcessSys" },{  "2_5", "404HPT500II" },
        {  "2_3", "404MTI" },{ "2_4", "404MTIIMTIII" },{  "2_6", "407HPT500I" },
        {  "2_7", "422ThermalDesorptionSys" },{"2_8","422UnifyThermalDesorptionSys"},{"5_9","亭子(运输部)"} ,
         {"4_10","209"},{"9_11","208烟囱监测程序"},{"10_12","201烟囱监测程序"},{"11_13","207烟囱监测程序"},{"6_14","207c"},{"3_15","2115楼"},{"12_16","亭子（新桥）"},{"13_17","亭子（内网）"} };

        /// <summary>
        /// 通过输入监测点的名称 获取监测点实例
        /// </summary>
        /// <param name="buildingName"></param>
        /// <returns></returns>
        public static Building getBuildingByName(string buildingName)
        {
                Building building = null ;
                foreach(Building b in globalMapForBuiding.Values)
                {
                    if (b.Name.Equals(buildingName))
                    {
                        building = b;
                        break;
                    }
                }
            return building;
        }

        public static bool isAllBuildingNormal()
        {
            foreach(Building b  in globalMapForBuiding.Values)
            {
                if(!b.State.Equals("Normal"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
