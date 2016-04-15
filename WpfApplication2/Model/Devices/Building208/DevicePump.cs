using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using WpfApplication2.Model.Vo;
using System.Data.OracleClient;

namespace Project208Home.Model
{
    /// <summary>
    /// 一个泵对应一套设备
    /// </summary>
    public delegate void PumpPropertyEventHandler(Boolean openstate);
    public class DevicePump : Device
    {
        private Boolean openState;

        public Boolean pumpOperation { get; set; }//存储最新的泵操作，当点击开关时，其值进行切换 

        private Boolean pumpNoUpdated;//是否更新

        
        //判定值是否改变，用于实时显示
        PumpPropertyEventHandler propertyChanged;

        public DevicePump() { }
        public DevicePump(OracleDataReader odr)
            :base(odr)
        {

        }
        public DevicePump(UInt32 id, UInt32 interfaceId, String type)
        {
            //this.DevId = id;
            //this.InterfaceId = interfaceId;
            //this.DevType = type;
        }
        public Boolean PumpNoUpdated
        {
            get { return pumpNoUpdated; }
            set { pumpNoUpdated = value; }
        }
        public PumpPropertyEventHandler PumpPropertyChanged
        {
            get { return propertyChanged; }
            set { propertyChanged = value; }
        }

        /// <summary>
        /// 针对泵进行控制
        /// </summary>
        public void setPumpOperation()
        {
            //监听开关量按钮是否被切换，如被点击，则应该更改pump操作
            pumpOperation = true;
        }

        public Boolean OpenState
        {
            get { return openState; }
            set
            {
                Boolean oldvalue = openState;
                openState = value;
                if (PumpNoUpdated || oldvalue != openState)
                {
                    propertyChanged(openState);
                    PumpNoUpdated = false;
                }
            }
        }
        public static string GenerateSql(Device d, string tablename)
        {
            return "INSERT INTO " + tablename + "( DD_ID, DEVID, DATATIME, VALUE1, UNITS,SAFESTATE)" + " VALUES(" + tablename + "_sequence" + ".nextval" + ", " + d.DeviceId + ", " + "'" + DateTime.Now + "'" + ", " + d.NowValue + ", " + "'" + d.DataUnit + "'" + ", " + "'" + d.State + "' )";
        }
    }
}
