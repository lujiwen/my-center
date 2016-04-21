using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using WpfApplication2.Controller;
using WpfApplication2.Model.Vo;
using System.Windows.Forms;
using WpfApplication2.Util;
using Project208Home.Model;

namespace WpfApplication2.Model.Db
{
   public class DBManager
    {
        private  OracleConnection Conn;

        public int OpenConnection(string str_uid, string str_pwd, string str_serveraddr, string str_port, string str_dbname, ref string strErr)
        {
            string str_conn = string.Format("user id={0};password={1};data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST={2})(PORT={3}))(CONNECT_DATA=(SERVICE_NAME={4})))", str_uid, str_pwd, str_serveraddr, str_port, str_dbname);//连接字符串格式化
            Conn = new OracleConnection(str_conn);//定义连接实例
            try
            {
                Conn.Open();//通过自带的方式打开连接，测试连接实例
                return (int)ErrorCode.ERR_CODE.OK;
            }
            catch (Exception ex)
            {
                strErr = ex.Message;
                LogUtil.Log(true,ex.Message,(int)ErrorCode.ERR_CODE.CONNECTION_OPEN_ERR);
                return (int)ErrorCode.ERR_CODE.CONNECTION_OPEN_ERR;
            }
           
        }

        public OracleConnection GetConnection()
        {
            return Conn;
        }


        public int CloseConnection()
        {

            if (Conn != null)
            {
                Conn.Close();
                return (int)ErrorCode.ERR_CODE.OK;
            }
            else
            {
                return (int)ErrorCode.ERR_CODE.CONNECTION_CLOSE_ERR;
            }
        }

        public OracleDataReader ReadDeviceInfomationFromDb(String sql)
        {
            OracleCommand command = Conn.CreateCommand();
            command.CommandText = sql;
            OracleDataReader odr = command.ExecuteReader();
            return odr;
        }

        public int InsertDataToDb(String tablename,Device value)
        {
            String sql = value.GenerateInsertSql(tablename);  //由每个具体的设备类提供插入sql的语句

            OracleCommand command = new OracleCommand(sql, Conn);
            int result  = command.ExecuteNonQuery();
            return result;
        }

        public int UpdateDeviceInfo(String tablename, Device value)
        {
            String sql = "UPDATE " + tablename + " set highthreshold=" + value.Highthreshold + " ,lowthreshold =" + value.Lowthreshold + " , correctFactor =" + value.CorrectFactor + " where BUILDINGID = " + value.BuildingId + " and D_ID = " + value.DeviceId;
            OracleCommand command = new OracleCommand(sql, Conn);
           int result = command.ExecuteNonQuery();
            return result;
        }

        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public int InsertExceptionToDb(String tableName, Device value, String Content) //tableName:EXCEPTIONINFO
        {
            String sql = "INSERT INTO " + tableName + " VALUES(" + "exceptioninfo_sequence" + ".nextval" + ", " + value.BuildingId + ", " + value.CabId + ", " + value.DeviceId + ", '" + Content+ " ' , '" + DateTime.Now  + "'" + ")";
            OracleCommand command = new OracleCommand(sql, Conn);
            int result = command.ExecuteNonQuery();
            return result;
        }

        public Dictionary<string, List<DeviceData>> getDataBetweenStartAndEndTime(Device device, String startTime, String endTime)
        {
            List<DeviceData> deviceDatas = new List<DeviceData>();
            OracleCommand command = Conn.CreateCommand();
            command.CommandText = device.GenerateSelectSql("DEVICEDATA_" + device.BuildingId, startTime,endTime);
            OracleDataReader odr = null ;
            try
            {
                 odr = command.ExecuteReader();
            }
            catch(Exception e)
            {
                return null;
            }
            Dictionary<string, List<DeviceData>> datas = device.getHistoryDataSet(odr);
            odr.Close();
            return datas;
        }

        public Dictionary<string, List<DeviceData>> getDataBetweenStartAndEndTime(Cab cab, String startTime, String endTime)
        {
            List<DeviceData> deviceDatas = new List<DeviceData>();
            OracleCommand command = Conn.CreateCommand();
            command.CommandText = cab.GenerateSelectSql(startTime, endTime);
            OracleDataReader odr = null;
            try
            {
                odr = command.ExecuteReader();
            }
            catch (Exception e)
            {
                return null;
            }
            Dictionary<string, List<DeviceData>> datas = cab.getHistoryDataSet(odr);
            odr.Close();
            return datas;
        }
    }
}