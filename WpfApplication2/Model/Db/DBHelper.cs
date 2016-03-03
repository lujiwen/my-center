using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
namespace WpfApplication2.Model.Db
{
    class DBHelper
    {
        /**
        * db常量
        * */
        DBManager initialDB;
        public static string db_name = "MYDB"; //数据库名
        public static string emergencyDb_name = "emergency_orcl"; //应急数据库名，为方便，与正常数据库的账户和密码端口等均一致
        //public static string db_sysName = "sys";  //系统账户名
        //public static string db_sysPassWord = "548983"; //系统密码
        public static string db_userName = "mianyang"; //操作账户名
        public static string db_userPassWord = "mianyang";//操作密码
        public static string db_ip = "192.168.1.101";//数据库ip
        public static string db_port = "1521"; //端口
        //public static string db_ip = "100.64.48.151";//数据库ip
        //public static string db_port = "15431"; //端口
        public static int dataTable_startIndex = 1; //存储数据的表开始值
        public static int dataTable_endIndex = 15; //存储数据的表结束值
        public static bool testFlag = true;// 是否需要插入测试数据
        public static String NORMALDATATABLE = "DEVICEDATA_";  //以下三个变量分别标志正常数据表，总表（不删数据）或者临时表（定期删除数据，对外接口）
        public static String TOTLEDATATABLE = "DEVICETOTLEDATA_";
        public static String TEMPDATATABLE = "DEVICETEMPDATA_";

        public DBHelper()
        {

        }

        //public void createUserAndGrant()   //不能用代码的形式创建用户，只能手动，原因是网上说：用.NET新提供的managed provider来访问Oracle数据(System.Data.OracleClient),是无法以sys用户登录的。
        //{ //创建用户并授权
        //    initialDB = new DBManager();
        //    string errorCode = "";
        //    initialDB.OpenConnection(db_sysName, db_sysPassWord, db_ip, db_port, db_name, ref errorCode);
        //    string createUserSql = "CREATE USER " + db_userName + " IDENTIFIED BY " + db_userPassWord;
        //    string grantSql = "GRANT DBA TO " + db_userName;
        //    OracleCommand createUserCommand = new OracleCommand(createUserSql, initialDB.GetConnection());
        //    OracleCommand grantCommand = new OracleCommand(createUserSql, initialDB.GetConnection());
        //    int result = createUserCommand.ExecuteNonQuery();
        //    int result2 = grantCommand.ExecuteNonQuery();
        //    initialDB.CloseConnection();
        //}
        public void createAllTableAndSequence(int type) //type 0表示正常数据库，1表示应急数据库
        {  //创建所有的表和序列
            initialDB = new DBManager();
            string errorCode = "";
            switch (type)
            {
                case 0:
                    initialDB.OpenConnection(db_userName, db_userPassWord, db_ip, db_port, db_name, ref errorCode);
                    break;
                case 1:
                    initialDB.OpenConnection(db_userName, db_userPassWord, db_ip, db_port, emergencyDb_name, ref                            errorCode);
                    break;
            }

            /**
             * 以前下面这些用循环创建的语句改成为每栋楼根据实际情况单独建表
             * 参数分别表示：创建的楼的id，楼中设备最多的有几个参数
             * */

            CreateDataTableAndSequence(1, 8);//这行的意思就是创建sid为2（208）的数据表，而208的设备中最多有8个参数


            createConstantTableAndInsertConstantData(); //创建除数据表以外的其他数据表，并插入测试数据

        }

        /**
          *下面这些以前是用循环创建的语句改成为每栋楼根据实际情况单独建表
          * 参数分别表示：创建的楼的id，楼中设备最多的有几个参数
          * */
        public void CreateDataTableAndSequence(int systemId, int number)
        {
            List<String> tableSQLs = new List<String>();  //为每个楼创建正常数据表、总表和临时表
            String createTableSql = GenerateTableSQL(NORMALDATATABLE, systemId, number);
            String createTableTotleSql = GenerateTableSQL(TOTLEDATATABLE, systemId, number);
            String createTableTempSql = GenerateTableSQL(TEMPDATATABLE, systemId, number);
            String createSequenceSql = GenerateSequenceSQL(NORMALDATATABLE, systemId);
            String createSequenceTotleSql = GenerateSequenceSQL(TOTLEDATATABLE, systemId);
            String createSequenceTempSql = GenerateSequenceSQL(TEMPDATATABLE, systemId);
            tableSQLs.Add(createTableSql);
            tableSQLs.Add(createTableTotleSql);
            tableSQLs.Add(createTableTempSql);
            tableSQLs.Add(createSequenceSql);
            tableSQLs.Add(createSequenceTotleSql);
            tableSQLs.Add(createSequenceTempSql);

            foreach (String item in tableSQLs)
            {
                OracleCommand createTableCommand = new OracleCommand(item, initialDB.GetConnection());
                int createTableResult = createTableCommand.ExecuteNonQuery();
                System.Threading.Thread.Sleep(100);
            }
        }


        /**
         * 根据传入的参数生成创建数据表的sql
         * */
        public String GenerateTableSQL(String tableName, int systemId, int number)
        {
            String args = " ";
            for (int i = 1; i <= number; i++)
            {
                args += "ARG" + i + " float(126) " + " , ";
            }
            String result = "CREATE TABLE " + tableName + systemId
                + "("
                + "  DD_ID NUMBER(38, 0) NOT NULL "
                + ", DEVID NUMBER(38, 0) "
                + ", DATATIME VARCHAR2(25 BYTE) "
                + ", NOWVALUE FLOAT(126) "
                + "," + args
                + " UNITS VARCHAR2(200 BYTE) "
                + ", SAFESTATE VARCHAR2(10 BYTE)"
                + ", CONSTRAINT PK_" + tableName + systemId + " PRIMARY KEY "
                + "  ("
                + "    DD_ID "
                + "  )"
                + ")";

            return result;
        }

        /**
       * 根据传入的参数生成创建数据表序列的sql
       * */
        public String GenerateSequenceSQL(String tableName, int systemId)
        {
            String result = "CREATE SEQUENCE " + tableName + systemId + "_sequence" + " INCREMENT BY 1 START WITH 1                                 NOMAXVALUE NOCYCLE CACHE 10";

            return result;
        }


        /**
         * 创建除数据表以外的其他只读数据表，并根据需要插入测试数据
         */
        public void createConstantTableAndInsertConstantData()
        {
            String createBuildingTableSql = "CREATE TABLE BUILDINGINFO (B_ID NUMBER(38,0) NOT NULL , NAME VARCHAR2(30 BYTE), OFFICE VARCHAR2(30 BYTE), LOCATION VARCHAR2(30 BYTE), LOCATION_LAT float(126), LOCATION_LNG float(126), MANAGER VARCHAR2(30 BYTE), CONSTRAINT BUILDINGINFO_PK PRIMARY KEY (B_ID))";
            String createCabTableSql = "CREATE TABLE CABINFO (C_ID NUMBER(38,0) NOT NULL ENABLE, BUILDINGID NUMBER(38,0), NAME VARCHAR2(30 BYTE), OFFICE VARCHAR2(30 BYTE), HOME VARCHAR2(50 BYTE), IP VARCHAR2(20 BYTE), PORT VARCHAR2(8 BYTE), CONSTRAINT CABINFO_PK PRIMARY KEY (C_ID))";
            String createDeviceTableSql = "CREATE TABLE DEVICEINFO (D_ID NUMBER(38,0) NOT NULL ENABLE, TYPE VARCHAR2(30 BYTE), CABID NUMBER(38,0), SUBSYSTEMSERIAL NUMBER(38,0), SUBSYSTEMNAME VARCHAR2(30 BYTE), HIGHTHRESHOLD float(126), LOWTHRESHOLD float(126), DEVLOCALADDRESS NUMBER(38,0), "
            + "INTERFACEID NUMBER(38,0), CORRECTFACTOR float(126), DATAUNIT VARCHAR2(20 BYTE), INPUTARG1 float(126), INPUTARG2 float(126), INPUTARG3 float(126), BUILDINGID NUMBER(38,0), HANDLETYPEINSYSTEM VARCHAR2(45 BYTE), CONSTRAINT DEVICEINFO_PK PRIMARY KEY (D_ID))";
            String createExceptionTableSql = "CREATE TABLE EXCEPTIONINFO (E_ID NUMBER(38,0) NOT NULL ENABLE, B_ID NUMBER(38,0), C_ID NUMBER(38,0), D_ID NUMBER(38,0), CONTENT VARCHAR2(100 BYTE), DATATIME VARCHAR2(25 BYTE), CONSTRAINT EXCEPTIONINFO_PK PRIMARY KEY (E_ID))";
            String createExceptionSequence = "CREATE SEQUENCE " + "exceptioninfo_sequence" + " INCREMENT BY 1 START WITH 1 NOMAXVALUE NOCYCLE CACHE 10";
            OracleCommand command1 = new OracleCommand(createBuildingTableSql, initialDB.GetConnection());
            OracleCommand command2 = new OracleCommand(createCabTableSql, initialDB.GetConnection());
            OracleCommand command3 = new OracleCommand(createDeviceTableSql, initialDB.GetConnection());
            OracleCommand command4 = new OracleCommand(createExceptionTableSql, initialDB.GetConnection());
            OracleCommand command5 = new OracleCommand(createExceptionSequence, initialDB.GetConnection());
            int result = command1.ExecuteNonQuery();
            int result2 = command2.ExecuteNonQuery();
            int result3 = command3.ExecuteNonQuery();
            int result4 = command4.ExecuteNonQuery();
            int result5 = command5.ExecuteNonQuery();
            System.Threading.Thread.Sleep(500);
            if (testFlag)
            {
                string path = new DirectoryInfo("../../").FullName;//当前应用程序路径的上两级目录
                StreamReader sr = new StreamReader(path + "testData.txt", Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    OracleCommand command = new OracleCommand(line.Replace(";", ""), initialDB.GetConnection());
                    int insertResult = command.ExecuteNonQuery();
                    System.Threading.Thread.Sleep(100);
                }
            }
            initialDB.CloseConnection();
        }
    }

}
