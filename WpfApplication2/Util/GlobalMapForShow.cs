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
        { "2_1", "324PurificationSys" },{  "2_2", "324WasteProcessSys" },{  "2_3", "404HPT500II" },
        {  "2_4", "404MTI" },{ "2_5", "404MTIIMTIII" },{  "2_6", "407HPT500I" },
        {  "2_7", "422ThermalDesorptionSys" },{"5_1","亭子"} };
 
    //,{"5_43","asm02"},{"5_46","jl900"},{"5_47","DryWet"}
    }
}
