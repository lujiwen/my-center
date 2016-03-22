using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Util
{
    public class ErrorCode
    {
        public enum ERR_CODE
        {
            OK = 0x00,
            CONNECTION_OPEN_ERR = 0x01,
            BUILDING_READ_ERR = 0x02,
            DEVICE_READ_ERR = 0x03,
            CAB_READ_ERR = 0x04, 
            CONNECTION_CLOSE_ERR = 0x05,
            MAP_POINTS_INIT_ERR = 0x06,
            WIRTE_CONFIG_ERR = 0x07
        } ;

        private string errDescription;
        private string errContent;
        public string ErrDescription { get { return errDescription; } set { errDescription = value; } }
        public string ErrContent { get { return errContent; } set { errContent = value; } }
        public ErrorCode(int code, string exMsg)
        {
            errContent = exMsg;
            switch (code)
            {
                case (int)ERR_CODE.CONNECTION_OPEN_ERR:
                    errDescription = "数据库连接打开异常";
                    break;
                case (int)ERR_CODE.BUILDING_READ_ERR:
                    errDescription = "监测点信息读取异常";
                    break;
                case (int)ERR_CODE.DEVICE_READ_ERR:
                    errDescription = "设备信息读取异常";
                    break;
                case (int)ERR_CODE.CAB_READ_ERR:
                    errDescription = "柜子信息读取异常";
                    break;
                case (int)ERR_CODE.CONNECTION_CLOSE_ERR:
                    errDescription = "数据库连接关闭异常！";
                    break;
                //case ERR_CODE.CONNECTION_OPEN_ERR:
                //    break ;
                case (int)ERR_CODE.MAP_POINTS_INIT_ERR:
                    errDescription = "地图监测点初始化异常";
                    break;
                default:
                    errDescription = "无异常";
                    errContent = "无异常";
                    break;
            }
        }
    }
}
